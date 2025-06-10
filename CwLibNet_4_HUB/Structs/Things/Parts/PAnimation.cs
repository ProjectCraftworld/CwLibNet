using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

public class PAnimation: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? Animation;
    public float Velocity, Position;
    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Animation, ResourceType.Animation, false, false, false);
        Serializer.Serialize(ref Velocity);
        Serializer.Serialize(ref Position);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}