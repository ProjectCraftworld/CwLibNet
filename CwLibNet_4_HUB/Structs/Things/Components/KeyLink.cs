using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Components;

public class KeyLink: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public SlotID? SlotId;
    public int Unknown;
    public void Serialize()
    {
        Serializer.Serialize(ref SlotId);
        if (Serializer.GetRevision().GetVersion() < 0x16f)
            Serializer.Serialize(ref Unknown);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}