using static net.torutheredfox.craftworld.serialization.Serializer;
﻿namespace CwLibNet.Enums;

public enum Revisions
{
    // Base revisions

    DEPENDENCIES = 0x109, // Added self describing dependency table to
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
    SLOT_GROUPS = 0x134, // Added primaryLinkGroup to Slot
    // 0x134
    // 0x135, removed unknown int from Poppet
    // 0x136
    // 0x137
    LIGHTING_RIM_COLOR2 = 0x138, // Added rimColor2 to PLevelSettings
    // 0x139
    // 0x13a
    SLOT_AUTHOR_NAME = 0x13b, // Added authorName to Slot
    CLEANUP_CLEANUP_EVERYBODY_EVERYWHERE = 0x13c, // removed PPaint,
    // PParticleClump, PParticleEmitter, PKeyframedPosition, PCameraZone, PCamera, and
    // PTriggerEffector, fucked around and removed half of RMaterial
    REF_STRIP_CHILDREN = 0x13d, // Added stripChildren to PRef
    EMITTER_EMIT_COUNT = 0x13f, // Add currentEmitted to PEmitter
    SWITCH_STICKER_PLAN = 0x140, // Add stickerPlan to PSwitch
    // 0x141
    // 0x142
    // 0x143
    EMITTER_FLIP = 0x144, // added emitFlip to PEmitter
    SCRIPT_FISH_TYPE = 0x145, // added fishType to FieldLayoutDetails in
    // PScript
    // 0x146
    BODY_FROZEN_MODE = 0x147, // replaced frozen boolean with frozen mode
    // in PBody
    REMOVE_PRIMITIVE_GMAT = 0x149, // removed alternate gfx material from
    // Primitive
    WORLD_IS_PAUSED = 0x14a, // Added isPaused to PWorld
    // 0x14b removes some array from PWorld
    // 0x14c
    // 0x14d
    DECAL_U32_COLOR = 0x14e, // added color to decal
    DETAILS_SOUND_GUID = 0x14f, // highlightSound uses GUID instead of
    // string in InventoryItemDetails
    PROFILE_PLAYER_ID = 0x150, // added playerId to RLocalProfile
    // 0x151
    WORLD_COLOR_CORRECTION = 0x152, // Added lighting and color
    // correction factor to PWorld
    LIGHTING_PRESETS = 0x153, // Added presets to LevelSettings, also
    // added unknown value to PLevelSettings that was removed in 0x15a (weight?)
    // 0x154 added some struct to PLevelSettings, has vignetting and matrix stuff?
    // 0x155 removed unknown field from PPos
    // 0x156 removed struct added in 0x154
    DETAILS_COLOR = 0x157, // color added to InventoryItemDetails
    DECAL_METADATA = 0x158, // added type, metadataIdx, numMetadata to
    // decal
    UNIFED_TRANSLATION_TAG = 0x159, // use one translation tag instead of
    // separate one for name/desc
    // 0x152, removed unknown field added in 0x15a
    GFXMATERIAL_SOUND_ENUM = 0x15b, // Added soundEnum to RGfxMaterial
    SHAPE_OLD_MATERIAL = 0x15c, // added oldMMaterial to PShape
    EYETOY_DATA = 0x15d, // added eyetoydata
    // 0x15e
    METADATA_REFERENCABLE = 0x15f, // added referenceable to PMetadata
    EGGLINK_DETAILS = 0x160, // Reference CInventoryItemDetails in
    // CEggLink for item details
    // 0x161
    DETAILS_EYETOY_DATA = 0x162, // eyetoyData added to
    // InventoryItemDetails
    // 0x163
    // 0x164
    AUDIOWORLD_IS_LOCAL = 0x165, // Added isLocal to PAudioWorld
    // 0x166
    MATERIAL_EXPLOSIONS = 0x167, // Added explosion and breakable fields
    // to RMaterial
    MATERIAL_ANGULAR_EXPLOSIONS = 0x168, // Added explosionMaxAngVel to
    // RMaterial
    // 0x169
    JOINT_SOUND_ENUM = 0x16a, // added jointSoundEnum to PJoint
    // 0x16b
    // 0x16c
    LOCAL_MOON_SLOTS = 0x16e, // added oldMyMoonSlots to RLocalProfile
    UNLOCKS_REWARDS = 0x16f, // Added unlocks and rewards to PWorld, and
    // 3 unknown ints to PWorld
    PLAYER_METRICS = 0x170, // added playerMetrics to RLocalProfile
    // 0x171
    // 0x172 Added unknown boolean to PRef, removed in 0x180
    // 0x173
    DETAILS_SINGLE_TRANSLATION_TAG = 0x174, // replace nameTranslationTag
    // /descTranslationTag with translationTag
    // 0x175
    PROFILE_EULA_AGREED = 0x176, // added eulaAgreed to RLocalProfile
    DETAILS_LEVEL_UNLOCK = 0x177, // add levelUnlockSlotID to
    // InventoryItemDetails
    // 0x178
    MESH_TEXTURE_ALTERNATIVES = 0x179, // textureAlternatives added to
    // CPrimitive
    // 0x17a, added unknown boolean to PScript, removed in 0x1a1
    // 0x17b
    // 0x17c
    // 0x17d
    PROFILE_LBP1_VIDEOS_PLAYED = 0x17e, // added lbp1VideosPlayed to
    // RLocalProfile
    // 0x17f
    // 0x180, added unknown boolean and NetworkPlayerID to PRef, removed in 0x19e, removes field
    // added in 0x172
    // 0x181
    DETAILS_PHOTO_DATA = 0x182, // photoData, copyright, and creator
    // added to InventoryItemDetails
    SLOT_TRANSLATION_TAG = 0x183, // Added translationTag to slot
    METRICS_MULTIPLAYER_GAMES = 0x184, // added multiplayerGamesCount to
    // PlayerMetrics
    // 0x185
    // 0x186
    // 0x187
    // 0x188, removed unknown field from Slot
    COMPRESSED_RESOURCES = 0x189, // Added optional compressed field to
    // resources
    // 0x18a
    // 0x18b
    REMOVE_PARTICLE_EMITTER2 = 0x18c, // Removed PParticleEmitter2
    JOINT_SLIDE_STIFFNESS = 0x18d, // added slideDir to PJoint, replaced
    // stiff with a float
    // 0x18e added creator, copyright to PGroup, removed(?) things
    // 0x18f
    // 0x190
    // 0x191, switched guid_hash flags 0x1 = HASH, 0x2 = GUID
    // 0x192
    // 0x193
    YELLOWHEAD_ON_SCREEN_COUNTER = 0x194, // Added onScreenCounter to
    // PYellowHead
    DETAILS_PRIMARY_INDEX = 0x195, // Added primaryIndex to
    // InventoryItemDetails
    WORLD_FOG_FACTOR = 0x196, // Added fog/darkness settings to PWorld
    PLAN_DETAILS = 0x197, // Plan details added to RPlan, removed
    // CInventoryItemDetails from EggLink, reference RPlan instead of CGlobalThingPtr, toolType
    // added to InventoryItemDetails
    AUDIOWORLD_VISIBILITY = 0x198, // Add hideInPlayMode to PAudioWorld
    // 0x199 added some unknown value to member variable reflection in PScript, removed in 0x19d
    COSTUME_ORIGINAL_PLAN = 0x19a, // added originalPlan to CostumePiece
    TRIGGER_HYSTERESIS = 0x19b, // add hysteresisMultplier and enabled to
    // PTrigger
    // 0x19c
    SCRIPT_REFLECT_DIVERGENT = 0x19d, // added reflectDivergent to
    // PScript, removed field added in 0x199
    // 0x19e removed field added in 0x180, removed unknown thing list from PRef
    ENEMY_SNAP = 0x19f, // Add snapVertex to PEnemy
    // 0x1a0
    // 0x1a1 - added optional serialize indicator for instance layout in PScript, removed field
    // added in 0x17a
    PROFILE_AUDIO_SWITCHES = 0x1a2, // added voipMutedPlayers,
    // voipChatPaused, and enableMusicInPlayMode to RLocalProfile
    // 0x1a3
    // 0x1a4 added CGameraCamera to PWorld, removed in 0x1d1, also removes a bunch of floats in
    // PWorld, (related to cgamecamera?)
    DYNAMIC_SWITCHES = 0x1a5, // added type to PSwitch, switches now
    // carry activation
    // 0x1a5, removed unknown thing reference from PSwitch, as well as unknown boolean and s32
    // 0x1a6, added joint array to Thing, removed in 0x1bc
    CHECKPOINT_SPAWNS_LEFT = 0x1a7, // added spawnsLeft to PCheckpoint
    // 0x1a8
    ENEMY_ANIMATION = 0x1a9, // centerOffset, animThing, and animSpeed
    // added to PEnemy
    // 0x1aa
    DETAILS_USER_DETAILS = 0x1ab, // userCreatedDetails added to
    // InventoryItemDetails
    // 0x1ac added storyLevelCompletionCount and levelCompletionCount to PlayerMetrics
    // 0x1ad added triggerByDestroy to PAudioWorld
    // 0x1ae added unknown boolean to Slot, removed in 0x1b9, (old game progression state?)
    // 0x1af
    // 0x1b0, something with things in PGroup
    DETAILS_CREATION_HISTORY = 0x1b1, // added creationHistory to
    // InventoryItemDetails
    // 0x1b2
    // 0x1b3
    // 0x1b4
    // 0x1b5 added enableMusicInPod to RLocalProfile
    // 0x1b6 added inventory collections/views to RLocalProfile
    // 0x1b7 unknown floats added to PCameraTweak, removed in 0x1d2
    // 0x1b8
    SLOT_PROGRESSION = 0x1b9, // added gameProgressionState to slot,
    // gameProgressionStatus to RLocalProfile, removed field added in 0x1ae
    // 0x1ba
    // 0x1bb
    // 0x1bc, removed joints array in Thing
    SWITCHKEY_VISIBILITY = 0x1bd, // added hideInPlayMode to PSwitchKey
    // 0x1be added unknown int to PWorld, removed in 0x213
    // 0x1bf, removed 3 unknown ints added to PWorld in 0x16f
    // 0x1c0
    DETAILS_USES = 0x1c1, // added numUses and lastUsed to
    // InventoryItemDetails
    WORLD_DEATH_COUNT = 0x1c2, // Added deathCount to PWorld
    // 0x1c3
    WORLD_MAX_PLAYERS = 0x1c4, // Added maxNumPlayers to PWorld
    // 0x1c5
    // 0x1c6, added maxSpawnsLeft to PCheckpoint
    THING_EMITTER = 0x1c7, // added emitter to thing
    EMITTER_MAX_EMITTED = 0x1c8, // Added maxEmittedAtOnce to PEmitter
    REF_ALIVE_FRAMES = 0x1c9, // Added oldAliveFrames to PRef
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
    TRIGGER_ZLAYERS = 0x1d5, // added allZLayers to PTrigger, removed
    // unknown int from PTrigger
    // 0x1d6
    // 0x1d7, switch cameraType data type from u8 to u32
    // 0x1d8
    // 0x1d9
    // 0x1da
    WORLD_DISSOLVING_THINGS = 0x1db, // Added dissolving things to PWorld
    // 0x1dc
    // 0x1dd
    WORLD_TUTORIAL_LEVEL = 0x1de, // added isTutorialLevel to PWorld
    SLOT_LEVEL_TYPE = 0x1df, // Added developerLevelType to slot
    // 0x1e0
    // 0x1e1, added avatarIcon to RLocalProfile
    // 0x1e2
    // 0x1e3
    // 0x1e4, added tutorialLevelStates to RLocalProfile, videos to PlayedLevelData
    SCRIPT_MODIFIERS = 0x1e5, // Added modifiers to RScript,
    // lbp1MainMenuButtonUnlocks added? something might be wrong there
    // 0x1e6, added lbp1CreateModeVOProgress to RLocalProfile
    // 0x1e7
    // 0x1e8
    // 0x1e9
    // 0x1ea, added stats to PlayerMetrics
    // 0x1eb, added instanceInfiniteLives to PCheckpoint
    SHARED_SCRIPT_DATA = 0x1ec, // replace all inline script fields with
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
    RENDERMESH_PARENT_DISTANCE = 0x1f6, // added parentDistanceFront/Side
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
    DETAILS_ALLOW_EMIT = 0x205, // add allowEmit flag to
    // InventoryItemDetails
    // 0x206, added lbp1VOPlayed to RLocalProfile
    // 0x207
    WORLD_SHAREABLE_REWARDS = 0x208, // added areRewardsShareable to
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
    THING_CREATED_PLAYER = 0x214, // added createdBy and changedBy to
    // thing, placedBy to decal
    DECAL_PLAYMODE_FRAME = 0x215, // added playmodeFrame to decal
    // 0x216
    // 0x217
    // 0x218
    DECAL_SCORCH_MARK = 0x219, // add scrorchMark to decal
    // 0x21a
    THING_STAMPING = 0x21b, // isStamping added to thing
    // 0x21c
    // 0x21d, tweakTargetMin/MaxLength to PJoint
    // 0x21e
    // 0x21f, added currentlyEditing to PJoint
    // 0x220
    // 0x221
    DETAILS_DATE_ADDED = 0x222, // add dateAdded to InventoryItemDetails
    DETAILS_SHAREABLE = 0x223, // add shareable flag to
    // InventoryItemDetails
    // 0x224, collidablePoppet added to PShape
    // 0x225
    // 0x226, added hideInPlayMode to PEmitter
    SHAPE_MASS_DEPTH = 0x227, // added massDepth to PShape
    // 0x228
    // 0x229
    // 0x22a, removed unknown value in EggLink that was added in 0x208
    // 0x22b
    // 0x22c, added editingPlayer to PBody
    // 0x22d
    WORLD_SPAWN_FAILURE = 0x22e, // Added everSpawned and
    // spawnFailureCount to PWorld
    // 0x22f, added flags to resource descriptor
    // 0x230, added modScaleActive to PJoint and PEmitter
    // 0x231,
    // 0x232, removed unknown v2 from Poppet
    // 0x233, details relayout lbp1? might actually be 0x234, need to double check
    // 0x234, re-do network online id struct
    ITEM_FLAGS = 0x235, // Added UID, flags to InventoryItem, add back
    // pad to InventoryItemDetails
    // 0x236
    // 0x237
    SLOT_DESCRIPTOR = 0x238, // Added shareable and backgroundGUID to Slot
    MESH_MINMAX_UV = 0x239, // Added min/maxUV and areaScaleFactor to
    // RMesh
    // 0x23a
    // 0x23b, added subtitleMode to RLocalProfile
    EGGLINK_SHAREABLE = 0x23c, // Adds shareable field to CEggLink
    // 0x23d, added saveIcon to RLocalProfile
    // 0x23e, added angleRange and includeTouching to PSwitch
    // 0x23f
    // 0x240
    // 0x241
    // 0x242
    // 0x243
    MATERIAL_BULLET = 0x244, // Adds bullet to RMaterial, bulletsRequired
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
    THING_PLAN_GUID = 0x254, // Adds planGUID to thing
    // 0x255,
    // 0x256, added unknown value to RLocalProfile, removed in 0x351
    // 0x257
    GENERATEDMESH_PLAN_GUID = 0x258, // Add planGUID to PGeneratedMesh
    // 0x259
    WORLD_GLOBAL_SETTINGS = 0x25a, // Added global settings fields to
    // PWorld
    PART_PLAN_GUIDS = 0x25b, // Plan GUIDs added to various parts
    // 0x25c, removed u32 color from Decal
    // 0x25d, added hideInPlayMode to PJoint
    // 0x25e, added editable and planDescriptor to PGroup
    // 0x25f, added unknown boolean to ObjectState, removed in 0x2bd
    DECAL_U16_COLOR = 0x260, // replace RGB32 color with RGB565 color in
    // decal
    // 0x261, added user settings to RLocalProfile
    // 0x262, removed unknown setting from RLocalProfile
    // 0x263, added tutorialPrizePlanList to RGame
    // 0x264, added categoryID to DLCFile
    ENEMY_WALK_CONSTRAINT = 0x265, // newWalkConstraintMass added to
    // PEnemy
    // 0x266, dlcPackViewed/Shown added to RLocalProfile, levelHash and levelHasDLC added to
    // PlayedLevelData, (levelHasDLC removed in 0x399)
    GROUP_EMITTER = 0x267, // emitter, lifetime, aliveFrames added to
    // PGroup
    // 0x268, fixedUpByDeveloper added to RLocalProfile
    // 0x269, added discovered and unlocked to PlayedLevelData (both removed in 0x399), removed
    // lockStates from RLocalProfile, changed play/completion/aced counts to u16 instead of u32
    // 0x26a, added requireAll to PCameraTweak
    // 0x26b, added levelStateAfterLastFinishedEditing to RGame
    // 0x26c, added lastGridMoveFrame, lastGridRotateFrame, and lastGridScaleFrame to
    // PoppetTweakObjectPlacement
    // 0x26d, added demoProgressionStatus to RLocalProfile
    GROUP_PICKUP_ALL_MEMBERS = 0x26e, // added pickupAllMembers to PGroup
    WATER_LEVEL = 0x26f, // Addede waterLevel and targetWaterLevel to
    // PWorld
    WATER_MAGNITUDE = 0x270, // Added water wave magnitude fields to
    // PWorld
    BRANCHES = 0x271, // Added branch descriptions to resources
    AIRTIME = 0x272, // airTimeLeft added to PCreature

