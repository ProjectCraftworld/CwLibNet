using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Streaming;
using CwLibNet.Types.Data;
using CwLibNet.Types.Things.Components;
using CwLibNet.Types.Things.Components.World;

namespace CwLibNet.Types.Things.Parts;

public class PWorld: ISerializable
{
    public ResourceDescriptor[] materials;
    public float backdropOffsetX, backdropOffsetY, backdropOffsetZ;
    public bool backdropOffsetZAuto;
    public String overrideBackdropAmbience;
    
    public StreamingManager streamingManager;
    public List<Thing> things = [];
    public float maxVel = 100.0f, maxAVel = 1.0f;
    public int frame;
    public int simFrame;
    public int frameLevelStarted;
    public int randy;
    public int thingUIDCounter = 2;
    public EditorSelection[] selections;
    public Thing backdrop, backdropNew;
    public float backdropTimer;
    public int lbp2NightDaySwapped = 1;
    public bool isPaused;
    public float lightingFactor = 1.0f, colorCorrectionFactor;
    public float fogFactor, fogTintFactor, darknessFactor;
    public EggLink[] completeRewards, collectRewards, aceRewards;
    public bool areRewardsShareable = true;
    public SlotID scoreboardLevelLinkSlot;
    public KeyLink[] completeUnlocks, collectUnlocks, aceUnlocks;
    public int deathCount;
    public int maxNumPlayers = 1;
    public Thing[] dissolvingThings, oldDissolvingThings;
    public bool isTutorialLevel;
    public bool everSpawned;
    public int spawnFailureCount;
    public GlobalSettings targetGlobalSettings = new(), fromGlobalSettings = new();
    public float globalSettingsBlendFactor;
    public bool hasLevelLightingBeenSetup;
    public int globalSettingsThingUID;
    public CameraSettings cameraSettings = new CameraSettings();
    public float waterLevel, targetWaterLevel;
    
    
    public float fromWaterLevel;
    public float waterWaveMagnitude, fromWaterWaveMagnitude, targetWaterWaveMagnitude;
    public float gravity = 1.0f, fromGravity = 1.0f, targetGravity = 1.0f;
    public float[] currGlobalSettingsBlendFactors;
    public int[] globalSettingsThingUIDs;
    public int[] globalSettingsThingPriority;
    
    
    
    public float waterTint, targetWaterTint;

    
    
    public Vector4? fromWaterTintColor = new Vector4(0.47451f, 0.898039f, 0.909804f, 0.125f);

    
    
    public float waterMurkiness, fromWaterMurkiness, targetWaterMurkiness;

    
    
    public float waterBits = 1.0f, fromWaterBits = 1.0f, targetWaterBits = 1.0f;

    public bool waterDrainSoundsEnabled = true, currWaterDrainSoundsEnabled = true;
    public bool waterCausticsEnabled = true, currWaterCausticsEnabled = true;
    
    
    public int waterMainColor = -1090453761, fromWaterMainColor = -1090453761, targetWaterMainColor = -1090453761;
    
    public int waterHintColorOne = -33554177, fromWaterHintColorOne = -33554177, targetWaterHintColorOne = -33554177;
    
    public int waterHintColorTwo = 16662783, fromWaterHintColorTwo = 16662783, targetWaterHintColorTwo = 16662783;

    public bool backdropEnabled = true, currBackdropEnabled = true;

    
    
    public float currWavePos = 0.0186706f;


    public int gameMode = 0;
    public int gameModeRequested = 0;

    public int nextSackbotPlayerNumber = -2;
    public CutsceneCameraManager cutsceneCameraManager = new CutsceneCameraManager();
    public GlobalAudioSettings globalAudioSettings = new GlobalAudioSettings();
    public ResourceDescriptor backdropPlan, backdropNewPlan;
    public bool subLevel;
    public bool scoreLocked;
    public int debugTimeInLevel;
    public bool useEvenNewerCheckpointCode;
    public MoveCursor[] moveCursors;
    
