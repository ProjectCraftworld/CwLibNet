using CwLibNet.IO;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Components.Shapes;

public class Contact: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public PShape Shape;
    public byte Flags;

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Shape);
        Serializer.Serialize(ref Flags);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Shape != null)
            size += Shape.GetAllocatedSize();
        return size;
    }


}