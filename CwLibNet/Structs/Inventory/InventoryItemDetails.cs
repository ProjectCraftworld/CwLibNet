using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Singleton;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using Branch = CwLibNet.Types.Branch;

namespace CwLibNet.Structs.Inventory;

public class InventoryItemDetails: ISerializable
{
    public const int BaseAllocationSize = 0xB0;

    public string? TranslationTag = "";

    public long DateAdded = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

    public SlotID LevelUnlockSlotId = new SlotID();

    public GUID? HighlightSound;

    public int Colour = -1;

    public HashSet<InventoryObjectType> Type = [InventoryObjectType.NONE];
    public int SubType = InventoryObjectSubType.NONE;

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

    public NetworkPlayerID Creator = new NetworkPlayerID();

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

    public InventoryItemDetails(PMetadata metadata)
    {
        // if (metadata.nameTranslationTag != null && metadata.nameTranslationTag.endsWith
        // ("_NAME"))
        //     this.translationTag = metadata.nameTranslationTag.split("_NAME")[0]; // LOL

        this.TitleKey = metadata.TitleKey;
        this.DescriptionKey = metadata.DescriptionKey;
        this.Location = metadata.Location;
        this.Category = metadata.Category;

        this.LocationTag = metadata.LocationTag;
        this.CategoryTag = metadata.CategoryTag;

        this.PrimaryIndex = (short) metadata.PrimaryIndex;
        this.FluffCost = metadata.FluffCost;
        this.Type = metadata.Type;
        this.SubType = metadata.SubType;
        this.Icon = metadata.Icon;

        this.AllowEmit = metadata.AllowEmit;
    }

    public void Serialize(Serializer serializer)
    {
        int head = serializer.GetRevision().GetVersion();

        if (serializer.IsWriting() && HighlightSound != null)
            serializer.AddDependency(new ResourceDescriptor(HighlightSound.Value,
                ResourceType.Filename));

        if (serializer.GetRevision().GetVersion() > 0x37c)
        {
            DateAdded = serializer.S64(DateAdded);
            LevelUnlockSlotId = serializer.Struct<SlotID>(LevelUnlockSlotId);
            HighlightSound = serializer.Guid(HighlightSound);
            Colour = serializer.I32(Colour);


            if (serializer.IsWriting())
                serializer.GetOutput().I32(Type.GetFlags());
            else
                Type = InvObjectBody.FromFlags(serializer.GetInput().I32(),
                    serializer.GetRevision());

            SubType = serializer.I32(SubType);

            TitleKey = serializer.U32(TitleKey);
            DescriptionKey = serializer.U32(DescriptionKey);

            CreationHistory = serializer.Reference(CreationHistory);
            Icon = serializer.Resource(Icon, ResourceType.Texture, true);
            UserCreatedDetails = serializer.Reference(UserCreatedDetails);
            PhotoData = serializer.Reference(PhotoData);
            EyetoyData = serializer.Reference(EyetoyData);

            LocationIndex = serializer.I16(LocationIndex);
            CategoryIndex = serializer.I16(CategoryIndex);
            PrimaryIndex = serializer.I16(PrimaryIndex);

            Creator = serializer.Reference(Creator);

            ToolType = Enums.ToolTypeBody.FromValue(serializer.I8(ToolType.GetValue()));
            Flags = serializer.I8(Flags);


            if (serializer.GetRevision().Has(Branch.Double11,
                (int)Revisions.D1DetailsProportional))
                MakeSizeProportional = serializer.Bool(MakeSizeProportional);

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
                TranslationTag = serializer.Str(TranslationTag);
            }

            LocationIndex = (short) serializer.I32(LocationIndex, true);
            CategoryIndex = (short) serializer.I32(CategoryIndex, true);
            if (head > 0x194)
                PrimaryIndex = (short) serializer.I32(PrimaryIndex, true);

            serializer.I32(0, true); // Pad

            if (serializer.IsWriting())
                serializer.GetOutput().I32(Type.GetFlags(), true);
            else
                Type = InvObjectBody.FromFlags(serializer.GetInput().I32(true),
                    serializer.GetRevision());
            SubType = serializer.I32(SubType, true);

            if (head > 0x196)
                ToolType =
                    Enums.ToolTypeBody.FromValue((byte) serializer.I32(ToolType.GetValue(),
                        true));
            Icon = serializer.Resource(Icon, ResourceType.Texture, true);
            if (head > 0x1c0)
            {
                NumUses = serializer.I32(NumUses, true);
                LastUsed = serializer.I32(LastUsed, true);
            }

            if (head > 0x14e)
                HighlightSound = serializer.Guid(HighlightSound, true);
            else
                serializer.Str(null); // Path to highlight sound?

            if (head > 0x156)
                Colour = serializer.I32(Colour, true);

            if (head > 0x161)
            {
                EyetoyData = serializer.Reference(EyetoyData);
            }

            // 0x17a < revision && revision < 0x182
            // 0x181 < revision ???
            if (head > 0x17a)
                PhotoData = serializer.Reference(PhotoData);

            if (head > 0x176)
            {
                LevelUnlockSlotId.SlotType =
                    SlotTypeExtensions.FromValue(
                        serializer.I32((int)LevelUnlockSlotId.SlotType)
                    );

                LevelUnlockSlotId.SlotNumber =
                    serializer.U32(LevelUnlockSlotId.SlotNumber, true);
            }

            if (head > 0x181)
            {
                Copyright = serializer.Bool(Copyright);
                Creator = serializer.Struct(Creator);
            }

            if (head > 0x1aa)
            {
                UserCreatedDetails = serializer.Struct(UserCreatedDetails);
                if (UserCreatedDetails is { Name: "", Description: "" })
                    UserCreatedDetails = null;
            }

            if (head > 0x1b0)
                CreationHistory = serializer.Struct(CreationHistory);

            if (head > 0x204)
                AllowEmit = serializer.Bool(AllowEmit);

            if (head > 0x221)
                DateAdded = serializer.S64(DateAdded, true);

            if (head > 0x222)
                Shareable = serializer.Bool(Shareable);

            if (!serializer.IsWriting())
                UpdateTranslations();

            return;
        }