    LBP1_MAX = 0x272, // last revision for lbp1

    // Deploy/LBP2 revisions

    ARCADE = 0x273, // Robo boots (wall jump), custom guns, recordings,
    // logic
    INPUT_UPDATE_TYPE = 0x274, // added updateType to PSwitchInput
    INPUT_LETHAL_TYPE = 0x275, // added lethalType to PSwitchInput
    // 0x276, ???
    // 0x277, add port thing to PSwitch, removed in 0x327
    // 0x278, add fromWaterLevel to PWorld, portThing to PSwitchInput, removed in 0x327
    // 0x279, add configuration to PPhysicsTweak
    // 0x27a, add unknown thing field to PPhysicsTweak, removed in 0x327
    MATERIAL_CIRCUITBOARD = 0x27b, // Add circuitboard and disableCSG to
    // RMaterial
    GENERATEDMESH_VISIBILITY = 0x27c, // add visible to PGeneratedMesh
    SWITCHKEY_TYPE = 0x27d, // added type to PSwitchKey
    // 0x27e, add switchConnectorUID to PoppetEditState
    THING_SWAP_UID_PARENT = 0x27f, // swap thing UID and thing parent
    // fields in serialization
    // 0x280, remove unknown field in NpcMoveCmd
    // 0x281, replace PPhysicsTweak inputs with vector types, switch tweak target length to
    // floats in PJoint
    // 0x282, add unknown structure to PWorld, removed in 0x287
    MICROCHIP_VISIBILITY = 0x283, // Added hideInPlayMode to PMicrochip
    // and PPhysicsTweak
    // 0x284, add includeRigidConnectors to PSwitch
    // 0x285, customPortOffset to PSwitch
    // 0x286, add decelStrength to PPhysicsTweak
    // 0x287, add BehaviorFollow/Act to PNpc, removed in 0x293, also removes some struct in PWorld
    INPUT_RIGID_CONNECTORS = 0x288, // added includeRigidConnectors to
    // PSwitchInput
    // 0x289, added scuba gear / headpiece to PCreature
    // 0x28a,
    // 0x28b, added ActivationFrame/Inverted to PSwitchInput
    // 0x28c, added hidden categories in RLocalProfile
    SWITCHINPUT_VISIBILITY = 0x297, // Added hideInPlayMode to
    // PSwitchInput
    THING_TEST_MARKER = 0x2a1, // Adds test serialization marker to CThing
    PARAMETER_ANIMATIONS = 0x2a2, // Added parameter animations to
    // RGfxMaterial
    PLAYER_SIGNAL = 0x2a3, // Added gameMode to PWorld, playerNumberColor
    // to PShape, player to CSwitchSignal
    RESPONSIBLE_PLAYER = 0x2a5, // responsiblePlayer/frames added to
    // PCreature
    WORLD_SACKBOT_NUMBER = 0x2b0, // added nextSackbotPlayerNumber to
    // PWorld
    WORLD_WATER_BITS = 0x2b4, // Add water bits to PWorld
    SHAPE_FLAGS = 0x2b5, // Add flags to PShape
    MATERIAL_PROPERTIES = 0x2b7, // Restitution, single grab, and sticky
    // added to PMaterialTweak
    MICROCHIP_WIRE_VISIBILITY = 0x2b8, // Adds wiresVisible to PMicrochip
    MATERIAL_NO_AUTO_DESTRUCT = 0x2b9, // Added noAutoDestruct to
    // PMaterialTweak
    MOTION_CONTROLLER_ZONE = 0x2ba, // Add motionControllerZone to
    // PCameraTweak
    LAMS_KEYS = 0x2bb, // Use LAMS keys instead of translation tags
    SWITCH_BEHAVIOR = 0x2c4, // Randomiser added, SwitchInput no longer
    // carries signals, behavior added to various structures
    TEMP_COSTUME = 0x2c5, // Adds costumePieceVec[COSTUME_TEMP_START] to
    // PCostume
    REMOVE_GAME_UPDATE_FRAME = 0x2c6, // Remove Game.LastUpdateFrame from
    // PEmitter
    SHOW_THERMOMETER = 0x2d3, // showThermometer added to RLocalProfile
    GAMEPLAY_DATA_TYPE = 0x2da, // added gameplayType to PGameplayData
    SWITCH_KEYNAME = 0x2dc, // name added to switchKey and switch
    WORLD_SIM_FRAME = 0x2e2, // added simFrame to world
    RENDERMESH_VISIBILITY_FLAGS = 0x2e3, // switch visible to
    // visibilityFlags in PRenderMesh
    MICROCHIP_LAST_TOUCHED = 0x2e4, // Last touched added to PMicroChip
    DIRECT_CONTROL = 0x2e5, // directControlPrompt added to PCreature
    MICROCHIP_OFFSET = 0x2e9, // Circuitboard board offset added to
    // PMicroChip
    SLOT_COLLECTABUBBLES_REQUIRED = 0x2ea, // added
    // collectabubblesRequired to slot
    WORLD_CUTSCENE_MANAGER = 0x2ee, // added cutSceneCameraManager to
    // PWorld
    THING_HIDDEN = 0x2f2, // added hidden to thing
    SLOT_COLLECTABUBBLES_CONTAINED = 0x2f4, // added
    // collectabubblesContained to slot
    GFXMATERIAL_ALPHA_MODE = 0x2fa, // Added alpha mode to RGfxMaterial
    SCRIPT_S64_TABLE = 0x30b, // Added S64 constants table to RScript
    WORLD_AUDIO_SETTINGS = 0x30c, // added globalAudioSettings to PWorld
    SIGNAL_TERNARY = 0x310, // Added ternary to switch signal
    ENEMY_SMOKE_COLOR = 0x31e, // smokeColor added to PEnemy
    GUN_MODIFIERS = 0x320, // added fireRate, gunAccuracy,
    // bulletEmitOffset/Rotation, gunThing, gunTrigger, and lastGunTriggerUID
    PALETTE_CONVERTED_PLANS = 0x323, // added convertedPlans to RPalette
    LEVEL_SETTINGS_AO = 0x325, // added shadow settings and dofNear to
    // PLevelSettings
    LEVEL_SETTINGS_DOF_FAR = 0x326, // added DofFar to PLevelSettings
    DROP_POWERUPS = 0x32c, // canDropPowerup added to PCreature, randy
    // seed also removed from pworld?
    LEVEL_SETTINGS_ZEFFECT = 0x331, // zEffect fields added to
    // PLevelSettings
    TRIGGER_ZOFFSET = 0x332, // add zOffset to PTrigger
    PLANET_DECORATIONS = 0x333, // Add planetDecorations to slot
    DETAILS_FLAGS = 0x335, // flags added to InventoryItemDetails
    REMOVE_UPDATED_SCRIPT = 0x33a, // removed upToDateScript from RScript
    PIN_FLAGS = 0x33b, // pinFlags added to PinsAwarded profile data
    SLOT_LABELS = 0x33c, // Added labels to slot
    THING_FLAGS = 0x341, // added flags to thing, remove localPosition
    // from PPos
    REMOVE_WORLD_TUTORIAL_LEVEL = 0x344, // isTutoriallevel removed from
    // PWorld
    USER_CATEGORIES = 0x34a, // userCategoryIndex added to InventoryItem
    WEEKDAYS_PLAYED_PIN = 0x34b, // added weekdaysPlayedBits to
    // PinsAwarded
    DECAL_PLAN_DESCRIPTOR = 0x34c, // replace decal planGUID with
    // descriptor
    COMPACT_MICROCHIP = 0x34d, // name, compactComponents, and
    // circuitBoardSizeX/Y added to PMicrochip
    WORLD_WATER_DRAIN = 0x34e, // Added water drain sounds enabled fields
    // to PWorld
    SLOT_SUBLEVEL = 0x352, // isSubLevel added to slot
    SLOT_SCOREBOARD_LINK = 0x35e, // added scoreboardLevelLinkSlot to
    // PWorld
    REMOVE_LBP1_TUTORIALS = 0x36c, // Removed tutorialLevel and
    // tutorialVideo from InventoryItem, removed gameProgressionState from slot
    WORLD_CAMERA_SETTINGS = 0x370, // Added camera settings to PWorld
    WORLD_FRAME_START = 0x377, // add frameLevelStarted to PWorld
    DETAILS_RELAYOUT_LBP2 = 0x37d, // NetworkOnlineID and NetworkPlayerID
    // are written as fixed length buffers, InventoryItemDetails has been remade again
    PROFILE_PINS = 0x385, // Add profile pin data
    ENEMY_SMOKE_BRIGHTNESS = 0x39a, // smokeBrightness added to PEnemy
    EYETOY_OUTLINE = 0x3a0, // outline added to EyetoyDATA
    WORLD_NIGHTDAY_SWAP = 0x3a3, // added lbp2NightDaySwapped to PWorld
    PRODUCTION_BUILD = 0x3b6, // fromProductionBuild added to resources
    // containing slot lists.
    PAINTING_PHOTO_DATA = 0x3c8, // painting added to
    // InventoryItemPhotoData
    SLOT_EXTRA_METADATA = 0x3d0, // min/maxPlayers, moveRecommended,
    // showOnPlanet, and livesOverride added to slot
    PTG_USE_DEFAULT_BACKGROUND = 0x3e0, // add useDefaultBackground to
    // PaintProperties
    LEVEL_CROSS_DEPENDENCIES = 0x3e6, // Added Vita cross dependency
    // hashes to RLevel
    SLOT_CROSS_COMPATIBLE = 0x3e9, // crossCompatible added to slot
    PROFILE_CROSS_DEPENDENCIES = 0x3eb, // Added Vita cross dependency
    // hashes to RBigProfile
    DATALABELS = 0x3ef,  // Creator data labels added to RBigProfile
    PIN_LBP1_TROPHY_AWARDED = 0x3f7, // Added a trophy for LBP1 enum to
    // RPin files
    PROFILE_OWNED_LBP = 0x3f8, // OwnedLBP field added to RLocalProfile

