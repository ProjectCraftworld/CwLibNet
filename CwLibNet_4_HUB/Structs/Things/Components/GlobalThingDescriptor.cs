using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Components;

public class GlobalThingDescriptor: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? LevelDesc;
    public int Uid;
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref LevelDesc, LevelDesc, ResourceType.Level, true, false, false);
        Serializer.Serialize(ref Uid);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}