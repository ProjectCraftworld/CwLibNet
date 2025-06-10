using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Animation;

public class Locator: ISerializable
{
    public const int BaseAllocationSize = 0x12;

    public Vector3? Position;
    public string Name;
    public byte Looping, Type;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Position);
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