using System;
using System.Numerics;
using CwLibNet.Resources;
using CwLibNet.Enums;
using static CwLibNet.Enums.Branch;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Server;
using System.Reflection.Emit;
using CwLibNet.Util;
using CwLibNet.Types;

namespace CwLibNet.Structs.Slot 
{
    public class Slot
    {
        public static readonly BASE_ALLOCATION_SIZE = 
            (SlotID.BASE_ALLOCATION_SIZE * 3) + 0x40 + NetworkOnlineID.BASE_ALLOCATION_SIZE + 0x30;
        public SlotID id = new SlotID();
        public ResourceDescriptor root;
        public ResourceDescriptor icon;
        public ResourceDescriptor adventure;
        public Vector4 location = new Vector4(0, 0, 0, 0); // x, y, z, rotation
        public string authorName;
        public string description;
        public string name;
        public string translationTag;
        public SlotID primaryLinkLevel = new SlotID(); 
        public NetworkOnlineID authorID; // used for online play
        public SlotID group = new SlotID(); 
        public bool initiallyLocked;
        public bool shareable;
        public GUID backgroundGUID;
        public LevelType developerLevelType = LevelType.MAIN_PATH; 
        public GameProgressionStatus gameProgressionStatus = GameProgressionStatus.NEW_GAME; 


        // lbp2 fields

        public ResourceDescriptor planetDecorations;
        public Label[] labels;
        public Collectabubble[] collectabubblesRequired;
        public Collectablebubble[] collectabubblesContained;
        public bool isSubLevel; 
        public byte minPlayers = 1; 
        public byte maxPlayers = 4; 
        public bool moveRecommended;
        public bool crossCompatible;
        public bool showOnPlanet = true;
        public byte livesOverride;

        // lbp3 fields
        public bool enforceMinMaxPlayers;
        public int gameMode = 0;
        public bool isGamekit;
        public string entranceName;
        public SlotID originalSlotID = new SlotID(); // used for online play
        public byte customBadgeSize = 1;
        public string localPath;
        public string[] thumbPath;

        // vita fields
        public bool acingEnabled;
        public int[] customRewardEnabled;
        public string[] rewardConditionDescription;
        public int customRewardCondition;
        public float[] amountNeededCustomReward;
        public string[] customRewardDescription;

        public bool containsCollectabubbles;
        public bool sameScreenGame;
        public int sizeOfResources;
        public int sizeOfSubLevels;

        public SlotID[] subLevels; // used for online play
        public ResourceDescriptor slotList;
        public short revision;

        // server specific fields
        public readonly int gameVersion;
        public readonly SHA1[] resources; // used for online play

        public Slot() { }
        
        public Slot(SlotID id, ResourceDescriptor root, Vector4 location)
        {
            this.id = id;
            this.root = root;
            this.location = location;
        }

        public Slot(SlotDescriptor descriptor)
        {
            id = new SlotID(SlotType.USER_CREATED_ON_SERVER, descriptor.id);
            this.name = descriptor.name;
            this.description = descriptor.description;

            if (Strings.IsSHA1(descriptor.root) || Strings.IsGUID(descriptor.root))
            {
                if (descriptor.isAdventurePlanet)
                {
                    this.adventure = new ResourceDescriptor(descriptor.root, ResourceType.ADVENTURE_CREATE_PROFILE);
                }
                else
                {
                    this.root = new ResourceDescriptor(descriptor.root, ResourceType.LEVEL);
                }
            }

            if (Strings.IsSHA1(descriptor.icon) || Strings.IsGUID(descriptor.icon))
            {
                this.icon = new ResourceDescriptor(descriptor.icon, ResourceType.TEXTURE);
            }
            if (descriptor.labels != null)
            {
                this.labels = new Label[descriptor.labels.Length];
                for (int i = 0; i < this.labels.Length; i++)
                {
                    this.labels[i]
                        = new Label(descriptor.labels[i]);
                }
            }

            this.initiallyLocked = descriptor.locked;
            this.isSubLevel = descriptor.subLevel;
            this.shareable = descriptor.shareable != 0;
            if (descriptor.background != 0)
            {
                this.backgroundGUID = new GUID(descriptor.background);
            }
            this.minPlayers = (byte)Math.Clamp(descriptor.minplayers, 1, 4);
            this.maxPlayers = (byte)Math.Clamp(descriptor.maxplayers, 1, 4);
        }

    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        id = serializer.Struct(id, typeof(SlotID));

        root = serializer.Resource(root, ResourceType.LEVEL, true);
        if (subVersion >= Revisions.ADVENTURE)
            adventure = serializer.Resource(adventure, ResourceType.LEVEL, true);
        icon = serializer.Resource(icon, ResourceType.TEXTURE, true);

        location = serializer.Vector4(location);

        authorID = serializer.Struct(authorID, typeof(NetworkOnlineID));
        if (version >= 0x13b)
            authorName = serializer.Wstr(authorName);

        if (version >= 0x183)
            translationTag = serializer.Str(translationTag);

        name = serializer.Wstr(name);
        description = serializer.Wstr(description);

        primaryLinkLevel = serializer.Struct(primaryLinkLevel, typeof(SlotID));
        if (version >= 0x134)
            group = serializer.Struct(group, typeof(SlotID));

