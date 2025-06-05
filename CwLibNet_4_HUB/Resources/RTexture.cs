using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.Extensions;
using CwLibNet.External;
using CwLibNet.Structs.Texture;
using CwLibNet.Types;
using CwLibNet.Util;
using SkiaSharp;

namespace CwLibNet.Resources;

public class RTexture
{
    private CellGcmTexture info;
    private byte[]? data;
    public bool NoSrgb;

    private SKBitmap? cached;

/*    public RTexture(SerializedResource resource)
    {
        this.process(resource);
    } */

    public RTexture(byte[]? data)
    {
        if (data == null || data.Length < 4)
        {
            // System.out.println("No data provided to Texture constructor");
            return;
        }

        var magic = (uint)Bytes.ToIntegerBE(data);
        this.data = data;
        switch (magic)
        {
            case 0xffd8ffe0:
            case 0x89504e47:
                try
                {
                    cached = SKBitmap.Decode(data);
                }
                catch (IOException)
                {
                    throw new SerializationException("An error occured reading " +
                                                     "BufferedImage");
                }

                return;
            case 0x44445320:
                cached = Images.FromDds(data);
                return;
        }

        Process(new SerializedResource(data));
    }

    private void Process(SerializedResource resource)
    {
        info = resource.GetTextureInfo();
        var type = resource.GetResourceType();
        if (type.Value != ResourceType.Texture.Value && type.Value != ResourceType.GtfTexture.Value)
            throw new SerializationException("Invalid resource provided to RTexture");
        data = resource.GetStream().GetBuffer();
        switch (resource.GetSerializationType().GetValue())
        {
            case " ":
                if (type.Value == ResourceType.Texture.Value)
                {
                    // ResourceSystem.println("Texture", "Detected COMPRESSED_TEXTURE,
                    // decompressing to DDS");
                    cached = Images.FromDds(data);

                    var footer = data.Skip(data.Length - 4).Take(4).ToArray();
                    if (Bytes.ToIntegerBE(footer) == 0x42554D50)
                        NoSrgb = true;
                }
                else
                {
                    // ResourceSystem.println("Texture", "Detected GTF_TEXTURE,
                    // generating DDS
                    // header");
                    ParseGtf();
                }
                break;
            case "s":
            case "S":
                if (!Equals(type, ResourceType.GtfTexture))
                    throw new SerializationException("Invalid ResourceType in GXT/GTF " +
                                                     "swizzled " +
                                                     "load");
                // ResourceSystem.println("Texture", "Converting GXT texture to DDS.");
                ParseGxt();
                break;
            default:
                throw new SerializationException("Invalid serialization type in RTexture " +
                                                 "resource!");
        }

        if (info != null && info.IsBumpTexture())
            NoSrgb = true;
    }

    public void ParseGxt()
    {
        UnswizzleCompressed();
        var header = GetDdsHeader();
        var gtf = data;

        var dds = new byte[gtf.Length + header.Length];
        Array.Copy(header, 0, dds, 0, header.Length);
        Array.Copy(gtf, 0, dds, header.Length, gtf.Length);

        data = dds;
        cached = GetImage();
    }

    /**
     * Assigns the properties of this class from the GTF resource header.
     */
    public void ParseGtf()
    {
        var header = GetDdsHeader();
        var gtf = data;

        var dds = new byte[gtf.Length + header.Length];
        Array.Copy(header, 0, dds, 0, header.Length);
        Array.Copy(gtf, 0, dds, header.Length, gtf.Length);

        data = dds;

        var format = info.GetFormat();
        if (format == CellGcmEnumForGtf.A8R8G8B8 || format == CellGcmEnumForGtf.B8 ||
            info.GetMethod() == SerializationType.GTF_SWIZZLED ||
            info.GetMethod() == SerializationType.GXT_SWIZZLED)
            Unswizzle();
        else cached = GetImage();
    }

    private int GetMortonNumber(int x, int y, int width, int height)
    {
        var logW = 31 - Longs.NumberOfLeadingZeros(width);
        var logH = 31 - Longs.NumberOfLeadingZeros(height);

        var d = Math.Min(logW, logH);
        var m = 0;

        for (var i = 0; i < d; ++i)
            m |= ((x & (1 << i)) << (i + 1)) | ((y & (1 << i)) << i);

        if (width < height)
            m |= (y & ~(width - 1)) << d;
        else
            m |= (x & ~(height - 1)) << d;

        return m;
    }

