using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Server;
using CwLibNet.Util;
using CwLibNet.Resources;

namespace CwLibNet.Structs.Slot;

public class Slot : ISerializable
{
    public const int BaseAllocationSize = SlotID.BaseAllocationSize * 3 + 0x40 + NetworkOnlineId.BaseAllocationSize + 0x30;

    public SlotID Id = new();
    public ResourceDescriptor Root;
    public ResourceDescriptor Icon;
    public ResourceDescriptor Adventure;
    public Vector4 Location = new(0, 0, 0, 0); // x, y, z, rotation
    public string AuthorName;
    public string Description;
    public string Name;
    public string TranslationTag;
    public SlotID PrimaryLinkLevel = new(); 
    public NetworkOnlineId AuthorId; // used for online play
    public SlotID Group = new(); 
    public bool InitiallyLocked;
    public bool Shareable;
    public GUID BackgroundGuid;
    public LevelType DeveloperLevelType = LevelType.MAIN_PATH; 
    public GameProgressionStatus GameProgressionStatus = GameProgressionStatus.NEW_GAME; 


    // lbp2 fields

    public ResourceDescriptor PlanetDecorations;
    public Label[]? Labels;
    public Collectabubble[]? CollectabubblesRequired;
    public Collectabubble[]? CollectabubblesContained;
    public bool IsSubLevel; 
    public byte MinPlayers = 1; 
    public byte MaxPlayers = 4; 
    public bool MoveRecommended;
    public bool CrossCompatible;
    public bool ShowOnPlanet = true;
    public byte LivesOverride;

    // lbp3 fields
    public bool EnforceMinMaxPlayers;
    public int GameMode;
    public bool IsGameKit;
    public string EntranceName;
    public SlotID OriginalSlotId = new(); // used for online play
    public byte CustomBadgeSize = 1;
    public string LocalPath;
    public string ThumbPath;

    // vita fields
    public bool AcingEnabled;
    public int[] CustomRewardEnabled;
    public string[] RewardConditionDescription;
    public int[] CustomRewardCondition;
    public float[] AmountNeededCustomReward;
    public string[] CustomRewardDescription;

    public bool ContainsCollectabubbles;
    public bool SameScreenGame;
    public int SizeOfResources;
    public int SizeOfSubLevels;

    public SlotID[]? SubLevels; // used for online play
    public ResourceDescriptor SlotList;
    public short Revision;

    // server specific fields
    public readonly int GameVersion;
    public readonly Sha1[] Resources; // used for online play

    public Slot() { }
        
    public Slot(SlotID id, ResourceDescriptor root, Vector4 location)
    {
        Id = id;
        Root = root;
        Location = location;
    }

    public Slot(SlotDescriptor descriptor)
    {
        Id = new SlotID(SlotType.USER_CREATED_ON_SERVER, descriptor.Id);
        Name = descriptor.Name;
        Description = descriptor.Description;

        if (Strings.IsSHA1(descriptor.Root) || Strings.IsGUID(descriptor.Root))
        {
            if (descriptor.IsAdventurePlanet)
            {
                Adventure = new ResourceDescriptor(descriptor.Root, ResourceType.AdventureCreateProfile);
            }
            else
            {
                Root = new ResourceDescriptor(descriptor.Root, ResourceType.Level);
            }
        }

        if (Strings.IsSHA1(descriptor.Icon) || Strings.IsGUID(descriptor.Icon))
        {
            Icon = new ResourceDescriptor(descriptor.Icon, ResourceType.Texture);
        }
        if (descriptor.Labels != null)
        {
            Labels = new Label[descriptor.Labels.Length];
            for (var i = 0; i < Labels.Length; i++)
            {
                Labels[i]
                    = new Label((int) RTranslationTable.MakeLamsKeyID(descriptor.Labels[i]), i);
            }
        }

        InitiallyLocked = descriptor.Locked;
        IsSubLevel = descriptor.IsSubLevel;
        Shareable = descriptor.Shareable != 0;
        if (descriptor.Background != 0)
        {
            BackgroundGuid = new GUID(descriptor.Background);
        }
        MinPlayers = (byte)Math.Clamp(descriptor.Minplayers, 1, 4);
        MaxPlayers = (byte)Math.Clamp(descriptor.Maxplayers, 1, 4);
    }

    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Id = serializer.Struct<SlotID>(Id);

        Root = serializer.Resource(Root, ResourceType.Level, true);
        if (subVersion >= (int)Revisions.ADVENTURE)
            Adventure = serializer.Resource(Adventure, ResourceType.Level, true);
        Icon = serializer.Resource(Icon, ResourceType.Texture, true);

        Location = (Vector4)serializer.V4(Location);

        AuthorId = serializer.Struct<NetworkOnlineId>(AuthorId);
        if (version >= 0x13b)
            AuthorName = serializer.Wstr(AuthorName);

        if (version >= 0x183)
            TranslationTag = serializer.Str(TranslationTag);

        Name = serializer.Wstr(Name);
        Description = serializer.Wstr(Description);

        PrimaryLinkLevel = serializer.Struct<SlotID>(PrimaryLinkLevel);
        if (version >= 0x134)
            Group = serializer.Struct<SlotID>(Group);

