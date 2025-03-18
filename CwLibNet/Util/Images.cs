using System.Diagnostics;
using System.Net.Mime;
using System.Text;
using CwLibNet.Enums;
using CwLibNet.Enums.ValueEnum;
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
        SKColor c = image.GetPixel(0, 0);
        bool isNormal = !(c.Red == c.Green && c.Red == c.Blue);
        if (isNormal)
        {
            for (int x = 0; x < image.Width; ++x)
            {
                for (int y = 0; y < image.Height; ++y)
                {
                    c = image.GetPixel(x, y);
                    byte green = c.Green;
                    byte red = c.Red;
                    image.SetPixel(x, y, new SKColor(
                        green,
                        green,
                        green,
                        red
                    ));
                }
            }
        }
        return image;
    }

    private static byte[] GetRgba(SKBitmap image)
    {
        List<uint> argb = [];
        for (int x = 0; x < image.Height; ++x)
            for (int y = 0; y < image.Width; ++y)
                argb.Add(image.GetPixel(x, y).ToColor());
        //image.getRGB(0, 0, image.getWidth(), image.getHeight(), null, 0,
//            image.getWidth());
        byte[] rgba = new byte[argb.Count * 4];
        for (int i = 0; i < argb.Count; ++i)
        {
            rgba[(4 * i)] = (byte) ((argb[i] >> 16) & 0xff);
            rgba[(4 * i) + 1] = (byte) ((argb[i] >> 8) & 0xff);
            rgba[(4 * i) + 2] = (byte) ((argb[i]) & 0xff);
            rgba[(4 * i) + 3] = (byte) ((argb[i] >> 24) & 0xff);
        }
        return rgba;
    }


    private static byte[] ToDds(SKBitmap image, SquishCompressionType type,
                                bool generateMips)
    {
        int width = ToNearest(image.Width);
        int height = ToNearest(image.Height);

        int originalWidth = width, originalHeight = height;

        CellGcmEnumForGtf? format = null;
        Squish.CompressionType _type = null;
        switch (type)
        {
            case SquishCompressionType.DXT1:
                format = CellGcmEnumForGtf.DXT1;
                _type = Squish.CompressionType.DXT1;
                break;
            case SquishCompressionType.DXT3:
                format = CellGcmEnumForGtf.DXT3;
                _type = Squish.CompressionType.DXT3;
                break;
            case SquishCompressionType.DXT5:
                format = CellGcmEnumForGtf.DXT5;
                _type = Squish.CompressionType.DXT5;
                break;
        }

        if (image.Width != width || image.Height != height)
            image = image.Resize(new SKSizeI(width, height), SKSamplingOptions.Default);
        byte[] dds = Util.Squish.CompressImage(image.Bytes, width, height, null, _type);
        int mipCount = 1;
        if (generateMips)
        {
            while (true)
            {
                width = ToNearest(width - 1);
                height = ToNearest(height - 1);
                image = image.ScaleFit(width, height);
                dds = Bytes.Combine(dds, Util.Squish.CompressImage(image.Bytes, width, height, null, _type));
                mipCount += 1;
                if (width == 1 || height == 1) break;
            }
        }

        return Bytes.Combine(
            DDS.getDDSHeader(format.Value, originalWidth, originalHeight, mipCount, false),
            dds
        );
    }

    public static byte[] ToGtf(SKBitmap image, SquishCompressionType type, bool noSrgb,
                               bool generateMips)
    {
        byte[] dds = ToDds(image, type, generateMips);
        CellGcmTexture info = new CellGcmTexture(dds, noSrgb);
        dds = dds.Skip(0x80).ToArray();
        return SerializedResource.compress(new SerializationData(dds, info));
    }

    public static byte[] ToTex(SKBitmap image, SquishCompressionType type, bool noSrgb,
                               bool generateMips)
    {
        byte[] dds = ToDds(image, type, generateMips);
        String texType = "\0\0\0\0";
        if (noSrgb)
            texType = "BUMP";
        dds = Bytes.Combine(dds, Encoding.ASCII.GetBytes(texType));
        return SerializedResource.compress(new SerializationData(dds));
    }

    public static SKBitmap FromDds(byte[] dds)
    {
        try
        {
            int[] pixels = DDSReader.Read(dds, DDSReader.ARGB, 0);
            int width = DDSReader.GetWidth(dds), height = DDSReader.GetHeight(dds);
            SKBitmap image = new SKBitmap(width, height);
            if (image != null)
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        image.SetPixel(x, y, new SKColor((uint)pixels[x + y * width]));
                return image;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("There was an error when converting DDS to BufferedImage.");
        }
        return null;
    }

    public static SKBitmap GetImageIcon(SKBitmap image)
    {
        return GetImageIcon(image, 320, 320);
    }

    public static SKBitmap GetImageIcon(SKBitmap image, int width, int height)
    {
        return GetImageScaled(image, width, height);
    }

    public static SKBitmap GetImageScaled(SKBitmap image, int w, int h)
    {
        int width = image.Width, height = image.Height;
        if (width > w || height > h)
        {
            if (width > height)
                return image.Resize(new SKSizeI(w, h / 2), SKSamplingOptions.Default);
            if (width < height)
                return image.Resize(new SKSizeI(w / 2, h), SKSamplingOptions.Default);
            return image.Resize(new SKSizeI(w, h), SKSamplingOptions.Default);
        }
        return image.Resize(new SKSizeI(width, height), SKSamplingOptions.Default);
    }

    public static SKBitmap ToImage(byte[] image)
    {
        
            return SKBitmap.FromImage(SKImage.FromEncodedData(image));
    }
}