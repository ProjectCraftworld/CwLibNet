using CwLibNet.IO;
using CwLibNet.IO.Serializer;

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

    
    public void Serialize(Serializer serializer)
    {
        First = serializer.I16(First);
        Count = serializer.I16(Count);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}