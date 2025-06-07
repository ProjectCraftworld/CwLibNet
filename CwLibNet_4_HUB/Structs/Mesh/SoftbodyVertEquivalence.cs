using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Mesh;

public class SoftbodyVertEquivalence: ISerializable
{
    public const int BaseAllocationSize = 0x4;

    public short First, Count;

    public SoftbodyVertEquivalence() { }

    public SoftbodyVertEquivalence(int first, int count)
    {
        First = (short) first;
        Count = (short) count;
    }

    
    public void Serialize()
    {
        Serializer.Serialize(ref First);
        Serializer.Serialize(ref Count);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}