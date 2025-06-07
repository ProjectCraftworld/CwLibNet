using CwLibNet.IO;
using CwLibNet.Structs.Things.Parts;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Components.Shapes;

public class Contact: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public PShape Shape;
    public byte Flags;

    
    public void Serialize()
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