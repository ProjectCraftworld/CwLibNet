using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.Structs.Slot;
using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;

namespace CwLibNet4Hub.Resources;

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
    public virtual void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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
        Serializer.SetCurrentSerializer(serializer);
        Serialize(serializer);
        return new SerializationData(
            Serializer.GetCurrentSerializer().GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Quest,
            SerializationType.BINARY,
            Serializer.GetCurrentSerializer().GetDependencies()
        );
        
    }
}