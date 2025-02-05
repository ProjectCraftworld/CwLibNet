using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace Cwlib.Structs.Font
{
    public class GlyphInfo : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public short character;
        public short advance;
        public byte boxLeft, boxTop;
        public byte boxW, boxH;
        public int offset;
        public short nextGlyph;
        public int cacheX, cacheY;
        public override void Serialize(Serializer serializer)
        {
            character = serializer.Serialize(character);
            advance = serializer.Serialize(advance);
            boxLeft = serializer.Serialize(boxLeft);
            boxTop = serializer.Serialize(boxTop);
            boxW = serializer.Serialize(boxW);
            boxH = serializer.Serialize(boxH);
            offset = serializer.Serialize(offset);
            nextGlyph = serializer.Serialize(nextGlyph);
        }

        public virtual int GetAllocatedSize()
        {
            return GlyphInfo.BASE_ALLOCATION_SIZE;
        }
    }
}