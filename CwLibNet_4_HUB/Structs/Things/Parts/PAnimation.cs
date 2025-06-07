using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PAnimation: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? Animation;
    public float Velocity, Position;
    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
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