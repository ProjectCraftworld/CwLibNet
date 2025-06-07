using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Streaming;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Structs.Things.Components.World;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PWorld: ISerializable
{
    public ResourceDescriptor[] Materials;
    public float BackdropOffsetX, BackdropOffsetY, BackdropOffsetZ;
    public bool BackdropOffsetZAuto;
    public string OverrideBackdropAmbience;
    
    public StreamingManager StreamingManager;
    public List<Thing> Things = [];
    public float MaxVel = 100.0f, MaxAVel = 1.0f;
    public int Frame;
    public int SimFrame;
    public int FrameLevelStarted;
    public int Randy;
    public int ThingUidCounter = 2;
    public EditorSelection[] Selections;
    public Thing Backdrop, BackdropNew;
    public float BackdropTimer;
    public int Lbp2NightDaySwapped = 1;
    public bool IsPaused;
    public float LightingFactor = 1.0f, ColorCorrectionFactor;
    public float FogFactor, FogTintFactor, DarknessFactor;
    public EggLink[]? CompleteRewards;
    public EggLink[]? CollectRewards;
    public EggLink[]? AceRewards;
    public bool AreRewardsShareable = true;
    public SlotID ScoreboardLevelLinkSlot;
    public KeyLink[]? CompleteUnlocks;
    public KeyLink[]? CollectUnlocks;
    public KeyLink[]? AceUnlocks;
    public int DeathCount;
    public int MaxNumPlayers = 1;
    public Thing[] DissolvingThings, OldDissolvingThings;
    public bool IsTutorialLevel;
    public bool EverSpawned;
    public int SpawnFailureCount;
    public GlobalSettings TargetGlobalSettings = new(), FromGlobalSettings = new();
    public float GlobalSettingsBlendFactor;
    public bool HasLevelLightingBeenSetup;
    public int GlobalSettingsThingUid;
    public CameraSettings CameraSettings = new();
    public float WaterLevel, TargetWaterLevel;
    
    
    public float FromWaterLevel;
    public float WaterWaveMagnitude, FromWaterWaveMagnitude, TargetWaterWaveMagnitude;
    public float Gravity = 1.0f, FromGravity = 1.0f, TargetGravity = 1.0f;
    public float[] CurrGlobalSettingsBlendFactors;
    public int[] GlobalSettingsThingUiDs;
    public int[] GlobalSettingsThingPriority;
    
    
    
    public float WaterTint, TargetWaterTint;

    
    
    public Vector4? FromWaterTintColor = new Vector4(0.47451f, 0.898039f, 0.909804f, 0.125f);

    
    
    public float WaterMurkiness, FromWaterMurkiness, TargetWaterMurkiness;

    
    
    public float WaterBits = 1.0f, FromWaterBits = 1.0f, TargetWaterBits = 1.0f;

    public bool WaterDrainSoundsEnabled = true, CurrWaterDrainSoundsEnabled = true;
    public bool WaterCausticsEnabled = true, CurrWaterCausticsEnabled = true;
    
    
    public int WaterMainColor = -1090453761, FromWaterMainColor = -1090453761, TargetWaterMainColor = -1090453761;
    
    public int WaterHintColorOne = -33554177, FromWaterHintColorOne = -33554177, TargetWaterHintColorOne = -33554177;
    
    public int WaterHintColorTwo = 16662783, FromWaterHintColorTwo = 16662783, TargetWaterHintColorTwo = 16662783;

    public bool BackdropEnabled = true, CurrBackdropEnabled = true;

    
    
    public float CurrWavePos = 0.0186706f;


    public int GameMode;
    public int GameModeRequested;

    public int NextSackbotPlayerNumber = -2;
    public CutsceneCameraManager CutsceneCameraManager = new();
    public GlobalAudioSettings GlobalAudioSettings = new();
    public ResourceDescriptor BackdropPlan, BackdropNewPlan;
    public bool SubLevel;
    public bool ScoreLocked;
    public int DebugTimeInLevel;
    public bool UseEvenNewerCheckpointCode;
    public MoveCursor[]? MoveCursors;
    
    public bool SinglePlayer;
    public int MinPlayers = 1, MaxPlayers = 4;
    public bool MoveRecommended;
    public bool FixInvalidInOutMoverContacts;
    public bool ContinueMusic;

    public BroadcastMicrochipEntry[]? BroadcastMicroChipEntries;
    public int ManualJumpDown;
    public Thing[] DeferredDestroys;
    public float GlobalDofFront, GlobalDofBack, GlobalDofSackTrack;
    public bool EnableSackpocket;
    public bool ShowQuestLog;
    public SlotID ScoreboardUnlockLevelSlot;
    public string ProgressBoardLevelLinkStartPoint;
    public bool IsLbp3World;

    /* Vita Fields */

    public bool NonLinearFog;
    public EggLink[]? BronzeRewards;
    public EggLink[]? SilverRewards;
    public EggLink[]? GoldRewards;
    public KeyLink[]? BronzeUnlocks;
    public KeyLink[]? SilverUnlocks;
    public KeyLink[]? GoldUnlocks;
    public byte BronzeTrophyConditionType, SilverTrophyConditionType, GoldTrophyConditionType;
    public int ScoreRequiredForBronzeTrophy, ScoreRequiredForSilverTrophy, ScoreRequiredForGoldTrophy;
    public float TimeRequiredForBronzeTrophy, TimeRequiredForSilverTrophy, TimeRequiredForGoldTrophy;
    public int LivesLostRequiredForBronzeTrophy, LivesLostRequiredForSilverTrophy, LivesLostRequiredForGoldTrophy;

    public bool EnableAcing, EnableGoldTrophy, EnableSilverTrophy, EnableBronzeTrophy;
    public bool EnforceMinMaxPlayers;
    
    public int WaterColor;
    public float WaterBrightness;

    public int GlobalTouchCursor;


    public bool PortraitMode; 
    public bool SharedScreen;
    public bool DisableHud;
    public bool DisableShadows;
    public bool FlipBackground;
    public bool MpSeparateScreen;
    
    public PWorld() {
        CurrGlobalSettingsBlendFactors = new float[12];
        GlobalSettingsThingUiDs = new int[12];
        GlobalSettingsThingPriority = new int[12];
        for (var i = 0; i < 12; ++i)
            CurrGlobalSettingsBlendFactors[i] = 1.0f;
    }

    public void Serialize() {
        var revision = Serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (subVersion >= 0x6d) {
            Serializer.Serialize(ref BackdropOffsetX);
            Serializer.Serialize(ref BackdropOffsetY);
            Serializer.Serialize(ref BackdropOffsetZ);
        }

        if (subVersion is >= 0x72 and <= 0x73) {
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
        }

        if (subVersion >= 0x70)
            Serializer.Serialize(ref BackdropOffsetZAuto);
        if (subVersion >= 0xe2)
            Serializer.Serialize(ref OverrideBackdropAmbience);
        
        if (version < 0x14b) {
            if (Serializer.IsWriting()) Serializer.GetOutput().I32(0);
            else {
                Materials = new ResourceDescriptor[Serializer.GetInput().I32()];
                for (var i = 0; i < Materials.Length; ++i)
                    Serializer.Serialize(ref Materials[i]);
            }
        }

        if (subVersion >= 0x3f)
            Serializer.Serialize(ref StreamingManager);
        
        if (!revision.IsToolkit() || revision.Before(Branch.Mizuki, (int)Revisions.MZ_SCENE_GRAPH)) {
            Things = Serializer.Serialize(ref Things);
            Serializer.Log("END OF WORLD THINGS");

            Serializer.Serialize(ref MaxVel);
            Serializer.Serialize(ref MaxAVel);
    
            Serializer.Serialize(ref Frame);
            if (version >= 0x2e2) Serializer.Serialize(ref SimFrame);
            if (version >= 0x377) Serializer.Serialize(ref FrameLevelStarted);

            Serializer.Serialize(ref ThingUidCounter);
        }

        
        if (version < 0x32d)
            Serializer.Serialize(ref Randy);

        if (version < 0x1a4) {
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            if (version < 0x14b)
                Serializer.Array<Thing>(null);
        }

        if (!revision.IsToolkit() || revision.Before(Branch.Mizuki, (int)Revisions.MZ_SCENE_GRAPH)) {
            Selections = Serializer.Serialize(ref Selections, true);

            Serializer.Serialize(ref Backdrop);
            Serializer.Serialize(ref BackdropNew);
            Serializer.Serialize(ref BackdropTimer);
        }

        if (version >= 0x3a3)
            Serializer.Serialize(ref Lbp2NightDaySwapped);

        if (version >= 0x14a)
            Serializer.Serialize(ref IsPaused);

        if (version >= 0x152) {
            Serializer.Serialize(ref LightingFactor);
            Serializer.Serialize(ref ColorCorrectionFactor);
            if (version >= 0x196) {
                Serializer.Serialize(ref FogFactor);
                Serializer.Serialize(ref FogTintFactor);
                Serializer.Serialize(ref DarknessFactor);
                if (revision.Has(Branch.Double11, 0x78))
                    Serializer.Serialize(ref NonLinearFog);
            }
        }

        if (version >= 0x16f) {
            CompleteRewards = Serializer.Serialize(ref CompleteRewards);
            CollectRewards = Serializer.Serialize(ref CollectRewards);
            AceRewards = Serializer.Serialize(ref AceRewards);
            if (version >= 0x208) 
                Serializer.Serialize(ref AreRewardsShareable);
            if (version >= 0x35e)
                Serializer.Serialize(ref ScoreboardLevelLinkSlot);
            CompleteUnlocks = Serializer.Serialize(ref CompleteUnlocks);
            CollectUnlocks = Serializer.Serialize(ref CollectUnlocks);
            AceUnlocks = Serializer.Serialize(ref AceUnlocks);
        }

        if (revision.IsVita()) {

            if (revision.Has(Branch.Double11, 0x22) && revision.Before(Branch.Double11, 0x25)) {
                Serializer.Array<EggLink>(null); // unlocks
                Serializer.Array<EggLink>(null);  // rewards
                Serializer.Serialize(ref 0); // timeRequired
            }

            if (revision.Has(Branch.Double11, 0x25)) {
                GoldRewards = Serializer.Serialize(ref GoldRewards);
                GoldUnlocks = Serializer.Serialize(ref GoldUnlocks);
                SilverRewards = Serializer.Serialize(ref SilverRewards);
                SilverUnlocks = Serializer.Serialize(ref SilverUnlocks);
                BronzeRewards = Serializer.Serialize(ref BronzeRewards);
                BronzeUnlocks = Serializer.Serialize(ref BronzeUnlocks);

                Serializer.Serialize(ref GoldTrophyConditionType);
                Serializer.Serialize(ref SilverTrophyConditionType);
                Serializer.Serialize(ref BronzeTrophyConditionType);

                Serializer.Serialize(ref ScoreRequiredForGoldTrophy);
                Serializer.Serialize(ref TimeRequiredForGoldTrophy);
                Serializer.Serialize(ref LivesLostRequiredForGoldTrophy);

                Serializer.Serialize(ref ScoreRequiredForSilverTrophy);
                Serializer.Serialize(ref TimeRequiredForSilverTrophy);
                Serializer.Serialize(ref LivesLostRequiredForSilverTrophy);

                Serializer.Serialize(ref ScoreRequiredForBronzeTrophy);
                Serializer.Serialize(ref TimeRequiredForBronzeTrophy);
                Serializer.Serialize(ref LivesLostRequiredForBronzeTrophy);
            }

            if (revision.Has(Branch.Double11, 0x3d)) {
                Serializer.Serialize(ref EnableAcing);
                Serializer.Serialize(ref EnableGoldTrophy);
                Serializer.Serialize(ref EnableSilverTrophy);
                Serializer.Serialize(ref EnableBronzeTrophy);
            }

            if (revision.Has(Branch.Double11, 0x4c))
                Serializer.Serialize(ref EnforceMinMaxPlayers);
        }

        if (version is > 0x16e and < 0x1bf) {
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0);
        }

        switch (version)
        {
            case > 0x1a3 and < 0x1d1:
                throw new SerializationException("CGameCamera serialization unsupported!");
            case > 0x1bd and < 0x213:
                Serializer.Serialize(ref 0);
                break;
        }

        if (version is >= 0x1c2 and < 0x36e)
            Serializer.Serialize(ref DeathCount);
        if (version >= 0x1c4)
            Serializer.Serialize(ref MaxNumPlayers);

        if (version >= 0x1db) {
            DissolvingThings = Serializer.Serialize(ref DissolvingThings, true);
            OldDissolvingThings = Serializer.Serialize(ref OldDissolvingThings, true);
        }

        if (version is >= 0x1de and < 0x345)
            Serializer.Serialize(ref IsTutorialLevel);

        if (version >= 0x22e) {
            Serializer.Serialize(ref EverSpawned);
            Serializer.Serialize(ref SpawnFailureCount);
        }

        if (version >= 0x25a) {
            Serializer.Serialize(ref TargetGlobalSettings);
            Serializer.Serialize(ref FromGlobalSettings);
            Serializer.Serialize(ref GlobalSettingsBlendFactor);
            Serializer.Serialize(ref HasLevelLightingBeenSetup);
            Serializer.Serialize(ref GlobalSettingsThingUid);
        }

        if (version >= 0x370)
            Serializer.Serialize(ref CameraSettings);

        if (version >= 0x26f) {
            Serializer.Serialize(ref WaterLevel);
            if (version >= 0x278 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_WATER_WAVE))
                Serializer.Serialize(ref FromWaterLevel);
            Serializer.Serialize(ref TargetWaterLevel);
        }

        if (version >= 0x270) {
            Serializer.Serialize(ref WaterWaveMagnitude);
            Serializer.Serialize(ref FromWaterWaveMagnitude);
            Serializer.Serialize(ref TargetWaterWaveMagnitude);
        }

        if (version >= 0x26f) {
            Serializer.Serialize(ref Gravity);
            Serializer.Serialize(ref FromGravity);
            Serializer.Serialize(ref TargetGravity);

            Serializer.Serialize(ref CurrGlobalSettingsBlendFactors);
            Serializer.Serialize(ref GlobalSettingsThingUiDs);
            if (version >= 0x270)
                Serializer.Serialize(ref GlobalSettingsThingPriority);
        }

        if (revision.Has(Branch.Double11, 0x2d)) {
            Serializer.Serialize(ref WaterColor);
            Serializer.Serialize(ref WaterBrightness);
        } else if (version >= 0x289 || revision.IsLeerdammer()) {
            Serializer.Serialize(ref WaterTint);
            Serializer.Serialize(ref FromWaterTintColor);
            Serializer.Serialize(ref TargetWaterTint);
        }
        
        if (version >= 0x289 || revision.IsLeerdammer()) {
            Serializer.Serialize(ref WaterMurkiness);
            Serializer.Serialize(ref FromWaterMurkiness);
            Serializer.Serialize(ref TargetWaterMurkiness);
        }

        if (version >= 0x2b4 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_WATER_BITS)) {
            Serializer.Serialize(ref WaterBits);
            Serializer.Serialize(ref FromWaterBits);
            Serializer.Serialize(ref TargetWaterBits);
        }

        if (version >= 0x34e) {
            Serializer.Serialize(ref WaterDrainSoundsEnabled);
            Serializer.Serialize(ref CurrWaterDrainSoundsEnabled);
        }

        if (subVersion >= 0xe8) {
            Serializer.Serialize(ref WaterCausticsEnabled);
            CurrSerializer.Serialize(ref WaterCausticsEnabled);
        }

        if (subVersion >= 0xf8) {
            Serializer.Serialize(ref WaterMainColor);
            Serializer.Serialize(ref FromWaterMainColor);
            Serializer.Serialize(ref TargetWaterMainColor);

            Serializer.Serialize(ref WaterHintColorOne);
            Serializer.Serialize(ref FromWaterHintColorOne);
            Serializer.Serialize(ref TargetWaterHintColorOne);

            Serializer.Serialize(ref WaterHintColorTwo);
            Serializer.Serialize(ref FromWaterHintColorTwo);
            Serializer.Serialize(ref TargetWaterHintColorTwo);
        }

        if (subVersion is >= 0xf8 and < 0x189)
            Serializer.Serialize(ref false);

        if (subVersion >= 0x182) {
            Serializer.Serialize(ref BackdropEnabled);
            Serializer.Serialize(ref CurrBackdropEnabled);
        }

        if (version >= 0x29c || revision.Has(Branch.Leerdammer, (int)Revisions.LD_WATER_WAVE))
            Serializer.Serialize(ref CurrWavePos);
        

        if (version is > 0x288 and < 0x29c || (revision.IsLeerdammer() && revision.Before(Branch.Leerdammer, (int)Revisions.LD_WATER_WAVE))) {
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref false);
        }

        switch (version)
        {
            // CBreadLoaf
            // Serializer.Serialize(ref 0); // loafAlloc.numHandles
            // Serializer.Serialize(ref 0); // loafAlloc.freehead
            // Serializer.Serialize(ref 0); // loafAlloc.numUsed
            // // handles?
            // // reflect array 2, loafMin floats
            // Serializer.Serialize(ref 0); // loafSize
            // Serializer.Serialize(ref 0); // maxDepth
            // Serializer.Serialize(ref 0); // first
            // // reflect array, x via numHandles float
            // // reflect array, y via numHandles float
            // // reflect 4 LoafHandle's short?
            // // firstcrumb short
            // // depth, bytearray?
            case > 0x281 and < 0x287 when Serializer.Serialize(ref 0) != 0:
                throw new SerializationException("CBreadLoaf serialization not supported!");
            case >= 0x2a3:
                Serializer.Serialize(ref GameMode);
                break;
        }

        if (subVersion >= 0x218) 
            Serializer.Serialize(ref GameModeRequested);

        if (version >= 0x2b0) 
            Serializer.Serialize(ref NextSackbotPlayerNumber);

        switch (version)
        {
            case > 0x2d3 and < 0x2f3:
                throw new SerializationException("Unsupported structure in serialization");
            case >= 0x2ee:
                Serializer.Serialize(ref CutsceneCameraManager);
                break;
        }

        if (version >= 0x30c)
            Serializer.Serialize(ref GlobalAudioSettings);
        
        if ((version >= 0x321 && !revision.IsToolkit()) || revision.Before(Branch.Mizuki, (int)Revisions.MZ_SCENE_GRAPH)) {
            Serializer.Serialize(ref BackdropPlan, BackdropPlan, ResourceType.Plan, true);
            Serializer.Serialize(ref BackdropNewPlan, BackdropNewPlan, ResourceType.Plan, true);
        }

        if (version >= 0x352)
            Serializer.Serialize(ref SubLevel);

        if (version >= 0x38a)
            Serializer.Serialize(ref ScoreLocked);

        if (version >= 0x38b)
            Serializer.Serialize(ref DebugTimeInLevel);

        if (version >= 0x3ac)
            Serializer.Serialize(ref UseEvenNewerCheckpointCode);
        
        if (version >= 0x3bd && subVersion <= 0x117)
            MoveCursors = Serializer.Serialize(ref MoveCursors);

        // version > 0x3c0, rather than 0x3e1 for some reason
        if (revision.Has(Branch.Double11, 0x8))
            Serializer.Serialize(ref GlobalTouchCursor);
        if (revision.Has(Branch.Double11, 0xa) && revision.Before(Branch.Double11, 0x28)) 
            Serializer.Serialize(ref SharedScreen);

        if (subVersion > 0x215)
            Serializer.Serialize(ref SinglePlayer);
        
        if (version >= 0x3d0) {
            Serializer.Serialize(ref MinPlayers);
            Serializer.Serialize(ref MaxPlayers);
            Serializer.Serialize(ref MoveRecommended);
        }

        if (revision.IsVita()) {
            int vita = revision.GetBranchRevision();

            // version > 0x3d4, rathern than 0x3e1 for some reason
            if (vita >= 0x18) 
                Serializer.Serialize(ref PortraitMode);

            if (vita is >= 0x28 and < 0x47) {
                if (Serializer.Serialize(ref 0) > 1 && !Serializer.IsWriting())
                    SharedScreen = true;
            }

            if (version >= 0x3dd)
                Serializer.Serialize(ref FixInvalidInOutMoverContacts);
            
            if (vita >= 0x47) Serializer.Serialize(ref SharedScreen);
            if (vita >= 0x39) Serializer.Serialize(ref DisableHud);
            if (vita >= 0x52) Serializer.Serialize(ref DisableShadows);
            if (vita >= 0x3b) Serializer.Serialize(ref FlipBackground); 
            if (vita >= 0x4f) Serializer.Serialize(ref MpSeparateScreen);

        }

        if (version >= 0x3dd)
            Serializer.Serialize(ref FixInvalidInOutMoverContacts);

        if (version >= 0x3f1)
            Serializer.Serialize(ref ContinueMusic);

        if (subVersion >= 0x2f)
            BroadcastMicroChipEntries = Serializer.Serialize(ref BroadcastMicroChipEntries);

        if (subVersion >= 0x5d)
            Serializer.Serialize(ref ManualJumpDown);

        if (subVersion is >= 0x98 and < 0xe5)
            Serializer.Serialize(ref null);
        if (subVersion is >= 0xc3 and < 0xe5)
            Serializer.Serialize(ref null);
        switch (subVersion)
        {
            case >= 0x98 and < 0xe5:
                Serializer.Serialize(ref null);
                break;
            case >= 0xe5:
                DeferredDestroys = Serializer.Serialize(ref DeferredDestroys, true);
                break;
        }

        if (subVersion >= 0xcf) {
            Serializer.Serialize(ref GlobalDofFront);
            Serializer.Serialize(ref GlobalDofBack);
            Serializer.Serialize(ref GlobalDofSackTrack);
        }

        if (subVersion >= 0xda)
            Serializer.Serialize(ref EnableSackpocket);

        if (subVersion >= 0x170)
            Serializer.Serialize(ref ShowQuestLog);

        if (subVersion >= 0x11d)
            Serializer.Serialize(ref ScoreboardUnlockLevelSlot);

        if (subVersion >= 0x154)
            Serializer.Serialize(ref ProgressBoardLevelLinkStartPoint);

        if (subVersion >= 0x15e)
            Serializer.Serialize(ref IsLbp3World);
    }

    public int GetAllocatedSize() { return 0; }
    
}