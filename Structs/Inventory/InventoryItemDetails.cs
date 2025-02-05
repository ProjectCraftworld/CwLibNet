using CwLibNet.Resources;
using CwLibNet.Singleton;
using CwLibNet.Enums;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Inventory
{
    public class InventoryItemDetails : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0xB0;
        public string translationTag = "";
        public long dateAdded = new Date().GetTime() / 1000;
        public SlotID levelUnlockSlotID = new SlotID();
        public GUID highlightSound;
        public int colour = -1;
        public EnumSet<InventoryObjectType> type = EnumSet.NoneOf(typeof(InventoryObjectType));
        public int subType = InventoryObjectSubType.NONE;
        public long titleKey, descriptionKey;
        public UserCreatedDetails userCreatedDetails;
        public CreationHistory creationHistory;
        public ResourceDescriptor icon = new ResourceDescriptor(15525, ResourceType.TEXTURE);
        public InventoryItemPhotoData photoData;
        public EyetoyData eyetoyData;
        public short locationIndex = -1, categoryIndex = -1;
        public short primaryIndex;
        public int lastUsed, numUses;
        public int fluffCost;
        public bool allowEmit;
        public bool shareable;
        public bool copyright;
        public NetworkPlayerID creator = new NetworkPlayerID();
        public ToolType toolType = ToolType.NONE;
        public byte flags;
        public bool makeSizeProportional = true;
        public long location, category;
        public string categoryTag = "", locationTag = "";
        public string translatedTitle = "";
        public string translatedDescription;
        public string translatedLocation = "";
        public string translatedCategory = "";
        public InventoryItemDetails()
        {Serialize
        }

        public InventoryItemDetails(PMetadata metadata)
        {

            // if (metadata.nameTranslationTag != null && metadata.nameTranslationTag.endsWith
            // ("_NAME"))
            //     this.translationTag = metadata.nameTranslationTag.split("_NAME")[0]; // LOL
            this.titleKey = metadata.titleKey;
            this.descriptionKey = metadata.descriptionKey;
            this.location = metadata.location;
            this.category = metadata.category;
            this.locationTag = metadata.locationTag;
            this.categoryTag = metadata.categoryTag;
            this.primaryIndex = (short)metadata.primaryIndex;
            this.fluffCost = metadata.fluffCost;
            this.type = metadata.type;
            this.subType = metadata.subType;
            this.icon = metadata.icon;
            this.allowEmit = metadata.allowEmit;
        }

        // if (metadata.nameTranslationTag != null && metadata.nameTranslationTag.endsWith
        // ("_NAME"))
        //     this.translationTag = metadata.nameTranslationTag.split("_NAME")[0]; // LOL
        public override void Serialize(Serializer serializer)
        {
            int head = serializer.GetRevision().GetVersion();
            if (serializer.IsWriting() && highlightSound != null)
                serializer.Serialize(new ResourceDescriptor(highlightSound, ResourceType.FILENAME));
            if (serializer.GetRevision().GetVersion() > 0x37c)
            {
                dateAdded = serializer.Serialize(dateAdded);
                levelUnlockSlotID = serializer.Serialize(levelUnlockSlotID, typeof(SlotID));
                highlightSound = serializer.Serialize(highlightSound);
                colour = serializer.Serialize(colour);
                if (serializer.IsWriting())
                    serializer.GetOutput().Serialize(InventoryObjectType.GetFlags(type));
                else
                    type = InventoryObjectType.FromFlags(serializer.GetInput().Serialize(), serializer.GetRevision());
                subType = serializer.Serialize(subType);
                titleKey = serializer.Serialize(titleKey);
                descriptionKey = serializer.Serialize(descriptionKey);
                creationHistory = serializer.Serialize(creationHistory, typeof(CreationHistory));
                icon = serializer.Serialize(icon, ResourceType.TEXTURE, true);
                userCreatedDetails = serializer.Serialize(userCreatedDetails, typeof(UserCreatedDetails));
                photoData = serializer.Serialize(photoData, typeof(InventoryItemPhotoData));
                eyetoyData = serializer.Serialize(eyetoyData, typeof(EyetoyData));
                locationIndex = serializer.Serialize(locationIndex);
                categoryIndex = serializer.Serialize(categoryIndex);
                primaryIndex = serializer.Serialize(primaryIndex);
                creator = serializer.Serialize(creator, typeof(NetworkPlayerID));
                toolType = ToolType.FromValue(serializer.Serialize(toolType.GetValue()));
                flags = serializer.Serialize(flags);
                if (serializer.GetRevision().Has(Branch.DOUBLE11, Revisions.D1_DETAILS_PROPORTIONAL))
                    makeSizeProportional = serializer.Serialize(makeSizeProportional);
                if (!serializer.IsWriting())
                    UpdateTranslations();
                return;
            }

            if (head < 0x233)
            {
                if (head < 0x174)
                {
                    serializer.Wstr(null); // nameTranslationTag
                    serializer.Wstr(null); // descTranslationTag
                }
                else
                {
                    translationTag = serializer.Str(translationTag);
                }

                locationIndex = (short)serializer.Serialize(locationIndex, true);
                categoryIndex = (short)serializer.Serialize(categoryIndex, true);
                if (head > 0x194)
                    primaryIndex = (short)serializer.Serialize(primaryIndex, true);
                serializer.Serialize(0, true); // Pad
                if (serializer.IsWriting())
                    serializer.GetOutput().Serialize(InventoryObjectType.GetFlags(type), true);
                else
                    type = InventoryObjectType.FromFlags(serializer.GetInput().Serialize(true), serializer.GetRevision());
                subType = serializer.Serialize(subType, true);
                if (head > 0x196)
                    toolType = ToolType.FromValue((byte)serializer.Serialize(toolType.GetValue(), true));
                icon = serializer.Resource(icon, ResourceType.TEXTURE, true);
                if (head > 0x1c0)
                {
                    numUses = serializer.Serialize(numUses, true);
                    lastUsed = serializer.Serialize(lastUsed, true);
                }

                if (head > 0x14e)
                    highlightSound = serializer.Serialize(highlightSound, true);
                else
                    serializer.Str(null); // Path to highlight sound?
                if (head > 0x156)
                    colour = serializer.Serialize(colour, true);
                if (head > 0x161)
                {
                    eyetoyData = serializer.Serialize(eyetoyData, typeof(EyetoyData));
                }


                // 0x17a < revision && revision < 0x182
                // 0x181 < revision ???
                if (head > 0x17a)
                    photoData = serializer.Serialize(photoData, typeof(InventoryItemPhotoData));
                if (head > 0x176)
                {
                    levelUnlockSlotID.slotType = SlotType.FromValue(serializer.Serialize(levelUnlockSlotID.slotType.GetValue()));
                    levelUnlockSlotID.slotNumber = serializer.Serialize(levelUnlockSlotID.slotNumber, true);
                }

                if (head > 0x181)
                {
                    copyright = serializer.Serialize(copyright);
                    creator = serializer.Serialize(creator, typeof(NetworkPlayerID));
                }

                if (head > 0x1aa)
                {
                    userCreatedDetails = serializer.Serialize(userCreatedDetails, typeof(UserCreatedDetails));
                    if (userCreatedDetails != null && userCreatedDetails.name.IsEmpty() && userCreatedDetails.description.IsEmpty())
                        userCreatedDetails = null;
                }

                if (head > 0x1b0)
                    creationHistory = serializer.Serialize(creationHistory, typeof(CreationHistory));
                if (head > 0x204)
                    allowEmit = serializer.Serialize(allowEmit);
                if (head > 0x221)
                    dateAdded = serializer.Serialize(dateAdded, true);
                if (head > 0x222)
                    shareable = serializer.Serialize(shareable);
                if (!serializer.IsWriting())
                    UpdateTranslations();
                return;
            }

            highlightSound = serializer.Serialize(highlightSound, true);

            // In these older versions of the inventory details,
            // 32 bit values are enforced while still using encoded values elsewhere,
            // so for some structures like SlotID, we need to force it manually.
            levelUnlockSlotID.slotType = SlotType.FromValue(serializer.Serialize(levelUnlockSlotID.slotType.GetValue(), true));
            levelUnlockSlotID.slotNumber = serializer.Serialize(levelUnlockSlotID.slotNumber, true);
            locationIndex = (short)serializer.Serialize(locationIndex, true);
            categoryIndex = (short)serializer.Serialize(categoryIndex, true);
            primaryIndex = (short)serializer.Serialize(primaryIndex, true);
            lastUsed = serializer.Serialize(lastUsed, true);
            numUses = serializer.Serialize(numUses, true);
            if (head > 0x234)
                serializer.Serialize(0, true); // Pad
            dateAdded = serializer.Serialize(dateAdded, true);
            fluffCost = serializer.Serialize(fluffCost, true);
            colour = serializer.Serialize(colour, true);
            if (serializer.IsWriting())
                serializer.GetOutput().Serialize(InventoryObjectType.GetFlags(type), true);
            else
                type = InventoryObjectType.FromFlags(serializer.GetInput().Serialize(true), serializer.GetRevision());
            subType = serializer.Serialize(subType, true);
            toolType = ToolType.FromValue((byte)serializer.Serialize(toolType.GetValue(), true));
            creator = serializer.Serialize(creator, typeof(NetworkPlayerID));
            allowEmit = serializer.Serialize(allowEmit);
            shareable = serializer.Serialize(shareable);
            copyright = serializer.Serialize(copyright);
            if (head > 0x334)
                flags = serializer.Serialize(flags);
            if (serializer.GetRevision().Has(Branch.LEERDAMMER, Revisions.LD_LAMS_KEYS) || head > 0x2ba)
            {
                titleKey = serializer.Serialize(titleKey);
                descriptionKey = serializer.Serialize(descriptionKey);
            }
            else
                translationTag = serializer.Str(translationTag);
            userCreatedDetails = serializer.Serialize(userCreatedDetails, typeof(UserCreatedDetails));
            if (userCreatedDetails != null && userCreatedDetails.name.IsEmpty() && userCreatedDetails.description.IsEmpty())
                userCreatedDetails = null;
            creationHistory = serializer.Serialize(creationHistory, typeof(CreationHistory));
            icon = serializer.Resource(icon, ResourceType.TEXTURE, true);
            photoData = serializer.Serialize(photoData, typeof(InventoryItemPhotoData));
            eyetoyData = serializer.Serialize(eyetoyData, typeof(EyetoyData));
            if (head > 0x358)
                serializer.U8(0);
            if (!serializer.IsWriting())
                UpdateTranslations();
        }

        // if (metadata.nameTranslationTag != null && metadata.nameTranslationTag.endsWith
        // ("_NAME"))
        //     this.translationTag = metadata.nameTranslationTag.split("_NAME")[0]; // LOL
        // nameTranslationTag
        // descTranslationTag
        // Pad
        // Path to highlight sound?
        // 0x17a < revision && revision < 0x182
        // 0x181 < revision ???
        // In these older versions of the inventory details,
        // 32 bit values are enforced while still using encoded values elsewhere,
        // so for some structures like SlotID, we need to force it manually.
        // Pad
        public virtual int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (this.translationTag != null)
                size += (this.translationTag.Length() * 2);
            if (this.userCreatedDetails != null)
                size += this.userCreatedDetails.GetAllocatedSize();
            if (this.creationHistory != null)
                size += this.creationHistory.GetAllocatedSize();
            if (this.photoData != null)
                size += this.photoData.GetAllocatedSize();
            if (this.eyetoyData != null)
                size += this.eyetoyData.GetAllocatedSize();
            return size;
        }

        // if (metadata.nameTranslationTag != null && metadata.nameTranslationTag.endsWith
        // ("_NAME"))
        //     this.translationTag = metadata.nameTranslationTag.split("_NAME")[0]; // LOL
        // nameTranslationTag
        // descTranslationTag
        // Pad
        // Path to highlight sound?
        // 0x17a < revision && revision < 0x182
        // 0x181 < revision ???
        // In these older versions of the inventory details,
        // 32 bit values are enforced while still using encoded values elsewhere,
        // so for some structures like SlotID, we need to force it manually.
        // Pad
        public virtual SHA1 GenerateHashCode(Revision revision)
        {

            // I wonder how slow this is...
            Serializer serializer = new Serializer(this.GetAllocatedSize(), revision, (byte)0);
            serializer.Serialize(this, typeof(InventoryItemDetails));
            return SHA1.FromBuffer(serializer.GetBuffer());
        }

        // if (metadata.nameTranslationTag != null && metadata.nameTranslationTag.endsWith
        // ("_NAME"))
        //     this.translationTag = metadata.nameTranslationTag.split("_NAME")[0]; // LOL
        // nameTranslationTag
        // descTranslationTag
        // Pad
        // Path to highlight sound?
        // 0x17a < revision && revision < 0x182
        // 0x181 < revision ???
        // In these older versions of the inventory details,
        // 32 bit values are enforced while still using encoded values elsewhere,
        // so for some structures like SlotID, we need to force it manually.
        // Pad
        // I wonder how slow this is...
        private void UpdateTranslations()
        {
            if (this.translationTag != null && !this.translationTag.IsEmpty())
            {
                this.titleKey = RTranslationTable.MakeLamsKeyID(this.translationTag + "_NAME");
                this.descriptionKey = RTranslationTable.MakeLamsKeyID(this.translationTag + "_DESC");
            }

            RTranslationTable LAMS = ResourceSystem.GetLAMS();
            if (LAMS != null)
            {
                if (this.titleKey != 0)
                    this.translatedTitle = LAMS.Translate(this.titleKey);
                if (this.descriptionKey != 0)
                    this.translatedDescription = LAMS.Translate(this.descriptionKey);
            }
        }
    }
}