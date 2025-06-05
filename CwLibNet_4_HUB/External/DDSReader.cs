namespace CwLibNet.External;

public sealed class DdsReader
{
    public static readonly Order ARGB = new(16, 8, 0, 24);
    public static readonly Order ABGR = new(0, 8, 16, 24);
    public static int GetHeight(byte[]? buffer)
    {
        return buffer[12] & 0xFF | (buffer[13] & 0xFF) << 8 | (buffer[14] & 0xFF) << 16 | (buffer[15] & 0xFF) << 24;
    }

    public static int GetWidth(byte[]? buffer)
    {
        return buffer[16] & 0xFF | (buffer[17] & 0xFF) << 8 | (buffer[18] & 0xFF) << 16 | (buffer[19] & 0xFF) << 24;
    }

    public static byte GetMipmap(byte[]? buffer)
    {
        return (byte)(buffer[28] & 0xFF | (buffer[29] & 0xFF) << 8 | (buffer[30] & 0xFF) << 16 | (buffer[31] & 0xFF) << 24);
    }

    public static uint GetPixelFormatFlags(byte[]? buffer)
    {
        return (uint)(buffer[80] & 0xFF | (buffer[81] & 0xFF) << 8 | (buffer[82] & 0xFF) << 16 | (buffer[83] & 0xFF) << 24);
    }

    public static uint GetFourCc(byte[]? buffer)
    {
        return (uint)((buffer[84] & 0xFF) << 24 | (buffer[85] & 0xFF) << 16 | (buffer[86] & 0xFF) << 8 | buffer[87] & 0xFF);
    }

    public static int GetBitCount(byte[]? buffer)
    {
        return buffer[88] & 0xFF | (buffer[89] & 0xFF) << 8 | (buffer[90] & 0xFF) << 16 | (buffer[91] & 0xFF) << 24;
    }

    public static int GetRedMask(byte[]? buffer)
    {
        return buffer[92] & 0xFF | (buffer[93] & 0xFF) << 8 | (buffer[94] & 0xFF) << 16 | (buffer[95] & 0xFF) << 24;
    }

    public static int GetGreenMask(byte[]? buffer)
    {
        return buffer[96] & 0xFF | (buffer[97] & 0xFF) << 8 | (buffer[98] & 0xFF) << 16 | (buffer[99] & 0xFF) << 24;
    }

    public static int GetBlueMask(byte[]? buffer)
    {
        return buffer[100] & 0xFF | (buffer[101] & 0xFF) << 8 | (buffer[102] & 0xFF) << 16 | (buffer[103] & 0xFF) << 24;
    }

    public static int GetAlphaMask(byte[]? buffer)
    {
        return buffer[104] & 0xFF | (buffer[105] & 0xFF) << 8 | (buffer[106] & 0xFF) << 16 | (buffer[107] & 0xFF) << 24;
    }

