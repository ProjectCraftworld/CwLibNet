using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Parts;

namespace CwLibNet.Structs.Things.Components.Shapes;

public class Contact: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public PShape Shape;
    public byte Flags;

    
    public void Serialize(Serializer serializer)
    {
        Shape = serializer.Reference(Shape);
        Flags = serializer.I8(Flags);
    }

    
    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (Shape != null)
            size += Shape.GetAllocatedSize();
        return size;
    }


}