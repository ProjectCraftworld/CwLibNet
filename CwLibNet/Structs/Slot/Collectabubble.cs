using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Slot;

public class Collectabubble : ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public ResourceDescriptor? Plan;
    public int Count;

    public void Serialize(Serializer serializer)
    {
        Plan = serializer.Resource(Plan, ResourceType.Plan, true);
        Count = serializer.I32(Count);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}