using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PAnimation: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? Animation;
    public float Velocity, Position;
    
    public void Serialize()
    {
        Serializer.Serialize(ref Animation, Animation, ResourceType.Animation);
        Serializer.Serialize(ref Velocity);
        Serializer.Serialize(ref Position);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}