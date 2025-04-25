using SkiaSharp;
using CwLibNet.Enums;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Font;
using CwLibNet.Types;
using CwLibNet.Util;

namespace CwLibNet.Resources;

public class RFontFace
{
    public int Revision;
    public short[] GlyphIndex = new short[128];
    public int[]? GlyphPageUsed = new int[16];
    public GlyphInfo[]? Glyphs;
    public byte[]? Data;
    public bool IsCompressed;

    public RFontFace(byte[]? data)
    {
        MemoryInputStream stream = new MemoryInputStream(data, CompressionFlags.USE_NO_COMPRESSION);
            
        if (stream.I32() != 0x464E5462) 
            throw new Exception("Not a supported RFontFace file.");

        Revision = stream.I32();
        stream.I32();
        stream.I32();

        Serializer serializer = new Serializer(stream, new Revision(Revision));
        GlyphIndex = serializer.Shortarray(GlyphIndex);
        GlyphPageUsed = serializer.Intarray(GlyphPageUsed);

        // Deserialize array properly
        Glyphs = serializer.Array(Glyphs, typeof(GlyphInfo));

        if (Revision > 0x272)
            IsCompressed = stream.Boole();

        Data = stream.Bytes(stream.GetLength() - stream.GetOffset());
    }

    public GlyphInfo? GetGlyph(char c)
    {
        return Glyphs.FirstOrDefault(info => info.Character == c);
    }

    public SKBitmap GetGlyphImage(GlyphInfo info)
    {
        byte[]? glyph = GetGlyphData(info);
        int w = info.BoxW & 0xFF;
        int h = info.BoxH & 0xFF;
        SKBitmap image = new SKBitmap(w, h,
            SKColorType.Rgba8888, SKAlphaType.Premul); // Corrected constructor
        var pixels = image.Pixels;
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                int i = y * w + x;
                SKColor color;

                if (IsCompressed)
                {
                    int value = glyph[i] & 0xFF;
                    color = new SKColor((byte)value, (byte)value, (byte)value, 255);
                }
                else
                {
                    int green = glyph[i * 2] & 0xFF;
                    int blue = glyph[(i * 2) + 1] & 0xFF;
                    color = new SKColor(0, (byte)green, (byte)blue, 255);
                }

                image.SetPixel(x, y, color);
            }
        }

        return image;
    }

    public byte[]? GetGlyphData(GlyphInfo info)
    {
        int imageSize = info.BoxW * info.BoxH;

        if (IsCompressed)
        {
            int size = (Data[info.Offset] << 8) | Data[info.Offset + 1];
            byte[]? stream = Data.Skip(info.Offset + 2).Take(size).ToArray();
            return Compressor.InflateData(stream, imageSize);
        }

        return Data.Skip(info.Offset).Take(imageSize * 2).ToArray();
    }

    public bool Export(string path)
    {
        int maxColumns = (int)Math.Floor(Math.Sqrt(Glyphs.Length));
        int col = 0, x = 0, y = 0, maxHeightInRow = 0;
        int w = 0, h = 0;

        foreach (var info in Glyphs)
        {
            if (col == maxColumns)
            {
                y += maxHeightInRow;
                maxHeightInRow = 0;
                col = 0;
                x = 0;
            }

            info.CacheX = x;
            info.CacheY = y;

            int sh = info.BoxH & 0xff;
            int sw = info.BoxW & 0xff;

            if ((x + sw) > w) w = (x + sw);
            if ((y + sh) > h) h = (y + sh);

            if (sh > maxHeightInRow)
                maxHeightInRow = sh;

            col++;
            x += (info.BoxW & 0xff);
        }

        using var image = new SKBitmap(w, h);
        using var canvas = new SKCanvas(image);
        canvas.Clear(SKColors.Black);

        foreach (var info in Glyphs)
        {
            SKBitmap glyph = GetGlyphImage(info);
            canvas.DrawBitmap(glyph, info.CacheX, info.CacheY);
        }

        using var imageStream = File.OpenWrite(path);
        image.Encode(imageStream, SKEncodedImageFormat.Png, 100);
        imageStream.Close();
        return true;
    }
}