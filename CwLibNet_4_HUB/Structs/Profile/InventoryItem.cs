using CwLibNet4Hub.Enums;
using CwLibNet4Hub.EX;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Inventory;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Profile;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Profile;

public class InventoryItem: ISerializable
{
    public const int BaseAllocationSize = 0x40;

    /**
     * The reference to the actual plan data used by this item.
     */
    public ResourceDescriptor? Plan;

    /**
     * Cache for inventory details of this item.
     */
    public InventoryItemDetails Details = new();

    /**
     * Cache of GUID for this item.
     */
    
    public GUID? Guid;

    /**
     * Unique ID of this item in your inventory(?)
     */
    public int Uid;

    /**
     * Tutorial related data, see ETutorialLevels
     */
    
    public TutorialLevel TutorialLevel = TutorialLevel.UNKNOWN;
    
    public TutorialLevel TutorialVideo = TutorialLevel.UNKNOWN;

    /**
     * State of the item in your inventory,
     * whether it's hearted, uploaded, etc
     */
    public int Flags = InventoryItemFlags.NONE;

    /**
     * Index of user defined category in the string table.
     */
    
    public int UserCategoryIndex;


    public InventoryItem() { }

    public InventoryItem(int uid, ResourceDescriptor? descriptor, InventoryItemDetails? details)
    {
        details ??= new InventoryItemDetails();

        details.DateAdded = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        Uid = uid;
        Details = details;
        Plan = descriptor;
    }

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Plan, ResourceType.Plan, true, false, false);
        if (Plan != null)
            serializer.AddDependency(Plan);

        if (serializer.GetRevision().GetSubVersion() >= (int)Revisions.ITEM_GUID)
            Serializer.Serialize(ref Guid);

        Serializer.Serialize(ref Details);

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        if (version >= (int)Revisions.ITEM_FLAGS)
        {
            Serializer.Serialize(ref Uid);
            if (version < (int)Revisions.REMOVE_LBP1_TUTORIALS)
            {
                if (Serializer.IsWriting())
                {
                    Serializer.GetCurrentSerializer().GetOutput().I32((int)TutorialLevel, true);
                    Serializer.GetCurrentSerializer().GetOutput().I32((int)TutorialVideo, true);
                }
                else
                {
                    TutorialLevel =
                        (TutorialLevel)Serializer.GetCurrentSerializer().GetInput().I32(true);
                    TutorialVideo =
                        (TutorialLevel)Serializer.GetCurrentSerializer().GetInput().I32(true);
                }
            }
            Serializer.Serialize(ref Flags);
            if (version >= (int)Revisions.USER_CATEGORIES)
                Serializer.Serialize(ref UserCategoryIndex);
        }
        else throw new SerializationException("InventoryItem's below r565 are not supported!");
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize + Details.GetAllocatedSize();
    }


}