    public static int[] Read(byte[]? buffer, Order order, int mipmapLevel)
    {
        var width = GetWidth(buffer);
        var height = GetHeight(buffer);
        int mipmap = GetMipmap(buffer);
        var type = GetType(buffer);
        if (type == 0)
            return null;
        var offset = 128;
        if (mipmapLevel > 0 && mipmapLevel < mipmap)
        {
            for (var i = 0; i < mipmapLevel; i++)
            {
                switch (type)
                {
                    case 1146639409:
                        offset += 8 * (width + 3) / 4 * (height + 3) / 4;
                        break;
                    case 1146639410:
                    case 1146639411:
                    case 1146639412:
                    case 1146639413:
                        offset += 16 * (width + 3) / 4 * (height + 3) / 4;
                        break;
                    case 65538:
                    case 65539:
                    case 65540:
                    case 131074:
                    case 131076:
                    case 196610:
                    case 196612:
                    case 262146:
                    case 262148:
                    case 327682:
                        offset += ((int)type & 0xFF) * width * height;
                        break;
                    case 0xFF:
                    case 0xFF00:
                    case 0xFF0000:
                    case 0xFF000000:
                        offset += width * height;
                        break;
                }

                width /= 2;
                height /= 2;
            }

            if (width <= 0)
                width = 1;
            if (height <= 0)
                height = 1;
        }

        int[] pixels = null;
        switch (type)
        {
            case 16711680:
                pixels = ReadL8(width, height, offset, buffer, order);
                break;
            case 1146639409:
                pixels = DecodeDxt1(width, height, offset, buffer, order);
                break;
            case 1146639410:
                pixels = DecodeDxt2(width, height, offset, buffer, order);
                break;
            case 1146639411:
                pixels = DecodeDxt3(width, height, offset, buffer, order);
                break;
            case 1146639412:
                pixels = DecodeDxt4(width, height, offset, buffer, order);
                break;
            case 1146639413:
                pixels = DecodeDxt5(width, height, offset, buffer, order);
                break;
            case 65538:
                pixels = ReadA1R5G5B5(width, height, offset, buffer, order);
                break;
            case 131074:
                pixels = ReadX1R5G5B5(width, height, offset, buffer, order);
                break;
            case 196610:
                pixels = ReadA4R4G4B4(width, height, offset, buffer, order);
                break;
            case 262146:
                pixels = ReadX4R4G4B4(width, height, offset, buffer, order);
                break;
            case 327682:
                pixels = ReadR5G6B5(width, height, offset, buffer, order);
                break;
            case 65539:
                pixels = ReadR8G8B8(width, height, offset, buffer, order);
                break;
            case 65540:
                pixels = ReadA8B8G8R8(width, height, offset, buffer, order);
                break;
            case 131076:
                pixels = ReadX8B8G8R8(width, height, offset, buffer, order);
                break;
            case 196612:
                pixels = ReadA8R8G8B8(width, height, offset, buffer, order);
                break;
            case 262148:
                pixels = ReadX8R8G8B8(width, height, offset, buffer, order);
                break;
        }

        return pixels;
    }

    public static uint GetType(byte[]? buffer)
    {
        uint type = 0;
        var flags = GetPixelFormatFlags(buffer);
        if ((flags & 0x4) != 0)
        {
            type = GetFourCc(buffer);
        }
        else
        {
            var bitCount = GetBitCount(buffer);
            var redMask = GetRedMask(buffer);
            var greenMask = GetGreenMask(buffer);
            var blueMask = GetBlueMask(buffer);
            var alphaMask = (flags & 0x1) != 0 ? GetAlphaMask(buffer) : 0;
            switch (bitCount)
            {
                case 8:
                {
                    if (redMask != 0)
                        type = 0x00FF0000;
                    else if (greenMask != 0)
                        type = 0x0000FF00;
                    else if (blueMask != 0)
                        type = 0x000000FF;
                    else if (alphaMask != 0)
                        type = 0xFF000000;
                    break;
                }
                case 16:
                {
                    if (redMask == A1R5G5B5Masks[0] && greenMask == A1R5G5B5Masks[1] && blueMask == A1R5G5B5Masks[2] && alphaMask == A1R5G5B5Masks[3])
                    {
                        type = 65538;
                    }
                    else if (redMask == X1R5G5B5Masks[0] && greenMask == X1R5G5B5Masks[1] && blueMask == X1R5G5B5Masks[2] && alphaMask == X1R5G5B5Masks[3])
                    {
                        type = 131074;
                    }
                    else if (redMask == A4R4G4B4Masks[0] && greenMask == A4R4G4B4Masks[1] && blueMask == A4R4G4B4Masks[2] && alphaMask == A4R4G4B4Masks[3])
                    {
                        type = 196610;
                    }
                    else if (redMask == X4R4G4B4Masks[0] && greenMask == X4R4G4B4Masks[1] && blueMask == X4R4G4B4Masks[2] && alphaMask == X4R4G4B4Masks[3])
                    {
                        type = 262146;
                    }
                    else if (redMask == R5G6B5Masks[0] && greenMask == R5G6B5Masks[1] && blueMask == R5G6B5Masks[2] && alphaMask == R5G6B5Masks[3])
                    {
                        type = 327682;
                    }

                    break;
                }
                case 24:
                {
                    if (redMask == R8G8B8Masks[0] && greenMask == R8G8B8Masks[1] && blueMask == R8G8B8Masks[2] && alphaMask == R8G8B8Masks[3])
                        type = 65539;
                    break;
                }
                case 32:
                {
                    if (redMask == A8B8G8R8Masks[0] && greenMask == A8B8G8R8Masks[1] && blueMask == A8B8G8R8Masks[2] && alphaMask == A8B8G8R8Masks[3])
                    {
                        type = 65540;
                    }
                    else if (redMask == X8B8G8R8Masks[0] && greenMask == X8B8G8R8Masks[1] && blueMask == X8B8G8R8Masks[2] && alphaMask == X8B8G8R8Masks[3])
                    {
                        type = 131076;
                    }
                    else if (redMask == A8R8G8B8Masks[0] && greenMask == A8R8G8B8Masks[1] && blueMask == A8R8G8B8Masks[2] && alphaMask == A8R8G8B8Masks[3])
                    {
                        type = 196612;
                    }
                    else if (redMask == X8R8G8B8Masks[0] && greenMask == X8R8G8B8Masks[1] && blueMask == X8R8G8B8Masks[2] && alphaMask == X8R8G8B8Masks[3])
                    {
                        type = 262148;
                    }

                    break;
                }
            }
        }

        return type;
    }

