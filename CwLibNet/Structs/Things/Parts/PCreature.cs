using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

public class WhipSim : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0xB0;

        public Thing CreatureThing;

        public Matrix4x4? BaseHandleMatrix;
        public Vector3? PrevDir;
        public Vector3? CurrDir;
        public int StateTimer, State;
        public Thing AttachedThing;
        public Vector3? AttachedLocalPos;
        public Vector3? AttachedLocalNormal;
        public float AttachedLocalAngle, AttachedScale;
        public bool PlayedFailToFireSound;
        public float AttachedZOffset;


        public void Serialize(Serializer serializer)
        {
            CreatureThing = serializer.Thing(CreatureThing);
            BaseHandleMatrix = serializer.M44(BaseHandleMatrix);
            PrevDir = serializer.V3(PrevDir);
            CurrDir = serializer.V3(CurrDir);
            StateTimer = serializer.I32(StateTimer);
            State = serializer.I32(State);
            AttachedThing = serializer.Thing(AttachedThing);
            AttachedLocalPos = serializer.V3(AttachedLocalPos);
            AttachedLocalNormal = serializer.V3(AttachedLocalNormal);
            AttachedLocalAngle = serializer.F32(AttachedLocalAngle);
            AttachedScale = serializer.F32(AttachedScale);
            PlayedFailToFireSound = serializer.Bool(PlayedFailToFireSound);
            AttachedZOffset = serializer.F32(AttachedZOffset);
        }


        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }

    public class SpringData : ISerializable
    {
        public const int BaseAllocationSize = 0x30;

        public Thing SpringThing;
        public int SpringTimer;
        public Vector3? SpringDirection;
        public Vector3? SpringThingPosition;


        public void Serialize(Serializer serializer)
        {
            SpringThing = serializer.Thing(SpringThing);
            SpringTimer = serializer.I32(SpringTimer);
            SpringDirection = serializer.V3(SpringDirection);
            SpringThingPosition = serializer.V3(SpringThingPosition);
        }


        public int GetAllocatedSize()
        {
            return BaseAllocationSize;
        }
    }

public class PCreature : ISerializable
{

    // ECreatureGunType
    // PAINT 0
    // RAY 1
    // CUSTOM 2

    // EDirectControlMode
    // NONE 0
    // FRONT  1
    // SIDE 2
    // MOTION_CONTROLLER 3

    // EEnemyMoveDirection
    // LEFT_FIRST 0
    // RIGHT_FIRST 1
    // LEFT_ONLY 2
    // RIGHT_ONLY 3
    // NONE 4

    // EEnemyPlayerAwareness
    // NONE 0
    // ATTRACT 1
    // REPULSE 2

    // EZEvent
    // NOTHING 0
    // WALK 1
    // AIR 2
    // JUMP_DOWN 3
    // WALK_TO_GRAB 4
    // WALK_BLOCKED 5
    // WALK_SCARED 6
    // AIR_BLOCKED 7
    // JUMP_DOWN_BLOCKED 8

    // EZMode
    // AUTO 0
    // MANUAL 1
    // HYBRID 2


    public ResourceDescriptor Config;

    public int JumpFrame;
    public float GroundDistance;
    public Vector3? GroundNormal;
    public Thing GrabJoint, JumpingOff;
    public int State;

    public int SubState;
    public int StateTimer;
    public float SpeedModifier, JumpModifier, StrengthModifier;
    public int ZMode, PlayerAwareness, MoveDirection;
    public Vector3? ForceThatSmashedCreature;
    public int CrushFrames;
    public float AwarenessRadius;


    public int AirTime;


    public int[] BouncepadThingUiDs, GrabbedThingUiDs;


    public bool HaveNotTouchedGroundSinceUsingJetpack;


    public Thing[] LegList, LifeSourceList;

    public Thing LifeCreature, AiCreature;


    public int JumpInterval, JumpIntervalPhase;


    public bool MeshDirty;


    public Thing[] EyeList, BrainAiList, BrainLifeList;


    public bool ReactToLethal;


    public Matrix4x4? OldAnimMatrix;

    public float AnimOffset;


    public Vector3? GroundNormalRaw;

    public Vector3? GroundNormalSmooth;


    public float BodyAdjustApplied;


    public float SwitchScale = 1.0f;


    public Vector3? GunDirAndDashVec;

    public float GunDirAndDashVecW;


    public Thing ResourceThing;


    public int GunFireFrame;


    public int BulletCount;


    public int BulletImmuneTimer;


    public Thing BulletEmitter0;


    public Thing BulletEmitter1;


    public int BulletPosIndex;


    public int MaxBulletCount;


    public float AmmoFillFactor;


    public bool GunDirPrecisionMode;


    public int FireRate;