        initiallyLocked = serializer.Bool(initiallyLocked);

        if (version > 0x237)
        {
            shareable = serializer.Bool(shareable);
            backgroundGUID = serializer.Guid(backgroundGUID);
        }

        if (version >= 0x333)
            planetDecorations = serializer.Resource(planetDecorations, ResourceType.PLAN, true);

        if (version < 0x188)
            serializer.U8(0); // Unknown

        if (version > 0x1de)
            developerLevelType = serializer.Enum32(developerLevelType);
        else
            serializer.Bool(false); // SideMission

        if (version > 0x1ad && version < 0x1b9)
            serializer.U8(0); // Unknown

        if (version > 0x1b8 && version < 0x36c)
            gameProgressionStatus = serializer.Enum32(gameProgressionStatus);

        if (version <= 0x2c3) return;

        if (version >= 0x33c)
            labels = serializer.Array(labels, typeof(Label));

        if (version >= 0x2ea)
            collectabubblesRequired = serializer.Array(collectabubblesRequired, typeof(Collectabubble));
        if (version >= 0x2f4)
            collectabubblesContained = serializer.Array(collectabubblesContained, typeof(Collectabubble));

        if (version >= 0x352)
            isSubLevel = serializer.Bool(isSubLevel);

        if (version < 0x3d0) return;

        minPlayers = serializer.I8(minPlayers);
        maxPlayers = serializer.I8(maxPlayers);

        if (subVersion >= Revisions.SLOT_ENFORCE_MINMAX)
            enforceMinMaxPlayers = serializer.Bool(enforceMinMaxPlayers);

        if (version >= 0x3d0)
            moveRecommended = serializer.Bool(moveRecommended);

        if (version >= 0x3e9)
            crossCompatible = serializer.Bool(crossCompatible);

        if (version >= 0x3d1)
            showOnPlanet = serializer.Bool(showOnPlanet);
        if (version >= 0x3d2)
            livesOverride = serializer.I8(livesOverride);

        if (revision.IsVita())
        {
            if (revision.Has(Branch.DOUBLE11, Revisions.D1_SLOT_REWARDS))
            {
                acingEnabled = serializer.Bool(acingEnabled);
                customRewardEnabled = serializer.Array<int>(customRewardEnabled);

                if (!serializer.IsWriting())
                    rewardConditionDescription = new string[serializer.GetInput().I32()];
                else serializer.GetOutput().I32(rewardConditionDescription.Length);
                for (int i = 0; i < rewardConditionDescription.Length; ++i)
                    rewardConditionDescription[i] = serializer.Wstr(rewardConditionDescription[i]);

                customRewardCondition = serializer.IntArray(customRewardCondition);
                amountNeededCustomReward = serializer.FloatArray(amountNeededCustomReward);

                if (!serializer.IsWriting())
                    customRewardDescription = new string[serializer.GetInput().I32()];
                else serializer.GetOutput().I32(customRewardDescription.Length);
                for (int i = 0; i < customRewardDescription.Length; ++i)
                    customRewardDescription[i] = serializer.Wstr(customRewardDescription[i]);
            }

            if (revision.Has(Branch.DOUBLE11, Revisions.D1_COLLECTABUBBLES))
                containsCollectabubbles = serializer.Bool(containsCollectabubbles);

            if (revision.Has(Branch.DOUBLE11, Revisions.D1_SLOT_ENFORCE_MINMAX))
                enforceMinMaxPlayers = serializer.Bool(enforceMinMaxPlayers);

            if (revision.Has(Branch.DOUBLE11, Revisions.D1_SLOT_SAME_SCREEN))
                sameScreenGame = serializer.Bool(sameScreenGame);

            if (revision.Has(Branch.DOUBLE11, Revisions.D1_SLOT_DOWNLOAD_DATA))
            {
                sizeOfResources = serializer.I32(sizeOfResources);
                sizeOfSubLevels = serializer.I32(sizeOfSubLevels);
                subLevels = serializer.Array(subLevels, typeof(SlotID));
                slotList = serializer.Resource(slotList, ResourceType.SLOT_LIST, true);
            }

            if (revision.Has(Branch.DOUBLE11, Revisions.D1_SLOT_REVISION))
                this.revision = serializer.I16(this.revision);
        }

        if (!revision.IsLBP3()) return;

        if (subVersion >= Revisions.SLOT_GAME_MODE)
            gameMode = serializer.U8(gameMode);

        if (subVersion >= Revisions.SLOT_GAME_KIT)
            isGameKit = serializer.Bool(isGameKit);

        if (subVersion >= Revisions.SLOT_ENTRANCE_DATA)
        {
            entranceName = serializer.Wstr(entranceName);
            originalSlotID = serializer.Struct(originalSlotID, typeof(SlotID));
        }

        if (subVersion >= Revisions.SLOT_BADGE_SIZE)
            customBadgeSize = serializer.I8(customBadgeSize);

        if (subVersion >= Revisions.SLOT_TRAILER_PATH)
        {
            localPath = serializer.Str(localPath);
            if (subVersion >= Revisions.SLOT_TRAILER_THUMBNAIL)
                thumbPath = serializer.Str(thumbPath);
        }
    }
}