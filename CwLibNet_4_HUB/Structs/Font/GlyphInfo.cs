using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Font;

public class GlyphInfo: ISerializable
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
        Serializer.Serialize(ref Character);
        Serializer.Serialize(ref BoxLeft);
        Serializer.Serialize(ref BoxTop);
        Serializer.Serialize(ref BoxW);
        Serializer.Serialize(ref BoxH);
        Serializer.Serialize(ref Offset);
        NextGlyph = Serializer.Serialize(ref (short)NextGlyph);
    }

    public virtual int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}