    public float GunAccuracy;

    public Vector3? BulletEmitOffset;

    public float BulletEmitRotation;

    public Thing GunThing, GunTrigger;

    public int LastGunTriggerUid;


    public int AirTimeLeft;


    public float AmountBodySubmerged, AmountHeadSubmerged;


    public bool HasScubaGear;


    public ResourceDescriptor HeadPiece;


    public bool OutOfWaterJumpBoost;


    public ResourceDescriptor HandPiece;


    public Thing Head, ToolTetherJoint;

    public float ToolTetherWidth;

    public Thing Jetpack;

    public int WallJumpDir;

    public Vector3? WallJumpPos;

    public Vector3[] BootContactForceList;

    public int GunType;

    public bool WallJumpMat;


    public Thing LastDirectControlPrompt;

    public Thing DirectControlPrompt;


    public Vector3? SmoothedDirectControlStick;


    public short DirectControlAnimFrame;


    public byte DirectControlAnimState;


    public byte DirectControlMode;


    public int ResponsiblePlayer, ResponsibleFramesLeft;

    public bool CanDropPowerup;


    public byte CapeExtraMaxVelocityCap;


    public int Behavior;


    public int EffectDestroy = 6;


    public WhipSim WhipSim;


    public int ShootAtTouch; // vita


    public Thing AlternateFormWorld;


    public int HookHatState;

    public Thing HookHatBogey;


    public int FlyingState,
        FlyingTimer,
        FlyingFlumpFrame,
        FlyingImpulseFrame,
        FlyingFlapButtonTimer,
        FlyingBrakeTimer,
        FlyingGrabFallTimer;

    public float FlyingLegScale;

    public Vector4? FlyingVels;

    public bool FlyingFlapLockout, FlyingFallLockout, FlyingInWind, FlyingThrustLatched;

    public short GlidingTime;


    public byte SpringState;

    public bool SpringHasSprung;

    public SpringData CurrentSpringData;

    public byte SpringPower;

    public bool SpringSeparateForces;

    public byte SpringForce, SpringStateTimer;


    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Config = serializer.Resource(Config, ResourceType.SettingsCharacter);
        if (version < 0x155)
        {
            if (serializer.IsWriting()) serializer.GetOutput().I32(0);
            else
            {
                var stream = serializer.GetInput();
                var count = stream.I32();
                for (var i = 0; i < count; ++i)
                    stream.V3();
            }
        }

        JumpFrame = serializer.S32(JumpFrame);

        GroundDistance = serializer.F32(GroundDistance);
        GroundNormal = serializer.V3(GroundNormal);

        GrabJoint = serializer.Thing(GrabJoint);
        if (version < 0x13c) serializer.Thing(null);
        JumpingOff = serializer.Thing(JumpingOff);

        State = serializer.I32(State);
        if (subVersion >= 0x132)
            SubState = serializer.I32(SubState);
        StateTimer = serializer.I32(StateTimer);

        SpeedModifier = serializer.F32(SpeedModifier);
        JumpModifier = serializer.F32(JumpModifier);
        StrengthModifier = serializer.F32(StrengthModifier);

        if (version < 0x142) serializer.V4(null);

        ZMode = serializer.I32(ZMode);

        switch (version)
        {
            case < 0x146:
                serializer.I32(0);
                break;
            case > 0x145 and < 0x1f0:
                serializer.I32(0);
                serializer.I32(0);
                serializer.Bool(false);
                serializer.Bool(false);
                serializer.I32(0);
                break;
        }

        PlayerAwareness = serializer.S32(PlayerAwareness);
        MoveDirection = serializer.S32(MoveDirection);

        if (version < 0x15d) serializer.U8(0);
        if (version < 0x1f0)
        {
            serializer.I32(0); // Some array of actual thing pointers, should be 0 length
            serializer.F32(0);
        }

        ForceThatSmashedCreature = serializer.V3(ForceThatSmashedCreature);
        CrushFrames = serializer.I32(CrushFrames);

        AwarenessRadius = serializer.F32(AwarenessRadius);

        if (version >= 0x1df)
            AirTime = serializer.I32(AirTime);
        if (version >= 0x354)
        {
            BouncepadThingUiDs = serializer.Intvector(BouncepadThingUiDs);
            GrabbedThingUiDs = serializer.Intvector(GrabbedThingUiDs);
        }

        if (version >= 0x221)
            HaveNotTouchedGroundSinceUsingJetpack =
                serializer.Bool(HaveNotTouchedGroundSinceUsingJetpack);

        if (version >= 0x15d)
        {
            LegList = serializer.Thingarray(LegList);
            if (version < 0x166)
            {
                serializer.Thingarray(null);
                serializer.Thingarray(null);
            }

            LifeSourceList = serializer.Thingarray(LifeSourceList);
            LifeCreature = serializer.Thing(LifeCreature);
            AiCreature = serializer.Thing(AiCreature);
        }

