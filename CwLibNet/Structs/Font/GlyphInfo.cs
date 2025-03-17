using System;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Font
{
    public class GlyphInfo
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public short character;
        public byte boxLeft, boxTop;
        public byte boxW, boxH;
        public int offset;
        public int nextGlyph;

        public int cacheX, cacheY;

        public virtual void serialize(Serializer serializer)
        {
            character = serializer.I16(character);
            boxLeft = serializer.I8(boxLeft);
            boxTop = serializer.I8(boxTop);
            boxW = serializer.I8(boxW);
            boxH = serializer.I8(boxH);
            offset = serializer.I32(offset);
            nextGlyph = serializer.I16((short)nextGlyph);
        }

        public virtual int getAllocatedSize()
        {
            return GlyphInfo.BASE_ALLOCATION_SIZE;
        }
    }
}