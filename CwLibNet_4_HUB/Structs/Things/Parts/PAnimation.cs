using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

public class PAnimation: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? Animation;
    public float Velocity, Position;
    
    public void Serialize(Serializer serializer)
    {
        Animation = serializer.Resource(Animation, ResourceType.Animation);
        Velocity = serializer.F32(Velocity);
        Position = serializer.F32(Position);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}