        if (version >= 0x163)
        {
            JumpInterval = serializer.I32(JumpInterval);
            JumpIntervalPhase = serializer.I32(JumpIntervalPhase);
        }

        if (version is > 0x162 and < 0x16d) serializer.Bool(false);

        if (version >= 0x169) MeshDirty = serializer.Bool(MeshDirty);

        if (version >= 0x166)
        {
            EyeList = serializer.Thingarray(EyeList);
            BrainAiList = serializer.Thingarray(BrainAiList);
            BrainLifeList = serializer.Thingarray(BrainLifeList);
        }

        if (version is > 0x177 and < 0x1e3) serializer.F32(0);

        if (version >= 0x19c)
            ReactToLethal = serializer.Bool(ReactToLethal);

        if (version >= 0x1a9)
        {
            OldAnimMatrix = serializer.M44(OldAnimMatrix);
            AnimOffset = serializer.F32(AnimOffset);
        }

        if (version is > 0x1ed and < 0x225)
            serializer.S32(0);

        if (version >= 0x1fc) GroundNormalRaw = serializer.V3(GroundNormalRaw);
        if (version >= 0x212)
        {
            GroundNormalSmooth = serializer.V3(GroundNormalSmooth);
            BodyAdjustApplied = serializer.F32(BodyAdjustApplied);
        }

        if (version is >= 0x240 and < 0x2c4)
            SwitchScale = serializer.F32(SwitchScale);

        if (version is > 0x242 and < 0x24d)
            serializer.Resource(null, ResourceType.Plan);

        if (version >= 0x243)
            GunDirAndDashVec = serializer.V3(GunDirAndDashVec);
        if (subVersion >= 0x19e)
            GunDirAndDashVecW = serializer.F32(GunDirAndDashVecW);

        if (version >= 0x246)
            ResourceThing = serializer.Thing(ResourceThing);

        if (version >= 0x247)
            GunFireFrame = serializer.I32(GunFireFrame);
        if (version >= 0x248)
            BulletCount = serializer.I32(BulletCount);
        if (version >= 0x24a)
            BulletImmuneTimer = serializer.I32(BulletImmuneTimer);
        if (version >= 0x24d)
            BulletEmitter0 = serializer.Thing(BulletEmitter0);
        if (version >= 0x3a2)
            BulletEmitter1 = serializer.Thing(BulletEmitter1);
        if (version >= 0x24e)
            BulletPosIndex = serializer.I32(BulletPosIndex); // game
        // .bulletposindex_dashboots_hoverboard_unionval
        if (version >= 0x24f)
        {
            MaxBulletCount = serializer.I32(MaxBulletCount);
            AmmoFillFactor = serializer.F32(AmmoFillFactor);
        }

        if (version >= 0x252)
            GunDirPrecisionMode = serializer.Bool(GunDirPrecisionMode);

        if (version >= 0x320)
        {
            FireRate = serializer.I32(FireRate);
            GunAccuracy = serializer.F32(GunAccuracy);
            BulletEmitOffset = serializer.V3(BulletEmitOffset);
            BulletEmitRotation = serializer.F32(BulletEmitRotation);
            GunThing = serializer.Thing(GunThing);
            GunTrigger = serializer.Thing(GunTrigger);
            LastGunTriggerUid = serializer.I32(LastGunTriggerUid);
        }

        if (version >= 0x272)
            AirTimeLeft = serializer.I32(AirTimeLeft);

