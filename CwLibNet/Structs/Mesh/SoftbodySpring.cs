using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Mesh;

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
        this.RestLengthSq = restLengthSq;
    }

    
    public void Serialize(Serializer serializer)
    {
        A = serializer.I16(A);
        B = serializer.I16(B);
        RestLengthSq = serializer.F32(RestLengthSq);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}