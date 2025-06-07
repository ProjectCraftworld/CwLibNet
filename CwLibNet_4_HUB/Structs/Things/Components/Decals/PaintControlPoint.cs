using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Things.Components.Decals;

public class PaintControlPoint: ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x4;

    public byte X, Y, StartRadius, EndRadius;

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref X);
        Serializer.Serialize(ref Y);
        Serializer.Serialize(ref StartRadius);
        Serializer.Serialize(ref EndRadius);
    }

    
    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }


}