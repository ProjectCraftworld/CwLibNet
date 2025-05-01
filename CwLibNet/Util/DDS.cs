using CwLibNet.Enums.ValueEnum;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Texture;

namespace CwLibNet.Util;

public class Dds
{
    public const uint DdsHeaderFlagsTexture = 0x00001007;
    public const uint DdsHeaderFlagsMipmap = 0x00020000;

    public const uint DdsSurfaceFlagsComplex = 0x00000008;
    public const uint DdsSurfaceFlagsTexture = 0x00001000;
    public const uint DdsSurfaceFlagsMipmap = 0x00400000;

    public const uint DdsSurfaceFlagsCubemap = 0x00000200;
    public const uint DdsSurfaceFlagsCubemapPositivex = 0x00000400;
    public const uint DdsSurfaceFlagsCubemapNegativex = 0x00000800;
    public const uint DdsSurfaceFlagsCubemapPositivey = 0x00001000;
    public const uint DdsSurfaceFlagsCubemapNegativey = 0x00002000;
    public const uint DdsSurfaceFlagsCubemapPositivez = 0x00004000;
    public const uint DdsSurfaceFlagsCubemapNegativez = 0x00008000;
    public static uint DdsSurfaceFlagsCubemapAllFaces = 0x0000FC00;
    public static uint DdsSurfaceFlagsVolume = 0x00200000;

    public const uint DdsFourcc = 0x4;
    public const uint DdsRgb = 0x40;
    public const uint DdsRgba = 0x41;
    public const uint DdsLuminance = 0x00020000;
    public const uint DdsLuminancea = 0x00020001;

    public static readonly uint[] DDSPF_DXT1 = [0x20, DdsFourcc, 0x31545844, 0, 0, 0, 0, 0];
    public static readonly uint[] DDSPF_DXT3 = [0x20, DdsFourcc, 0x33545844, 0, 0, 0, 0, 0];
    public static readonly uint[] DDSPF_DXT5 = [0x20, DdsFourcc, 0x35545844, 0, 0, 0, 0, 0];
    public static readonly uint[] DDSPF_A8_R8_G8_B8 =
    [
        0x20, DdsRgba, 0, 32, 0x00ff0000, 0x0000ff00,
        0x000000ff, 0xff000000
    ];
    public static readonly uint[] DDSPF_R5_G6_B5 =
    [
        0x20, DdsRgb, 0, 16, 0x0000f800, 0x000007e0, 0x0000001f,
        0x00000000
    ];
    public static readonly uint[] DDSPF_A4_R4_G4_B4 =
    [
        0x20, DdsRgba, 0, 16, 0x00000f00, 0x000000f0,
        0x0000000f, 0x0000f000
    ];
    public static uint[] DdspfA16B16G16R16F = [0x20, DdsFourcc, 113, 0, 0, 0, 0, 0];
    public static uint[] DdspfA8L8 = [0x20, DdsLuminancea, 0, 16, 0xff, 0, 0, 0xff00];
    public static uint[] DdspfL8 = [0x20, DdsLuminance, 0, 8, 0xff, 0, 0, 0];
    public static readonly uint[] DDSPF_B8 = [0x20, DdsLuminance, 0, 8, 0, 0, 0x000000ff, 0];
    public static readonly uint[] DDSPF_A1_R5_G5_B5 =
    [
        0x20, DdsRgba, 0, 16, 0x00007c00, 0x000003e0,
        0x0000001f, 0x00008000
    ];

    /**
     * Generates a DDS header.
     *
     * @param texture Texture header data
     * @return Generated DDS header.
     */
    public static byte[] GetDdsHeader(CellGcmTexture texture)
    {
        return GetDdsHeader(
            texture.GetFormat(),
            texture.GetWidth(),
            texture.GetHeight(),
            texture.GetMipCount(),
            texture.IsCubemap()
        );
    }

