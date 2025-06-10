using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Mesh;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref First);
        Serializer.Serialize(ref Count);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}