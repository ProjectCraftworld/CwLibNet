using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Resources;
using CwLibNet4Hub.Structs.Slot;
using CwLibNet4Hub.Structs.Things.Parts;
using CwLibNet4Hub.Types.Data;
using Branch = CwLibNet4Hub.Enums.Branch;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Profile;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Inventory;

public class InventoryItemDetails: ISerializable
{
    public const int BaseAllocationSize = 0xB0;

    public string? TranslationTag = "";

    public long DateAdded = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

    public SlotID LevelUnlockSlotId = new();

    public GUID? HighlightSound;

    public int Colour = -1;

    public HashSet<InventoryObjectType> Type = [InventoryObjectType.NONE];
    public int SubType = InventoryObjectSubType.None;

    public long TitleKey, DescriptionKey;

    public UserCreatedDetails? UserCreatedDetails;

    public CreationHistory? CreationHistory;

    public ResourceDescriptor? Icon = new(15525, ResourceType.Texture);

    public InventoryItemPhotoData? PhotoData;
    public EyetoyData? EyetoyData;

    public short LocationIndex = -1, CategoryIndex = -1;
    public short PrimaryIndex;

    public int LastUsed, NumUses;

    public int FluffCost;

    public bool AllowEmit;
    public bool Shareable;
    public bool Copyright;

    public NetworkPlayerID? Creator = new();

    public ToolType ToolType = ToolType.NONE;
    public byte Flags;

    public bool MakeSizeProportional = true;

    public long Location, Category;

    public string? CategoryTag = "";
    public string? LocationTag = "";

    public string TranslatedTitle = "";
    public string? TranslatedDescription;
    public string TranslatedLocation = "";
    public string TranslatedCategory = "";

    public InventoryItemDetails() { }

