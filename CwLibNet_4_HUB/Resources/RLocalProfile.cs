using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Profile;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Resources
{
    public class RLocalProfile : Resource
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;

        /// <summary>
        /// List of items this user has.
        /// </summary>
        public List<InventoryItem> Inventory { get; set; } = [];

        /// <summary>
        /// Stores categories and locations referenced by items.
        /// </summary>
        public StringLookupTable StringTable { get; set; } = new();

        /// <summary>
        /// Whether or not this profile was from a production build.
        /// </summary>
        public bool FromProductionBuild { get; set; } = true;

        public Dictionary<TutorialLevel, TutorialState> Lbp1TutorialStates { get; set; } = [];

        public int[] DlcPackViewed { get; set; }
        public int[] DlcPackShown { get; set; }

        public int Lbp1MainMenuButtonUnlocks { get; set; }

        public List<PlayedLevelData> PlayedLevelData { get; set; }

        public List<ViewedLevelData> ViewedLevelData { get; set; }
        public List<ViewedPlayerData> ViewedPlayerData { get; set; }

        public Dictionary<SlotID, Slot> OldMyMoonSlots { get; set; } = [];

        public bool CopiedFromAnotherUser { get; set; }

        public bool FixedUpByDeveloper { get; set; }

        public bool ShowSaveGameInfo { get; set; }

        public int TotalLevelCount { get; set; }

        public int LastMmPicksCount { get; set; }

        public long LastNewsItemTimestamp { get; set; }

        public long LastStreamEventTimestamp { get; set; }

        public long LolcatFtwTimestamp { get; set; }

        public string[] OldTextSearches { get; set; }

        public SlotLink[]? OldDiscoveredLinks { get; set; }
        public SlotLink[]? PendingLinks { get; set; }
        public SlotID LastPlayed { get; set; } = new();

        public int[] Lbp1VideosPlayed { get; set; }

        public int[] Lbp2VideosPlayed { get; set; }

        public PlayerMetrics CurrentPlayerMetrics { get; set; }

        public PlayerMetrics LastPostedPlayerMetrics { get; set; }

        public GameProgressionStatus Lbp1GameProgressionStatus { get; set; } = GameProgressionStatus.NEW_GAME;

        public int Lbp1GameProgressionEventHappenedBits { get; set; }

        public int Lbp1GameProgressionEventsExplainedBits { get; set; }

        public int DemoProgressionStatus { get; set; }

        public int Lbp2GameProgressionFlags { get; set; }

        public InventoryItem[]? PendingInventoryItems { get; set; }

        public ResourceDescriptor PodLevel { get; set; }

        public NetworkPlayerID PlayerId { get; set; }

        public NetworkPlayerID OwnerPlayerId { get; set; }

        public OpenPSID OwnerPsid { get; set; }

        public int OwnerUserId { get; set; }

        public Sha1 EulaAgreed { get; set; }

        public NetworkPlayerID AcceptingPlayer { get; set; }

        public ResourceDescriptor SyncedProfile { get; set; }

        public NetworkPlayerID[]? VoipMutedPlayers { get; set; }

        public bool VoipChatPaused { get; set; }

        public bool EnableMusicInPlayMode { get; set; }

        public bool EnableMusicInPod { get; set; }

        public LegacyInventoryCollection[] LegacyInventoryCollections { get; set; }

        public InventoryView[] LegacyInventoryViews { get; set; }

        public InventoryCollection[] InventoryCollections { get; set; }

        public ResourceDescriptor AvatarIcon { get; set; }

        public ResourceDescriptor SaveIcon { get; set; }

        public int Lbp1CreateModeVOProgress { get; set; }

        public float Gamma { get; set; }

        public float ScreenSize { get; set; }

        public bool HasSeenCalibrationScreen { get; set; }

        public int[] Lbp1VOPlayed { get; set; }

        public int[] Lbp2VOPlayed { get; set; }

        public int SubtitleMode { get; set; }

        public PinsAwarded PinsAwarded { get; set; }

        public bool UserSettingPhotoBooth { get; set; }

        public bool UserSettingCollection { get; set; }
        
        public bool UserSettingAdvancedEditMode { get; set; }

        public bool ShowThermometer { get; set; }

        public bool SaveOnlinePlaySettings { get; set; }

        public int OnlinePlayMode { get; set; }

        public int FriendJoinRequestMode { get; set; }

        public int NonFriendJoinRequestMode { get; set; }

        public int[] Lbp2TutorialsPlayed { get; set; }

        public int[] SectionHeadingToggled { get; set; }

        public MysteryPodEventSeen[]? MysteryPodEventsSeen { get; set; }

        public Sha1[] LastLegacyImportedProfileHashLBP1 { get; set; }

        public Sha1[] LastLegacyImportedProfileHashLBP2 { get; set; }

        public bool PlayedLBP1 { get; set; }

        public bool HasPerformedFirstRun { get; set; }

        public bool Lbp2TutorialUnlockButtonUnleashed { get; set; }

        public int[] HiddenCategories { get; set; }

        public CollectedBubble[]? CollectedBubbles { get; set; }

        public int NumWaterLevelsPlayed { get; set; }

        public bool AutoFilterOn { get; set; }

        public bool HasUsed6Axis { get; set; }
        public bool HasUsedMove { get; set; }

        public PaintProperties PaintProperties { get; set; }

        public PlayerDataLabels PlayerDataLabels { get; set; }

    /* Vita stuff */

        /// <summary>
        /// 3G connectivity stats for Vita.
        /// </summary>
        public int Total3GUpStream { get; set; }
        public int Total3GDownStream { get; set; }

        /// <summary>
        /// Touch settings for Vita create mode.
        /// </summary>
        public bool CreateRearTouchPan { get; set; }
        public bool CreateRearTouchPinchZoom { get; set; }
        public bool CreateRearTapZoom { get; set; }

        /// <summary>
        /// Whether to show frame rate warning messages in create mode.
        /// </summary>
        public bool CreateFrameRateWarningMessages { get; set; }

        /// <summary>
        /// Online beta network timestamp.
        /// </summary>
        public long OnlineBetaNetworkTimestamp { get; set; }

        /// <summary>
        /// Online beta stats.
        /// </summary>
        public int OnlineBetaPlayTimeSinceOnline { get; set; }
        public int OnlineBetaBootsSinceOnline { get; set; }

        /// <summary>
        /// Distance moved whilst attracted.
        /// </summary>
        public float DistanceMovedWhilstAttracted { get; set; }

        /// <summary>
        /// Beakers used in story mode.
        /// </summary>
        public int BeakersUsedPart1 { get; set; }
        public int BeakersUsedPart2 { get; set; }

        /// <summary>
        /// Profile flags.
        /// </summary>
        public int ProfileFlags { get; set; }

        /// <summary>
        /// Gold medal stats.
        /// </summary>
        public byte GoldMedalsAwarded { get; set; }
        public byte GoldMedalStoryArcPins { get; set; }

        /// <summary>
        /// Social media settings.
        /// </summary>
        public bool TwitterEnabled { get; set; }
        public bool FacebookEnabled { get; set; }
        public string TwitterAccessToken { get; set; }
        public string TwitterAccessTokenSecret { get; set; }

        /// <summary>
        /// Whether the player has played LBP2.
        /// </summary>
        public bool PlayedLBP2 { get; set; }

        /// <summary>
        /// Challenge tutorial settings.
        /// </summary>
        public bool CreateChallengeTutorialSeen { get; set; }
        public bool PlayChallengeTutorialSeen { get; set; }

        /// <summary>
        /// Owned LBP games.
        /// </summary>
        public int OwnedLBP { get; set; }

        /// <summary>
        /// DCE UUID state (LBP3).
        /// </summary>
        public int DceUuidState { get; set; }

        /// <summary>
        /// Whether the last played level was PPP (LBP3).
        /// </summary>
        public bool LastPlayedPPP { get; set; }

        /// <summary>
        /// Last played planet slot ID (LBP3).
        /// </summary>
        public SlotID LastPlayedPlanet { get; set; }

        /// <summary>
        /// Whether the last played level was an Earth adventure (LBP3).
        /// </summary>
        public bool LastPlayedEarthAdv { get; set; }

        /// <summary>
        /// Whether the player has seen the dive in beta message (LBP3).
        /// </summary>
        public bool HasSeenDiveInBetaMessage { get; set; }

        /// <summary>
        /// Touch create cursor mode (LBP3).
        /// </summary>
        public int TouchCreateCursorMode { get; set; }

        /// <summary>
        /// Whether to show advanced edit mode message counter (LBP3).
        /// </summary>
        public bool ShowAdvancedEditModeMessageCounter { get; set; }

        /// <summary>
        /// Whether to show adventure save warning (LBP3).
        /// </summary>
        public bool ShowAdventureSaveWarning { get; set; }

        /// <summary>
        /// Total frames spent in edit mode (LBP3).
        /// </summary>
        public int TotalFramesInEditMode { get; set; }

        /// <summary>
        /// Online tutorials played (LBP3).
        /// </summary>
        public Sha1[] OnlineTutorialsPlayed { get; set; }

        /// <summary>
        /// Popit puzzles completed (LBP3).
        /// </summary>
        public SlotID[]? PopitPuzzlesCompleted { get; set; }

        /// <summary>
        /// Whether the timesaver notice has been viewed (LBP3).
        /// </summary>
        public bool TimesaverNoticeViewed { get; set; }

        /// <summary>
        /// Quest progress pin (LBP3).
        /// </summary>
        public int QuestProgressPin { get; set; }

        /// <summary>
        /// Activity filter toggle settings (LBP3).
        /// </summary>
        public int[] ActivityFilterToggled { get; set; }

        /// <summary>
        /// Last landing page focus items (LBP3).
        /// </summary>
        public int LastLandingPageFocusItems { get; set; }

        /// <summary>
        /// Go Play cache (LBP3).
        /// </summary>
        public GoPlayCache[]? GoPlayCache { get; set; }

        public override void Serialize()
        {
            var revision = Serializer.GetRevision();
            var version = revision.GetVersion();
            var subVersion = revision.GetSubVersion();

            if (version < 0x187)
            {
                // descriptor array with types, no pointer
                return;
            }

            Serializer.Serialize(ref Inventory);
            StringTable = Serializer.Serialize(ref StringTable);

            switch (version)
            {
                case >= 0x3b6:
                    Serializer.Serialize(ref FromProductionBuild);
                    break;
                case >= 0x133 and < 0x1df:
                    // SlotID[]
                    break;
            }

            if (version >= 0x1e4)
            {
                if (!Serializer.IsWriting())
                {
                    var keys = new List<TutorialLevel>();
                    var values = new List<TutorialState>();
                    var count = Serializer.GetInput().I32();
                    for (var i = 0; i < count; ++i)
                    {
                        keys.Add(Serializer.GetInput().Enum32<TutorialLevel>());
                        values.Add(Serializer.GetInput().Enum32<TutorialState>());
                    }

                    Lbp1TutorialStates = new Dictionary<TutorialLevel, TutorialState>();
                    for (var i = 0; i < count; ++i)
                        Lbp1TutorialStates[keys[i]] = values[i];
                }
                else
                {
                    var keys = new List<TutorialLevel>(Lbp1TutorialStates.Keys);
                    Serializer.GetOutput().I32(keys.Count);
                    foreach (var key in keys)
                    {
                        Serializer.GetOutput().Enum32(key);
                        Serializer.GetOutput().Enum32(Lbp1TutorialStates[key]);
                    }
                }
            }

            if (version >= 0x266)
            {
                Serializer.Serialize(ref DlcPackViewed);
                Serializer.Serialize(ref DlcPackShown);
            }

            if (version >= 0x1e5)
                Serializer.Serialize(ref Lbp1MainMenuButtonUnlocks);

            if (version >= 0x16e)
                Serializer.Serialize(ref PlayedLevelData);

            if (version >= 0x37a)
            {
                Serializer.Serialize(ref ViewedLevelData);
                Serializer.Serialize(ref ViewedPlayerData);
            }

            if (version >= 0x16e)
            {
                if (!Serializer.IsWriting())
                {
                    var count = Serializer.GetInput().I32();
                    OldMyMoonSlots = new Dictionary<SlotID, Slot>(count);
                    for (var i = 0; i < count; ++i)
                    {
                        OldMyMoonSlots[Serializer.Serialize(ref default(SlotID))] =
                            Serializer.Serialize(ref default(Slot));
                    }
                }
                else
                {
                    Serializer.GetOutput().I32(OldMyMoonSlots.Count);
                    foreach (var entry in OldMyMoonSlots)
                    {
                        Serializer.Serialize(ref entry.Key);
                        Serializer.Serialize(ref entry.Value);
                    }
                }
            }

            if (version >= 0x20e)
                Serializer.Serialize(ref CopiedFromAnotherUser);

            if (version >= 0x268)
                Serializer.Serialize(ref FixedUpByDeveloper);

            if (version >= 0x297 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_SAVEGAME_INFO))
            {
                Serializer.Serialize(ref ShowSaveGameInfo);
                Serializer.Serialize(ref TotalLevelCount);
            }

            if (version >= 0x394)
                Serializer.Serialize(ref LastMmPicksCount);

            if (version >= 0x2cc || revision.Has(Branch.Leerdammer, (int)Revisions.LD_NEWS_TIMESTAMP))
                LastNewsItemTimestamp = Serializer.Serialize(ref LastNewsItemTimestamp);

            if (version >= 0x324)
                LastStreamEventTimestamp = Serializer.Serialize(ref LastStreamEventTimestamp);

            if (version >= 0x2cd || revision.Has(Branch.Leerdammer, (int)Revisions.LD_QUEUE))
                LolcatFtwTimestamp = Serializer.Serialize(ref LolcatFtwTimestamp);

            if (version >= 0x328)
            {
                if (!Serializer.IsWriting())
                    OldTextSearches = new string[Serializer.GetInput().I32()];
                else
                    Serializer.GetOutput().I32(OldTextSearches.Length);
                for (var i = 0; i < OldTextSearches.Length; ++i)
                    Serializer.Serialize(ref OldTextSearches[i]);
            }

            if (version >= 0x32a)
            {
                Serializer.Serialize(ref OldDiscoveredLinks);
                Serializer.Serialize(ref PendingLinks);
            }

            if (version >= 0x239)
                Serializer.Serialize(ref LastPlayed);

            if (version >= 0x1c2)
                Serializer.Serialize(ref Lbp1VideosPlayed);
        
            // ArrayList<GUID>
            if (version >= 0x219)
                Serializer.Serialize(ref Lbp2VideosPlayed);

            if (version >= 0x16f)
                Serializer.Serialize(ref CurrentPlayerMetrics);

            if (version >= 0x1ab)
                Serializer.Serialize(ref LastPostedPlayerMetrics);

            if (version >= 0x1e1)
                Serializer.Serialize(ref Lbp1GameProgressionStatus);

            if (version >= 0x1cf)
            {
                Serializer.Serialize(ref Lbp1GameProgressionEventHappenedBits);
                Serializer.Serialize(ref Lbp1GameProgressionEventsExplainedBits);
            }

            if (version >= 0x1f4)
                Serializer.Serialize(ref DemoProgressionStatus);

            if (version >= 0x1bd)
                Serializer.Serialize(ref Lbp2GameProgressionFlags);

            if (version >= 0x188)
                Serializer.Serialize(ref PendingInventoryItems);

            if (version >= 0x14f)
                Serializer.Serialize(ref PodLevel, PodLevel, ResourceType.Level, true);

            if (version >= 0x18f)
                PlayerId = Serializer.Serialize(ref PlayerId);

            if (version >= 0x296 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_OWNER_ID))
                OwnerPlayerId = Serializer.Serialize(ref OwnerPlayerId);

            if (version >= 0x296 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_OWNER_ID))
            {
                OwnerPsid = Serializer.Serialize(ref OwnerPsid);
                Serializer.Serialize(ref OwnerUserId);
            }

            if (version >= 0x175)
                EulaAgreed = Serializer.Serialize(ref EulaAgreed);

            if (version >= 0x18d)
                AcceptingPlayer = Serializer.Serialize(ref AcceptingPlayer);

            if (version >= 0x199)
                Serializer.Serialize(ref SyncedProfile, SyncedProfile, ResourceType.SyncedProfile, true);

            if (version >= 0x1f9)
                Serializer.Serialize(ref VoipMutedPlayers);

            if (version >= 0x1fc)
                Serializer.Serialize(ref VoipChatPaused);

            if (version >= 0x1bc)
                Serializer.Serialize(ref EnableMusicInPlayMode);

            if (version >= 0x1c5)
                Serializer.Serialize(ref EnableMusicInPod);

            if (version >= 0x27d)
            {
                LegacyInventoryCollections = Serializer.Array<LegacyInventoryCollection>(LegacyInventoryCollections, true);
                LegacyInventoryViews = Serializer.Array<InventoryView>(LegacyInventoryViews, true);
            }

            if (version >= 0x37b)
                InventoryCollections = Serializer.Array<InventoryCollection>(InventoryCollections, true);

            if (version >= 0x1bc)
                Serializer.Serialize(ref AvatarIcon, AvatarIcon, ResourceType.Texture, true);

            if (version >= 0x23c)
                Serializer.Serialize(ref SaveIcon, SaveIcon, ResourceType.Texture, true);

            if (version >= 0x263)
                Serializer.Serialize(ref Lbp1CreateModeVOProgress);

            if (version >= 0x283)
            {
                Serializer.Serialize(ref Gamma);
                Serializer.Serialize(ref ScreenSize);
                Serializer.Serialize(ref HasSeenCalibrationScreen);
            }

            if (version >= 0x35e)
            {
                Serializer.Serialize(ref Lbp1VOPlayed);
                Serializer.Serialize(ref Lbp2VOPlayed);
            }

            if (version >= 0x23a)
                Serializer.Serialize(ref SubtitleMode);

            if (version >= 0x36f)
                PinsAwarded = Serializer.Serialize(ref PinsAwarded);

            if (version >= 0x32e)
            {
                Serializer.Serialize(ref UserSettingPhotoBooth);
                if (subVersion >= 0x17e)
                    Serializer.Serialize(ref UserSettingAdvancedEditMode);
                Serializer.Serialize(ref UserSettingCollection);
            }

            if (version == 0x261)
                Serializer.Serialize(ref 0); // Unused: gameSaveProgressBits

            if (version >= 0x341 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_THERMOMETER))
                Serializer.Serialize(ref ShowThermometer);

            if (version >= 0x383)
                Serializer.Serialize(ref SaveOnlinePlaySettings);

            if (version >= 0x30d)
            {
                Serializer.Serialize(ref OnlinePlayMode);
                Serializer.Serialize(ref FriendJoinRequestMode);
                Serializer.Serialize(ref NonFriendJoinRequestMode);
            }

            if (version >= 0x31c)
                Serializer.Serialize(ref Lbp2TutorialsPlayed);

            if (version >= 0x31d)
                Serializer.Serialize(ref SectionHeadingToggled);

            if (subVersion >= 0x20d)
                Serializer.Serialize(ref MysteryPodEventsSeen);

            if (version is >= 0x35f and < 0x3f9)
            {
                if (!Serializer.IsWriting())
                    LastLegacyImportedProfileHashLBP1 = new Sha1[Serializer.GetInput().I32()];
                else 
                    Serializer.GetOutput().I32(LastLegacyImportedProfileHashLBP1.Length);
                for (var i = 0; i < LastLegacyImportedProfileHashLBP1.Length; ++i)
                    Serializer.Serialize(ref LastLegacyImportedProfileHashLBP1[i]);
            }

            if (version >= 0x360)
                Serializer.Serialize(ref PlayedLBP1);

            if (subVersion >= 0x12c)
            {
                Serializer.Serialize(ref HasPerformedFirstRun);
                Serializer.Serialize(ref Lbp2TutorialUnlockButtonUnleashed);
            }

            if (version >= 0x28c)
                Serializer.Serialize(ref HiddenCategories);

            if (subVersion >= 0x160)
                Serializer.Serialize(ref CollectedBubbles);

            if (version >= 0x2f5 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_WATER_LEVELS))
                Serializer.Serialize(ref NumWaterLevelsPlayed);

            if (version >= 0x3cf)
                Serializer.Serialize(ref AutoFilterOn);

            if (version >= 0x3d2)
            {
                Serializer.Serialize(ref HasUsed6Axis);
                Serializer.Serialize(ref HasUsedMove);
            }

            // Serializer.Serialize(ref true); // hasSeenCrossCompatInfo
            // Serializer.Serialize(ref false); // wantsCrossCompatDownloadNotification

            if (version >= 0x3a3)
            {
                PaintProperties = Serializer.Serialize(ref PaintProperties);
                PlayerDataLabels = Serializer.Serialize(ref PlayerDataLabels);
            }

            if (version >= 0x3c9 || revision.Has(Branch.Double11, (int)Revisions.D_1DATALABELS))
            {
                if (revision.Has(Branch.Double11, (int)Revisions.D13_G_CONNECTIVITY))
                {
                    Serializer.Serialize(ref Total3GUpStream);
                    Serializer.Serialize(ref Total3GDownStream);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1_TOUCH_CREATE))
                {
                    Serializer.Serialize(ref CreateRearTouchPan);
                    Serializer.Serialize(ref CreateRearTouchPinchZoom);
                    Serializer.Serialize(ref CreateRearTapZoom);
                
                    if (revision.Has(Branch.Double11, (int)Revisions.D1_CREATE_WARNING))
                        Serializer.Serialize(ref CreateFrameRateWarningMessages);

                    if (revision.Has(Branch.Double11, (int)Revisions.D1_CREATE_BETA))
                    {
                        OnlineBetaNetworkTimestamp = Serializer.Serialize(ref OnlineBetaNetworkTimestamp);
                        Serializer.Serialize(ref OnlineBetaPlayTimeSinceOnline);
                        Serializer.Serialize(ref OnlineBetaBootsSinceOnline);
                    }
                }
            }

            if (version >= 0x3e2)
                Serializer.Serialize(ref DistanceMovedWhilstAttracted);

            if (version >= 0x3e4)
            {
                Serializer.Serialize(ref BeakersUsedPart1);
                Serializer.Serialize(ref BeakersUsedPart2);
            }

            if (version >= 0x3ec)
                Serializer.Serialize(ref ProfileFlags);

            if (version >= 0x3f2)
            {
                Serializer.Serialize(ref GoldMedalsAwarded);
                Serializer.Serialize(ref GoldMedalStoryArcPins);
            }

            if (version >= 0x3f2)
            {
                Serializer.Serialize(ref TwitterEnabled);
                Serializer.Serialize(ref FacebookEnabled);
                Serializer.Serialize(ref TwitterAccessToken);
                Serializer.Serialize(ref TwitterAccessTokenSecret);
            }

            if (version >= 0x3f3)
                Serializer.Serialize(ref PlayedLBP2);

            if (version >= 0x3f4)
            {
                Serializer.Serialize(ref CreateChallengeTutorialSeen);
                Serializer.Serialize(ref PlayChallengeTutorialSeen);
            }

            if (version is >= 0x3f5 and < 0x3f9)
            {
                if (!Serializer.IsWriting())
                    LastLegacyImportedProfileHashLBP2 = new Sha1[Serializer.GetInput().I32()];
                else 
                    Serializer.GetOutput().I32(LastLegacyImportedProfileHashLBP2.Length);
                for (var i = 0; i < LastLegacyImportedProfileHashLBP2.Length; ++i)
                    Serializer.Serialize(ref LastLegacyImportedProfileHashLBP2[i]);
            }

            if (version >= 0x3f7)
                Serializer.Serialize(ref OwnedLBP);

            if (subVersion >= 0xef)
                Serializer.Serialize(ref DceUuidState);

            if (subVersion >= 0x15b)
                Serializer.Serialize(ref LastPlayedPPP);

            if (subVersion >= 0x20e)
            {
                LastPlayedPlanet = Serializer.Serialize(ref LastPlayedPlanet);
                Serializer.Serialize(ref LastPlayedEarthAdv);
                Serializer.Serialize(ref HasSeenDiveInBetaMessage);
            }

            if (subVersion >= 0x183)
                Serializer.Serialize(ref TouchCreateCursorMode);

            if (subVersion >= 0x187)
                Serializer.Serialize(ref ShowAdvancedEditModeMessageCounter);

            if (subVersion >= 0x194)
                Serializer.Serialize(ref ShowAdventureSaveWarning);

            if (subVersion >= 0x19f)
                Serializer.Serialize(ref TotalFramesInEditMode);

            if (subVersion >= 0x1a1)
            {
                if (!Serializer.IsWriting())
                    OnlineTutorialsPlayed = new Sha1[Serializer.GetInput().I32()];
                else 
                    Serializer.GetOutput().I32(OnlineTutorialsPlayed.Length);
                for (var i = 0; i < OnlineTutorialsPlayed.Length; ++i)
                    Serializer.Serialize(ref OnlineTutorialsPlayed[i]);

                Serializer.Serialize(ref PopitPuzzlesCompleted);
            }

            if (subVersion >= 0x1aa)
                Serializer.Serialize(ref TimesaverNoticeViewed);
            
            if (subVersion >= 0x1ad)
                Serializer.Serialize(ref QuestProgressPin);
            
            if (subVersion >= 0x209)
                Serializer.Serialize(ref ActivityFilterToggled);
            
            if (subVersion >= 0x212)
                Serializer.Serialize(ref LastLandingPageFocusItems);
            
            if (subVersion >= 0x215)
                Serializer.Serialize(ref GoPlayCache);
        }

        public override int GetAllocatedSize()
        {
            var size = BASE_ALLOCATION_SIZE;
            size += StringTable.GetAllocatedSize();
            return size;
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            var serializer = new Serializer(0xFFFF00 + (Inventory.Count * 0xC0),
                revision, compressionFlags);
            Serializer.Serialize(ref this);
            return new SerializationData(
                Serializer.GetBuffer(),
                revision,
                compressionFlags,
                ResourceType.LocalProfile,
                SerializationType.BINARY,
                Serializer.GetDependencies()
            );
        }

        /// <summary>
        /// Adds an item to your inventory from a RPlan.
        /// </summary>
        /// <param name="plan">Plan to add</param>
        /// <param name="descriptor">GUID/SHA1 reference to the plan to add</param>
        /// <param name="table">Translation table for resolving categories/locations</param>
        /// <returns>Added inventory item</returns>
        public InventoryItem AddItem(RPlan plan, ResourceDescriptor? descriptor, RTranslationTable table)
        {
            var existing = GetItem(descriptor);
            if (existing != null) 
                return existing;

            var item = new InventoryItem
            {
                Plan = descriptor
            };
            if (descriptor.IsGUID())
                item.Guid = descriptor.GetGUID() ?? default(GUID);
            
            var details = plan.InventoryData;
            details.DateAdded = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            details.Icon ??= new ResourceDescriptor(2551, ResourceType.Texture);

            if (table != null)
            {
                var category = "";
                if (plan.InventoryData.Category != 0)
                    category = table.Translate(plan.InventoryData.Category);
                
                var location = "";
                if (plan.InventoryData.Location != 0)
                    location = table.Translate(plan.InventoryData.Location);
                
                location ??= "";
                category ??= "";

                details.CategoryIndex = (short)StringTable.Add(category, (int)plan.InventoryData.Category);
                details.LocationIndex = (short)StringTable.Add(location, (int)plan.InventoryData.Location);
            }

            item.Details = plan.InventoryData;
            item.Flags = 4;
            item.Uid = GetLastInventoryUID() + 1;

            Inventory.Add(item);

            return item;
        }

        /// <summary>
        /// Gets the last used inventory UID
        /// </summary>
        /// <returns>The highest inventory UID in use</returns>
        public int GetLastInventoryUID()
        {
            return Inventory.Select(item => item.Uid).Prepend(1).Max();
        }

        /// <summary>
        /// Checks if this profile has an item with the specified resource descriptor
        /// </summary>
        /// <param name="descriptor">Resource descriptor to check for</param>
        /// <returns>Whether the item exists in the inventory</returns>
        public bool HasItem(ResourceDescriptor descriptor)
        {
            if (descriptor == null) 
                return false;
            
            if (descriptor.IsGUID()) 
            {
                var guid = descriptor.GetGUID();
                return guid.HasValue && HasItem(guid.Value);
            }
            else if (descriptor.IsHash()) 
                return HasItem(descriptor.GetSHA1());
            
            return false;
        }

        /// <summary>
        /// Checks if this profile has an item with the specified SHA1 hash
        /// </summary>
        /// <param name="hash">SHA1 hash to check for</param>
        /// <returns>Whether the item exists in the inventory</returns>
        public bool HasItem(Sha1 hash)
        {
            if (hash == null)
                return false;
                
            foreach (var item in Inventory)
            {
                var plan = item.Plan;
                if (plan == null) 
                    continue;
                
                if (plan.IsHash() && plan.GetSHA1().Equals(hash))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if this profile has an item with the specified GUID
        /// </summary>
        /// <param name="guid">GUID to check for</param>
        /// <returns>Whether the item exists in the inventory</returns>
        public bool HasItem(GUID guid)
        {
            return (from plan in Inventory.Select(item => item.Plan).OfType<ResourceDescriptor>() where plan.IsGUID() select plan.GetGUID()).OfType<GUID?>().Any(planGuid => planGuid.Value.Equals(guid));
        }

        /// <summary>
        /// Gets an item with the specified resource descriptor
        /// </summary>
        /// <param name="descriptor">Resource descriptor to get</param>
        /// <returns>The inventory item if found, null otherwise</returns>
        public InventoryItem? GetItem(ResourceDescriptor? descriptor)
        {
            return descriptor == null ? null : Inventory.FirstOrDefault(item => descriptor.Equals(item.Plan));
        }
    }
}