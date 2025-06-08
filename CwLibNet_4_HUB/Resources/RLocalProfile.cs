using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Profile;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;
using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;

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

        public override void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
        int temp_int = 0;
        int temp_Lbp2GameProgressionFlags = 0;
        int temp_HiddenCategories = 0;
        int temp_EnableMusicInPod = 0;
        int temp_EnableMusicInPlayMode = 0;
        int temp_CollectedBubbles = 0;
        int temp_AvatarIcon = 0;
        int temp_UserSettingPhotoBooth = 0;
        int temp_Lbp2TutorialUnlockButtonUnleashed = 0;
        int temp_Gamma = 0;
        int temp_FriendJoinRequestMode = 0;
        int temp_CreateRearTouchPinchZoom = 0;
        int temp_CreateRearTapZoom = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

            var revision = Serializer.GetCurrentSerializer().GetRevision();
            var version = revision.GetVersion();
            var subVersion = revision.GetSubVersion();

            if (version < 0x187)
            {
                // descriptor array with types, no pointer
                return;
            }

            var tempInventory = Inventory;
            Serializer.Serialize(ref tempInventory);
            Inventory = tempInventory;
            var tempStringTable = StringTable;
            Serializer.Serialize(ref tempStringTable);
            StringTable = tempStringTable;

            switch (version)
            {
                case >= 0x3b6:
                    var tempFromProductionBuild = FromProductionBuild;
                    Serializer.Serialize(ref tempFromProductionBuild);
                    FromProductionBuild = tempFromProductionBuild;
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
                    var count = Serializer.GetCurrentSerializer().GetInput().I32();
                    for (var i = 0; i < count; ++i)
                    {
                        keys.Add(Serializer.GetCurrentSerializer().GetInput().Enum32<TutorialLevel>());
                        values.Add(Serializer.GetCurrentSerializer().GetInput().Enum32<TutorialState>());
                    }

                    Lbp1TutorialStates = new Dictionary<TutorialLevel, TutorialState>();
                    for (var i = 0; i < count; ++i)
                        Lbp1TutorialStates[keys[i]] = values[i];
                }
                else
                {
                    var keys = new List<TutorialLevel>(Lbp1TutorialStates.Keys);
                    Serializer.GetCurrentSerializer().GetOutput().I32(keys.Count);
                    foreach (var key in keys)
                    {
                        Serializer.GetCurrentSerializer().GetOutput().Enum32(key);
                        Serializer.GetCurrentSerializer().GetOutput().Enum32(Lbp1TutorialStates[key]);
                    }
                }
            }

            if (version >= 0x266)
            {
                var tempDlcPackViewed = DlcPackViewed;
                Serializer.Serialize(ref tempDlcPackViewed);
                DlcPackViewed = tempDlcPackViewed;
                var tempDlcPackShown = DlcPackShown;
                Serializer.Serialize(ref tempDlcPackShown);
                DlcPackShown = tempDlcPackShown;
            }

            if (version >= 0x1e5)
            {
                var tempLbp1MainMenuButtonUnlocks = Lbp1MainMenuButtonUnlocks;
                Serializer.Serialize(ref tempLbp1MainMenuButtonUnlocks);
                Lbp1MainMenuButtonUnlocks = tempLbp1MainMenuButtonUnlocks;
            }

            if (version >= 0x16e)
            {
                var tempPlayedLevelData = PlayedLevelData;
                Serializer.Serialize(ref tempPlayedLevelData);
                PlayedLevelData = tempPlayedLevelData;
            }

            if (version >= 0x37a)
            {
                var tempViewedLevelData = ViewedLevelData;
                Serializer.Serialize(ref tempViewedLevelData);
                ViewedLevelData = tempViewedLevelData;
                var tempViewedPlayerData = ViewedPlayerData;
                Serializer.Serialize(ref tempViewedPlayerData);
                ViewedPlayerData = tempViewedPlayerData;
            }

            if (version >= 0x16e)
            {
                if (!Serializer.IsWriting())
                {
                    var count = Serializer.GetCurrentSerializer().GetInput().I32();
                    OldMyMoonSlots = new Dictionary<SlotID, Slot>(count);
                    for (var i = 0; i < count; ++i)
                    {
                        var tempSlotId = default(SlotID);
                        var tempSlot = default(Slot);
                        Serializer.Serialize(ref tempSlotId);
                        Serializer.Serialize(ref tempSlot);
                        OldMyMoonSlots[tempSlotId] = tempSlot;
                    }
                }
                else
                {
                    Serializer.GetCurrentSerializer().GetOutput().I32(OldMyMoonSlots.Count);
                    foreach (var entry in OldMyMoonSlots)
                    {
                        var tempKey = entry.Key;
                        var tempValue = entry.Value;
                        Serializer.Serialize(ref tempKey);
                        Serializer.Serialize(ref tempValue);
                    }
                }
            }

            if (version >= 0x20e)
            {
                var tempCopiedFromAnotherUser = CopiedFromAnotherUser;
                Serializer.Serialize(ref tempCopiedFromAnotherUser);
                CopiedFromAnotherUser = tempCopiedFromAnotherUser;
            }

            if (version >= 0x268)
            {
                var tempFixedUpByDeveloper = FixedUpByDeveloper;
                Serializer.Serialize(ref tempFixedUpByDeveloper);
                FixedUpByDeveloper = tempFixedUpByDeveloper;
            }

            if (version >= 0x297 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_SAVEGAME_INFO))
            {
                var tempShowSaveGameInfo = ShowSaveGameInfo;
                Serializer.Serialize(ref tempShowSaveGameInfo);
                ShowSaveGameInfo = tempShowSaveGameInfo;
                var tempTotalLevelCount = TotalLevelCount;
                Serializer.Serialize(ref tempTotalLevelCount);
                TotalLevelCount = tempTotalLevelCount;
            }

            if (version >= 0x394)
            {
                var tempLastMmPicksCount = LastMmPicksCount;
                Serializer.Serialize(ref tempLastMmPicksCount);
                LastMmPicksCount = tempLastMmPicksCount;
            }

            if (version >= 0x2cc || revision.Has(Branch.Leerdammer, (int)Revisions.LD_NEWS_TIMESTAMP))
            {
                var tempLastNewsItemTimestamp = LastNewsItemTimestamp;
                Serializer.Serialize(ref tempLastNewsItemTimestamp);
                LastNewsItemTimestamp = tempLastNewsItemTimestamp;
            }

            if (version >= 0x324)
            {
                var tempLastStreamEventTimestamp = LastStreamEventTimestamp;
                Serializer.Serialize(ref tempLastStreamEventTimestamp);
                LastStreamEventTimestamp = tempLastStreamEventTimestamp;
            }

            if (version >= 0x2cd || revision.Has(Branch.Leerdammer, (int)Revisions.LD_QUEUE))
            {
                var tempLolcatFtwTimestamp = LolcatFtwTimestamp;
                Serializer.Serialize(ref tempLolcatFtwTimestamp);
                LolcatFtwTimestamp = tempLolcatFtwTimestamp;
            }

            if (version >= 0x328)
            {
                if (!Serializer.IsWriting())
                    OldTextSearches = new string[Serializer.GetCurrentSerializer().GetInput().I32()];
                else
                    Serializer.GetCurrentSerializer().GetOutput().I32(OldTextSearches.Length);
                for (var i = 0; i < OldTextSearches.Length; ++i)
                    Serializer.Serialize(ref OldTextSearches[i]);
            }

            if (version >= 0x32a)
            {
                var tempOldDiscoveredLinks = OldDiscoveredLinks;
                Serializer.Serialize(ref tempOldDiscoveredLinks);
                OldDiscoveredLinks = tempOldDiscoveredLinks;
                var tempPendingLinks = PendingLinks;
                Serializer.Serialize(ref tempPendingLinks);
                PendingLinks = tempPendingLinks;
            }

            if (version >= 0x239)
            {
                var tempLastPlayed = LastPlayed;
                Serializer.Serialize(ref tempLastPlayed);
                LastPlayed = tempLastPlayed;
            }

            if (version >= 0x1c2)
            {
                var tempLbp1VideosPlayed = Lbp1VideosPlayed;
                Serializer.Serialize(ref tempLbp1VideosPlayed);
                Lbp1VideosPlayed = tempLbp1VideosPlayed;
            }
        
            // ArrayList<GUID>
            if (version >= 0x219)
            {
                var tempLbp2VideosPlayed = Lbp2VideosPlayed;
                Serializer.Serialize(ref tempLbp2VideosPlayed);
                Lbp2VideosPlayed = tempLbp2VideosPlayed;
            }

            if (version >= 0x16f)
            {
                var tempCurrentPlayerMetrics = CurrentPlayerMetrics;
                Serializer.Serialize(ref tempCurrentPlayerMetrics);
                CurrentPlayerMetrics = tempCurrentPlayerMetrics;
            }

            if (version >= 0x1ab)
            {
                var tempLastPostedPlayerMetrics = LastPostedPlayerMetrics;
                Serializer.Serialize(ref tempLastPostedPlayerMetrics);
                LastPostedPlayerMetrics = tempLastPostedPlayerMetrics;
            }

            if (version >= 0x1e1)
            {
                var tempLbp1GameProgressionStatus = Lbp1GameProgressionStatus;
                tempLbp1GameProgressionStatus = serializer.Enum32(tempLbp1GameProgressionStatus);
                Lbp1GameProgressionStatus = tempLbp1GameProgressionStatus;
            }

            if (version >= 0x1cf)
            {
                var tempLbp1GameProgressionEventHappenedBits = Lbp1GameProgressionEventHappenedBits;
                Serializer.Serialize(ref tempLbp1GameProgressionEventHappenedBits);
                Lbp1GameProgressionEventHappenedBits = tempLbp1GameProgressionEventHappenedBits;
                var tempLbp1GameProgressionEventsExplainedBits = Lbp1GameProgressionEventsExplainedBits;
                Serializer.Serialize(ref tempLbp1GameProgressionEventsExplainedBits);
                Lbp1GameProgressionEventsExplainedBits = tempLbp1GameProgressionEventsExplainedBits;
            }

            if (version >= 0x1f4)
            {
                DemoProgressionStatus = serializer.I32(DemoProgressionStatus);
            }

            if (version >= 0x1bd)
            {            }
                Serializer.Serialize(ref temp_Lbp2GameProgressionFlags);
                temp_Lbp2GameProgressionFlags = temp_Lbp2GameProgressionFlags;
                Lbp2GameProgressionFlags = temp_Lbp2GameProgressionFlags;

            if (version >= 0x188)
            {
                PendingInventoryItems = serializer.Array(PendingInventoryItems);
            }

            if (version >= 0x14f)
            {
                PodLevel = serializer.Resource(PodLevel, ResourceType.Level, true);
            }

            if (version >= 0x18f)
            {
                PlayerId = serializer.Struct<NetworkPlayerID>(PlayerId);
            }

            if (version >= 0x296 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_OWNER_ID))
            {
                OwnerPlayerId = serializer.Struct<NetworkPlayerID>(OwnerPlayerId);
            }

            if (version >= 0x296 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_OWNER_ID))
            {
                OwnerPsid = serializer.Struct<OpenPSID>(OwnerPsid);
                OwnerUserId = serializer.I32(OwnerUserId);
            }

            if (version >= 0x175)
            {
                EulaAgreed = serializer.Sha1(EulaAgreed);
            }

            if (version >= 0x18d)
            {
                AcceptingPlayer = serializer.Struct<NetworkPlayerID>(AcceptingPlayer);
            }

            if (version >= 0x199)
            {
                SyncedProfile = serializer.Resource(SyncedProfile, ResourceType.SyncedProfile, true, false, false);
            }

            if (version >= 0x1f9)
            {
                VoipMutedPlayers = serializer.Array<NetworkPlayerID>(VoipMutedPlayers);
            }

            if (version >= 0x1fc)
            {
                VoipChatPaused = serializer.Bool(VoipChatPaused);
            }

            if (version >= 0x1bc)
            {
                Serializer.Serialize(ref temp_EnableMusicInPlayMode);
                EnableMusicInPlayMode = temp_EnableMusicInPlayMode != 0;
            }

            if (version >= 0x1c5)
            {
                Serializer.Serialize(ref temp_EnableMusicInPod);
                EnableMusicInPod = temp_EnableMusicInPod != 0;
            }

            if (version >= 0x27d)
            {
                LegacyInventoryCollections = serializer.Array<LegacyInventoryCollection>(LegacyInventoryCollections, true);
                LegacyInventoryViews = serializer.Array<InventoryView>(LegacyInventoryViews, true);
            }

            if (version >= 0x37b)
                InventoryCollections = serializer.Array<InventoryCollection>(InventoryCollections, true);

            if (version >= 0x1bc)
            {
                AvatarIcon = serializer.Resource(AvatarIcon, ResourceType.Texture, true);
            }

            if (version >= 0x23c)
            {
                SaveIcon = serializer.Resource(SaveIcon, ResourceType.Texture, true, false, false);
            }

            if (version >= 0x263)
            {
                Lbp1CreateModeVOProgress = serializer.I32(Lbp1CreateModeVOProgress);
            }

            if (version >= 0x283)
            {
                Gamma = serializer.F32(Gamma);
                ScreenSize = serializer.F32(ScreenSize);
                HasSeenCalibrationScreen = serializer.Bool(HasSeenCalibrationScreen);
            }

            if (version >= 0x35e)
            {
                Lbp1VOPlayed = serializer.Intarray(Lbp1VOPlayed);
                Lbp2VOPlayed = serializer.Intarray(Lbp2VOPlayed);
            }

            if (version >= 0x23a)
            {
                SubtitleMode = serializer.I32(SubtitleMode);
            }

            if (version >= 0x36f)
            {
                PinsAwarded = serializer.Struct<PinsAwarded>(PinsAwarded);
            }

            if (version >= 0x32e)
            {
                UserSettingPhotoBooth = serializer.Bool(UserSettingPhotoBooth);
                if (subVersion >= 0x17e)
                {
                    UserSettingAdvancedEditMode = serializer.Bool(UserSettingAdvancedEditMode);
                }
                UserSettingCollection = serializer.Bool(UserSettingCollection);
            }

            if (version == 0x261)
                Serializer.Serialize(ref temp_int); // Unused: gameSaveProgressBits

            if (version >= 0x341 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_THERMOMETER))
            {
                ShowThermometer = serializer.Bool(ShowThermometer);
            }

            if (version >= 0x383)
            {
                SaveOnlinePlaySettings = serializer.Bool(SaveOnlinePlaySettings);
            }

            if (version >= 0x30d)
            {
                OnlinePlayMode = serializer.I32(OnlinePlayMode);
                FriendJoinRequestMode = serializer.I32(FriendJoinRequestMode);
                NonFriendJoinRequestMode = serializer.I32(NonFriendJoinRequestMode);
            }

            if (version >= 0x31c)
            {
                Lbp2TutorialsPlayed = serializer.Intarray(Lbp2TutorialsPlayed);
            }

            if (version >= 0x31d)
            {
                SectionHeadingToggled = serializer.Intarray(SectionHeadingToggled);
            }

            if (subVersion >= 0x20d)
            {
                MysteryPodEventsSeen = serializer.Array(MysteryPodEventsSeen);
            }

            if (version is >= 0x35f and < 0x3f9)
            {
                if (!Serializer.IsWriting())
                    LastLegacyImportedProfileHashLBP1 = new Sha1[serializer.GetInput().I32()];
                else 
                    serializer.GetOutput().I32(LastLegacyImportedProfileHashLBP1.Length);
                for (var i = 0; i < LastLegacyImportedProfileHashLBP1.Length; ++i)
                {
                    var temp = LastLegacyImportedProfileHashLBP1[i];
                    temp = serializer.Sha1(temp);
                    LastLegacyImportedProfileHashLBP1[i] = temp;
                }
            }

            if (version >= 0x360)
            {
                PlayedLBP1 = serializer.Bool(PlayedLBP1);
            }

            if (subVersion >= 0x12c)
            {
                HasPerformedFirstRun = serializer.Bool(HasPerformedFirstRun);
                Lbp2TutorialUnlockButtonUnleashed = serializer.Bool(Lbp2TutorialUnlockButtonUnleashed);
            }

            if (version >= 0x28c)
            {
                HiddenCategories = serializer.Intarray(HiddenCategories);
            }

            if (subVersion >= 0x160)
            {
                CollectedBubbles = serializer.Array<CollectedBubble>(CollectedBubbles);
            }

            if (version >= 0x2f5 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_WATER_LEVELS))
            {
                var tempNumWaterLevelsPlayed = NumWaterLevelsPlayed;
                Serializer.Serialize(ref tempNumWaterLevelsPlayed);
                NumWaterLevelsPlayed = tempNumWaterLevelsPlayed;
            }

            if (version >= 0x3cf)
            {
                var tempAutoFilterOn = AutoFilterOn;
                Serializer.Serialize(ref tempAutoFilterOn);
                AutoFilterOn = tempAutoFilterOn;
            }

            if (version >= 0x3d2)
            {
                var tempHasUsed6Axis = HasUsed6Axis;
                Serializer.Serialize(ref tempHasUsed6Axis);
                HasUsed6Axis = tempHasUsed6Axis;
                var tempHasUsedMove = HasUsedMove;
                Serializer.Serialize(ref tempHasUsedMove);
                HasUsedMove = tempHasUsedMove;
            }

            // Serializer.Serialize(ref temp_bool_true); // hasSeenCrossCompatInfo
            // Serializer.Serialize(ref temp_bool_false);
            // temp_bool_false = temp_bool_false; // wantsCrossCompatDownloadNotification

            if (version >= 0x3a3)
            {
                var tempPaintProperties = PaintProperties;
                Serializer.Serialize(ref tempPaintProperties);
                PaintProperties = tempPaintProperties;
                var tempPlayerDataLabels = PlayerDataLabels;
                Serializer.Serialize(ref tempPlayerDataLabels);
                PlayerDataLabels = tempPlayerDataLabels;
            }

            if (version >= 0x3c9 || revision.Has(Branch.Double11, (int)Revisions.D_1DATALABELS))
            {
                if (revision.Has(Branch.Double11, (int)Revisions.D13_G_CONNECTIVITY))
                {
                    Total3GUpStream = serializer.I32(Total3GUpStream);
                    Total3GDownStream = serializer.I32(Total3GDownStream);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1_TOUCH_CREATE))
                {
                    CreateRearTouchPan = serializer.Bool(CreateRearTouchPan);
                    temp_CreateRearTouchPinchZoom = serializer.I32(temp_CreateRearTouchPinchZoom);
                    CreateRearTouchPinchZoom = temp_CreateRearTouchPinchZoom != 0;
                    temp_CreateRearTapZoom = serializer.I32(temp_CreateRearTapZoom);
                    CreateRearTapZoom = temp_CreateRearTapZoom != 0;
                
                    if (revision.Has(Branch.Double11, (int)Revisions.D1_CREATE_WARNING))
                    {
                        CreateFrameRateWarningMessages = serializer.Bool(CreateFrameRateWarningMessages);
                    }

                    if (revision.Has(Branch.Double11, (int)Revisions.D1_CREATE_BETA))
                    {
                        OnlineBetaNetworkTimestamp = serializer.S64(OnlineBetaNetworkTimestamp);
                        OnlineBetaPlayTimeSinceOnline = (int)serializer.S64(OnlineBetaPlayTimeSinceOnline);
                        OnlineBetaBootsSinceOnline = serializer.I32(OnlineBetaBootsSinceOnline);
                    }
                }
            }

            if (version >= 0x3e2)
            {
                DistanceMovedWhilstAttracted = serializer.F32(DistanceMovedWhilstAttracted);
            }

            if (version >= 0x3e4)
            {
                BeakersUsedPart1 = serializer.I32(BeakersUsedPart1);
                BeakersUsedPart2 = serializer.I32(BeakersUsedPart2);
            }

            if (version >= 0x3ec)
            {
                ProfileFlags = serializer.I32(ProfileFlags);
            }

            if (version >= 0x3f2)
            {
                GoldMedalsAwarded = serializer.I8(GoldMedalsAwarded);
                GoldMedalStoryArcPins = serializer.I8(GoldMedalStoryArcPins);
            }

            if (version >= 0x3f2)
            {
                TwitterEnabled = serializer.Bool(TwitterEnabled);
                FacebookEnabled = serializer.Bool(FacebookEnabled);
                TwitterAccessToken = serializer.Str(TwitterAccessToken);
                TwitterAccessTokenSecret = serializer.Str(TwitterAccessTokenSecret);
            }

            if (version >= 0x3f3)
            {
                PlayedLBP2 = serializer.Bool(PlayedLBP2);
            }

            if (version >= 0x3f4)
            {
                CreateChallengeTutorialSeen = serializer.Bool(CreateChallengeTutorialSeen);
                PlayChallengeTutorialSeen = serializer.Bool(PlayChallengeTutorialSeen);
            }

            if (version is >= 0x3f5 and < 0x3f9)
            {
                if (!Serializer.IsWriting())
                    LastLegacyImportedProfileHashLBP2 = new Sha1[Serializer.GetCurrentSerializer().GetInput().I32()];
                else 
                    Serializer.GetCurrentSerializer().GetOutput().I32(LastLegacyImportedProfileHashLBP2.Length);
                for (var i = 0; i < LastLegacyImportedProfileHashLBP2.Length; ++i)
                    Serializer.Serialize(ref LastLegacyImportedProfileHashLBP2[i]);
            }

            if (version >= 0x3f7)
            {
                OwnedLBP = serializer.I32(OwnedLBP);
            }

            if (subVersion >= 0xef)
            {
                DceUuidState = serializer.I32(DceUuidState);
            }

            if (subVersion >= 0x15b)
            {
                LastPlayedPPP = serializer.Bool(LastPlayedPPP);
            }

            if (subVersion >= 0x20e)
            {
                LastPlayedPlanet = serializer.Struct<SlotID>(LastPlayedPlanet);
                LastPlayedEarthAdv = serializer.Bool(LastPlayedEarthAdv);
                HasSeenDiveInBetaMessage = serializer.Bool(HasSeenDiveInBetaMessage);
            }

            if (subVersion >= 0x183)
            {
                TouchCreateCursorMode = serializer.I32(TouchCreateCursorMode);
            }

            if (subVersion >= 0x187)
            {
                ShowAdvancedEditModeMessageCounter = serializer.Bool(ShowAdvancedEditModeMessageCounter);
            }

            if (subVersion >= 0x194)
            {
                ShowAdventureSaveWarning = serializer.Bool(ShowAdventureSaveWarning);
            }

            if (subVersion >= 0x19f)
            {
                TotalFramesInEditMode = serializer.I32(TotalFramesInEditMode);
            }

            if (subVersion >= 0x1a1)
            {
                if (!Serializer.IsWriting())
                    OnlineTutorialsPlayed = new Sha1[serializer.GetInput().I32()];
                else 
                    serializer.GetOutput().I32(OnlineTutorialsPlayed.Length);
                for (var i = 0; i < OnlineTutorialsPlayed.Length; ++i)
                {
                    var temp = OnlineTutorialsPlayed[i];
                    temp = serializer.Sha1(temp);
                    OnlineTutorialsPlayed[i] = temp;
                }

                PopitPuzzlesCompleted = serializer.Array<SlotID>(PopitPuzzlesCompleted);
            }

            if (subVersion >= 0x1aa)
            {
                TimesaverNoticeViewed = serializer.Bool(TimesaverNoticeViewed);
            }
            
            if (subVersion >= 0x1ad)
            {
                QuestProgressPin = serializer.I32(QuestProgressPin);
            }
            
            if (subVersion >= 0x209)
            {
                ActivityFilterToggled = serializer.Intarray(ActivityFilterToggled);
            }
            
            if (subVersion >= 0x212)
            {
                LastLandingPageFocusItems = serializer.I32(LastLandingPageFocusItems);
            }
            
            if (subVersion >= 0x215)
            {
                GoPlayCache = serializer.Array<GoPlayCache>(GoPlayCache);
            }
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
            this.Serialize(serializer);
            return new SerializationData(
                Serializer.GetCurrentSerializer().GetBuffer(),
                revision,
                compressionFlags,
                ResourceType.LocalProfile,
                SerializationType.BINARY,
                Serializer.GetCurrentSerializer().GetDependencies()
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