    public InventoryItemDetails(PMetadata? metadata)
    {
        // if (metadata.nameTranslationTag != null && metadata.nameTranslationTag.endsWith
        // ("_NAME"))
        //     this.translationTag = metadata.nameTranslationTag.split("_NAME")[0]; // LOL

        TitleKey = metadata.TitleKey;
        DescriptionKey = metadata.DescriptionKey;
        Location = metadata.Location;
        Category = metadata.Category;

        LocationTag = metadata.LocationTag;
        CategoryTag = metadata.CategoryTag;

        PrimaryIndex = (short) metadata.PrimaryIndex;
        FluffCost = metadata.FluffCost;
        Type = metadata.Type;
        SubType = metadata.SubType;
        Icon = metadata.Icon;

        AllowEmit = metadata.AllowEmit;
    }

    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var head = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        if (Serializer.IsWriting() && HighlightSound != null)
            Serializer.GetCurrentSerializer().AddDependency(new ResourceDescriptor(HighlightSound.Value,
                ResourceType.Filename));

        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() > 0x37c)
        {
            Serializer.Serialize(ref DateAdded);
            Serializer.Serialize(ref LevelUnlockSlotId);
            Serializer.Serialize(ref HighlightSound);
            Serializer.Serialize(ref Colour);


            if (Serializer.IsWriting())
                Serializer.GetCurrentSerializer().GetOutput().I32(Type.GetFlags());
            else
                Type = InvObjectBody.FromFlags(Serializer.GetCurrentSerializer().GetInput().I32(),
                    Serializer.GetCurrentSerializer().GetRevision());

            Serializer.Serialize(ref SubType);

            Serializer.Serialize(ref TitleKey);
            Serializer.Serialize(ref DescriptionKey);

            Serializer.Serialize(ref CreationHistory);
            Serializer.Serialize(ref Icon, ResourceType.Texture, true, false, false);
            Serializer.Serialize(ref UserCreatedDetails);
            Serializer.Serialize(ref PhotoData);
            Serializer.Serialize(ref EyetoyData);

            Serializer.Serialize(ref LocationIndex);
            Serializer.Serialize(ref CategoryIndex);
            Serializer.Serialize(ref PrimaryIndex);

            Serializer.Serialize(ref Creator);

            var tempToolTypeValue = ToolType.GetValue();
            Serializer.Serialize(ref tempToolTypeValue);
            ToolType = ToolTypeBody.FromValue(tempToolTypeValue);
            Serializer.Serialize(ref Flags);


            if (Serializer.GetCurrentSerializer().GetRevision().Has(Branch.Double11,
                    (int)Revisions.D_1DETAILS_PROPORTIONAL))
                Serializer.Serialize(ref MakeSizeProportional);

            if (!Serializer.IsWriting())
                UpdateTranslations();

            return;
        }

        if (head < 0x233)
        {
            if (head < 0x174)
            {
                string? tempNullName = null;
                string? tempNullDesc = null;
                Serializer.Serialize(ref tempNullName); // nameTranslationTag
                Serializer.Serialize(ref tempNullDesc); // descTranslationTag
            }
            else
            {
                Serializer.Serialize(ref TranslationTag);
            }

            Serializer.Serialize(ref LocationIndex);
            Serializer.Serialize(ref CategoryIndex);
            if (head > 0x194)
                Serializer.Serialize(ref PrimaryIndex);

            Serializer.Serialize(ref temp_int); // Pad

            if (Serializer.IsWriting())
                Serializer.GetCurrentSerializer().GetOutput().I32(Type.GetFlags(), true);
            else
                Type = InvObjectBody.FromFlags(Serializer.GetCurrentSerializer().GetInput().I32(true),
                    Serializer.GetCurrentSerializer().GetRevision());
            Serializer.Serialize(ref SubType);

            if (head > 0x196)
            {
                var tempToolTypeValue = ToolType.GetValue();
                Serializer.Serialize(ref tempToolTypeValue);
                ToolType = ToolTypeBody.FromValue((byte)tempToolTypeValue);
            }
            Serializer.Serialize(ref Icon, ResourceType.Texture);
            if (head > 0x1c0)
            {
                Serializer.Serialize(ref NumUses);
                Serializer.Serialize(ref LastUsed);
            }

            if (head > 0x14e)
                Serializer.Serialize(ref HighlightSound);
            else
            {
                GUID? tempNullGuid = null;
                Serializer.Serialize(ref tempNullGuid); // Path to highlight sound?
            }

            if (head > 0x156)
                Serializer.Serialize(ref Colour);

            if (head > 0x161)
            {
                Serializer.Serialize(ref EyetoyData);
            }

            // 0x17a < revision && revision < 0x182
            // 0x181 < revision ???
            if (head > 0x17a)
                Serializer.Serialize(ref PhotoData);

            if (head > 0x176)
            {
                LevelUnlockSlotId.SlotType =
                    Serializer.GetCurrentSerializer().Enum32(LevelUnlockSlotId.SlotType);

                Serializer.Serialize(ref LevelUnlockSlotId.SlotNumber);
            }

            if (head > 0x181)
            {
                Serializer.Serialize(ref Copyright);
                Serializer.Serialize(ref Creator);
            }

            if (head > 0x1aa)
            {
                Serializer.Serialize(ref UserCreatedDetails);
                if (UserCreatedDetails is { Name: "", Description: "" })
                    UserCreatedDetails = null;
            }

            if (head > 0x1b0)
                Serializer.Serialize(ref CreationHistory);

            if (head > 0x204)
                Serializer.Serialize(ref AllowEmit);

            if (head > 0x221)
                Serializer.Serialize(ref DateAdded);

            if (head > 0x222)
                Serializer.Serialize(ref Shareable);

            if (!Serializer.IsWriting())
                UpdateTranslations();

            return;
        }

        Serializer.Serialize(ref HighlightSound);

        // In these older versions of the inventory details,
        // 32-bit values are enforced while still using encoded values elsewhere,
        // so for some structures like SlotID, we need to force it manually.

        var tempSlotType = (int)LevelUnlockSlotId.SlotType;
        Serializer.Serialize(ref tempSlotType);
        LevelUnlockSlotId.SlotType = (SlotType)tempSlotType;

        Serializer.Serialize(ref LevelUnlockSlotId.SlotNumber);

        Serializer.Serialize(ref LocationIndex);
        Serializer.Serialize(ref CategoryIndex);
        Serializer.Serialize(ref PrimaryIndex);

        Serializer.Serialize(ref LastUsed);
        Serializer.Serialize(ref NumUses);
        if (head > 0x234)
            Serializer.Serialize(ref temp_int); // Pad


        Serializer.Serialize(ref DateAdded);

        Serializer.Serialize(ref FluffCost);

        Serializer.Serialize(ref Colour);

        if (Serializer.IsWriting())
            Serializer.GetCurrentSerializer().GetOutput().I32(Type.GetFlags(), true);
        else
            Type = InvObjectBody.FromFlags(Serializer.GetCurrentSerializer().GetInput().I32(true),
                Serializer.GetCurrentSerializer().GetRevision());
        Serializer.Serialize(ref SubType);
        var tempToolTypeValue2 = ToolType.GetValue();
        Serializer.Serialize(ref tempToolTypeValue2);
        ToolType = ToolTypeBody.FromValue((byte)tempToolTypeValue2);

        Serializer.Serialize(ref Creator);

        Serializer.Serialize(ref AllowEmit);
        Serializer.Serialize(ref Shareable);
        Serializer.Serialize(ref Copyright);
        if (head > 0x334)
            Serializer.Serialize(ref Flags);

        if (Serializer.GetCurrentSerializer().GetRevision().Has(Branch.Leerdammer, (int)Revisions.LD_LAMS_KEYS) || head > 0x2ba)
        {
            Serializer.Serialize(ref TitleKey);
            Serializer.Serialize(ref DescriptionKey);
        }
        else
            Serializer.Serialize(ref TranslationTag);

        Serializer.Serialize(ref UserCreatedDetails);
        if (UserCreatedDetails is { Name: "", Description: "" })
            UserCreatedDetails = null;

        Serializer.Serialize(ref CreationHistory);

        Serializer.Serialize(ref Icon, ResourceType.Texture, true, false, false);
        Serializer.Serialize(ref PhotoData);
        Serializer.Serialize(ref EyetoyData);

        if (head > 0x358)
            Serializer.Serialize(ref temp_int);

        if (!Serializer.IsWriting())
            UpdateTranslations();


    }
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (TranslationTag != null)
            size += TranslationTag.Length * 2;
        if (UserCreatedDetails != null)
            size += UserCreatedDetails.GetAllocatedSize();
        if (CreationHistory != null)
            size += CreationHistory.GetAllocatedSize();
        if (PhotoData != null)
            size += PhotoData.GetAllocatedSize();
        if (EyetoyData != null)
            size += EyetoyData.GetAllocatedSize();
        return size;
    }

    public Sha1 GenerateHashCode(Revision revision)
    {
        // I wonder how slow this is...
        var serializer = new Serializer(GetAllocatedSize(), revision, 0);
        this.Serialize(serializer);
        return Sha1.FromBuffer(Serializer.GetCurrentSerializer().GetBuffer());
    }

    private void UpdateTranslations()
    {
        if (!string.IsNullOrEmpty(TranslationTag))
        {
            TitleKey =
                RTranslationTable.MakeLamsKeyID(TranslationTag + "_NAME");
            DescriptionKey =
                RTranslationTable.MakeLamsKeyID(TranslationTag + "_DESC");
        }

        /* RTranslationTable lams = ResourceSystem.getLAMS();
        if (lams == null) return;
        if (this.TitleKey != 0)
            this.TranslatedTitle = lams.translate(this.TitleKey);
        if (this.DescriptionKey != 0)
            this.TranslatedDescription = lams.translate(this.DescriptionKey); */
        
        // This should never be implemented as we don't want the ResourceSystem singleton in the library!
    }
}