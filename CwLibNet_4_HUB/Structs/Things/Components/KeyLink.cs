using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Components;

public class KeyLink: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public SlotID? SlotId;
    public int Unknown;
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref SlotId);
        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() < 0x16f)
            Serializer.Serialize(ref Unknown);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}