    LBP2_MAX = 0x3f8, // last revision for lbp2, technically 0x3f9, but
    // no resource exists with this until lbp3

    // Leerdammmer revisions

    LD_HEAD = 0x272, // Head revision for Leerdammer branch

    LD_WATER_SETTINGS = 0x0, // Adds scuba gear and water related tweak
    // settings
    LD_WATER_TINT = 0x1, // Adds water tint and murkiness to PWorld
    LD_RESOURCES = 0x2, // Remove's depreciated CValue from PMetaData,
    // adds compressionFlags to Resource's, adds compressed parts to CThing
    LD_SAVEGAME_INFO = 0x3, // Add showSaveGameInfo and totalLevelCount
    // to RLocalProfile
    LD_WATER_WAVE = 0x4, // Adds Game.CurrWavePos and fromWaterLevel to
    // PWorld
    LD_TEST_MARKER = 0x5, // Adds test serialization marker to CThing
    // 0x4c44:0x0006
    LD_WATER_BITS = 0x7, // Add water bits to PWorld
    LD_LAMS_KEYS = 0x8, // Use LAMS keys instead of translation tags
    // 0x4c44::0x0009
    // 0x4c44:0x000a
    LD_TEMP_COSTUME = 0xb, // Adds costumePieceVec[COSTUME_TEMP_START] to
    // PCostume
    LD_REMOVED_HEAD_PIECE = 0xc, // Removes hasBreathedAir from
    // PCreature, or is that headPiece
    LD_NEWS_TIMESTAMP = 0xd, // Adds lastNewsItemTimestamp to
    // RLocalProfile
    LD_FAKE_TIMESTAMP = 0xe, // I think this is what this is, probably
    LD_SUBMERGED = 0xf, // Adds Game.AmountBody/HeadSubmerged to PWorld
    // 0x4c44:0x0010
    LD_REMOVED_ENEMY_STAT = 0x11, // Removes enemiesKilled from
    // PlayerMetrics
    LD_QUEUE = 0x12, // Added lolcatFtwTimestamp to RLocalProfile
    LD_SHADER = 0x13, // Added another shader to RGfxMaterial
    LD_THERMOMETER = 0x14, // Added showThermometer to RLocalProfile
    LD_USED_SCUBA = 0x15, // Added hasUsedScuba to RGame
    LD_OWNER_ID = 0x16, // Added ownerUserId to RLocalProfile
    LD_WATER_LEVELS = 0x17, // Added numWaterLevelsPlayed to RLocalProfile

