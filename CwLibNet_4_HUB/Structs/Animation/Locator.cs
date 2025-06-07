using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Animation;

public class Locator: ISerializable
{
    public const int BaseAllocationSize = 0x12;

    public Vector3? Position;
    public string Name;
    public byte Looping, Type;

    
    public void Serialize()
    {
        Position = Serializer.Serialize(ref Position);
        Serializer.Serialize(ref Name);
        Serializer.Serialize(ref Looping);
        Serializer.Serialize(ref Type);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null)
            size += Name.Length;
        return size;
    }


}