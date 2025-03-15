using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;

namespace CwLibNet.Types.Things.Components;

public class KeyLink: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public SlotID? SlotId;
    public int Unknown;
    public void Serialize(Serializer serializer)
    {
        SlotId = serializer.Struct(SlotId);
        if (serializer.GetRevision().GetVersion() < 0x16f)
            Unknown = serializer.I32(Unknown);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}