    // Custom leerdammer revisions for cambridge project

    LD_CAMBRIDGE_ADD_STATIC = 0x18, // Added isStatic to PShape

    LD_MAX = 0x17, // last revision for leerdammer revisions

    // Vita revisions

    D1_HEAD = 0x3e2, // Head revision for Double11/Vita branch

    D1_PERFDATA = 0x4, // Added perf data to RGfxMaterial
    D1_SHADER = 0x10, // Added additional shaders to RGfxMaterial
    D1_UV_OFFSCALE = 0x19, // Added UV offsets/scales to RGfxMaterial
    D1_VERTEX_COLORS = 0x28, // Added vertexColors to RMesh
    D1_WATER_BRIGHTNESS = 0x2d, // Adds waterBrightness to PWorld
    D_1DATALABELS = 0x2e, // Creator data labels added to RBigProfile
    D1_PROTECTED_LABELS = 0x31, // added protectedIds to PlayerDataLabels
    D1_LABEL_ANALOGUE_ARRAY = 0x33, // Make analogue value from
    // DataLabelValue an array, rather than a singular type
    D1_LABEL_TERNARY = 0x3c, // Ternary value added to DataLabelValue
    D1_SLOT_REWARDS = 0x3d, // Added rewards and acingEnabled fields to
    // slot
    D1_SLOT_ENFORCE_MINMAX = 0x4c, // added enforceMinMaxPlayers to slot
    D1_SLOT_SAME_SCREEN = 0x4d, // added sameScreenGame to slot
    D1_CHECKPOINT_PLAY_AUDIO = 0x50, // added playAudio to PCheckpoint
    D1_NEAR_CHALLENGES = 0x57, // Near challenges added to RBigProfile
    D13_G_CONNECTIVITY = 0x58, // added total3gUp/DownStream to
    // RLocalProfile
    D1_NEAR_TREASURES = 0x59, // Near treasure log added to RBigProfile
    D_1DOWNLOADED_SLOTS = 0x5a, // Downloaded slots added to RBigProfile
    D1_CHALLENGE_LEVEL_TYPE = 0x5b, // levelType added to Challenge
    D_1DEFERRED_PLAYS = 0x5c, // Added deferredPlayCount(Uploaded) to
    // PlayedLevelData
    D1_SLOT_DOWNLOAD_DATA = 0x5d, // sizeOfResources, sizeOfSubLevels,
    // subLevels, and slotList added to slot for download metadata
    D1_COLLECTABUBBLES = 0x5e, // containsCollectabubbles added to slot
    D1_LEVEL_TIMES_MAP = 0x60, // added levelTimesMap to PlayerMetrics
    D1_UPLOADED_HIGH_SCORE = 0x61, // added uploadedLocalHighScore to
    // PlayedLevelData
    D1_MOE_PIN_PROGRESS = 0x64, // added moreOfEverythingPinProgress to
    // PinsAwarded
    D1_CHALLENGE_SCORE = 0x68, // myScore and networkOnlineID added to
    // Challenge
    D1_TOUCH_CREATE = 0x69, // createRearTouchPan,
    // createRearTouchPinchZoom, createRearTapZoom aded to RLocalProfile
    D1_TROPHIES = 0x72, // added gold/silver/bronze trophy counts to
    // PlayedLevelDATA
    D1_CREATE_WARNING = 0x74, // Added createFrameRateWarningMessages to
    // RLocalProfile
    D1_CREATE_BETA = 0x75, // Added online beta timestamps to
    // RLocalProfile
    D1_PLANET_DECORATIONS = 0x7b, // Planet decorations added to
    // RBigProfile
    D_1DETAILS_PROPORTIONAL = 0x7d, // add makeSizeProportional to
    // InventoryItemDetails
    D1_SLOT_REVISION = 0x80, // download revision added to slot

