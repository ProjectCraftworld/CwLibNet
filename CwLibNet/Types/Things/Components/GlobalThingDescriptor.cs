using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Types.Things.Components;

public class GlobalThingDescriptor: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? LevelDesc;
    public int Uid;
    public void Serialize(Serializer serializer)
    {
        LevelDesc = serializer.Resource(LevelDesc, ResourceType.Level, true, false, false);
        Uid = serializer.I32(Uid);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}