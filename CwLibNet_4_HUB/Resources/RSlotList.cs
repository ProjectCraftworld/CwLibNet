using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Enums;

using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Slot;
namespace CwLibNet4Hub.Resources;

public class RSlotList : Resource
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x8;

    private List<Slot> slots = [];

    public bool FromProductionBuild = true;

    public RSlotList() { }

    public RSlotList(List<Slot> slots)
    {
        this.slots = slots;
    }

    public RSlotList(Slot[] slots)
    {
        this.slots = [.. slots];
    }


    public override void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref slots);
        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() >= (int)Revisions.PRODUCTION_BUILD)
            Serializer.Serialize(ref FromProductionBuild);
    }

    
    public override int GetAllocatedSize()
    {
        var size = BASE_ALLOCATION_SIZE;
        if (slots != null)
        {
            size += slots.Sum(slot => slot.GetAllocatedSize());
        }
        return size;
    }


    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        Serializer serializer = new(GetAllocatedSize(), revision,
            compressionFlags);
        this.Serialize(serializer);
        return new SerializationData(
            Serializer.GetCurrentSerializer().GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.SlotList,
            SerializationType.BINARY,
            Serializer.GetCurrentSerializer().GetDependencies()
        );
    }

}