using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Font;

public class GlyphInfo
{
    public const int BaseAllocationSize = 0x10;
    public short Character;
    public byte BoxLeft, BoxTop;
    public byte BoxW, BoxH;
    public int Offset;
    public int NextGlyph;

    public int CacheX, CacheY;

    public virtual void Serialize(Serializer serializer)
    {
        Character = serializer.I16(Character);
        BoxLeft = serializer.I8(BoxLeft);
        BoxTop = serializer.I8(BoxTop);
        BoxW = serializer.I8(BoxW);
        BoxH = serializer.I8(BoxH);
        Offset = serializer.I32(Offset);
        NextGlyph = serializer.I16((short)NextGlyph);
    }

    public virtual int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}