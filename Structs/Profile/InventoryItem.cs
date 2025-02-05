using CwLibNet.Resources;
using CwLibNet.Singleton;
using CwLibNet.Enums;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Inventory;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serialization;


namespace CwLibNet.Structs.Profile
{
    /// <summary>
    /// Represents an instance of an item in your inventory.
    /// Used in the RLocalProfile and RBigProfile.
    /// </summary>
    public class InventoryItem : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x40;
        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        public ResourceDescriptor plan;
        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        public InventoryItemDetails details = new InventoryItemDetails();
        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        public GUID guid;
        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        /// <summary>
        /// Unique ID of this item in your inventory(?)
        /// </summary>
        public int UID;
        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        /// <summary>
        /// Unique ID of this item in your inventory(?)
        /// </summary>
        /// <summary>
        /// Tutorial related data, see ETutorialLevels
        /// </summary>
        public TutorialLevel tutorialLevel = TutorialLevel.UNKNOWN;
        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        /// <summary>
        /// Unique ID of this item in your inventory(?)
        /// </summary>
        /// <summary>
        /// Tutorial related data, see ETutorialLevels
        /// </summary>
        public TutorialLevel tutorialVideo = TutorialLevel.UNKNOWN;
        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        /// <summary>
        /// Unique ID of this item in your inventory(?)
        /// </summary>
        /// <summary>
        /// Tutorial related data, see ETutorialLevels
        /// </summary>
        /// <summary>
        /// State of the item in your inventory,
        /// whether it's hearted, uploaded, etc
        /// </summary>
        public int flags = InventoryItemFlags.NONE;
        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        /// <summary>
        /// Unique ID of this item in your inventory(?)
        /// </summary>
        /// <summary>
        /// Tutorial related data, see ETutorialLevels
        /// </summary>
        /// <summary>
        /// State of the item in your inventory,
        /// whether it's hearted, uploaded, etc
        /// </summary>
        /// <summary>
        /// Index of user defined category in the string table.
        /// </summary>
        public int userCategoryIndex;
        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        /// <summary>
        /// Unique ID of this item in your inventory(?)
        /// </summary>
        /// <summary>
        /// Tutorial related data, see ETutorialLevels
        /// </summary>
        /// <summary>
        /// State of the item in your inventory,
        /// whether it's hearted, uploaded, etc
        /// </summary>
        /// <summary>
        /// Index of user defined category in the string table.
        /// </summary>
        public InventoryItem()
        {
        }

        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        /// <summary>
        /// Unique ID of this item in your inventory(?)
        /// </summary>
        /// <summary>
        /// Tutorial related data, see ETutorialLevels
        /// </summary>
        /// <summary>
        /// State of the item in your inventory,
        /// whether it's hearted, uploaded, etc
        /// </summary>
        /// <summary>
        /// Index of user defined category in the string table.
        /// </summary>
        public InventoryItem(int UID, ResourceDescriptor descriptor, InventoryItemDetails details)
        {
            if (details == null)
                details = new InventoryItemDetails();
            details.dateAdded = new Date().GetTime() / 1000;
            this.UID = UID;
            this.details = details;
            this.plan = descriptor;
        }

        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        /// <summary>
        /// Unique ID of this item in your inventory(?)
        /// </summary>
        /// <summary>
        /// Tutorial related data, see ETutorialLevels
        /// </summary>
        /// <summary>
        /// State of the item in your inventory,
        /// whether it's hearted, uploaded, etc
        /// </summary>
        /// <summary>
        /// Index of user defined category in the string table.
        /// </summary>
        public override void Serialize(Serializer serializer)
        {
            plan = serializer.Serialize(plan, ResourceType.PLAN, true);
            if (plan != null)
                serializer.AddDependency(plan);
            if (serializer.GetRevision().GetSubVersion() >= Revisions.ITEM_GUID)
                guid = serializer.Serialize(guid);
            details = serializer.Serialize(details, typeof(InventoryItemDetails));
            int version = serializer.GetRevision().GetVersion();
            if (version >= Revisions.ITEM_FLAGS)
            {
                UID = serializer.Serialize(UID, true);
                if (version < Revisions.REMOVE_LBP1_TUTORIALS)
                {
                    if (serializer.IsWriting())
                    {
                        serializer.GetOutput().Serialize(tutorialLevel.GetValue(), true);
                        serializer.GetOutput().Serialize(tutorialVideo.GetValue(), true);
                    }
                    else
                    {
                        tutorialLevel = TutorialLevel.FromValue(serializer.GetInput().I32(true));
                        tutorialVideo = TutorialLevel.FromValue(serializer.GetInput().I32(true));
                    }
                }

                flags = serializer.I32(flags, true);
                if (version >= Revisions.USER_CATEGORIES)
                    userCategoryIndex = serializer.Serialize(userCategoryIndex, true);
            }
            else
                throw new SerializationException("InventoryItem's below r565 are not supported!");
        }

        /// <summary>
        /// The reference to the actual plan data used by this item.
        /// </summary>
        /// <summary>
        /// Cache for inventory details of this item.
        /// </summary>
        /// <summary>
        /// Cache of GUID for this item.
        /// </summary>
        /// <summary>
        /// Unique ID of this item in your inventory(?)
        /// </summary>
        /// <summary>
        /// Tutorial related data, see ETutorialLevels
        /// </summary>
        /// <summary>
        /// State of the item in your inventory,
        /// whether it's hearted, uploaded, etc
        /// </summary>
        /// <summary>
        /// Index of user defined category in the string table.
        /// </summary>
        public virtual int GetAllocatedSize()
        {
            return InventoryItem.BASE_ALLOCATION_SIZE + this.details.GetAllocatedSize();
        }
    }
}