    public bool singlePlayer;
    public int minPlayers = 1, maxPlayers = 4;
    public bool moveRecommended;
    public bool fixInvalidInOutMoverContacts;
    public bool continueMusic;

    public BroadcastMicrochipEntry[] broadcastMicroChipEntries;
    public int manualJumpDown;
    public Thing[] deferredDestroys;
    public float globalDofFront, globalDofBack, globalDofSackTrack;
    public bool enableSackpocket;
    public bool showQuestLog;
    public SlotID scoreboardUnlockLevelSlot;
    public String progressBoardLevelLinkStartPoint;
    public bool isLBP3World;

    /* Vita Fields */

    public bool nonLinearFog;
    public EggLink[] bronzeRewards, silverRewards, goldRewards;
    public KeyLink[] bronzeUnlocks, silverUnlocks, goldUnlocks;
    public byte bronzeTrophyConditionType, silverTrophyConditionType, goldTrophyConditionType;
    public int scoreRequiredForBronzeTrophy, scoreRequiredForSilverTrophy, scoreRequiredForGoldTrophy;
    public float timeRequiredForBronzeTrophy, timeRequiredForSilverTrophy, timeRequiredForGoldTrophy;
    public int livesLostRequiredForBronzeTrophy, livesLostRequiredForSilverTrophy, livesLostRequiredForGoldTrophy;

    public bool enableAcing, enableGoldTrophy, enableSilverTrophy, enableBronzeTrophy;
    public bool enforceMinMaxPlayers;
    
    public int waterColor;
    public float waterBrightness;

    public int globalTouchCursor;


    public bool portraitMode; 
    public bool sharedScreen;
    public bool disableHUD;
    public bool disableShadows;
    public bool flipBackground;
    public bool mpSeparateScreen;
    
    public PWorld() {
        this.currGlobalSettingsBlendFactors = new float[12];
        this.globalSettingsThingUIDs = new int[12];
        this.globalSettingsThingPriority = new int[12];
        for (int i = 0; i < 12; ++i)
            this.currGlobalSettingsBlendFactors[i] = 1.0f;
    }

