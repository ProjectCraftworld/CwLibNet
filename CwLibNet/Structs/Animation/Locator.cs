using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Animation;

public class Locator: ISerializable
{
    public const int BaseAllocationSize = 0x12;

    public Vector3? Position;
    public String Name;
    public byte Looping, Type;

    
    public void Serialize(Serializer serializer)
    {
        Position = serializer.V3(Position);
        Name = serializer.Str(Name);
        Looping = serializer.I8(Looping);
        Type = serializer.I8(Type);
    }

    
    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (Name != null)
            size += (Name.Length);
        return size;
    }


}