using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Mesh;

public class SoftbodySpring: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public short A, B;
    public float RestLengthSq;

    public SoftbodySpring() { }

    public SoftbodySpring(int a, int b, float restLengthSq)
    {
        A = (short) a;
        B = (short) b;
        RestLengthSq = restLengthSq;
    }

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref A);
        Serializer.Serialize(ref B);
        Serializer.Serialize(ref RestLengthSq);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}