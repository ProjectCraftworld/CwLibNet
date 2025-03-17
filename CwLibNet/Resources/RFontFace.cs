using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;
using CwLibNet.Enums;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Font;
using CwLibNet.Types;
using CwLibNet.Util;

namespace CwLibNet.Resources
{
    public class RFontFace
    {
        public int revision;
        public short[] glyphIndex = new short[128];
        public int[] glyphPageUsed = new int[16];
        public GlyphInfo[] glyphs;
        public byte[] data;
        public bool isCompressed;

        public RFontFace(byte[] data)
        {
            MemoryInputStream stream = new MemoryInputStream(data, CompressionFlags.USE_NO_COMPRESSION);
            
            if (stream.I32() != 0x464E5462) 
                throw new Exception("Not a supported RFontFace file.");

            this.revision = stream.I32();
            stream.I32();
            stream.I32();

            Serializer serializer = new Serializer(stream, new Revision(this.revision));
            this.glyphIndex = serializer.Shortarray(this.glyphIndex);
            this.glyphPageUsed = serializer.Intarray(this.glyphPageUsed);

            // Deserialize array properly
            this.glyphs = serializer.Array(this.glyphs, typeof(GlyphInfo));

            if (this.revision > 0x272)
                this.isCompressed = stream.Boole();

            this.data = stream.Bytes(stream.GetLength() - stream.GetOffset());
        }

        public GlyphInfo GetGlyph(char c)
        {
            return this.glyphs.FirstOrDefault(info => info.character == c);
        }

        public SKBitmap GetGlyphImage(GlyphInfo info)
        {
            byte[] glyph = this.GetGlyphData(info);
            int w = info.boxW & 0xFF;
            int h = info.boxH & 0xFF;
            SKBitmap image = new SKBitmap(w, h,
                SKColorType.Rgba8888, SKAlphaType.Premul); // Corrected constructor
            var pixels = image.Pixels;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int i = y * w + x;
                    SKColor color;

                    if (this.isCompressed)
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

        public byte[] GetGlyphData(GlyphInfo info)
        {
            int imageSize = info.boxW * info.boxH;

            if (this.isCompressed)
            {
                int size = (this.data[info.offset] << 8) | this.data[info.offset + 1];
                byte[] stream = this.data.Skip(info.offset + 2).Take(size).ToArray();
                return Compressor.InflateData(stream, imageSize);
            }

            return this.data.Skip(info.offset).Take(imageSize * 2).ToArray();
        }

        public bool Export(string path)
        {
            int maxColumns = (int)Math.Floor(Math.Sqrt(this.glyphs.Length));
            int col = 0, x = 0, y = 0, maxHeightInRow = 0;
            int w = 0, h = 0;

            foreach (var info in this.glyphs)
            {
                if (col == maxColumns)
                {
                    y += maxHeightInRow;
                    maxHeightInRow = 0;
                    col = 0;
                    x = 0;
                }

                info.cacheX = x;
                info.cacheY = y;

                int sh = info.boxH & 0xff;
                int sw = info.boxW & 0xff;

                if ((x + sw) > w) w = (x + sw);
                if ((y + sh) > h) h = (y + sh);

                if (sh > maxHeightInRow)
                    maxHeightInRow = sh;

                col++;
                x += (info.boxW & 0xff);
            }

            using (var image = new SKBitmap(w, h))
            using (var canvas = new SKCanvas(image))
            {
                canvas.Clear(SKColors.Black);

                foreach (var info in this.glyphs)
                {
                    SKBitmap glyph = this.GetGlyphImage(info);
                    canvas.DrawBitmap(glyph, info.cacheX, info.cacheY);
                }

                using (var imageStream = File.OpenWrite(path))
                {
                    image.Encode(imageStream, SKEncodedImageFormat.Png, 100);
                }

                return true;
            }
        }
    }
}