    /**
     * Generates a DDS header.
     *
     * @param format DDS format for PS3
     * @param width  Width of texture
     * @param height Height of texture
     * @param mips   Mip level count
     * @return Generated DDS header
     */
    public static byte[] GetDdsHeader(CellGcmEnumForGtf format, int width, int height, int mips,
                                      bool cubemap)
    {
        // For details on the DDS header structure, see:
        // https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header

        var header = new MemoryOutputStream(0x80);
        header.SetLittleEndian(true);

        header.Str("DDS ", 4);
        header.U32(0x7C); // dwSize
        header.U32(DdsHeaderFlagsTexture | (mips != 1 ? DdsHeaderFlagsMipmap
                                                       : 0));
        header.U32(height);
        header.U32(width);
        header.U32(0); // dwPitchOrLinearSize
        header.U32(0); // dwDepth
        header.U32(mips);
        for (var i = 0; i < 11; ++i)
            header.U32(0); // dwReserved[11]

        // DDS_PIXELFORMAT
        var pixelFormat = format switch
        {
            CellGcmEnumForGtf.B8 => DDSPF_B8,
            CellGcmEnumForGtf.A1R5G5B5 => DDSPF_A1_R5_G5_B5,
            CellGcmEnumForGtf.A4R4G4B4 => DDSPF_A4_R4_G4_B4,
            CellGcmEnumForGtf.R5G6B5 => DDSPF_R5_G6_B5,
            CellGcmEnumForGtf.A8R8G8B8 => DDSPF_A8_R8_G8_B8,
            CellGcmEnumForGtf.DXT1 => DDSPF_DXT1,
            CellGcmEnumForGtf.DXT3 => DDSPF_DXT3,
            CellGcmEnumForGtf.DXT5 => DDSPF_DXT5,
            _ => throw new NotImplementedException("Unknown or unimplemented DDS Type!")
        };
        foreach (int value in pixelFormat)
            header.U32(value);

        var caps1 = DdsSurfaceFlagsTexture;
        uint caps2 = 0;

        if (mips != 1)
        {
            caps1 |= DdsSurfaceFlagsMipmap;
            caps1 |= DdsSurfaceFlagsComplex;
        }

        if (cubemap)
        {
            caps1 |= DdsSurfaceFlagsComplex;

            caps2 |= DdsSurfaceFlagsCubemap;

            caps2 |= DdsSurfaceFlagsCubemapPositivex;
            caps2 |= DdsSurfaceFlagsCubemapNegativex;

            caps2 |= DdsSurfaceFlagsCubemapPositivey;
            caps2 |= DdsSurfaceFlagsCubemapNegativey;

            caps2 |= DdsSurfaceFlagsCubemapPositivez;
            caps2 |= DdsSurfaceFlagsCubemapNegativez;
        }

        header.U32(caps1);
        header.U32(caps2);

        for (var i = 0; i < 3; ++i)
            header.U32(0); // dwReserved

        return header.GetBuffer();
    }

    /**
     * Unswizzles pixel data
     *
     * @param pixels Pixel data
     * @param height Height of texture
     * @param width  Width of texture
     * @return Unswizzled pixels
     */
    public static int[] Unswizzle(int[] pixels, int height, int width)
    {
        // NOTE(Aidan): For original source, see:
        // https://github.com/RPCS3/rpcs3/blob/3d49976b3c0f2d2fe5fbd9dba0419c13b389c6ba/rpcs3/Emu/RSX/rsx_utils.h

        var unswizzled = new int[pixels.Length];

        var log2Width = (int) (Math.Log(width) / Math.Log(2));
        var log2Height = (int) (Math.Log(height) / Math.Log(2));

        var xMask = 0x55555555;
        var yMask = 0xAAAAAAAA;

        var limitMask = log2Width < log2Height ? log2Width : log2Height;
        limitMask = 1 << (limitMask << 1);


        xMask |= ~(limitMask - 1);
        yMask = (uint)(yMask & (limitMask - 1));

        var offsetY = 0;
        int offsetX0 = 0, yIncr = limitMask;

        for (var y = 0; y < height; ++y)
        {
            var offsetX = offsetX0;
            for (var x = 0; x < width; ++x)
            {
                unswizzled[y * width + x] = pixels[offsetY + offsetX];
                offsetX = (offsetX - xMask) & xMask;
            }
            offsetY = (int)((offsetY - yMask) & yMask);
            if (offsetY == 0) offsetX0 += yIncr;
        }

        return unswizzled;
    }
}