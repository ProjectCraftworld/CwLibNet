using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.Decals;

public class PaintControlPoint: ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x4;

    public byte X, Y, StartRadius, EndRadius;

    
    public void Serialize(Serializer serializer)
    {
        X = serializer.I8(X);
        Y = serializer.I8(Y);
        StartRadius = serializer.I8(StartRadius);
        EndRadius = serializer.I8(EndRadius);
    }

    
    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }


}