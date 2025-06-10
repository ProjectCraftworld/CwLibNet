using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components.Decals;

public class PaintControlPoint: ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x4;

    public byte X, Y, StartRadius, EndRadius;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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