    /**
     * Unswizzles each DXT1/5 compressed block in a Vita GXT texture.
     */
    private void UnswizzleCompressed()
    {
        var pixels = new byte[data.Length];

        const int blockWidth = 4;
        const int blockHeight = 4;
        var bpp = 4;
        if (info.GetFormat().Equals(CellGcmEnumForGtf.DXT5))
            bpp = 8;

        var @base = 0;

        var width = Math.Max(info.GetWidth(), blockWidth);
        var height = Math.Max(info.GetHeight(), blockHeight);

        var log2Width = 1 << (31 - Longs.NumberOfLeadingZeros(width + (width - 1)));
        var log2Height = 1 << (31 - Longs.NumberOfLeadingZeros(height + (height - 1)));

        for (var i = 0; i < info.GetMipCount(); ++i)
        {
            var w = (width + blockWidth - 1) / blockWidth;
            var h = (height + blockHeight - 1) / blockHeight;
            var blockSize = bpp * blockWidth * blockHeight;

            var log2W = 1 << (31 - Longs.NumberOfLeadingZeros(w + (w - 1)));
            var log2H = 1 << (31 - Longs.NumberOfLeadingZeros(h + (h - 1)));

            var mx = GetMortonNumber(log2W - 1, 0, log2W, log2H);
            var my = GetMortonNumber(0, log2H - 1, log2W, log2H);

            var pixelSize = blockSize / 8;

            int oy = 0, tgt = @base;
            for (var y = 0; y < h; ++y)
            {
                var ox = 0;
                for (var x = 0; x < w; ++x)
                {
                    var offset = @base + (ox + oy) * pixelSize;
                    Array.Copy(data, offset, pixels, tgt, pixelSize);
                    tgt += pixelSize;
                    ox = (ox - mx) & mx;
                }
                oy = (oy - my) & my;
            }

            @base += bpp * log2Width * log2Height / 8;

            width = width > blockWidth ? width / 2 : blockWidth;
            height = height > blockHeight ? height / 2 : blockHeight;

            log2Width = log2Width > blockWidth ? log2Width / 2 : blockWidth;
            log2Height = log2Height > blockHeight ? log2Height / 2 : blockHeight;
        }

        data = pixels;
    }

    /**
     * Unswizzles the texture's pixel data
     */
    private void Unswizzle()
    {
        var pixels = DdsReader.Read(data, DdsReader.ARGB, 0);
        pixels = Dds.Unswizzle(pixels, info.GetHeight(), info.GetWidth());

        for (var i = 0; i < pixels.Length; ++i)
        {
            var pixel = pixels[i];
            pixels[i] =
                (pixel & 0xff) << 24 | (pixel & 0xff00) << 8 | (pixel & 0xff0000) >> 8 | (pixel >> 24) & 0xff;
        }

        cached = new SKBitmap(info.GetWidth(), info.GetHeight(), SKColorType.Argb4444, SKAlphaType.Opaque);
        if (cached != null)
            for (var i = 0; i < info.GetHeight(); ++i)
                for (var j = 0; j < info.GetWidth(); ++j)
                    cached.SetPixel(j, i, new SKColor((uint)pixels[j*i]));
    }

    /**
     * Convert texture to BufferedImage
     *
     * @return Converted texture
     */
    public SKBitmap? GetImage()
    {
        if (cached != null)
            return cached;
        return Images.FromDds(data);
    }

    /**
     * Scales texture to specified W/H and creates an ImageIcon.
     *
     * @param width  Desired width
     * @param height Desired height
     * @return Scaled ImageIcon of Texture
     */
    public SKBitmap GetImageIcon(int width = 320, int height = 320)
    {
        cached ??= GetImage();
        return cached != null ? cached.Resize(new SKSizeI(width, height), SKSamplingOptions.Default) : null;
    }

    /**
     * Generates a DDS header from attributes of Texture instance.
     *
     * @return Generated DDS header
     */
    public byte[] GetDdsHeader()
    {
        //System.out.println(String.format("DDS Type: %s (%s)", Bytes.toHex(this.info
        // .getFormat()
        // .getValue()), this.info.getFormat().name()));
        //System.out.println(String.format("Image Width: %spx", this.info.getWidth()));
        //System.out.println(String.format("Image Height: %spx", this.info.getHeight()));
        return null; // DDS.getDDSHeader(this.info);
    }

    public CellGcmTexture GetInfo()
    {
        return info;
    }

    public byte[]? GetData()
    {
        return data;
    }
}