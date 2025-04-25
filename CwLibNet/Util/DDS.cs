using CwLibNet.Enums.ValueEnum;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Texture;

namespace CwLibNet.Util;

public class DDS
{
    public static uint DDS_HEADER_FLAGS_TEXTURE = 0x00001007;
    public static uint DDS_HEADER_FLAGS_MIPMAP = 0x00020000;

    public static uint DDS_SURFACE_FLAGS_COMPLEX = 0x00000008;
    public static uint DDS_SURFACE_FLAGS_TEXTURE = 0x00001000;
    public static uint DDS_SURFACE_FLAGS_MIPMAP = 0x00400000;

    public static uint DDS_SURFACE_FLAGS_CUBEMAP = 0x00000200;
    public static uint DDS_SURFACE_FLAGS_CUBEMAP_POSITIVEX = 0x00000400;
    public static uint DDS_SURFACE_FLAGS_CUBEMAP_NEGATIVEX = 0x00000800;
    public static uint DDS_SURFACE_FLAGS_CUBEMAP_POSITIVEY = 0x00001000;
    public static uint DDS_SURFACE_FLAGS_CUBEMAP_NEGATIVEY = 0x00002000;
    public static uint DDS_SURFACE_FLAGS_CUBEMAP_POSITIVEZ = 0x00004000;
    public static uint DDS_SURFACE_FLAGS_CUBEMAP_NEGATIVEZ = 0x00008000;
    public static uint DDS_SURFACE_FLAGS_CUBEMAP_ALL_FACES = 0x0000FC00;
    public static uint DDS_SURFACE_FLAGS_VOLUME = 0x00200000;

    public static uint DDS_FOURCC = 0x4;
    public static uint DDS_RGB = 0x40;
    public static uint DDS_RGBA = 0x41;
    public static uint DDS_LUMINANCE = 0x00020000;
    public static uint DDS_LUMINANCEA = 0x00020001;

    public static uint[] DDSPF_DXT1 = { 0x20, DDS_FOURCC, 0x31545844, 0, 0, 0, 0, 0 };
    public static uint[] DDSPF_DXT3 = { 0x20, DDS_FOURCC, 0x33545844, 0, 0, 0, 0, 0 };
    public static uint[] DDSPF_DXT5 = { 0x20, DDS_FOURCC, 0x35545844, 0, 0, 0, 0, 0 };
    public static uint[] DDSPF_A8R8G8B8 = { 0x20, DDS_RGBA, 0, 32, 0x00ff0000, 0x0000ff00,
        0x000000ff, 0xff000000 };
    public static uint[] DDSPF_R5G6B5 = { 0x20, DDS_RGB, 0, 16, 0x0000f800, 0x000007e0, 0x0000001f,
        0x00000000 };
    public static uint[] DDSPF_A4R4G4B4 = { 0x20, DDS_RGBA, 0, 16, 0x00000f00, 0x000000f0,
        0x0000000f, 0x0000f000 };
    public static uint[] DDSPF_A16B16G16R16F = { 0x20, DDS_FOURCC, 113, 0, 0, 0, 0, 0 };
    public static uint[] DDSPF_A8L8 = { 0x20, DDS_LUMINANCEA, 0, 16, 0xff, 0, 0, 0xff00 };
    public static uint[] DDSPF_L8 = { 0x20, DDS_LUMINANCE, 0, 8, 0xff, 0, 0, 0 };
    public static uint[] DDSPF_B8 = { 0x20, DDS_LUMINANCE, 0, 8, 0, 0, 0x000000ff, 0 };
    public static uint[] DDSPF_A1R5G5B5 = { 0x20, DDS_RGBA, 0, 16, 0x00007c00, 0x000003e0,
        0x0000001f, 0x00008000 };

