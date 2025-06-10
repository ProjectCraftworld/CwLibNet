using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Slot;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Slot;

public class Collectabubble : ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public ResourceDescriptor? Plan;
    public int Count;

    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Plan, ResourceType.Plan, true, false, false);
        Serializer.Serialize(ref Count);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}