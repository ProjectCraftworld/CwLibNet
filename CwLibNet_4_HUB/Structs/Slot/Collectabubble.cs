using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Slot;

public class Collectabubble : ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public ResourceDescriptor? Plan;
    public int Count;

    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Plan, ResourceType.Plan, true, false, false);
        Serializer.Serialize(ref Count);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}