using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Slot;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components;

public class KeyLink: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public SlotID? SlotId;
    public int Unknown;
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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