        if (version >= 0x2c9 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_SUBMERGED))
        {
            AmountBodySubmerged = serializer.F32(AmountBodySubmerged);
            AmountHeadSubmerged = serializer.F32(AmountHeadSubmerged);
        }

        if (version >= 0x289 || revision.IsLeerdammer())
            HasScubaGear = serializer.Bool(HasScubaGear);

        if (version is >= 0x289 and < 0x2c8 || revision.Before(Branch.Leerdammer,
                (int)Revisions.LD_REMOVED_HEAD_PIECE))
            HeadPiece = serializer.Resource(HeadPiece, ResourceType.Plan);

        if (version >= 0x289 || revision.IsLeerdammer())
            OutOfWaterJumpBoost = serializer.Bool(OutOfWaterJumpBoost);

        if (version >= 0x2a9)
            HandPiece = serializer.Resource(HandPiece, ResourceType.Plan);

        if (version >= 0x273)
        {
            Head = serializer.Thing(Head);
            ToolTetherJoint = serializer.Thing(ToolTetherJoint);
            ToolTetherWidth = serializer.F32(ToolTetherWidth);
            Jetpack = serializer.Thing(Jetpack);
            WallJumpDir = serializer.S32(WallJumpDir);
            WallJumpPos = serializer.V3(WallJumpPos);

            if (!serializer.IsWriting())
                BootContactForceList = new Vector3[serializer.GetInput().I32()];
            else
            {
                BootContactForceList ??= [];
                serializer.GetOutput().I32(BootContactForceList.Length);
            }

            for (var i = 0; i < BootContactForceList.Length; ++i)
                BootContactForceList[i] = serializer.V3(BootContactForceList[i]).Value;


            GunType = serializer.S32(GunType);
            WallJumpMat = serializer.Bool(WallJumpMat);
        }

        if (version is >= 0x29e and < 0x336)
            LastDirectControlPrompt = serializer.Thing(LastDirectControlPrompt);

        if (version > 0x2e4)
            DirectControlPrompt = serializer.Thing(DirectControlPrompt);

        if (version is >= 0x29e and < 0x336)
        {
            SmoothedDirectControlStick =
                serializer.V3(SmoothedDirectControlStick);
            DirectControlAnimFrame = serializer.I16(DirectControlAnimFrame);
            DirectControlAnimState = serializer.I8(DirectControlAnimState);
        }

        switch (version)
        {
            case >= 0x29f and < 0x2c1:
                DirectControlMode = serializer.I8(DirectControlMode);
                break;
            case >= 0x2c1 and < 0x336:
                serializer.U8(0);
                break;
        }

        if (version >= 0x2a5)
        {
            ResponsiblePlayer = serializer.I32(ResponsiblePlayer);
            ResponsibleFramesLeft = serializer.I32(ResponsibleFramesLeft);
        }

        if (version >= 0x32c)
            CanDropPowerup = serializer.Bool(CanDropPowerup);

        if (version >= 0x3f0)
            CapeExtraMaxVelocityCap = serializer.I8(CapeExtraMaxVelocityCap);

        if (version >= 0x35a)
            Behavior = serializer.I32(Behavior);

        if (version >= 0x373)
            EffectDestroy = serializer.I32(EffectDestroy);

        if (version >= 0x3c0)
            WhipSim = serializer.Reference(WhipSim);

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            switch (vita)
            {
                case < 0x53:
                    serializer.U8(0);
                    break;
                case >= 0x53:
                    ShootAtTouch = serializer.I32(ShootAtTouch);
                    break;
            }
        }

        switch (subVersion)
        {
            case >= 0x88 and <= 0xa4:
                serializer.Resource(null, ResourceType.Plan);
                break;
            case >= 0xaa:
                AlternateFormWorld = serializer.Thing(AlternateFormWorld);
                break;
        }

        if (subVersion >= 0xd7)
            HookHatState = serializer.I32(HookHatState);

        if (subVersion is >= 0xd7 and < 0xea)
        {
            serializer.Thing(null);
            serializer.Thing(null);
        }

        if (subVersion >= 0xdf)
            HookHatBogey = serializer.Thing(HookHatBogey);

        if (subVersion >= 0x196)
        {
            FlyingState = serializer.I32(FlyingState);
            FlyingTimer = serializer.I32(FlyingTimer);
            FlyingFlumpFrame = serializer.I32(FlyingFlumpFrame);
            FlyingImpulseFrame = serializer.I32(FlyingImpulseFrame);
            FlyingFlapButtonTimer = serializer.I32(FlyingFlapButtonTimer);
            FlyingBrakeTimer = serializer.I32(FlyingBrakeTimer);
            FlyingGrabFallTimer = serializer.I32(FlyingGrabFallTimer);
            FlyingLegScale = serializer.F32(FlyingLegScale);
            FlyingVels = serializer.V4(FlyingVels);
            FlyingFlapLockout = serializer.Bool(FlyingFlapLockout);
            FlyingFallLockout = serializer.Bool(FlyingFallLockout);
            FlyingInWind = serializer.Bool(FlyingInWind);
            FlyingThrustLatched = serializer.Bool(FlyingThrustLatched);
            GlidingTime = serializer.I16(GlidingTime);
        }

        if (subVersion >= 0x20c)
        {
            SpringState = serializer.I8(SpringState);
            SpringHasSprung = serializer.Bool(SpringHasSprung);
            CurrentSpringData = serializer.Reference(CurrentSpringData);
            SpringPower = serializer.I8(SpringPower);
            SpringSeparateForces = serializer.Bool(SpringSeparateForces);
            SpringForce = serializer.I8(SpringForce);
            SpringStateTimer = serializer.I8(SpringStateTimer);
        }
    }

    // TODO: Actually implement

    public int GetAllocatedSize()
    {
        return 0;
    }
}