    D1_MAX = 0x87, // last revision for vita

    // LBP3 revisions

    COLLECTION_POPPET_POWERUP = 0x9, // added poppetPowerUpSelection to
    // InventoryCollection
    SLOT_GAME_MODE = 0x12, // gameMode added to slot
    TRIGGER_ZRANGE = 0x2a, // zRangeHundreds added to PTrigger
    WORLD_BACKDROP_OFFSET = 0x6d, // add backdrop offset adjustment
    // fields to PWorld
    WORLD_BACKDROP_AUTOZ = 0x70, // added backdropOffsetZAuto to PWorld
    TRIGGER_SCORE_VALUE = 0x90, // scoreValue added to PTrigger
    DECORATION_SHADOW = 0xc4, // Added hasShadow to decorations
    STREAMING_PLAN = 0xcc, // isUsedForStreaming added to RPlan
    SLOT_GAME_KIT = 0xd2, // isGameKit added to slot
    MESH_SKELETON_TYPE = 0xd6, // added skeletonType to RMesh
    WORLD_AMBIENCE_OVERRIDE = 0xe2, // added overrideBackdropAmbience to
    // world
    MOVE_GAMEPLAY_DATA_TYPE = 0xef, // move gameplayType to top of struct
    // in PGameplayData
    GAMEPLAY_DATA_TREASURE = 0xf3, // treasureType and treasureCount
    // added to PGameplayData
    WORLD_WATER_COLORS = 0xf8, // waterHint colors added to PWorld
    ITEM_GUID = 0x106, // Added tempGUID to InventoryItem
    SLOT_ENTRANCE_DATA = 0x11b, // entranceName and originalSlotID added
    // to slot
    CREATURE_SUBSTATE = 0x132, // subState added to PCreature
    FRESNEL = 0x13a, // Added refractive fresnel fields to RGfxMaterial
    ADVENTURE = 0x145, // Added adventure resource to slot
    SLOT_BADGE_SIZE = 0x153, // customBadgeSize added to slot
    FUZZ = 0x16b, // Added fuzzLengthAndRefractiveFlag to RGfxMaterial
    // and triangleAdjacencyInfo to RMesh
    DECORATION_QUEST = 0x16c, // Added isQuest to decorations
    SLAPPED_AS_PIN = 0x177, // added slappedAsBits to PinsAwarded
    FUZZ_LIGHTING = 0x17c, // Added fuzz lighting/swirl fields to
    // RGfxMaterial
    WORLD_BACKDROP_TOGGLE = 0x182, // added backdropEnabled and
    // currBackdropEnabled to PWorld
    SLOT_TRAILER_PATH = 0x192, // added localPath for trailer to slot
    SLOT_TRAILER_THUMBNAIL = 0x206, // added thumbPath to slot
    SIMON_SAYS_PIN = 0x209, // added simonSaysProgress to PinsAwarded
    SPRINGINATOR = 0x20c, // Added springinator fields
    SLOT_ENFORCE_MINMAX = 0x215, // add enforceMinMaxPlayers to slot