        HighlightSound = serializer.Guid(HighlightSound, true);

        // In these older versions of the inventory details,
        // 32 bit values are enforced while still using encoded values elsewhere,
        // so for some structures like SlotID, we need to force it manually.

        LevelUnlockSlotId.SlotType =
            SlotTypeExtensions.FromValue(
                serializer.I32((int)LevelUnlockSlotId.SlotType, true)
            );

        LevelUnlockSlotId.SlotNumber =
            serializer.U32(LevelUnlockSlotId.SlotNumber, true);

        LocationIndex = (short) serializer.I32(LocationIndex, true);
        CategoryIndex = (short) serializer.I32(CategoryIndex, true);
        PrimaryIndex = (short) serializer.I32(PrimaryIndex, true);

        LastUsed = serializer.I32(LastUsed, true);
        NumUses = serializer.I32(NumUses, true);
        if (head > 0x234)
            serializer.I32(0, true); // Pad


        DateAdded = serializer.S64(DateAdded, true);

        FluffCost = serializer.I32(FluffCost, true);

        Colour = serializer.I32(Colour, true);

        if (serializer.IsWriting())
            serializer.GetOutput().I32(Type.GetFlags(), true);
        else
            Type = InvObjectBody.FromFlags(serializer.GetInput().I32(true),
                serializer.GetRevision());
        SubType = serializer.I32(SubType, true);
        ToolType = ToolTypeBody.FromValue((byte) serializer.I32(ToolType.GetValue(),
            true));

        Creator = serializer.Struct(Creator);

        AllowEmit = serializer.Bool(AllowEmit);
        Shareable = serializer.Bool(Shareable);
        Copyright = serializer.Bool(Copyright);
        if (head > 0x334)
            Flags = serializer.I8(Flags);

        if (serializer.GetRevision().Has(Branch.Leerdammer, (int)Revisions.LdLamsKeys) || head > 0x2ba)
        {
            TitleKey = serializer.U32(TitleKey);
            DescriptionKey = serializer.U32(DescriptionKey);
        }
        else
            TranslationTag = serializer.Str(TranslationTag);

        UserCreatedDetails = serializer.Struct(UserCreatedDetails);
        if (UserCreatedDetails is { Name: "", Description: "" })
            UserCreatedDetails = null;

        CreationHistory = serializer.Struct(CreationHistory);

        Icon = serializer.Resource(Icon, ResourceType.Texture, true);
        PhotoData = serializer.Reference(PhotoData);
        EyetoyData = serializer.Reference(EyetoyData);

        if (head > 0x358)
            serializer.U8(0);

        if (!serializer.IsWriting())
            UpdateTranslations();
    }
    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (this.TranslationTag != null)
            size += (this.TranslationTag.Length * 2);
        if (this.UserCreatedDetails != null)
            size += this.UserCreatedDetails.GetAllocatedSize();
        if (this.CreationHistory != null)
            size += this.CreationHistory.GetAllocatedSize();
        if (this.PhotoData != null)
            size += this.PhotoData.GetAllocatedSize();
        if (this.EyetoyData != null)
            size += this.EyetoyData.GetAllocatedSize();
        return size;
    }

    public SHA1 GenerateHashCode(Revision revision)
    {
        // I wonder how slow this is...
        Serializer serializer = new Serializer(this.GetAllocatedSize(), revision, (byte) 0);
        serializer.Struct(this);
        return SHA1.FromBuffer(serializer.GetBuffer());
    }

    private void UpdateTranslations()
    {
        /* if (!string.IsNullOrEmpty(this.TranslationTag))
        {
            this.TitleKey =
                RTranslationTable.makeLamsKeyID(this.TranslationTag + "_NAME");
            this.DescriptionKey =
                RTranslationTable.makeLamsKeyID(this.TranslationTag + "_DESC");
        }

        RTranslationTable lams = ResourceSystem.getLAMS();
        if (lams == null) return;
        if (this.TitleKey != 0)
            this.TranslatedTitle = lams.translate(this.TitleKey);
        if (this.DescriptionKey != 0)
            this.TranslatedDescription = lams.translate(this.DescriptionKey);
            */
        
        throw new NotImplementedException("UpdateTranslations");
    }
}