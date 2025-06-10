using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Things.Parts;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components.Shapes;

public class Contact: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public PShape Shape;
    public byte Flags;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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