    LBP3_MAX = 0x218,

    // Toolkit revisions

    MZ_HEAD = 0x021803f9,

    MZ_BASE = 0x0, // Base revision for Toolkit assets
    MZ_BST_REMOVE_SK = 0x1, // Remove skeleton type from RBoneSet
    MZ_CGC_ORBIS = 0x2, // Keep track if a shader cache is for PS4 or not.
    MZ_CGC_PATH = 0x3, // Keep original path of RGfxMaterial in
    // RShaderCache
    MZ_CGC_SWIZZLE = 0x4, // Swizzled color vector in RShaderCache
    MZ_CGC_SHORT_FLAGS = 0x5, // Use short flags in RShaderCache
    MZ_SCENE_GRAPH = 0x6, // RSceneGraph added, fields removed in PWorld
    // that are now stored in scene graph, removed thing createdBy indices, add name field to
    // Things
    MZ_REMOVE_ASPECT = 0x7, // Remove aspectRatio from Camera
    // serialization
    MZ_REMOVE_GFX_CODE = 0x8, // Removes code from RGfxMaterial for
    // generation on the fly
    MZ_SCE_DEFAULT_LIGHTING = 0x9, // Serializes lighting in RSceneGraph
    // to use as default.
    MZ_SCE_PACKED_DATA = 0xa, // Allows storing data in scene graph
    // caches.
    MZ_CGC_OFFSETS = 0xb, // Adds offsets for properties in cached shaders
    MZ_GLSL_SHADERS = 0xc, // Store GLSL shaders in RGfxMaterial

