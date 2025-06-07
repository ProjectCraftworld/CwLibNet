using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Slot;

public class Collectabubble : ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public ResourceDescriptor? Plan;
    public int Count;

    public void Serialize()
    {
        Serializer.Serialize(ref Plan, Plan, ResourceType.Plan, true);
        Serializer.Serialize(ref Count);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}