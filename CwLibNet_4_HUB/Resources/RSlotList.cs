using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Resources;

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


    public override void Serialize(Serializer serializer)
    {
        slots = serializer.Arraylist(slots);
        if (serializer.GetRevision().GetVersion() >= (int)Revisions.PRODUCTION_BUILD)
            FromProductionBuild = serializer.Bool(FromProductionBuild);
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
        serializer.Struct(this);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.SlotList,
            SerializationType.BINARY,
            serializer.GetDependencies()
        );
    }

}