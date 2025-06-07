using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Resources;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.Types.Data;
using Branch = CwLibNet.Enums.Branch;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Inventory;

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

    public void Serialize()
    {
        var head = Serializer.GetRevision().GetVersion();

        if (Serializer.IsWriting() && HighlightSound != null)
            Serializer.AddDependency(new ResourceDescriptor(HighlightSound.Value,
                ResourceType.Filename));

        if (Serializer.GetRevision().GetVersion() > 0x37c)
        {
            DateAdded = Serializer.Serialize(ref DateAdded);
            Serializer.Serialize(ref LevelUnlockSlotId);
            HighlightSound = Serializer.Serialize(ref HighlightSound);
            Serializer.Serialize(ref Colour);


            if (Serializer.IsWriting())
                Serializer.GetOutput().I32(Type.GetFlags());
            else
                Type = InvObjectBody.FromFlags(Serializer.GetInput().I32(),
                    Serializer.GetRevision());

            Serializer.Serialize(ref SubType);

            Serializer.Serialize(ref TitleKey);
            Serializer.Serialize(ref DescriptionKey);

            Serializer.Serialize(ref CreationHistory);
            Serializer.Serialize(ref Icon, Icon, ResourceType.Texture, true);
            Serializer.Serialize(ref UserCreatedDetails);
            Serializer.Serialize(ref PhotoData);
            Serializer.Serialize(ref EyetoyData);

            Serializer.Serialize(ref LocationIndex);
            Serializer.Serialize(ref CategoryIndex);
            Serializer.Serialize(ref PrimaryIndex);

            Serializer.Serialize(ref Creator);

            ToolType = ToolTypeBody.FromValue(Serializer.Serialize(ref ToolType.GetValue()));
            Serializer.Serialize(ref Flags);


            if (Serializer.GetRevision().Has(Branch.Double11,
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
                Serializer.Serialize(ref null); // nameTranslationTag
                Serializer.Serialize(ref null); // descTranslationTag
            }
            else
            {
                Serializer.Serialize(ref TranslationTag);
            }

            LocationIndex = (short) Serializer.Serialize(ref LocationIndex);
            CategoryIndex = (short) Serializer.Serialize(ref CategoryIndex);
            if (head > 0x194)
                PrimaryIndex = (short) Serializer.Serialize(ref PrimaryIndex);

            Serializer.Serialize(ref 0); // Pad

            if (Serializer.IsWriting())
                Serializer.GetOutput().I32(Type.GetFlags(), true);
            else
                Type = InvObjectBody.FromFlags(Serializer.GetInput().I32(true),
                    Serializer.GetRevision());
            SubType = Serializer.Serialize(ref SubType);

            if (head > 0x196)
                ToolType =
                    ToolTypeBody.FromValue((byte) Serializer.Serialize(ref ToolType.GetValue(),
                        true));
            Serializer.Serialize(ref Icon, Icon, ResourceType.Texture, true);
            if (head > 0x1c0)
            {
                NumUses = Serializer.Serialize(ref NumUses);
                LastUsed = Serializer.Serialize(ref LastUsed);
            }

            if (head > 0x14e)
                HighlightSound = Serializer.Serialize(ref HighlightSound);
            else
                Serializer.Serialize(ref null); // Path to highlight sound?

            if (head > 0x156)
                Colour = Serializer.Serialize(ref Colour);

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
                    (SlotType)Serializer.Serialize(ref (int)LevelUnlockSlotId.SlotType);

                LevelUnlockSlotId.SlotNumber =
                    Serializer.Serialize(ref LevelUnlockSlotId.SlotNumber, true);
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
                DateAdded = Serializer.Serialize(ref DateAdded);

            if (head > 0x222)
                Serializer.Serialize(ref Shareable);

            if (!Serializer.IsWriting())
                UpdateTranslations();

            return;
        }

        HighlightSound = Serializer.Serialize(ref HighlightSound);

        // In these older versions of the inventory details,
        // 32-bit values are enforced while still using encoded values elsewhere,
        // so for some structures like SlotID, we need to force it manually.

        LevelUnlockSlotId.SlotType =
            (SlotType)Serializer.Serialize(ref (int)LevelUnlockSlotId.SlotType, true);

        LevelUnlockSlotId.SlotNumber =
            Serializer.Serialize(ref LevelUnlockSlotId.SlotNumber, true);

        LocationIndex = (short) Serializer.Serialize(ref LocationIndex);
        CategoryIndex = (short) Serializer.Serialize(ref CategoryIndex);
        PrimaryIndex = (short) Serializer.Serialize(ref PrimaryIndex);

        LastUsed = Serializer.Serialize(ref LastUsed);
        NumUses = Serializer.Serialize(ref NumUses);
        if (head > 0x234)
            Serializer.Serialize(ref 0); // Pad


        DateAdded = Serializer.Serialize(ref DateAdded);

        FluffCost = Serializer.Serialize(ref FluffCost);

        Colour = Serializer.Serialize(ref Colour);

        if (Serializer.IsWriting())
            Serializer.GetOutput().I32(Type.GetFlags(), true);
        else
            Type = InvObjectBody.FromFlags(Serializer.GetInput().I32(true),
                Serializer.GetRevision());
        SubType = Serializer.Serialize(ref SubType);
        ToolType = ToolTypeBody.FromValue((byte) Serializer.Serialize(ref ToolType.GetValue(),
            true));

        Serializer.Serialize(ref Creator);

        Serializer.Serialize(ref AllowEmit);
        Serializer.Serialize(ref Shareable);
        Serializer.Serialize(ref Copyright);
        if (head > 0x334)
            Serializer.Serialize(ref Flags);

        if (Serializer.GetRevision().Has(Branch.Leerdammer, (int)Revisions.LD_LAMS_KEYS) || head > 0x2ba)
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

        Serializer.Serialize(ref Icon, Icon, ResourceType.Texture, true);
        Serializer.Serialize(ref PhotoData);
        Serializer.Serialize(ref EyetoyData);

        if (head > 0x358)
            Serializer.Serialize(ref 0);

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
        Serializer.Serialize(ref this);
        return Sha1.FromBuffer(Serializer.GetBuffer());
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