    /**
     * Generates a DDS header.
     *
     * @param texture Texture header data
     * @return Generated DDS header.
     */
    public static byte[] getDDSHeader(CellGcmTexture texture)
    {
        return getDDSHeader(
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
    public static byte[] getDDSHeader(CellGcmEnumForGtf format, int width, int height, int mips,
                                      bool cubemap)
    {
        // For details on the DDS header structure, see:
        // https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header

        MemoryOutputStream header = new MemoryOutputStream(0x80);
        header.SetLittleEndian(true);

        header.Str("DDS ", 4);
        header.U32(0x7C); // dwSize
        header.U32(DDS_HEADER_FLAGS_TEXTURE | ((mips != 1) ? DDS_HEADER_FLAGS_MIPMAP
                                                       : 0));
        header.U32(height);
        header.U32(width);
        header.U32(0); // dwPitchOrLinearSize
        header.U32(0); // dwDepth
        header.U32(mips);
        for (int i = 0; i < 11; ++i)
            header.U32(0); // dwReserved[11]

        // DDS_PIXELFORMAT
        uint[] pixelFormat = format switch
        {
            CellGcmEnumForGtf.B8 => DDSPF_B8,
            CellGcmEnumForGtf.A1R5G5B5 => DDSPF_A1R5G5B5,
            CellGcmEnumForGtf.A4R4G4B4 => DDSPF_A4R4G4B4,
            CellGcmEnumForGtf.R5G6B5 => DDSPF_R5G6B5,
            CellGcmEnumForGtf.A8R8G8B8 => DDSPF_A8R8G8B8,
            CellGcmEnumForGtf.DXT1 => DDSPF_DXT1,
            CellGcmEnumForGtf.DXT3 => DDSPF_DXT3,
            CellGcmEnumForGtf.DXT5 => DDSPF_DXT5,
            _ => throw new NotImplementedException("Unknown or unimplemented DDS Type!")
        };
        foreach (int value in pixelFormat)
            header.U32(value);

        uint caps1 = DDS_SURFACE_FLAGS_TEXTURE;
        uint caps2 = 0;

        if (mips != 1)
        {
            caps1 |= DDS_SURFACE_FLAGS_MIPMAP;
            caps1 |= DDS_SURFACE_FLAGS_COMPLEX;
        }

        if (cubemap)
        {
            caps1 |= DDS_SURFACE_FLAGS_COMPLEX;

            caps2 |= DDS_SURFACE_FLAGS_CUBEMAP;

            caps2 |= DDS_SURFACE_FLAGS_CUBEMAP_POSITIVEX;
            caps2 |= DDS_SURFACE_FLAGS_CUBEMAP_NEGATIVEX;

            caps2 |= DDS_SURFACE_FLAGS_CUBEMAP_POSITIVEY;
            caps2 |= DDS_SURFACE_FLAGS_CUBEMAP_NEGATIVEY;

            caps2 |= DDS_SURFACE_FLAGS_CUBEMAP_POSITIVEZ;
            caps2 |= DDS_SURFACE_FLAGS_CUBEMAP_NEGATIVEZ;
        }

        header.U32(caps1);
        header.U32(caps2);

        for (int i = 0; i < 3; ++i)
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
    public static int[] unswizzle(int[] pixels, int height, int width)
    {
        // NOTE(Aidan): For original source, see:
        // https://github.com/RPCS3/rpcs3/blob/3d49976b3c0f2d2fe5fbd9dba0419c13b389c6ba/rpcs3/Emu/RSX/rsx_utils.h

        int[] unswizzled = new int[pixels.Length];

        int log2width = (int) (Math.Log(width) / Math.Log(2));
        int log2height = (int) (Math.Log(height) / Math.Log(2));

        int xMask = 0x55555555;
        uint yMask = 0xAAAAAAAA;

        int limitMask = (log2width < log2height) ? log2width : log2height;
        limitMask = 1 << (limitMask << 1);


        xMask = (xMask | ~(limitMask - 1));
        yMask = (uint)(yMask & (limitMask - 1));

        int offsetY = 0, offsetX = 0, offsetX0 = 0, yIncr = limitMask, adv = width;

        for (int y = 0; y < height; ++y)
        {
            offsetX = offsetX0;
            for (int x = 0; x < width; ++x)
            {
                unswizzled[(y * adv) + x] = pixels[offsetY + offsetX];
                offsetX = (offsetX - xMask) & xMask;
            }
            offsetY = (int)((offsetY - yMask) & yMask);
            if (offsetY == 0) offsetX0 += yIncr;
        }

        return unswizzled;
    }
}