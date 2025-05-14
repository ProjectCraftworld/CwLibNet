using System.Text;
using CwLibNet.Enums;
using CwLibNet.Extensions;
using CwLibNet.External;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Texture;
using CwLibNet.Types;
using SkiaSharp;

namespace CwLibNet.Util;

public class Images
{
    
    private static int ToNearest(int x)
    {
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;
        return x ^ (x >> 1);
    }

    public static SKBitmap ToBump(SKBitmap image)
    {
        var c = image.GetPixel(0, 0);
        var isNormal = !(c.Red == c.Green && c.Red == c.Blue);
        if (!isNormal) return image;
        for (var x = 0; x < image.Width; ++x)
        {
            for (var y = 0; y < image.Height; ++y)
            {
                c = image.GetPixel(x, y);
                var green = c.Green;
                var red = c.Red;
                image.SetPixel(x, y, new SKColor(
                    green,
                    green,
                    green,
                    red
                ));
            }
        }
        return image;
    }

    private static byte[] GetRgba(SKBitmap image)
    {
        List<uint> argb = [];
        for (var x = 0; x < image.Height; ++x)
            for (var y = 0; y < image.Width; ++y)
                argb.Add(image.GetPixel(x, y).ToColor());
        //image.getRGB(0, 0, image.getWidth(), image.getHeight(), null, 0,
//            image.getWidth());
        var rgba = new byte[argb.Count * 4];
        for (var i = 0; i < argb.Count; ++i)
        {
            rgba[4 * i] = (byte) ((argb[i] >> 16) & 0xff);
            rgba[4 * i + 1] = (byte) ((argb[i] >> 8) & 0xff);
            rgba[4 * i + 2] = (byte) (argb[i] & 0xff);
            rgba[4 * i + 3] = (byte) ((argb[i] >> 24) & 0xff);
        }
        return rgba;
    }


    private static byte[] ToDds(SKBitmap image, SquishCompressionType type,
                                bool generateMips)
    {
        var width = ToNearest(image.Width);
        var height = ToNearest(image.Height);

        int originalWidth = width, originalHeight = height;

        CellGcmEnumForGtf? format = null;
        Squish.CompressionType compressionType;
        switch (type)
        {
            case SquishCompressionType.DXT1:
                format = CellGcmEnumForGtf.DXT1;
                compressionType = Squish.CompressionType.DXT1;
                break;
            case SquishCompressionType.DXT3:
                format = CellGcmEnumForGtf.DXT3;
                compressionType = Squish.CompressionType.DXT3;
                break;
            case SquishCompressionType.DXT5:
                format = CellGcmEnumForGtf.DXT5;
                compressionType = Squish.CompressionType.DXT5;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        if (image.Width != width || image.Height != height)
            image = image.Resize(new SKSizeI(width, height), SKSamplingOptions.Default);
        var dds = Squish.CompressImage(image.Bytes, width, height, null, compressionType);
        var mipCount = 1;
        if (!generateMips)
            return Bytes.Combine(
                Dds.GetDdsHeader(format.Value, originalWidth, originalHeight, mipCount, false),
                dds
            );
        while (true)
        {
            width = ToNearest(width - 1);
            height = ToNearest(height - 1);
            image = image.ScaleFit(width, height);
            dds = Bytes.Combine(dds, Squish.CompressImage(image.Bytes, width, height, null, compressionType));
            mipCount += 1;
            if (width == 1 || height == 1) break;
        }

        return Bytes.Combine(
            Dds.GetDdsHeader(format.Value, originalWidth, originalHeight, mipCount, false),
            dds
        );
    }

    public static byte[] ToGtf(SKBitmap image, SquishCompressionType type, bool noSrgb,
                               bool generateMips)
    {
        var dds = ToDds(image, type, generateMips);
        var info = new CellGcmTexture(dds, noSrgb);
        dds = dds.Skip(0x80).ToArray();
        return SerializedResource.Compress(new SerializationData(dds, info));
    }

    public static byte[] ToTex(SKBitmap image, SquishCompressionType type, bool noSrgb,
                               bool generateMips)
    {
        var dds = ToDds(image, type, generateMips);
        var texType = "\0\0\0\0";
        if (noSrgb)
            texType = "BUMP";
        dds = Bytes.Combine(dds, Encoding.ASCII.GetBytes(texType));
        return SerializedResource.Compress(new SerializationData(dds));
    }

    public static SKBitmap? FromDds(byte[] dds)
    {
        try
        {
            var pixels = DdsReader.Read(dds, DdsReader.ARGB, 0);
            int width = DdsReader.GetWidth(dds), height = DdsReader.GetHeight(dds);
            var image = new SKBitmap(width, height);
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                image.SetPixel(x, y, new SKColor((uint)pixels[x + y * width]));
            return image;
        }
        catch (Exception)
        {
            Console.Error.WriteLine("There was an error when converting DDS to SKBitmap.");
        }
        return null;
    }

    public static SKBitmap GetImageIcon(SKBitmap image, int width = 320, int height = 320)
    {
        return GetImageScaled(image, width, height);
    }

    public static SKBitmap GetImageScaled(SKBitmap image, int w, int h)
    {
        int width = image.Width, height = image.Height;
        if (width <= w && height <= h) return image.Resize(new SKSizeI(width, height), SKSamplingOptions.Default);
        return width > height ? image.Resize(new SKSizeI(w, h / 2), SKSamplingOptions.Default) : image.Resize(width < height ? new SKSizeI(w / 2, h) : new SKSizeI(w, h), SKSamplingOptions.Default);
    }

    public static SKBitmap ToImage(byte[] image)
    {
        
            return SKBitmap.FromImage(SKImage.FromEncodedData(image));
    }
}