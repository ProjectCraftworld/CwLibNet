using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Components;

public class GlobalThingDescriptor: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? LevelDesc;
    public int Uid;
    public void Serialize()
    {
        Serializer.Serialize(ref LevelDesc, LevelDesc, ResourceType.Level, true, false, false);
        Serializer.Serialize(ref Uid);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}