    public void Serialize(Serializer serializer) {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        if (subVersion >= 0x6d) {
            backdropOffsetX = serializer.F32(backdropOffsetX);
            backdropOffsetY = serializer.F32(backdropOffsetY);
            backdropOffsetZ = serializer.F32(backdropOffsetZ);
        }

        if (subVersion >= 0x72 && subVersion <= 0x73) {
            serializer.F32(0);
            serializer.F32(0);
            serializer.F32(0);
            serializer.U8(0);
            serializer.U8(0);
        }

        if (subVersion >= 0x70)
            backdropOffsetZAuto = serializer.Bool(backdropOffsetZAuto);
        if (subVersion >= 0xe2)
            overrideBackdropAmbience = serializer.Str(overrideBackdropAmbience);
        
        if (version < 0x14b) {
            if (serializer.IsWriting()) serializer.GetOutput().I32(0);
            else {
                materials = new ResourceDescriptor[serializer.GetInput().I32()];
                for (int i = 0; i < materials.Length; ++i)
                    materials[i] = serializer.Resource(null, ResourceType.Material);
            }
        }

        if (subVersion >= 0x3f)
            streamingManager = serializer.Reference(streamingManager);
        
        if (!revision.IsToolkit() || revision.Before(Branch.Mizuki, (int)Revisions.MzSceneGraph)) {
            things = serializer.Arraylist(things, true);
            serializer.Log("END OF WORLD THINGS");

            maxVel = serializer.F32(maxVel);
            maxAVel = serializer.F32(maxAVel);
    
            frame = serializer.S32(frame);
            if (version >= 0x2e2) simFrame = serializer.S32(simFrame);
            if (version >= 0x377) frameLevelStarted = serializer.I32(frameLevelStarted);

            thingUIDCounter = serializer.I32(thingUIDCounter);
        }

        
        if (version < 0x32d)
            randy = serializer.I32(randy);

        if (version < 0x1a4) {
            serializer.F32(0);
            serializer.F32(0);
            serializer.F32(0);
            serializer.F32(0);
            serializer.F32(0);
            serializer.F32(0);
            serializer.I32(0);
            if (version < 0x14b)
                serializer.Array<Thing>(null);
        }

        if (!revision.IsToolkit() || revision.Before(Branch.Mizuki, (int)Revisions.MzSceneGraph)) {
            selections = serializer.Array(selections, true);

            backdrop = serializer.Reference(backdrop);
            backdropNew = serializer.Reference(backdropNew);
            backdropTimer = serializer.F32(backdropTimer);
        }

        if (version >= 0x3a3)
            lbp2NightDaySwapped = serializer.S32(lbp2NightDaySwapped);

        if (version >= 0x14a)
            isPaused = serializer.Bool(isPaused);

        if (version >= 0x152) {
            lightingFactor = serializer.F32(lightingFactor);
            colorCorrectionFactor = serializer.F32(colorCorrectionFactor);
            if (version >= 0x196) {
                fogFactor = serializer.F32(fogFactor);
                fogTintFactor = serializer.F32(fogTintFactor);
                darknessFactor = serializer.F32(darknessFactor);
                if (revision.Has(Branch.Double11, 0x78))
                    nonLinearFog = serializer.Bool(nonLinearFog);
            }
        }

        if (version >= 0x16f) {
            completeRewards = serializer.Array(completeRewards);
            collectRewards = serializer.Array(collectRewards);
            aceRewards = serializer.Array(aceRewards);
            if (version >= 0x208) 
                areRewardsShareable = serializer.Bool(areRewardsShareable);
            if (version >= 0x35e)
                scoreboardLevelLinkSlot = serializer.Struct(scoreboardLevelLinkSlot);
            completeUnlocks = serializer.Array(completeUnlocks);
            collectUnlocks = serializer.Array(collectUnlocks);
            aceUnlocks = serializer.Array(aceUnlocks);
        }

        if (revision.IsVita()) {

            if (revision.Has(Branch.Double11, 0x22) && revision.Before(Branch.Double11, 0x25)) {
                serializer.Array<EggLink>(null); // unlocks
                serializer.Array<EggLink>(null);  // rewards
                serializer.S32(0); // timeRequired
            }

            if (revision.Has(Branch.Double11, 0x25)) {
                goldRewards = serializer.Array(goldRewards);
                goldUnlocks = serializer.Array(goldUnlocks);
                silverRewards = serializer.Array(silverRewards);
                silverUnlocks = serializer.Array(silverUnlocks);
                bronzeRewards = serializer.Array(bronzeRewards);
                bronzeUnlocks = serializer.Array(bronzeUnlocks);

                goldTrophyConditionType = serializer.I8(goldTrophyConditionType);
                silverTrophyConditionType = serializer.I8(silverTrophyConditionType);
                bronzeTrophyConditionType = serializer.I8(bronzeTrophyConditionType);

                scoreRequiredForGoldTrophy = serializer.S32(scoreRequiredForGoldTrophy);
                timeRequiredForGoldTrophy = serializer.F32(timeRequiredForGoldTrophy);
                livesLostRequiredForGoldTrophy = serializer.S32(livesLostRequiredForGoldTrophy);

                scoreRequiredForSilverTrophy = serializer.S32(scoreRequiredForSilverTrophy);
                timeRequiredForSilverTrophy = serializer.F32(timeRequiredForSilverTrophy);
                livesLostRequiredForSilverTrophy = serializer.S32(livesLostRequiredForSilverTrophy);

                scoreRequiredForBronzeTrophy = serializer.S32(scoreRequiredForBronzeTrophy);
                timeRequiredForBronzeTrophy = serializer.F32(timeRequiredForBronzeTrophy);
                livesLostRequiredForBronzeTrophy = serializer.S32(livesLostRequiredForBronzeTrophy);
            }

            if (revision.Has(Branch.Double11, 0x3d)) {
                enableAcing = serializer.Bool(enableAcing);
                enableGoldTrophy = serializer.Bool(enableGoldTrophy);
                enableSilverTrophy = serializer.Bool(enableSilverTrophy);
                enableBronzeTrophy = serializer.Bool(enableBronzeTrophy);
            }

            if (revision.Has(Branch.Double11, 0x4c))
                enforceMinMaxPlayers = serializer.Bool(enforceMinMaxPlayers);
        }

        if (0x16e < version && version < 0x1bf) {
            serializer.I32(0);
            serializer.I32(0);
            serializer.I32(0);
        }

        if (0x1a3 < version && version < 0x1d1)
            throw new SerializationException("CGameCamera serialization unsupported!");

        if (0x1bd < version && version < 0x213)
            serializer.I32(0);

        if (version >= 0x1c2 && version < 0x36e)
            deathCount = serializer.I32(deathCount);
        if (version >= 0x1c4)
            maxNumPlayers = serializer.I32(maxNumPlayers);

        if (version >= 0x1db) {
            dissolvingThings = serializer.Array(dissolvingThings, true);
            oldDissolvingThings = serializer.Array(oldDissolvingThings, true);
        }

        if (version >= 0x1de && version < 0x345)
            isTutorialLevel = serializer.Bool(isTutorialLevel);

        if (version >= 0x22e) {
            everSpawned = serializer.Bool(everSpawned);
            spawnFailureCount = serializer.I32(spawnFailureCount);
        }

        if (version >= 0x25a) {
            targetGlobalSettings = serializer.Struct(targetGlobalSettings);
            fromGlobalSettings = serializer.Struct(fromGlobalSettings);
            globalSettingsBlendFactor = serializer.F32(globalSettingsBlendFactor);
            hasLevelLightingBeenSetup = serializer.Bool(hasLevelLightingBeenSetup);
            globalSettingsThingUID = serializer.I32(globalSettingsThingUID);
        }

        if (version >= 0x370)
            cameraSettings = serializer.Struct(cameraSettings);

        if (version >= 0x26f) {
            waterLevel = serializer.F32(waterLevel);
            if (version >= 0x278 || revision.Has(Branch.Leerdammer, (int)Revisions.LdWaterWave))
                fromWaterLevel = serializer.F32(fromWaterLevel);
            targetWaterLevel = serializer.F32(targetWaterLevel);
        }

        if (version >= 0x270) {
            waterWaveMagnitude = serializer.F32(waterWaveMagnitude);
            fromWaterWaveMagnitude = serializer.F32(fromWaterWaveMagnitude);
            targetWaterWaveMagnitude = serializer.F32(targetWaterWaveMagnitude);
        }

        if (version >= 0x26f) {
            gravity = serializer.F32(gravity);
            fromGravity = serializer.F32(fromGravity);
            targetGravity = serializer.F32(targetGravity);

            currGlobalSettingsBlendFactors = serializer.Floatarray(currGlobalSettingsBlendFactors);
            globalSettingsThingUIDs = serializer.Intarray(globalSettingsThingUIDs);
            if (version >= 0x270)
                globalSettingsThingPriority = serializer.Intarray(globalSettingsThingPriority);
        }

        if (revision.Has(Branch.Double11, 0x2d)) {
            waterColor = serializer.S32(waterColor);
            waterBrightness = serializer.F32(waterBrightness);
        } else if (version >= 0x289 || revision.IsLeerdammer()) {
            waterTint = serializer.F32(waterTint);
            fromWaterTintColor = serializer.V4(fromWaterTintColor);
            targetWaterTint = serializer.F32(targetWaterTint);
        }
        
        if (version >= 0x289 || revision.IsLeerdammer()) {
            waterMurkiness = serializer.F32(waterMurkiness);
            fromWaterMurkiness = serializer.F32(fromWaterMurkiness);
            targetWaterMurkiness = serializer.F32(targetWaterMurkiness);
        }

        if (version >= 0x2b4 || revision.Has(Branch.Leerdammer, (int)Revisions.LdWaterBits)) {
            waterBits = serializer.F32(waterBits);
            fromWaterBits = serializer.F32(fromWaterBits);
            targetWaterBits = serializer.F32(targetWaterBits);
        }

        if (version >= 0x34e) {
            waterDrainSoundsEnabled = serializer.Bool(waterDrainSoundsEnabled);
            currWaterDrainSoundsEnabled = serializer.Bool(currWaterDrainSoundsEnabled);
        }

        if (subVersion >= 0xe8) {
            waterCausticsEnabled = serializer.Bool(waterCausticsEnabled);
            currWaterCausticsEnabled = serializer.Bool(waterCausticsEnabled);
        }

        if (subVersion >= 0xf8) {
            waterMainColor = serializer.I32(waterMainColor);
            fromWaterMainColor = serializer.I32(fromWaterMainColor);
            targetWaterMainColor = serializer.I32(targetWaterMainColor);

            waterHintColorOne = serializer.I32(waterHintColorOne);
            fromWaterHintColorOne = serializer.I32(fromWaterHintColorOne);
            targetWaterHintColorOne = serializer.I32(targetWaterHintColorOne);

            waterHintColorTwo = serializer.I32(waterHintColorTwo);
            fromWaterHintColorTwo = serializer.I32(fromWaterHintColorTwo);
            targetWaterHintColorTwo = serializer.I32(targetWaterHintColorTwo);
        }

        if (subVersion >= 0xf8 && subVersion < 0x189)
            serializer.Bool(false);

        if (subVersion >= 0x182) {
            backdropEnabled = serializer.Bool(backdropEnabled);
            currBackdropEnabled = serializer.Bool(currBackdropEnabled);
        }

        if (version >= 0x29c || revision.Has(Branch.Leerdammer, (int)Revisions.LdWaterWave))
            currWavePos = serializer.F32(currWavePos);
        

        if ((version > 0x288 && version < 0x29c) || (revision.IsLeerdammer() && revision.Before(Branch.Leerdammer, (int)Revisions.LdWaterWave))) {
            serializer.F32(0);
            serializer.Bool(false);
        }

        // CBreadLoaf
        if (0x281 < version && version < 0x287) {
            if (serializer.I32(0) != 0)
                throw new SerializationException("CBreadLoaf serialization not supported!");

            // serializer.I32(0); // loafAlloc.numHandles
            // serializer.I32(0); // loafAlloc.freehead
            // serializer.I32(0); // loafAlloc.numUsed
            // // handles?

            // // reflect array 2, loafMin floats
            // serializer.I32(0); // loafSize
            // serializer.u16(0); // maxDepth
            // serializer.u16(0); // first

            // // reflect array, x via numHandles float
            // // reflect array, y via numHandles float
            // // reflect 4 LoafHandle's short?
            // // firstcrumb short
            // // depth, bytearray?
        }

        if (version >= 0x2a3) gameMode = serializer.I32(gameMode);
    
        if (subVersion >= 0x218) 
            gameModeRequested = serializer.I32(gameModeRequested);

        if (version >= 0x2b0) 
            nextSackbotPlayerNumber = serializer.S32(nextSackbotPlayerNumber);

        if (0x2d3 < version && version < 0x2f3)
            throw new SerializationException("Unsupported structure in serialization");

        if (version >= 0x2ee)
            cutsceneCameraManager = serializer.Struct(cutsceneCameraManager);

        if (version >= 0x30c)
            globalAudioSettings = serializer.Struct(globalAudioSettings);
        
        if ((version >= 0x321 && !revision.IsToolkit()) || revision.Before(Branch.Mizuki, (int)Revisions.MzSceneGraph)) {
            backdropPlan = serializer.Resource(backdropPlan, ResourceType.Plan, true);
            backdropNewPlan = serializer.Resource(backdropNewPlan, ResourceType.Plan, true);
        }

        if (version >= 0x352)
            subLevel = serializer.Bool(subLevel);

        if (version >= 0x38a)
            scoreLocked = serializer.Bool(scoreLocked);

        if (version >= 0x38b)
            debugTimeInLevel = serializer.S32(debugTimeInLevel);

        if (version >= 0x3ac)
            useEvenNewerCheckpointCode = serializer.Bool(useEvenNewerCheckpointCode);
        
        if (version >= 0x3bd && subVersion <= 0x117)
            moveCursors = serializer.Array(moveCursors);

        // version > 0x3c0, rather than 0x3e1 for some reason
        if (revision.Has(Branch.Double11, 0x8))
            globalTouchCursor = serializer.I32(globalTouchCursor);
        if (revision.Has(Branch.Double11, 0xa) && revision.Before(Branch.Double11, 0x28)) 
            sharedScreen = serializer.Bool(sharedScreen);

        if (subVersion > 0x215)
            singlePlayer = serializer.Bool(singlePlayer);
        
        if (version >= 0x3d0) {
            minPlayers = serializer.U8(minPlayers);
            maxPlayers = serializer.U8(maxPlayers);
            moveRecommended = serializer.Bool(moveRecommended);
        }

        if (revision.IsVita()) {
            int vita = revision.GetBranchRevision();

            // version > 0x3d4, rathern than 0x3e1 for some reason
            if (vita >= 0x18) 
                portraitMode = serializer.Bool(portraitMode);

            if (vita >= 0x28 && vita < 0x47) {
                if (serializer.U8(0) > 1 && !serializer.IsWriting())
                    sharedScreen = true;
            }

            if (version >= 0x3dd)
                fixInvalidInOutMoverContacts = serializer.Bool(fixInvalidInOutMoverContacts);
            
            if (vita >= 0x47) sharedScreen = serializer.Bool(sharedScreen);
            if (vita >= 0x39) disableHUD = serializer.Bool(disableHUD);
            if (vita >= 0x52) disableShadows = serializer.Bool(disableShadows);
            if (vita >= 0x3b) flipBackground = serializer.Bool(flipBackground); 
            if (vita >= 0x4f) mpSeparateScreen = serializer.Bool(mpSeparateScreen);

        }

        if (version >= 0x3dd)
            fixInvalidInOutMoverContacts = serializer.Bool(fixInvalidInOutMoverContacts);

        if (version >= 0x3f1)
            continueMusic = serializer.Bool(continueMusic);

        if (subVersion >= 0x2f)
            broadcastMicroChipEntries = serializer.Array(broadcastMicroChipEntries);

        if (subVersion >= 0x5d)
            manualJumpDown = serializer.I32(manualJumpDown);

        if (subVersion >= 0x98 && subVersion < 0xe5)
            serializer.Thingarray(null);
        if (subVersion >= 0xc3 && subVersion < 0xe5)
            serializer.Thingarray(null);
        if (subVersion >= 0x98 && subVersion < 0xe5)
            serializer.Thingarray(null);

        if (subVersion >= 0xe5)
            deferredDestroys = serializer.Array(deferredDestroys, true);

        if (subVersion >= 0xcf) {
            globalDofFront = serializer.F32(globalDofFront);
            globalDofBack = serializer.F32(globalDofBack);
            globalDofSackTrack = serializer.F32(globalDofSackTrack);
        }

        if (subVersion >= 0xda)
            enableSackpocket = serializer.Bool(enableSackpocket);

        if (subVersion >= 0x170)
            showQuestLog = serializer.Bool(showQuestLog);

        if (subVersion >= 0x11d)
            scoreboardUnlockLevelSlot = serializer.Struct(scoreboardUnlockLevelSlot);

        if (subVersion >= 0x154)
            progressBoardLevelLinkStartPoint = serializer.Wstr(progressBoardLevelLinkStartPoint);

        if (subVersion >= 0x15e)
            isLBP3World = serializer.Bool(isLBP3World);
    }

    public int GetAllocatedSize() { return 0; }
    
}