    MZ_MAX = 0xc, // last revision for toolkit revisions

    // Legacy mod revisions
    // Somewhat weird, but apparently Toolkit started with mod
    // revisions of 0x3, all older ones were only present in early versions
    // of Workbench

    LM_OLD_HEAD = 0xffff, // Head revision for mods prior to LM_TYPES
    LM_HEAD = 0x01ae03fa, // Head revision for mods >= LM_TYPES

    LM_BASE = 0x0, // Base revisions for old mods
    LM_ITEMS = 0x1, // Added cached item metadata
    LM_TRANSLATIONS = 0x2, // Store translated locations/categories in mod
    LM_TOOLKIT = 0x3, // When mods were added to Toolkit, move icon to
    // bottom of mod, title/desription use wstr instead of int16 size + char, added minor
    // version, entries now contain data size rather, file data is now one big buffer rather than
    // an array of bytearrays
    LM_SLOTS_TIMESTAMPS = 0x4, // Timestamps added to each entry, slot
    // array added
    LM_MINMAX = 0x5, // Added min/max revision to entries, meant for mods
    // to contain multiple versions of gmats/plans, never used.
    LM_TYPES = 0x6, // author is now str16, itemCount switched from u16
    // to u32, base revision bumped to support LBP3 slot metadata

    LM_MAX = 0x6, // last revision fo legacy mods
}