        InitiallyLocked = serializer.Bool(InitiallyLocked);

        if (version > 0x237)
        {
            Shareable = serializer.Bool(Shareable);
            BackgroundGuid = (GUID) serializer.Guid(BackgroundGuid);
        }

        switch (version)
        {
            case >= 0x333:
                PlanetDecorations = serializer.Resource(PlanetDecorations, ResourceType.Plan, true);
                break;
            case < 0x188:
                serializer.U8(0); // Unknown
                break;
        }

        if (version > 0x1de)
            DeveloperLevelType = serializer.Enum32(DeveloperLevelType);
        else
            serializer.Bool(false); // SideMission

        switch (version)
        {
            case > 0x1ad and < 0x1b9:
                serializer.U8(0); // Unknown
                break;
            case > 0x1b8 and < 0x36c:
                GameProgressionStatus = serializer.Enum32(GameProgressionStatus);
                break;
        }

        switch (version)
        {
            case <= 0x2c3:
                return;
            case >= 0x33c:
                Labels = serializer.Array<Label>(Labels);
                break;
        }

        if (version >= 0x2ea)
            CollectabubblesRequired = serializer.Array<Collectabubble>(CollectabubblesRequired);
        if (version >= 0x2f4)
            CollectabubblesContained = serializer.Array<Collectabubble>(CollectabubblesContained);

        if (version >= 0x352)
            IsSubLevel = serializer.Bool(IsSubLevel);

        if (version < 0x3d0) return;

        MinPlayers = serializer.I8(MinPlayers);
        MaxPlayers = serializer.I8(MaxPlayers);

        if (subVersion >= (int)Revisions.SLOT_ENFORCE_MINMAX)
            EnforceMinMaxPlayers = serializer.Bool(EnforceMinMaxPlayers);

        if (version >= 0x3d0)
            MoveRecommended = serializer.Bool(MoveRecommended);

        if (version >= 0x3e9)
            CrossCompatible = serializer.Bool(CrossCompatible);

        if (version >= 0x3d1)
            ShowOnPlanet = serializer.Bool(ShowOnPlanet);
        if (version >= 0x3d2)
            LivesOverride = serializer.I8(LivesOverride);

        if (revision.IsVita())
        {
            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_REWARDS))
            {
                AcingEnabled = serializer.Bool(AcingEnabled);
                CustomRewardEnabled = serializer.Intarray(CustomRewardEnabled);

                if (!serializer.IsWriting())
                    RewardConditionDescription = new string[serializer.GetInput().I32()];
                else serializer.GetOutput().I32(RewardConditionDescription.Length);
                for (var i = 0; i < RewardConditionDescription.Length; ++i)
                    RewardConditionDescription[i] = serializer.Wstr(RewardConditionDescription[i]);

                CustomRewardCondition = serializer.Intarray(CustomRewardCondition);
                AmountNeededCustomReward = serializer.Floatarray(AmountNeededCustomReward);

                if (!serializer.IsWriting())
                    CustomRewardDescription = new string[serializer.GetInput().I32()];
                else serializer.GetOutput().I32(CustomRewardDescription.Length);
                for (var i = 0; i < CustomRewardDescription.Length; ++i)
                    CustomRewardDescription[i] = serializer.Wstr(CustomRewardDescription[i]);
            }

            if (revision.Has(Branch.Double11, (int)Revisions.D1_COLLECTABUBBLES))
                ContainsCollectabubbles = serializer.Bool(ContainsCollectabubbles);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_ENFORCE_MINMAX))
                EnforceMinMaxPlayers = serializer.Bool(EnforceMinMaxPlayers);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_SAME_SCREEN))
                SameScreenGame = serializer.Bool(SameScreenGame);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_DOWNLOAD_DATA))
            {
                SizeOfResources = serializer.I32(SizeOfResources);
                SizeOfSubLevels = serializer.I32(SizeOfSubLevels);
                SubLevels = serializer.Array<SlotID>(SubLevels);
                SlotList = serializer.Resource(SlotList, ResourceType.SlotList, true);
            }

            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_REVISION))
                Revision = serializer.I16(Revision);
        }

        // if (!revision.IsLBP3()) return;

        if (subVersion >= (int)Revisions.SLOT_GAME_MODE)
            GameMode = serializer.U8(GameMode);

        if (subVersion >= (int)Revisions.SLOT_GAME_KIT)
            IsGameKit = serializer.Bool(IsGameKit);

        if (subVersion >= (int)Revisions.SLOT_ENTRANCE_DATA)
        {
            EntranceName = serializer.Wstr(EntranceName);
            OriginalSlotId = serializer.Struct<SlotID>(OriginalSlotId);
        }

        if (subVersion >= (int)Revisions.SLOT_BADGE_SIZE)
            CustomBadgeSize = serializer.I8(CustomBadgeSize);

        if (subVersion >= (int)Revisions.SLOT_TRAILER_PATH)
        {
            LocalPath = serializer.Str(LocalPath);
            if (subVersion >= (int)Revisions.SLOT_TRAILER_PATH)
                ThumbPath = serializer.Str(ThumbPath);
        }
    }

    public int GetAllocatedSize()
    {
        // Implement the logic to calculate the allocated size
        return BaseAllocationSize;
    }
}