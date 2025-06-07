using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Server;
using CwLibNet.Util;
using CwLibNet.Resources;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using static CwLibNet.IO.Serializer.Serializer;

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

    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Serializer.Serialize(ref Id);

        Serializer.Serialize(ref Root, ResourceType.Level, true, true, true);
        if (subVersion >= (int)Revisions.ADVENTURE)
            Serializer.Serialize(ref Adventure, ResourceType.Level, true, true, true);
        Serializer.Serialize(ref Icon, ResourceType.Texture, true, true, true);

        Serializer.Serialize(ref Location);

        Serializer.Serialize(ref AuthorId);
        if (version >= 0x13b)
            Serializer.Serialize(ref AuthorName);

        if (version >= 0x183)
            Serializer.Serialize(ref TranslationTag);

        Serializer.Serialize(ref Name);
        Serializer.Serialize(ref Description);

        Serializer.Serialize(ref PrimaryLinkLevel);
        if (version >= 0x134)
            Serializer.Serialize(ref Group);

        Serializer.Serialize(ref InitiallyLocked);

        if (version > 0x237)
        {
            Serializer.Serialize(ref Shareable);
            Serializer.Serialize(ref BackgroundGuid);
        }

        switch (version)
        {
            case >= 0x333:
                Serializer.Serialize(ref PlanetDecorations, ResourceType.Plan, true, true, true);
                break;
            case < 0x188:
                Serializer.Serialize(ref temp_int); // Unknown
                break;
        }

        if (version > 0x1de)
            DeveloperLevelType = serializer.Enum32(DeveloperLevelType);
        else
            Serializer.Serialize(ref temp_bool_false); // SideMission

        switch (version)
        {
            case > 0x1ad and < 0x1b9:
                Serializer.Serialize(ref temp_int); // Unknown
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
                Serializer.Serialize(ref Labels);
                break;
        }

        if (version >= 0x2ea)
            Serializer.Serialize(ref CollectabubblesRequired);
        if (version >= 0x2f4)
            Serializer.Serialize(ref CollectabubblesContained);

        if (version >= 0x352)
            Serializer.Serialize(ref IsSubLevel);

        if (version < 0x3d0) return;

        Serializer.Serialize(ref MinPlayers);
        Serializer.Serialize(ref MaxPlayers);

        if (subVersion >= (int)Revisions.SLOT_ENFORCE_MINMAX)
            Serializer.Serialize(ref EnforceMinMaxPlayers);

        if (version >= 0x3d0)
            Serializer.Serialize(ref MoveRecommended);

        if (version >= 0x3e9)
            Serializer.Serialize(ref CrossCompatible);

        if (version >= 0x3d1)
            Serializer.Serialize(ref ShowOnPlanet);
        if (version >= 0x3d2)
            Serializer.Serialize(ref LivesOverride);

        if (revision.IsVita())
        {
            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_REWARDS))
            {
                Serializer.Serialize(ref AcingEnabled);
                Serializer.Serialize(ref CustomRewardEnabled);

                if (!Serializer.IsWriting())
                    RewardConditionDescription = new string[Serializer.GetCurrentSerializer().GetInput().I32()];
                else Serializer.GetCurrentSerializer().GetOutput().I32(RewardConditionDescription.Length);
                for (var i = 0; i < RewardConditionDescription.Length; ++i)
                    Serializer.Serialize(ref RewardConditionDescription[i]);

                Serializer.Serialize(ref CustomRewardCondition);
                Serializer.Serialize(ref AmountNeededCustomReward);

                if (!Serializer.IsWriting())
                    CustomRewardDescription = new string[Serializer.GetCurrentSerializer().GetInput().I32()];
                else Serializer.GetCurrentSerializer().GetOutput().I32(CustomRewardDescription.Length);
                for (var i = 0; i < CustomRewardDescription.Length; ++i)
                    Serializer.Serialize(ref CustomRewardDescription[i]);
            }

            if (revision.Has(Branch.Double11, (int)Revisions.D1_COLLECTABUBBLES))
                Serializer.Serialize(ref ContainsCollectabubbles);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_ENFORCE_MINMAX))
                Serializer.Serialize(ref EnforceMinMaxPlayers);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_SAME_SCREEN))
                Serializer.Serialize(ref SameScreenGame);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_DOWNLOAD_DATA))
            {
                Serializer.Serialize(ref SizeOfResources);
                Serializer.Serialize(ref SizeOfSubLevels);
                Serializer.Serialize(ref SubLevels);
                Serializer.Serialize(ref SlotList, ResourceType.SlotList, true, true, true);
            }

            if (revision.Has(Branch.Double11, (int)Revisions.D1_SLOT_REVISION))
                Serializer.Serialize(ref Revision);
        }

        // if (!revision.IsLBP3()) return;

        if (subVersion >= (int)Revisions.SLOT_GAME_MODE)
            Serializer.Serialize(ref GameMode);

        if (subVersion >= (int)Revisions.SLOT_GAME_KIT)
            Serializer.Serialize(ref IsGameKit);

        if (subVersion >= (int)Revisions.SLOT_ENTRANCE_DATA)
        {
            Serializer.Serialize(ref EntranceName);
            Serializer.Serialize(ref OriginalSlotId);
        }

        if (subVersion >= (int)Revisions.SLOT_BADGE_SIZE)
            Serializer.Serialize(ref CustomBadgeSize);

        if (subVersion >= (int)Revisions.SLOT_TRAILER_PATH)
        {
            Serializer.Serialize(ref LocalPath);
            if (subVersion >= (int)Revisions.SLOT_TRAILER_PATH)
                Serializer.Serialize(ref ThumbPath);
        }
    }

    public int GetAllocatedSize()
    {
        // Implement the logic to calculate the allocated size
        return BaseAllocationSize;
    }
}