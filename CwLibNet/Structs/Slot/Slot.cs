using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Server;
using CwLibNet.Util;
using CwLibNet.Types;
using CwLibNet.Resources;

namespace CwLibNet.Structs.Slot 
{
    public class Slot : ISerializable
    {
        public static readonly int BaseAllocationSize = 
            (SlotID.BaseAllocationSize * 3) + 0x40 + NetworkOnlineID.BASE_ALLOCATION_SIZE + 0x30;
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
        public Collectabubble[] collectabubblesContained;
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
        public bool isGameKit;
        public string entranceName;
        public SlotID originalSlotID = new SlotID(); // used for online play
        public byte customBadgeSize = 1;
        public string localPath;
        public string thumbPath;

        // vita fields
        public bool acingEnabled;
        public int[] customRewardEnabled;
        public string[] rewardConditionDescription;
        public int[] customRewardCondition;
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
            id = new SlotID(SlotType.USER_CREATED_ON_SERVER, descriptor.Id);
            name = descriptor.Name;
            description = descriptor.Description;

            if (Strings.IsSHA1(descriptor.Root) || Strings.IsGUID(descriptor.Root))
            {
                if (descriptor.IsAdventurePlanet)
                {
                    adventure = new ResourceDescriptor(descriptor.Root, ResourceType.AdventureCreateProfile);
                }
                else
                {
                    root = new ResourceDescriptor(descriptor.Root, ResourceType.Level);
                }
            }

            if (Strings.IsSHA1(descriptor.Icon) || Strings.IsGUID(descriptor.Icon))
            {
                icon = new ResourceDescriptor(descriptor.Icon, ResourceType.Texture);
            }
            if (descriptor.Labels != null)
            {
                labels = new Label[descriptor.Labels.Length];
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i]
                        = new Label((int) RTranslationTable.MakeLamsKeyID(descriptor.Labels[i]), i);
                }
            }

            initiallyLocked = descriptor.Locked;
            isSubLevel = descriptor.IsSubLevel;
            shareable = descriptor.Shareable != 0;
            if (descriptor.Background != 0)
            {
                backgroundGUID = new GUID(descriptor.Background);
            }
            minPlayers = (byte)Math.Clamp(descriptor.Minplayers, 1, 4);
            maxPlayers = (byte)Math.Clamp(descriptor.Maxplayers, 1, 4);
        }

        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int version = revision.GetVersion();
            int subVersion = revision.GetSubVersion();

            id = serializer.Struct<SlotID>(id);

            root = serializer.Resource(root, ResourceType.Level, true);
            if (subVersion >= (int)Revisions.Adventure)
                adventure = serializer.Resource(adventure, ResourceType.Level, true);
            icon = serializer.Resource(icon, ResourceType.Texture, true);

            location = (Vector4)serializer.V4(location);

            authorID = serializer.Struct<NetworkOnlineID>(authorID);
            if (version >= 0x13b)
                authorName = serializer.Wstr(authorName);

            if (version >= 0x183)
                translationTag = serializer.Str(translationTag);

            name = serializer.Wstr(name);
            description = serializer.Wstr(description);

            primaryLinkLevel = serializer.Struct<SlotID>(primaryLinkLevel);
            if (version >= 0x134)
                group = serializer.Struct<SlotID>(group);

            initiallyLocked = serializer.Bool(initiallyLocked);

            if (version > 0x237)
            {
                shareable = serializer.Bool(shareable);
                backgroundGUID = (GUID) serializer.Guid(backgroundGUID);
            }

            if (version >= 0x333)
                planetDecorations = serializer.Resource(planetDecorations, ResourceType.Plan, true);

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
                labels = serializer.Array<Label>(labels);

            if (version >= 0x2ea)
                collectabubblesRequired = serializer.Array<Collectabubble>(collectabubblesRequired);
            if (version >= 0x2f4)
                collectabubblesContained = serializer.Array<Collectabubble>(collectabubblesContained);

            if (version >= 0x352)
                isSubLevel = serializer.Bool(isSubLevel);

            if (version < 0x3d0) return;

            minPlayers = serializer.I8(minPlayers);
            maxPlayers = serializer.I8(maxPlayers);

            if (subVersion >= (int)Revisions.SlotEnforceMinmax)
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
                if (revision.Has(Branch.Double11, (int)Revisions.D1SlotRewards))
                {
                    acingEnabled = serializer.Bool(acingEnabled);
                    customRewardEnabled = serializer.Intarray(customRewardEnabled);

                    if (!serializer.IsWriting())
                        rewardConditionDescription = new string[serializer.GetInput().I32()];
                    else serializer.GetOutput().I32(rewardConditionDescription.Length);
                    for (int i = 0; i < rewardConditionDescription.Length; ++i)
                        rewardConditionDescription[i] = serializer.Wstr(rewardConditionDescription[i]);

                    customRewardCondition = serializer.Intarray(customRewardCondition);
                    amountNeededCustomReward = serializer.Floatarray(amountNeededCustomReward);

                    if (!serializer.IsWriting())
                        customRewardDescription = new string[serializer.GetInput().I32()];
                    else serializer.GetOutput().I32(customRewardDescription.Length);
                    for (int i = 0; i < customRewardDescription.Length; ++i)
                        customRewardDescription[i] = serializer.Wstr(customRewardDescription[i]);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1Collectabubbles))
                    containsCollectabubbles = serializer.Bool(containsCollectabubbles);

                if (revision.Has(Branch.Double11, (int)Revisions.D1SlotEnforceMinmax))
                    enforceMinMaxPlayers = serializer.Bool(enforceMinMaxPlayers);

                if (revision.Has(Branch.Double11, (int)Revisions.D1SlotSameScreen))
                    sameScreenGame = serializer.Bool(sameScreenGame);

                if (revision.Has(Branch.Double11, (int)Revisions.D1SlotDownloadData))
                {
                    sizeOfResources = serializer.I32(sizeOfResources);
                    sizeOfSubLevels = serializer.I32(sizeOfSubLevels);
                    subLevels = serializer.Array<SlotID>(subLevels);
                    slotList = serializer.Resource(slotList, ResourceType.SlotList, true);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1SlotRevision))
                    this.revision = serializer.I16(this.revision);
            }

            // if (!revision.IsLBP3()) return;

            if (subVersion >= (int)Revisions.SlotGameMode)
                gameMode = serializer.U8(gameMode);

            if (subVersion >= (int)Revisions.SlotGameKit)
                isGameKit = serializer.Bool(isGameKit);

            if (subVersion >= (int)Revisions.SlotEntranceData)
            {
                entranceName = serializer.Wstr(entranceName);
                originalSlotID = serializer.Struct<SlotID>(originalSlotID);
            }

            if (subVersion >= (int)Revisions.SlotBadgeSize)
                customBadgeSize = serializer.I8(customBadgeSize);

            if (subVersion >= (int)Revisions.SlotTrailerPath)
            {
                localPath = serializer.Str(localPath);
                if (subVersion >= (int)Revisions.SlotTrailerPath)
                    thumbPath = serializer.Str(thumbPath);
            }
        }

        public int GetAllocatedSize()
        {
            // Implement the logic to calculate the allocated size
            return BaseAllocationSize;
        }
    }
}