    private static int[] DecodeDxt1(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var pixels = new int[width * height];
        var index = offset;
        var w = (width + 3) / 4;
        var h = (height + 3) / 4;
        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                var c0 = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
                index += 2;
                var c1 = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
                index += 2;
                for (var k = 0; k < 4 && 4 * i + k < height; k++)
                {
                    var t0 = buffer[index] & 0x3;
                    var t1 = (buffer[index] & 0xC) >> 2;
                    var t2 = (buffer[index] & 0x30) >> 4;
                    var t3 = (buffer[index++] & 0xC0) >> 6;
                    pixels[4 * width * i + 4 * j + width * k] = GetDxtColor(c0, c1, 255, t0, order);
                    if (4 * j + 1 < width)
                    {
                        pixels[4 * width * i + 4 * j + width * k + 1] = GetDxtColor(c0, c1, 255, t1, order);
                        if (4 * j + 2 < width)
                        {
                            pixels[4 * width * i + 4 * j + width * k + 2] = GetDxtColor(c0, c1, 255, t2, order);
                            if (4 * j + 3 < width)
                                pixels[4 * width * i + 4 * j + width * k + 3] = GetDxtColor(c0, c1, 255, t3, order);
                        }
                    }
                }
            }
        }

        return pixels;
    }

    private static int[] DecodeDxt2(int width, int height, int offset, byte[]? buffer, Order order)
    {
        return DecodeDxt3(width, height, offset, buffer, order);
    }

    private static int[] DecodeDxt3(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var w = (width + 3) / 4;
        var h = (height + 3) / 4;
        var pixels = new int[width * height];
        var alphaTable = new int[16];
        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                for (var k = 0; k < 4; k++)
                {
                    var a0 = buffer[index++] & 0xFF;
                    var a1 = buffer[index++] & 0xFF;
                    alphaTable[4 * k] = 17 * ((a0 & 0xF0) >> 4);
                    alphaTable[4 * k + 1] = 17 * (a0 & 0xF);
                    alphaTable[4 * k + 2] = 17 * ((a1 & 0xF0) >> 4);
                    alphaTable[4 * k + 3] = 17 * (a1 & 0xF);
                }

                var c0 = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
                index += 2;
                var c1 = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
                index += 2;
                for (var m = 0; m < 4 && 4 * i + m < height; m++)
                {
                    var t0 = buffer[index] & 0x3;
                    var t1 = (buffer[index] & 0xC) >> 2;
                    var t2 = (buffer[index] & 0x30) >> 4;
                    var t3 = (buffer[index++] & 0xC0) >> 6;
                    pixels[4 * width * i + 4 * j + width * m] = GetDxtColor(c0, c1, alphaTable[4 * m], t0, order);
                    if (4 * j + 1 < width)
                    {
                        pixels[4 * width * i + 4 * j + width * m + 1] = GetDxtColor(c0, c1, alphaTable[4 * m + 1], t1, order);
                        if (4 * j + 2 < width)
                        {
                            pixels[4 * width * i + 4 * j + width * m + 2] = GetDxtColor(c0, c1, alphaTable[4 * m + 2], t2, order);
                            if (4 * j + 3 < width)
                                pixels[4 * width * i + 4 * j + width * m + 3] = GetDxtColor(c0, c1, alphaTable[4 * m + 3], t3, order);
                        }
                    }
                }
            }
        }

        return pixels;
    }

    private static int[] DecodeDxt4(int width, int height, int offset, byte[]? buffer, Order order)
    {
        return DecodeDxt5(width, height, offset, buffer, order);
    }

    private static int[] DecodeDxt5(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var w = (width + 3) / 4;
        var h = (height + 3) / 4;
        var pixels = new int[width * height];
        var alphaTable = new int[16];
        for (var i = 0; i < h; i++)
        {
            for (var j = 0; j < w; j++)
            {
                var a0 = buffer[index++] & 0xFF;
                var a1 = buffer[index++] & 0xFF;
                var b0 = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8 | (buffer[index + 2] & 0xFF) << 16;
                index += 3;
                var b1 = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8 | (buffer[index + 2] & 0xFF) << 16;
                index += 3;
                alphaTable[0] = b0 & 0x7;
                alphaTable[1] = b0 >> 3 & 0x7;
                alphaTable[2] = b0 >> 6 & 0x7;
                alphaTable[3] = b0 >> 9 & 0x7;
                alphaTable[4] = b0 >> 12 & 0x7;
                alphaTable[5] = b0 >> 15 & 0x7;
                alphaTable[6] = b0 >> 18 & 0x7;
                alphaTable[7] = b0 >> 21 & 0x7;
                alphaTable[8] = b1 & 0x7;
                alphaTable[9] = b1 >> 3 & 0x7;
                alphaTable[10] = b1 >> 6 & 0x7;
                alphaTable[11] = b1 >> 9 & 0x7;
                alphaTable[12] = b1 >> 12 & 0x7;
                alphaTable[13] = b1 >> 15 & 0x7;
                alphaTable[14] = b1 >> 18 & 0x7;
                alphaTable[15] = b1 >> 21 & 0x7;
                var c0 = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
                index += 2;
                var c1 = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
                index += 2;
                for (var k = 0; k < 4 && 4 * i + k < height; k++)
                {
                    var t0 = buffer[index] & 0x3;
                    var t1 = (buffer[index] & 0xC) >> 2;
                    var t2 = (buffer[index] & 0x30) >> 4;
                    var t3 = (buffer[index++] & 0xC0) >> 6;
                    pixels[4 * width * i + 4 * j + width * k] = GetDxtColor(c0, c1, GetDxt5Alpha(a0, a1, alphaTable[4 * k]), t0, order);
                    if (4 * j + 1 < width)
                    {
                        pixels[4 * width * i + 4 * j + width * k + 1] = GetDxtColor(c0, c1, GetDxt5Alpha(a0, a1, alphaTable[4 * k + 1]), t1, order);
                        if (4 * j + 2 < width)
                        {
                            pixels[4 * width * i + 4 * j + width * k + 2] = GetDxtColor(c0, c1, GetDxt5Alpha(a0, a1, alphaTable[4 * k + 2]), t2, order);
                            if (4 * j + 3 < width)
                                pixels[4 * width * i + 4 * j + width * k + 3] = GetDxtColor(c0, c1, GetDxt5Alpha(a0, a1, alphaTable[4 * k + 3]), t3, order);
                        }
                    }
                }
            }
        }

        return pixels;
    }

    private static int[] ReadA1R5G5B5(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var rgba = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
            index += 2;
            var r = Bit5[(rgba & A1R5G5B5Masks[0]) >> 10];
            var g = Bit5[(rgba & A1R5G5B5Masks[1]) >> 5];
            var b = Bit5[rgba & A1R5G5B5Masks[2]];
            var a = 255 * ((rgba & A1R5G5B5Masks[3]) >> 15);
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadX1R5G5B5(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var rgba = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
            index += 2;
            var r = Bit5[(rgba & X1R5G5B5Masks[0]) >> 10];
            var g = Bit5[(rgba & X1R5G5B5Masks[1]) >> 5];
            var b = Bit5[rgba & X1R5G5B5Masks[2]];
            var a = 255;
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadA4R4G4B4(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var rgba = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
            index += 2;
            var r = 17 * ((rgba & A4R4G4B4Masks[0]) >> 8);
            var g = 17 * ((rgba & A4R4G4B4Masks[1]) >> 4);
            var b = 17 * (rgba & A4R4G4B4Masks[2]);
            var a = 17 * ((rgba & A4R4G4B4Masks[3]) >> 12);
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadX4R4G4B4(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var rgba = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
            index += 2;
            var r = 17 * ((rgba & A4R4G4B4Masks[0]) >> 8);
            var g = 17 * ((rgba & A4R4G4B4Masks[1]) >> 4);
            var b = 17 * (rgba & A4R4G4B4Masks[2]);
            var a = 255;
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadR5G6B5(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var rgba = buffer[index] & 0xFF | (buffer[index + 1] & 0xFF) << 8;
            index += 2;
            var r = Bit5[(rgba & R5G6B5Masks[0]) >> 11];
            var g = Bit6[(rgba & R5G6B5Masks[1]) >> 5];
            var b = Bit5[rgba & R5G6B5Masks[2]];
            var a = 255;
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadL8(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; ++i)
        {
            var r = buffer[index] & 0xFF;
            index++;
            pixels[i] = 255 << order.AlphaShift | r << order.RedShift | r << order.GreenShift | r << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadR8G8B8(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var b = buffer[index++] & 0xFF;
            var g = buffer[index++] & 0xFF;
            var r = buffer[index++] & 0xFF;
            var a = 255;
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadA8B8G8R8(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var r = buffer[index++] & 0xFF;
            var g = buffer[index++] & 0xFF;
            var b = buffer[index++] & 0xFF;
            var a = buffer[index++] & 0xFF;
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadX8B8G8R8(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var r = buffer[index++] & 0xFF;
            var g = buffer[index++] & 0xFF;
            var b = buffer[index++] & 0xFF;
            var a = 255;
            index++;
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadA8R8G8B8(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var b = buffer[index++] & 0xFF;
            var g = buffer[index++] & 0xFF;
            var r = buffer[index++] & 0xFF;
            var a = buffer[index++] & 0xFF;
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int[] ReadX8R8G8B8(int width, int height, int offset, byte[]? buffer, Order order)
    {
        var index = offset;
        var pixels = new int[width * height];
        for (var i = 0; i < height * width; i++)
        {
            var b = buffer[index++] & 0xFF;
            var g = buffer[index++] & 0xFF;
            var r = buffer[index++] & 0xFF;
            var a = 255;
            index++;
            pixels[i] = a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
        }

        return pixels;
    }

    private static int GetDxtColor(int c0, int c1, int a, int t, Order order)
    {
        switch (t)
        {
            case 0:
                return GetDxtColor1(c0, a, order);
            case 1:
                return GetDxtColor1(c1, a, order);
            case 2:
                return c0 > c1 ? GetDXTColor2_1(c0, c1, a, order) : GetDXTColor1_1(c0, c1, a, order);
            case 3:
                return c0 > c1 ? GetDXTColor2_1(c1, c0, a, order) : 0;
        }

        return 0;
    }

    private static int GetDXTColor2_1(int c0, int c1, int a, Order order)
    {
        var r = (2 * Bit5[(c0 & 0xFC00) >> 11] + Bit5[(c1 & 0xFC00) >> 11]) / 3;
        var g = (2 * Bit6[(c0 & 0x7E0) >> 5] + Bit6[(c1 & 0x7E0) >> 5]) / 3;
        var b = (2 * Bit5[c0 & 0x1F] + Bit5[c1 & 0x1F]) / 3;
        return a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
    }

    private static int GetDXTColor1_1(int c0, int c1, int a, Order order)
    {
        var r = (Bit5[(c0 & 0xFC00) >> 11] + Bit5[(c1 & 0xFC00) >> 11]) / 2;
        var g = (Bit6[(c0 & 0x7E0) >> 5] + Bit6[(c1 & 0x7E0) >> 5]) / 2;
        var b = (Bit5[c0 & 0x1F] + Bit5[c1 & 0x1F]) / 2;
        return a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
    }

    private static int GetDxtColor1(int c, int a, Order order)
    {
        var r = Bit5[(c & 0xFC00) >> 11];
        var g = Bit6[(c & 0x7E0) >> 5];
        var b = Bit5[c & 0x1F];
        return a << order.AlphaShift | r << order.RedShift | g << order.GreenShift | b << order.BlueShift;
    }

    private static int GetDxt5Alpha(int a0, int a1, int t)
    {
        if (a0 > a1)
        {
            switch (t)
            {
                case 0:
                    return a0;
                case 1:
                    return a1;
                case 2:
                    return (6 * a0 + a1) / 7;
                case 3:
                    return (5 * a0 + 2 * a1) / 7;
                case 4:
                    return (4 * a0 + 3 * a1) / 7;
                case 5:
                    return (3 * a0 + 4 * a1) / 7;
                case 6:
                    return (2 * a0 + 5 * a1) / 7;
                case 7:
                    return (a0 + 6 * a1) / 7;
            }
        }
        else
        {
            switch (t)
            {
                case 0:
                    return a0;
                case 1:
                    return a1;
                case 2:
                    return (4 * a0 + a1) / 5;
                case 3:
                    return (3 * a0 + 2 * a1) / 5;
                case 4:
                    return (2 * a0 + 3 * a1) / 5;
                case 5:
                    return (a0 + 4 * a1) / 5;
                case 6:
                    return 0;
                case 7:
                    return 255;
            }
        }

        return 0;
    }

    private static readonly int[] A1R5G5B5Masks =
    [
        31744,
        992,
        31,
        32768
    ];
    private static readonly int[] X1R5G5B5Masks =
    [
        31744,
        992,
        31,
        0
    ];
    private static readonly int[] A4R4G4B4Masks =
    [
        3840,
        240,
        15,
        61440
    ];
    private static readonly int[] X4R4G4B4Masks =
    [
        3840,
        240,
        15,
        0
    ];
    private static readonly int[] R5G6B5Masks =
    [
        63488,
        2016,
        31,
        0
    ];
    private static readonly int[] R8G8B8Masks =
    [
        16711680,
        65280,
        255,
        0
    ];
    private static readonly int[] A8B8G8R8Masks =
    [
        255,
        65280,
        16711680,
        -16777216
    ];
    private static readonly int[] X8B8G8R8Masks =
    [
        255,
        65280,
        16711680,
        0
    ];
    private static readonly int[] A8R8G8B8Masks =
    [
        16711680,
        65280,
        255,
        -16777216
    ];
    private static readonly int[] X8R8G8B8Masks =
    [
        16711680,
        65280,
        255,
        0
    ];
    private static readonly int[] Bit5 =
    [
        0,
        8,
        16,
        25,
        33,
        41,
        49,
        58,
        66,
        74,
        82,
        90,
        99,
        107,
        115,
        123,
        132,
        140,
        148,
        156,
        165,
        173,
        181,
        189,
        197,
        206,
        214,
        222,
        230,
        239,
        247,
        255
    ];
    private static readonly int[] Bit6 =
    [
        0,
        4,
        8,
        12,
        16,
        20,
        24,
        28,
        32,
        36,
        40,
        45,
        49,
        53,
        57,
        61,
        65,
        69,
        73,
        77,
        81,
        85,
        89,
        93,
        97,
        101,
        105,
        109,
        113,
        117,
        121,
        125,
        130,
        134,
        138,
        142,
        146,
        150,
        154,
        158,
        162,
        166,
        170,
        174,
        178,
        182,
        186,
        190,
        194,
        198,
        202,
        206,
        210,
        215,
        219,
        223,
        227,
        231,
        235,
        239,
        243,
        247,
        251,
        255
    ];
    public sealed class Order
    {
        public int RedShift;
        public int GreenShift;
        public int BlueShift;
        public int AlphaShift;
        public Order(int redShift, int greenShift, int blueShift, int alphaShift)
        {
            RedShift = redShift;
            GreenShift = greenShift;
            BlueShift = blueShift;
            AlphaShift = alphaShift;
        }
    }
}