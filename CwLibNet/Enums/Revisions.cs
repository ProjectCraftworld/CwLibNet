namespace CwLibNet.Enums
{
    public enum Revisions
    {
        // Base revisions

        Dependencies = 0x109, // Added self describing dependency table to
        // resources
        // 0x129, added lockStates to RLocalProfile
        // 0x12a
        // 0x12b
        // 0x12c
        // 0x12d
        // 0x12e
        // 0x12f
        // 0x130
        // 0x131
        // 0x132
        // 0x133
        SlotGroups = 0x134, // Added primaryLinkGroup to Slot
        // 0x134
        // 0x135, removed unknown int from Poppet
        // 0x136
        // 0x137
        LightingRimColor2 = 0x138, // Added rimColor2 to PLevelSettings
        // 0x139
        // 0x13a
        SlotAuthorName = 0x13b, // Added authorName to Slot
        CleanupCleanupEverybodyEverywhere = 0x13c, // removed PPaint,
        // PParticleClump, PParticleEmitter, PKeyframedPosition, PCameraZone, PCamera, and
        // PTriggerEffector, fucked around and removed half of RMaterial
        RefStripChildren = 0x13d, // Added stripChildren to PRef
        EmitterEmitCount = 0x13f, // Add currentEmitted to PEmitter
        SwitchStickerPlan = 0x140, // Add stickerPlan to PSwitch
        // 0x141
        // 0x142
        // 0x143
        EmitterFlip = 0x144, // added emitFlip to PEmitter
        ScriptFishType = 0x145, // added fishType to FieldLayoutDetails in
        // PScript
        // 0x146
        BodyFrozenMode = 0x147, // replaced frozen boolean with frozen mode
        // in PBody
        RemovePrimitiveGmat = 0x149, // removed alternate gfx material from
        // Primitive
        WorldIsPaused = 0x14a, // Added isPaused to PWorld
        // 0x14b removes some array from PWorld
        // 0x14c
        // 0x14d
        DecalU32Color = 0x14e, // added color to decal
        DetailsSoundGuid = 0x14f, // highlightSound uses GUID instead of
        // string in InventoryItemDetails
        ProfilePlayerId = 0x150, // added playerId to RLocalProfile
        // 0x151
        WorldColorCorrection = 0x152, // Added lighting and color
        // correction factor to PWorld
        LightingPresets = 0x153, // Added presets to LevelSettings, also
        // added unknown value to PLevelSettings that was removed in 0x15a (weight?)
        // 0x154 added some struct to PLevelSettings, has vignetting and matrix stuff?
        // 0x155 removed unknown field from PPos
        // 0x156 removed struct added in 0x154
        DetailsColor = 0x157, // color added to InventoryItemDetails
        DecalMetadata = 0x158, // added type, metadataIdx, numMetadata to
        // decal
        UnifedTranslationTag = 0x159, // use one translation tag instead of
        // separate one for name/desc
        // 0x152, removed unknown field added in 0x15a
        GfxmaterialSoundEnum = 0x15b, // Added soundEnum to RGfxMaterial
        ShapeOldMaterial = 0x15c, // added oldMMaterial to PShape
        EyetoyData = 0x15d, // added eyetoydata
        // 0x15e
        MetadataReferencable = 0x15f, // added referenceable to PMetadata
        EgglinkDetails = 0x160, // Reference CInventoryItemDetails in
        // CEggLink for item details
        // 0x161
        DetailsEyetoyData = 0x162, // eyetoyData added to
        // InventoryItemDetails
        // 0x163
        // 0x164
        AudioworldIsLocal = 0x165, // Added isLocal to PAudioWorld
        // 0x166
        MaterialExplosions = 0x167, // Added explosion and breakable fields
        // to RMaterial
        MaterialAngularExplosions = 0x168, // Added explosionMaxAngVel to
        // RMaterial
        // 0x169
        JointSoundEnum = 0x16a, // added jointSoundEnum to PJoint
        // 0x16b
        // 0x16c
        LocalMoonSlots = 0x16e, // added oldMyMoonSlots to RLocalProfile
        UnlocksRewards = 0x16f, // Added unlocks and rewards to PWorld, and
        // 3 unknown ints to PWorld
        PlayerMetrics = 0x170, // added playerMetrics to RLocalProfile
        // 0x171
        // 0x172 Added unknown boolean to PRef, removed in 0x180
        // 0x173
        DetailsSingleTranslationTag = 0x174, // replace nameTranslationTag
        // /descTranslationTag with translationTag
        // 0x175
        ProfileEulaAgreed = 0x176, // added eulaAgreed to RLocalProfile
        DetailsLevelUnlock = 0x177, // add levelUnlockSlotID to
        // InventoryItemDetails
        // 0x178
        MeshTextureAlternatives = 0x179, // textureAlternatives added to
        // CPrimitive
        // 0x17a, added unknown boolean to PScript, removed in 0x1a1
        // 0x17b
        // 0x17c
        // 0x17d
        ProfileLbp1VideosPlayed = 0x17e, // added lbp1VideosPlayed to
        // RLocalProfile
        // 0x17f
        // 0x180, added unknown boolean and NetworkPlayerID to PRef, removed in 0x19e, removes field
        // added in 0x172
        // 0x181
        DetailsPhotoData = 0x182, // photoData, copyright, and creator
        // added to InventoryItemDetails
        SlotTranslationTag = 0x183, // Added translationTag to slot
        MetricsMultiplayerGames = 0x184, // added multiplayerGamesCount to
        // PlayerMetrics
        // 0x185
        // 0x186
        // 0x187
        // 0x188, removed unknown field from Slot
        CompressedResources = 0x189, // Added optional compressed field to
        // resources
        // 0x18a
        // 0x18b
        RemoveParticleEmitter2 = 0x18c, // Removed PParticleEmitter2
        JointSlideStiffness = 0x18d, // added slideDir to PJoint, replaced
        // stiff with a float
        // 0x18e added creator, copyright to PGroup, removed(?) things
        // 0x18f
        // 0x190
        // 0x191, switched guid_hash flags 0x1 = HASH, 0x2 = GUID
        // 0x192
        // 0x193
        YellowheadOnScreenCounter = 0x194, // Added onScreenCounter to
        // PYellowHead
        DetailsPrimaryIndex = 0x195, // Added primaryIndex to
        // InventoryItemDetails
        WorldFogFactor = 0x196, // Added fog/darkness settings to PWorld
        PlanDetails = 0x197, // Plan details added to RPlan, removed
        // CInventoryItemDetails from EggLink, reference RPlan instead of CGlobalThingPtr, toolType
        // added to InventoryItemDetails
        AudioworldVisibility = 0x198, // Add hideInPlayMode to PAudioWorld
        // 0x199 added some unknown value to member variable reflection in PScript, removed in 0x19d
        CostumeOriginalPlan = 0x19a, // added originalPlan to CostumePiece
        TriggerHysteresis = 0x19b, // add hysteresisMultplier and enabled to
        // PTrigger
        // 0x19c
        ScriptReflectDivergent = 0x19d, // added reflectDivergent to
        // PScript, removed field added in 0x199
        // 0x19e removed field added in 0x180, removed unknown thing list from PRef
        EnemySnap = 0x19f, // Add snapVertex to PEnemy
        // 0x1a0
        // 0x1a1 - added optional serialize indicator for instance layout in PScript, removed field
        // added in 0x17a
        ProfileAudioSwitches = 0x1a2, // added voipMutedPlayers,
        // voipChatPaused, and enableMusicInPlayMode to RLocalProfile
        // 0x1a3
        // 0x1a4 added CGameraCamera to PWorld, removed in 0x1d1, also removes a bunch of floats in
        // PWorld, (related to cgamecamera?)
        DynamicSwitches = 0x1a5, // added type to PSwitch, switches now
        // carry activation
        // 0x1a5, removed unknown thing reference from PSwitch, as well as unknown boolean and s32
        // 0x1a6, added joint array to Thing, removed in 0x1bc
        CheckpointSpawnsLeft = 0x1a7, // added spawnsLeft to PCheckpoint
        // 0x1a8
        EnemyAnimation = 0x1a9, // centerOffset, animThing, and animSpeed
        // added to PEnemy
        // 0x1aa
        DetailsUserDetails = 0x1ab, // userCreatedDetails added to
        // InventoryItemDetails
        // 0x1ac added storyLevelCompletionCount and levelCompletionCount to PlayerMetrics
        // 0x1ad added triggerByDestroy to PAudioWorld
        // 0x1ae added unknown boolean to Slot, removed in 0x1b9, (old game progression state?)
        // 0x1af
        // 0x1b0, something with things in PGroup
        DetailsCreationHistory = 0x1b1, // added creationHistory to
        // InventoryItemDetails
        // 0x1b2
        // 0x1b3
        // 0x1b4
        // 0x1b5 added enableMusicInPod to RLocalProfile
        // 0x1b6 added inventory collections/views to RLocalProfile
        // 0x1b7 unknown floats added to PCameraTweak, removed in 0x1d2
        // 0x1b8
        SlotProgression = 0x1b9, // added gameProgressionState to slot,
        // gameProgressionStatus to RLocalProfile, removed field added in 0x1ae
        // 0x1ba
        // 0x1bb
        // 0x1bc, removed joints array in Thing
        SwitchkeyVisibility = 0x1bd, // added hideInPlayMode to PSwitchKey
        // 0x1be added unknown int to PWorld, removed in 0x213
        // 0x1bf, removed 3 unknown ints added to PWorld in 0x16f
        // 0x1c0
        DetailsUses = 0x1c1, // added numUses and lastUsed to
        // InventoryItemDetails
        WorldDeathCount = 0x1c2, // Added deathCount to PWorld
        // 0x1c3
        WorldMaxPlayers = 0x1c4, // Added maxNumPlayers to PWorld
        // 0x1c5
        // 0x1c6, added maxSpawnsLeft to PCheckpoint
        ThingEmitter = 0x1c7, // added emitter to thing
        EmitterMaxEmitted = 0x1c8, // Added maxEmittedAtOnce to PEmitter
        RefAliveFrames = 0x1c9, // Added oldAliveFrames to PRef
        // 0x1ca playLadderPoints and gamesWithRandomPlayersCount added to PlayerMetrics
        // 0x1cb
        // 0x1cc
        // 0x1cd
        // 0x1ce, added emit offset related fields to PEmitter
        // 0x1cf
        // 0x1d0, added game progression explanation fields to RLocalProfile
        // 0x1d1, removed CGameCamera from PWorld
        // 0x1d2, removed 2 unknown floats from PCameraTweak added in 0x1b7
        // 0x1d3, remove unknown boolean from RMaterial
        // 0x1d4
        TriggerZlayers = 0x1d5, // added allZLayers to PTrigger, removed
        // unknown int from PTrigger
        // 0x1d6
        // 0x1d7, switch cameraType data type from u8 to u32
        // 0x1d8
        // 0x1d9
        // 0x1da
        WorldDissolvingThings = 0x1db, // Added dissolving things to PWorld
        // 0x1dc
        // 0x1dd
        WorldTutorialLevel = 0x1de, // added isTutorialLevel to PWorld
        SlotLevelType = 0x1df, // Added developerLevelType to slot
        // 0x1e0
        // 0x1e1, added avatarIcon to RLocalProfile
        // 0x1e2
        // 0x1e3
        // 0x1e4, added tutorialLevelStates to RLocalProfile, videos to PlayedLevelData
        ScriptModifiers = 0x1e5, // Added modifiers to RScript,
        // lbp1MainMenuButtonUnlocks added? something might be wrong there
        // 0x1e6, added lbp1CreateModeVOProgress to RLocalProfile
        // 0x1e7
        // 0x1e8
        // 0x1e9
        // 0x1ea, added stats to PlayerMetrics
        // 0x1eb, added instanceInfiniteLives to PCheckpoint
        SharedScriptData = 0x1ec, // replace all inline script fields with
        // shared pools, and dependingGUIDs in RScript, removed fieldNameIdx from InstanceLayout
        // 0x1ed
        // 0x1ee
        // 0x1ef
        // 0x1f0
        // 0x1f1
        // 0x1f2
        // 0x1f3, removed unknown thing and int in PCheckpoint, added spawningList and spawnDelay
        // 0x1f4
        // 0x1f5, added photoBoothTimerLength to PCameraTweak
        RendermeshParentDistance = 0x1f6, // added parentDistanceFront/Side
        // to PRenderMesh
        // 0x1f7
        // 0x1f8, added levelsTaggedCount to PlayerMetrics
        // 0x1f9
        // 0x1fa, added lifeMultiplier to PCheckpoint
        // 0x1fb, added connectorPos/Offset fields to PSwitch, removed in 0x327
        // 0x1fc
        // 0x1fd, removed world from Thing
        // 0x1fe, added lastPlayedTimestamp to PlayedLevelData
        // 0x1ff
        // 0x200, added gamma and screenSize to RLocalProfile
        // 0x201, added localHighScore to PlayedLevelData
        // 0x202
        // 0x203
        // 0x204
        DetailsAllowEmit = 0x205, // add allowEmit flag to
        // InventoryItemDetails
        // 0x206, added lbp1VOPlayed to RLocalProfile
        // 0x207
        WorldShareableRewards = 0x208, // added areRewardsShareable to
        // PWorld, (added unknown boolean to EggLink, removed in 0x22a)
        // 0x209
        // 0x20a
        // 0x20b
        // 0x20c
        // 0x20d
        // 0x20e added copiedFromAnotherUser to RLocalProfile
        // 0x20f
        // 0x210
        // 0x211
        // 0x212
        // 0x213, removed unknown value added to PWorld in 0x1be
        ThingCreatedPlayer = 0x214, // added createdBy and changedBy to
        // thing, placedBy to decal
        DecalPlaymodeFrame = 0x215, // added playmodeFrame to decal
        // 0x216
        // 0x217
        // 0x218
        DecalScorchMark = 0x219, // add scrorchMark to decal
        // 0x21a
        ThingStamping = 0x21b, // isStamping added to thing
        // 0x21c
        // 0x21d, tweakTargetMin/MaxLength to PJoint
        // 0x21e
        // 0x21f, added currentlyEditing to PJoint
        // 0x220
        // 0x221
        DetailsDateAdded = 0x222, // add dateAdded to InventoryItemDetails
        DetailsShareable = 0x223, // add shareable flag to
        // InventoryItemDetails
        // 0x224, collidablePoppet added to PShape
        // 0x225
        // 0x226, added hideInPlayMode to PEmitter
        ShapeMassDepth = 0x227, // added massDepth to PShape
        // 0x228
        // 0x229
        // 0x22a, removed unknown value in EggLink that was added in 0x208
        // 0x22b
        // 0x22c, added editingPlayer to PBody
        // 0x22d
        WorldSpawnFailure = 0x22e, // Added everSpawned and
        // spawnFailureCount to PWorld
        // 0x22f, added flags to resource descriptor
        // 0x230, added modScaleActive to PJoint and PEmitter
        // 0x231,
        // 0x232, removed unknown v2 from Poppet
        // 0x233, details relayout lbp1? might actually be 0x234, need to double check
        // 0x234, re-do network online id struct
        ItemFlags = 0x235, // Added UID, flags to InventoryItem, add back
        // pad to InventoryItemDetails
        // 0x236
        // 0x237
        SlotDescriptor = 0x238, // Added shareable and backgroundGUID to Slot
        MeshMinmaxUv = 0x239, // Added min/maxUV and areaScaleFactor to
        // RMesh
        // 0x23a
        // 0x23b, added subtitleMode to RLocalProfile
        EgglinkShareable = 0x23c, // Adds shareable field to CEggLink
        // 0x23d, added saveIcon to RLocalProfile
        // 0x23e, added angleRange and includeTouching to PSwitch
        // 0x23f
        // 0x240
        // 0x241
        // 0x242
        // 0x243
        MaterialBullet = 0x244, // Adds bullet to RMaterial, bulletsRequired
        // to PSwitch, unknown field to PSwitch, removed in 0x245 (unsigned bullets detected?)
        // 0x245, adds bulletsDetected to PSwitch, removes unknown field added in 0x244
        // 0x246, added bulletPlayerNumber to PSwitch, removed in 0x398
        // 0x247
        // 0x248
        // 0x249, added bulletRefreshTime to PSwitch
        // 0x24a
        // 0x24b, added hideConnectors to PSwitch, removed in 0x327
        // 0x24c
        // 0x24d
        // 0x24e
        // 0x24f
        // 0x250
        // 0x251
        // 0x252, added plan descriptor, removed the revision after, then added again in 0x25e, or is
        // this something else?
        // 0x253
        ThingPlanGuid = 0x254, // Adds planGUID to thing
        // 0x255,
        // 0x256, added unknown value to RLocalProfile, removed in 0x351
        // 0x257
        GeneratedmeshPlanGuid = 0x258, // Add planGUID to PGeneratedMesh
        // 0x259
        WorldGlobalSettings = 0x25a, // Added global settings fields to
        // PWorld
        PartPlanGuids = 0x25b, // Plan GUIDs added to various parts
        // 0x25c, removed u32 color from Decal
        // 0x25d, added hideInPlayMode to PJoint
        // 0x25e, added editable and planDescriptor to PGroup
        // 0x25f, added unknown boolean to ObjectState, removed in 0x2bd
        DecalU16Color = 0x260, // replace RGB32 color with RGB565 color in
        // decal
        // 0x261, added user settings to RLocalProfile
        // 0x262, removed unknown setting from RLocalProfile
        // 0x263, added tutorialPrizePlanList to RGame
        // 0x264, added categoryID to DLCFile
        EnemyWalkConstraint = 0x265, // newWalkConstraintMass added to
        // PEnemy
        // 0x266, dlcPackViewed/Shown added to RLocalProfile, levelHash and levelHasDLC added to
        // PlayedLevelData, (levelHasDLC removed in 0x399)
        GroupEmitter = 0x267, // emitter, lifetime, aliveFrames added to
        // PGroup
        // 0x268, fixedUpByDeveloper added to RLocalProfile
        // 0x269, added discovered and unlocked to PlayedLevelData (both removed in 0x399), removed
        // lockStates from RLocalProfile, changed play/completion/aced counts to u16 instead of u32
        // 0x26a, added requireAll to PCameraTweak
        // 0x26b, added levelStateAfterLastFinishedEditing to RGame
        // 0x26c, added lastGridMoveFrame, lastGridRotateFrame, and lastGridScaleFrame to
        // PoppetTweakObjectPlacement
        // 0x26d, added demoProgressionStatus to RLocalProfile
        GroupPickupAllMembers = 0x26e, // added pickupAllMembers to PGroup
        WaterLevel = 0x26f, // Addede waterLevel and targetWaterLevel to
        // PWorld
        WaterMagnitude = 0x270, // Added water wave magnitude fields to
        // PWorld
        Branches = 0x271, // Added branch descriptions to resources
        Airtime = 0x272, // airTimeLeft added to PCreature

        Lbp1Max = 0x272, // last revision for lbp1

        // Deploy/LBP2 revisions

        Arcade = 0x273, // Robo boots (wall jump), custom guns, recordings,
        // logic
        InputUpdateType = 0x274, // added updateType to PSwitchInput
        InputLethalType = 0x275, // added lethalType to PSwitchInput
        // 0x276, ???
        // 0x277, add port thing to PSwitch, removed in 0x327
        // 0x278, add fromWaterLevel to PWorld, portThing to PSwitchInput, removed in 0x327
        // 0x279, add configuration to PPhysicsTweak
        // 0x27a, add unknown thing field to PPhysicsTweak, removed in 0x327
        MaterialCircuitboard = 0x27b, // Add circuitboard and disableCSG to
        // RMaterial
        GeneratedmeshVisibility = 0x27c, // add visible to PGeneratedMesh
        SwitchkeyType = 0x27d, // added type to PSwitchKey
        // 0x27e, add switchConnectorUID to PoppetEditState
        ThingSwapUidParent = 0x27f, // swap thing UID and thing parent
        // fields in serialization
        // 0x280, remove unknown field in NpcMoveCmd
        // 0x281, replace PPhysicsTweak inputs with vector types, switch tweak target length to
        // floats in PJoint
        // 0x282, add unknown structure to PWorld, removed in 0x287
        MicrochipVisibility = 0x283, // Added hideInPlayMode to PMicrochip
        // and PPhysicsTweak
        // 0x284, add includeRigidConnectors to PSwitch
        // 0x285, customPortOffset to PSwitch
        // 0x286, add decelStrength to PPhysicsTweak
        // 0x287, add BehaviorFollow/Act to PNpc, removed in 0x293, also removes some struct in PWorld
        InputRigidConnectors = 0x288, // added includeRigidConnectors to
        // PSwitchInput
        // 0x289, added scuba gear / headpiece to PCreature
        // 0x28a,
        // 0x28b, added ActivationFrame/Inverted to PSwitchInput
        // 0x28c, added hidden categories in RLocalProfile
        SwitchinputVisibility = 0x297, // Added hideInPlayMode to
        // PSwitchInput
        ThingTestMarker = 0x2a1, // Adds test serialization marker to CThing
        ParameterAnimations = 0x2a2, // Added parameter animations to
        // RGfxMaterial
        PlayerSignal = 0x2a3, // Added gameMode to PWorld, playerNumberColor
        // to PShape, player to CSwitchSignal
        ResponsiblePlayer = 0x2a5, // responsiblePlayer/frames added to
        // PCreature
        WorldSackbotNumber = 0x2b0, // added nextSackbotPlayerNumber to
        // PWorld
        WorldWaterBits = 0x2b4, // Add water bits to PWorld
        ShapeFlags = 0x2b5, // Add flags to PShape
        MaterialProperties = 0x2b7, // Restitution, single grab, and sticky
        // added to PMaterialTweak
        MicrochipWireVisibility = 0x2b8, // Adds wiresVisible to PMicrochip
        MaterialNoAutoDestruct = 0x2b9, // Added noAutoDestruct to
        // PMaterialTweak
        MotionControllerZone = 0x2ba, // Add motionControllerZone to
        // PCameraTweak
        LamsKeys = 0x2bb, // Use LAMS keys instead of translation tags
        SwitchBehavior = 0x2c4, // Randomiser added, SwitchInput no longer
        // carries signals, behavior added to various structures
        TempCostume = 0x2c5, // Adds costumePieceVec[COSTUME_TEMP_START] to
        // PCostume
        RemoveGameUpdateFrame = 0x2c6, // Remove Game.LastUpdateFrame from
        // PEmitter
        ShowThermometer = 0x2d3, // showThermometer added to RLocalProfile
        GameplayDataType = 0x2da, // added gameplayType to PGameplayData
        SwitchKeyname = 0x2dc, // name added to switchKey and switch
        WorldSimFrame = 0x2e2, // added simFrame to world
        RendermeshVisibilityFlags = 0x2e3, // switch visible to
        // visibilityFlags in PRenderMesh
        MicrochipLastTouched = 0x2e4, // Last touched added to PMicroChip
        DirectControl = 0x2e5, // directControlPrompt added to PCreature
        MicrochipOffset = 0x2e9, // Circuitboard board offset added to
        // PMicroChip
        SlotCollectabubblesRequired = 0x2ea, // added
        // collectabubblesRequired to slot
        WorldCutsceneManager = 0x2ee, // added cutSceneCameraManager to
        // PWorld
        ThingHidden = 0x2f2, // added hidden to thing
        SlotCollectabubblesContained = 0x2f4, // added
        // collectabubblesContained to slot
        GfxmaterialAlphaMode = 0x2fa, // Added alpha mode to RGfxMaterial
        ScriptS64Table = 0x30b, // Added S64 constants table to RScript
        WorldAudioSettings = 0x30c, // added globalAudioSettings to PWorld
        SignalTernary = 0x310, // Added ternary to switch signal
        EnemySmokeColor = 0x31e, // smokeColor added to PEnemy
        GunModifiers = 0x320, // added fireRate, gunAccuracy,
        // bulletEmitOffset/Rotation, gunThing, gunTrigger, and lastGunTriggerUID
        PaletteConvertedPlans = 0x323, // added convertedPlans to RPalette
        LevelSettingsAo = 0x325, // added shadow settings and dofNear to
        // PLevelSettings
        LevelSettingsDofFar = 0x326, // added DofFar to PLevelSettings
        DropPowerups = 0x32c, // canDropPowerup added to PCreature, randy
        // seed also removed from pworld?
        LevelSettingsZeffect = 0x331, // zEffect fields added to
        // PLevelSettings
        TriggerZoffset = 0x332, // add zOffset to PTrigger
        PlanetDecorations = 0x333, // Add planetDecorations to slot
        DetailsFlags = 0x335, // flags added to InventoryItemDetails
        RemoveUpdatedScript = 0x33a, // removed upToDateScript from RScript
        PinFlags = 0x33b, // pinFlags added to PinsAwarded profile data
        SlotLabels = 0x33c, // Added labels to slot
        ThingFlags = 0x341, // added flags to thing, remove localPosition
        // from PPos
        RemoveWorldTutorialLevel = 0x344, // isTutoriallevel removed from
        // PWorld
        UserCategories = 0x34a, // userCategoryIndex added to InventoryItem
        WeekdaysPlayedPin = 0x34b, // added weekdaysPlayedBits to
        // PinsAwarded
        DecalPlanDescriptor = 0x34c, // replace decal planGUID with
        // descriptor
        CompactMicrochip = 0x34d, // name, compactComponents, and
        // circuitBoardSizeX/Y added to PMicrochip
        WorldWaterDrain = 0x34e, // Added water drain sounds enabled fields
        // to PWorld
        SlotSublevel = 0x352, // isSubLevel added to slot
        SlotScoreboardLink = 0x35e, // added scoreboardLevelLinkSlot to
        // PWorld
        RemoveLbp1Tutorials = 0x36c, // Removed tutorialLevel and
        // tutorialVideo from InventoryItem, removed gameProgressionState from slot
        WorldCameraSettings = 0x370, // Added camera settings to PWorld
        WorldFrameStart = 0x377, // add frameLevelStarted to PWorld
        DetailsRelayoutLbp2 = 0x37d, // NetworkOnlineID and NetworkPlayerID
        // are written as fixed length buffers, InventoryItemDetails has been remade again
        ProfilePins = 0x385, // Add profile pin data
        EnemySmokeBrightness = 0x39a, // smokeBrightness added to PEnemy
        EyetoyOutline = 0x3a0, // outline added to EyetoyDATA
        WorldNightdaySwap = 0x3a3, // added lbp2NightDaySwapped to PWorld
        ProductionBuild = 0x3b6, // fromProductionBuild added to resources
        // containing slot lists.
        PaintingPhotoData = 0x3c8, // painting added to
        // InventoryItemPhotoData
        SlotExtraMetadata = 0x3d0, // min/maxPlayers, moveRecommended,
        // showOnPlanet, and livesOverride added to slot
        PtgUseDefaultBackground = 0x3e0, // add useDefaultBackground to
        // PaintProperties
        LevelCrossDependencies = 0x3e6, // Added Vita cross dependency
        // hashes to RLevel
        SlotCrossCompatible = 0x3e9, // crossCompatible added to slot
        ProfileCrossDependencies = 0x3eb, // Added Vita cross dependency
        // hashes to RBigProfile
        Datalabels = 0x3ef,  // Creator data labels added to RBigProfile
        PinLbp1TrophyAwarded = 0x3f7, // Added a trophy for LBP1 enum to
        // RPin files
        ProfileOwnedLbp = 0x3f8, // OwnedLBP field added to RLocalProfile

        Lbp2Max = 0x3f8, // last revision for lbp2, technically 0x3f9, but
        // no resource exists with this until lbp3

        // Leerdammmer revisions

        LdHead = 0x272, // Head revision for Leerdammer branch

        LdWaterSettings = 0x0, // Adds scuba gear and water related tweak
        // settings
        LdWaterTint = 0x1, // Adds water tint and murkiness to PWorld
        LdResources = 0x2, // Remove's depreciated CValue from PMetaData,
        // adds compressionFlags to Resource's, adds compressed parts to CThing
        LdSavegameInfo = 0x3, // Add showSaveGameInfo and totalLevelCount
        // to RLocalProfile
        LdWaterWave = 0x4, // Adds Game.CurrWavePos and fromWaterLevel to
        // PWorld
        LdTestMarker = 0x5, // Adds test serialization marker to CThing
        // 0x4c44:0x0006
        LdWaterBits = 0x7, // Add water bits to PWorld
        LdLamsKeys = 0x8, // Use LAMS keys instead of translation tags
        // 0x4c44::0x0009
        // 0x4c44:0x000a
        LdTempCostume = 0xb, // Adds costumePieceVec[COSTUME_TEMP_START] to
        // PCostume
        LdRemovedHeadPiece = 0xc, // Removes hasBreathedAir from
        // PCreature, or is that headPiece
        LdNewsTimestamp = 0xd, // Adds lastNewsItemTimestamp to
        // RLocalProfile
        LdFakeTimestamp = 0xe, // I think this is what this is, probably
        LdSubmerged = 0xf, // Adds Game.AmountBody/HeadSubmerged to PWorld
        // 0x4c44:0x0010
        LdRemovedEnemyStat = 0x11, // Removes enemiesKilled from
        // PlayerMetrics
        LdQueue = 0x12, // Added lolcatFtwTimestamp to RLocalProfile
        LdShader = 0x13, // Added another shader to RGfxMaterial
        LdThermometer = 0x14, // Added showThermometer to RLocalProfile
        LdUsedScuba = 0x15, // Added hasUsedScuba to RGame
        LdOwnerId = 0x16, // Added ownerUserId to RLocalProfile
        LdWaterLevels = 0x17, // Added numWaterLevelsPlayed to RLocalProfile

        // Custom leerdammer revisions for cambridge project

        LdCambridgeAddStatic = 0x18, // Added isStatic to PShape

        LdMax = 0x17, // last revision for leerdammer revisions

        // Vita revisions

        D1Head = 0x3e2, // Head revision for Double11/Vita branch

        D1Perfdata = 0x4, // Added perf data to RGfxMaterial
        D1Shader = 0x10, // Added additional shaders to RGfxMaterial
        D1UvOffscale = 0x19, // Added UV offsets/scales to RGfxMaterial
        D1VertexColors = 0x28, // Added vertexColors to RMesh
        D1WaterBrightness = 0x2d, // Adds waterBrightness to PWorld
        D1Datalabels = 0x2e, // Creator data labels added to RBigProfile
        D1ProtectedLabels = 0x31, // added protectedIds to PlayerDataLabels
        D1LabelAnalogueArray = 0x33, // Make analogue value from
        // DataLabelValue an array, rather than a singular type
        D1LabelTernary = 0x3c, // Ternary value added to DataLabelValue
        D1SlotRewards = 0x3d, // Added rewards and acingEnabled fields to
        // slot
        D1SlotEnforceMinmax = 0x4c, // added enforceMinMaxPlayers to slot
        D1SlotSameScreen = 0x4d, // added sameScreenGame to slot
        D1CheckpointPlayAudio = 0x50, // added playAudio to PCheckpoint
        D1NearChallenges = 0x57, // Near challenges added to RBigProfile
        D13GConnectivity = 0x58, // added total3gUp/DownStream to
        // RLocalProfile
        D1NearTreasures = 0x59, // Near treasure log added to RBigProfile
        D1DownloadedSlots = 0x5a, // Downloaded slots added to RBigProfile
        D1ChallengeLevelType = 0x5b, // levelType added to Challenge
        D1DeferredPlays = 0x5c, // Added deferredPlayCount(Uploaded) to
        // PlayedLevelData
        D1SlotDownloadData = 0x5d, // sizeOfResources, sizeOfSubLevels,
        // subLevels, and slotList added to slot for download metadata
        D1Collectabubbles = 0x5e, // containsCollectabubbles added to slot
        D1LevelTimesMap = 0x60, // added levelTimesMap to PlayerMetrics
        D1UploadedHighScore = 0x61, // added uploadedLocalHighScore to
        // PlayedLevelData
        D1MoePinProgress = 0x64, // added moreOfEverythingPinProgress to
        // PinsAwarded
        D1ChallengeScore = 0x68, // myScore and networkOnlineID added to
        // Challenge
        D1TouchCreate = 0x69, // createRearTouchPan,
        // createRearTouchPinchZoom, createRearTapZoom aded to RLocalProfile
        D1Trophies = 0x72, // added gold/silver/bronze trophy counts to
        // PlayedLevelDATA
        D1CreateWarning = 0x74, // Added createFrameRateWarningMessages to
        // RLocalProfile
        D1CreateBeta = 0x75, // Added online beta timestamps to
        // RLocalProfile
        D1PlanetDecorations = 0x7b, // Planet decorations added to
        // RBigProfile
        D1DetailsProportional = 0x7d, // add makeSizeProportional to
        // InventoryItemDetails
        D1SlotRevision = 0x80, // download revision added to slot

        D1Max = 0x87, // last revision for vita

        // LBP3 revisions

        CollectionPoppetPowerup = 0x9, // added poppetPowerUpSelection to
        // InventoryCollection
        SlotGameMode = 0x12, // gameMode added to slot
        TriggerZrange = 0x2a, // zRangeHundreds added to PTrigger
        WorldBackdropOffset = 0x6d, // add backdrop offset adjustment
        // fields to PWorld
        WorldBackdropAutoz = 0x70, // added backdropOffsetZAuto to PWorld
        TriggerScoreValue = 0x90, // scoreValue added to PTrigger
        DecorationShadow = 0xc4, // Added hasShadow to decorations
        StreamingPlan = 0xcc, // isUsedForStreaming added to RPlan
        SlotGameKit = 0xd2, // isGameKit added to slot
        MeshSkeletonType = 0xd6, // added skeletonType to RMesh
        WorldAmbienceOverride = 0xe2, // added overrideBackdropAmbience to
        // world
        MoveGameplayDataType = 0xef, // move gameplayType to top of struct
        // in PGameplayData
        GameplayDataTreasure = 0xf3, // treasureType and treasureCount
        // added to PGameplayData
        WorldWaterColors = 0xf8, // waterHint colors added to PWorld
        ItemGuid = 0x106, // Added tempGUID to InventoryItem
        SlotEntranceData = 0x11b, // entranceName and originalSlotID added
        // to slot
        CreatureSubstate = 0x132, // subState added to PCreature
        Fresnel = 0x13a, // Added refractive fresnel fields to RGfxMaterial
        Adventure = 0x145, // Added adventure resource to slot
        SlotBadgeSize = 0x153, // customBadgeSize added to slot
        Fuzz = 0x16b, // Added fuzzLengthAndRefractiveFlag to RGfxMaterial
        // and triangleAdjacencyInfo to RMesh
        DecorationQuest = 0x16c, // Added isQuest to decorations
        SlappedAsPin = 0x177, // added slappedAsBits to PinsAwarded
        FuzzLighting = 0x17c, // Added fuzz lighting/swirl fields to
        // RGfxMaterial
        WorldBackdropToggle = 0x182, // added backdropEnabled and
        // currBackdropEnabled to PWorld
        SlotTrailerPath = 0x192, // added localPath for trailer to slot
        SlotTrailerThumbnail = 0x206, // added thumbPath to slot
        SimonSaysPin = 0x209, // added simonSaysProgress to PinsAwarded
        Springinator = 0x20c, // Added springinator fields
        SlotEnforceMinmax = 0x215, // add enforceMinMaxPlayers to slot

        Lbp3Max = 0x218,

        // Toolkit revisions

        MzHead = 0x021803f9,

        MzBase = 0x0, // Base revision for Toolkit assets
        MzBstRemoveSk = 0x1, // Remove skeleton type from RBoneSet
        MzCgcOrbis = 0x2, // Keep track if a shader cache is for PS4 or not.
        MzCgcPath = 0x3, // Keep original path of RGfxMaterial in
        // RShaderCache
        MzCgcSwizzle = 0x4, // Swizzled color vector in RShaderCache
        MzCgcShortFlags = 0x5, // Use short flags in RShaderCache
        MzSceneGraph = 0x6, // RSceneGraph added, fields removed in PWorld
        // that are now stored in scene graph, removed thing createdBy indices, add name field to
        // Things
        MzRemoveAspect = 0x7, // Remove aspectRatio from Camera
        // serialization
        MzRemoveGfxCode = 0x8, // Removes code from RGfxMaterial for
        // generation on the fly
        MzSceDefaultLighting = 0x9, // Serializes lighting in RSceneGraph
        // to use as default.
        MzScePackedData = 0xa, // Allows storing data in scene graph
        // caches.
        MzCgcOffsets = 0xb, // Adds offsets for properties in cached shaders
        MzGlslShaders = 0xc, // Store GLSL shaders in RGfxMaterial

        MzMax = 0xc, // last revision for toolkit revisions

        // Legacy mod revisions
        // Somewhat weird, but apparently Toolkit started with mod
        // revisions of 0x3, all older ones were only present in early versions
        // of Workbench

        LmOldHead = 0xffff, // Head revision for mods prior to LM_TYPES
        LmHead = 0x01ae03fa, // Head revision for mods >= LM_TYPES

        LmBase = 0x0, // Base revisions for old mods
        LmItems = 0x1, // Added cached item metadata
        LmTranslations = 0x2, // Store translated locations/categories in mod
        LmToolkit = 0x3, // When mods were added to Toolkit, move icon to
        // bottom of mod, title/desription use wstr instead of int16 size + char, added minor
        // version, entries now contain data size rather, file data is now one big buffer rather than
        // an array of bytearrays
        LmSlotsTimestamps = 0x4, // Timestamps added to each entry, slot
        // array added
        LmMinmax = 0x5, // Added min/max revision to entries, meant for mods
        // to contain multiple versions of gmats/plans, never used.
        LmTypes = 0x6, // author is now str16, itemCount switched from u16
        // to u32, base revision bumped to support LBP3 slot metadata

        LmMax = 0x6, // last revision fo legacy mods
    }
}