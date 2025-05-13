using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Enums;
using CwLibNet.Structs.Slot;

namespace CwLibNet.Resources;

public class RQuest : ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x8; // for the GUID

    public int state;
    public ResourceDescriptor[] prizes; // RPlan
    public ResourceDescriptor[] prizeIcons;
    // objectives
    // short count
    // short required
    // bool hidden
    // int key
    public DecalBody[] decals;
    // decorations
    // PDecoration decoration
    // ResourceDescriptor mesh;
    public bool isPrimary;
    public bool isInvisible;
    public SlotID[] deactivatedInLevels;
    public SlotID[] activatedInLevels;
    public SlotID[] completedInLevels;
    public bool stateUpdateViewed;
    public int key;
    public int availabilityDependsUponKey;
    // newDecorations
    // s32 state
    // PDecoration decoration
    // ResourceDescriptor mesh;
    // s32 state
    // s32 objectiveIdx
    // s32 areaIdx
    // newStickers
    // Decal sticker
    // s32 objectiveIdx
    // s32 areaIdx
    // potentiallyCompletedObjectsInLevels
    // items
    //     SlotID
    //     int[]
    public virtual void Serialize(Serializer serializer)
    {
    }

    public virtual int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }

    public SerializationData Build(Revision revision, byte compressionFlags)
    {
        var serializer = new Serializer(GetAllocatedSize(), revision,
            compressionFlags);
        serializer.Struct<RQuest>(this);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Quest,
            SerializationType.BINARY,
            serializer.GetDependencies()
        );
        
    }
}