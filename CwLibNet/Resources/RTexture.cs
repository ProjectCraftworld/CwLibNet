using CwLibNet.Enums;
using CwLibNet.Enums.ValueEnum;
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
    public bool noSRGB = false;

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

        uint magic = (uint)Bytes.ToIntegerBE(data);
        this.data = data;
        switch (magic)
        {
            case 0xffd8ffe0:
            case 0x89504e47:
                try
                {
                    cached = SKBitmap.Decode(data);
                }
                catch (IOException ex)
                {
                    throw new SerializationException("An error occured reading " +
                                                     "BufferedImage");
                }

                return;
            case 0x44445320:
                cached = Images.FromDds(data);
                return;
        }

        process(new SerializedResource(data));
    }

    private void process(SerializedResource resource)
    {
        info = resource.getTextureInfo();
        ResourceType type = resource.getResourceType();
        if (type.Value != ResourceType.Texture.Value && type.Value != ResourceType.GtfTexture.Value)
            throw new SerializationException("Invalid resource provided to RTexture");
        data = resource.getStream().GetBuffer();
        switch (resource.getSerializationType().GetValue())
        {
            case " ":
                if (type.Value == ResourceType.Texture.Value)
                {
                    // ResourceSystem.println("Texture", "Detected COMPRESSED_TEXTURE,
                    // decompressing to DDS");
                    cached = Images.FromDds(data);

                    byte[]? footer = data.Skip(data.Length - 4).Take(4).ToArray();
                    if (Bytes.ToIntegerBE(footer) == 0x42554D50)
                        noSRGB = true;
                }
                else
                {
                    // ResourceSystem.println("Texture", "Detected GTF_TEXTURE,
                    // generating DDS
                    // header");
                    parseGTF();
                }
                break;
            case "s":
            case "S":
                if (!Equals(type, ResourceType.GtfTexture))
                    throw new SerializationException("Invalid ResourceType in GXT/GTF " +
                                                     "swizzled " +
                                                     "load");
                // ResourceSystem.println("Texture", "Converting GXT texture to DDS.");
                parseGXT();
                break;
            default:
                throw new SerializationException("Invalid serialization type in RTexture " +
                                                 "resource!");
        }

        if (info != null && info.IsBumpTexture())
            noSRGB = true;
    }

    public void parseGXT()
    {
        unswizzleCompressed();
        byte[] header = getDDSHeader();
        byte[]? gtf = data;

        byte[]? DDS = new byte[gtf.Length + header.Length];
        Array.Copy(header, 0, DDS, 0, header.Length);
        Array.Copy(gtf, 0, DDS, header.Length, gtf.Length);

        data = DDS;
        cached = getImage();
    }

    /**
     * Assigns the properties of this class from the GTF resource header.
     */
    public void parseGTF()
    {
        byte[] header = getDDSHeader();
        byte[]? gtf = data;

        byte[]? DDS = new byte[gtf.Length + header.Length];
        Array.Copy(header, 0, DDS, 0, header.Length);
        Array.Copy(gtf, 0, DDS, header.Length, gtf.Length);

        data = DDS;

        CellGcmEnumForGtf format = info.GetFormat();
        if (format == CellGcmEnumForGtf.A8R8G8B8 || format == CellGcmEnumForGtf.B8 ||
            info.GetMethod() == SerializationType.GTF_SWIZZLED ||
            info.GetMethod() == SerializationType.GXT_SWIZZLED)
            unswizzle();
        else cached = getImage();
    }

    private int getMortonNumber(int x, int y, int width, int height)
    {
        int logW = 31 - Longs.NumberOfLeadingZeros(width);
        int logH = 31 - Longs.NumberOfLeadingZeros(height);

        int d = Math.Min(logW, logH);
        int m = 0;

        for (int i = 0; i < d; ++i)
            m |= ((x & (1 << i)) << (i + 1)) | ((y & (1 << i)) << i);

        if (width < height)
            m |= ((y & ~(width - 1)) << d);
        else
            m |= ((x & ~(height - 1)) << d);

        return m;
    }

    /**
     * Unswizzles each DXT1/5 compressed block in a Vita GXT texture.
     */
    private void unswizzleCompressed()
    {
        byte[]? pixels = new byte[data.Length];

        int blockWidth = 4, blockHeight = 4;
        int bpp = 4;
        if (info.GetFormat().Equals(CellGcmEnumForGtf.DXT5))
            bpp = 8;

        int _base = 0;

        int width = Math.Max(info.GetWidth(), blockWidth);
        int height = Math.Max(info.GetHeight(), blockHeight);

        int log2width = 1 << (31 - Longs.NumberOfLeadingZeros(width + (width - 1)));
        int log2height = 1 << (31 - Longs.NumberOfLeadingZeros(height + (height - 1)));

        for (int i = 0; i < info.GetMipCount(); ++i)
        {
            int w = ((width + blockWidth - 1) / blockWidth);
            int h = ((height + blockHeight - 1) / blockHeight);
            int blockSize = bpp * blockWidth * blockHeight;

            int log2w = 1 << (31 - Longs.NumberOfLeadingZeros(w + (w - 1)));
            int log2h = 1 << (31 - Longs.NumberOfLeadingZeros(h + (h - 1)));

            int mx = getMortonNumber(log2w - 1, 0, log2w, log2h);
            int my = getMortonNumber(0, log2h - 1, log2w, log2h);

            int pixelSize = blockSize / 8;

            int oy = 0, tgt = _base;
            for (int y = 0; y < h; ++y)
            {
                int ox = 0;
                for (int x = 0; x < w; ++x)
                {
                    int offset = _base + ((ox + oy) * pixelSize);
                    Array.Copy(data, offset, pixels, tgt, pixelSize);
                    tgt += pixelSize;
                    ox = (ox - mx) & mx;
                }
                oy = (oy - my) & my;
            }

            _base += ((bpp * log2width * log2height) / 8);

            width = width > blockWidth ? width / 2 : blockWidth;
            height = height > blockHeight ? height / 2 : blockHeight;

            log2width = log2width > blockWidth ? log2width / 2 : blockWidth;
            log2height = log2height > blockHeight ? log2height / 2 : blockHeight;
        }

        data = pixels;
    }

    /**
     * Unswizzles the texture's pixel data
     */
    private void unswizzle()
    {
        int[] pixels = DDSReader.Read(data, DDSReader.ARGB, 0);
        pixels = DDS.unswizzle(pixels, info.GetHeight(), info.GetWidth());

        for (int i = 0; i < pixels.Length; ++i)
        {
            int pixel = pixels[i];
            pixels[i] =
                (pixel & 0xff) << 24 | (pixel & 0xff00) << 8 | (pixel & 0xff0000) >> 8 | (pixel >> 24) & 0xff;
        }

        cached = new SKBitmap(info.GetWidth(), info.GetHeight(), SKColorType.Argb4444, SKAlphaType.Opaque);
        if (cached != null)
            for (int i = 0; i < info.GetHeight(); ++i)
                for (int j = 0; j < info.GetWidth(); ++j)
                    cached.SetPixel(j, i, new SKColor((uint)pixels[j*i]));
    }

    /**
     * Convert texture to BufferedImage
     *
     * @return Converted texture
     */
    public SKBitmap getImage()
    {
        if (cached != null)
            return cached;
        return Images.FromDds(data);
    }

    /**
     * Scales texture to 320x320 and creates an ImageIcon.
     *
     * @return 320x320 ImageIcon of Texture
     */
    public SKBitmap getImageIcon()
    {
        return getImageIcon(320, 320);
    }

    /**
     * Scales texture to specified W/H and creates an ImageIcon.
     *
     * @param width  Desired width
     * @param height Desired height
     * @return Scaled ImageIcon of Texture
     */
    public SKBitmap getImageIcon(int width, int height)
    {
        cached ??= getImage();
        return cached != null ? cached.Resize(new SKSizeI(width, height), SKSamplingOptions.Default) : null;
    }

    /**
     * Generates a DDS header from attributes of Texture instance.
     *
     * @return Generated DDS header
     */
    public byte[] getDDSHeader()
    {
        //System.out.println(String.format("DDS Type: %s (%s)", Bytes.toHex(this.info
        // .getFormat()
        // .getValue()), this.info.getFormat().name()));
        //System.out.println(String.format("Image Width: %spx", this.info.getWidth()));
        //System.out.println(String.format("Image Height: %spx", this.info.getHeight()));
        return null; // DDS.getDDSHeader(this.info);
    }

    public CellGcmTexture getInfo()
    {
        return info;
    }

    public byte[]? getData()
    {
        return data;
    }
}