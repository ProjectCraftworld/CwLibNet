using CwLibNet.Types.Data;

using static net.torutheredfox.craftworld.serialization.Serializer;
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


    public override void Serialize()
    {
        Serializer.Serialize(ref slots);
        if (Serializer.GetRevision().GetVersion() >= (int)Revisions.PRODUCTION_BUILD)
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
        Serializer.Serialize(ref this);
        return new SerializationData(
            Serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.SlotList,
            SerializationType.BINARY,
            Serializer.GetDependencies()
        );
    }

}