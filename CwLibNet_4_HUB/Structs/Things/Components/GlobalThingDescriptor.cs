using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components;

public class GlobalThingDescriptor: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? LevelDesc;
    public int Uid;
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref LevelDesc, ResourceType.Level, true, false, false);
        Serializer.Serialize(ref Uid);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}