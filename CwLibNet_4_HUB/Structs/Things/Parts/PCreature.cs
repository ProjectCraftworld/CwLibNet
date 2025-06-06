using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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


        public void Serialize()
        {
            CreatureThing = Serializer.Reference(CreatureThing);
            Serializer.Serialize(ref BaseHandleMatrix);
            PrevDir = Serializer.Serialize(ref PrevDir);
            CurrDir = Serializer.Serialize(ref CurrDir);
            Serializer.Serialize(ref StateTimer);
            Serializer.Serialize(ref State);
            AttachedThing = Serializer.Reference(AttachedThing);
            AttachedLocalPos = Serializer.Serialize(ref AttachedLocalPos);
            AttachedLocalNormal = Serializer.Serialize(ref AttachedLocalNormal);
            Serializer.Serialize(ref AttachedLocalAngle);
            Serializer.Serialize(ref AttachedScale);
            Serializer.Serialize(ref PlayedFailToFireSound);
            Serializer.Serialize(ref AttachedZOffset);
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


        public void Serialize()
        {
            SpringThing = Serializer.Reference(SpringThing);
            Serializer.Serialize(ref SpringTimer);
            SpringDirection = Serializer.Serialize(ref SpringDirection);
            SpringThingPosition = Serializer.Serialize(ref SpringThingPosition);
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


    public void Serialize()
    {
        var revision = Serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Serializer.Serialize(ref Config, Config, ResourceType.SettingsCharacter);
        if (version < 0x155)
        {
            if (Serializer.IsWriting()) Serializer.GetOutput().I32(0);
            else
            {
                var stream = Serializer.GetInput();
                var count = stream.I32();
                for (var i = 0; i < count; ++i)
                    stream.V3();
            }
        }

        Serializer.Serialize(ref JumpFrame);

        Serializer.Serialize(ref GroundDistance);
        GroundNormal = Serializer.Serialize(ref GroundNormal);

        GrabJoint = Serializer.Reference(GrabJoint);
        if (version < 0x13c) Serializer.Reference(null);
        JumpingOff = Serializer.Reference(JumpingOff);

        Serializer.Serialize(ref State);
        if (subVersion >= 0x132)
            Serializer.Serialize(ref SubState);
        Serializer.Serialize(ref StateTimer);

        Serializer.Serialize(ref SpeedModifier);
        Serializer.Serialize(ref JumpModifier);
        Serializer.Serialize(ref StrengthModifier);

        if (version < 0x142) Serializer.Serialize(ref null);

        Serializer.Serialize(ref ZMode);

        switch (version)
        {
            case < 0x146:
                Serializer.Serialize(ref 0);
                break;
            case > 0x145 and < 0x1f0:
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref false);
                Serializer.Serialize(ref false);
                Serializer.Serialize(ref 0);
                break;
        }

        Serializer.Serialize(ref PlayerAwareness);
        Serializer.Serialize(ref MoveDirection);

        if (version < 0x15d) Serializer.Serialize(ref 0);
        if (version < 0x1f0)
        {
            Serializer.Serialize(ref 0); // Some array of actual thing pointers, should be 0 length
            Serializer.Serialize(ref 0);
        }

        ForceThatSmashedCreature = Serializer.Serialize(ref ForceThatSmashedCreature);
        Serializer.Serialize(ref CrushFrames);

        Serializer.Serialize(ref AwarenessRadius);

        if (version >= 0x1df)
            Serializer.Serialize(ref AirTime);
        if (version >= 0x354)
        {
            Serializer.Serialize(ref BouncepadThingUiDs);
            Serializer.Serialize(ref GrabbedThingUiDs);
        }

        if (version >= 0x221)
            Serializer.Serialize(ref HaveNotTouchedGroundSinceUsingJetpack);

        if (version >= 0x15d)
        {
            LegList = Serializer.Serialize(ref LegList);
            if (version < 0x166)
            {
                Serializer.Serialize(ref null);
                Serializer.Serialize(ref null);
            }

            LifeSourceList = Serializer.Serialize(ref LifeSourceList);
            LifeCreature = Serializer.Reference(LifeCreature);
            AiCreature = Serializer.Reference(AiCreature);
        }

        if (version >= 0x163)
        {
            Serializer.Serialize(ref JumpInterval);
            Serializer.Serialize(ref JumpIntervalPhase);
        }

        if (version is > 0x162 and < 0x16d) Serializer.Serialize(ref false);

        if (version >= 0x169) Serializer.Serialize(ref MeshDirty);

        if (version >= 0x166)
        {
            EyeList = Serializer.Serialize(ref EyeList);
            BrainAiList = Serializer.Serialize(ref BrainAiList);
            BrainLifeList = Serializer.Serialize(ref BrainLifeList);
        }

        if (version is > 0x177 and < 0x1e3) Serializer.Serialize(ref 0);

        if (version >= 0x19c)
            Serializer.Serialize(ref ReactToLethal);

        if (version >= 0x1a9)
        {
            Serializer.Serialize(ref OldAnimMatrix);
            Serializer.Serialize(ref AnimOffset);
        }

        if (version is > 0x1ed and < 0x225)
            Serializer.Serialize(ref 0);

        if (version >= 0x1fc) GroundNormalRaw = Serializer.Serialize(ref GroundNormalRaw);
        if (version >= 0x212)
        {
            GroundNormalSmooth = Serializer.Serialize(ref GroundNormalSmooth);
            Serializer.Serialize(ref BodyAdjustApplied);
        }

        if (version is >= 0x240 and < 0x2c4)
            Serializer.Serialize(ref SwitchScale);

        if (version is > 0x242 and < 0x24d)
            Serializer.Serialize(ref null, ResourceType.Plan);

        if (version >= 0x243)
            GunDirAndDashVec = Serializer.Serialize(ref GunDirAndDashVec);
        if (subVersion >= 0x19e)
            Serializer.Serialize(ref GunDirAndDashVecW);

        if (version >= 0x246)
            ResourceThing = Serializer.Reference(ResourceThing);

        if (version >= 0x247)
            Serializer.Serialize(ref GunFireFrame);
        if (version >= 0x248)
            Serializer.Serialize(ref BulletCount);
        if (version >= 0x24a)
            Serializer.Serialize(ref BulletImmuneTimer);
        if (version >= 0x24d)
            BulletEmitter0 = Serializer.Reference(BulletEmitter0);
        if (version >= 0x3a2)
            BulletEmitter1 = Serializer.Reference(BulletEmitter1);
        if (version >= 0x24e)
            Serializer.Serialize(ref BulletPosIndex); // game
        // .bulletposindex_dashboots_hoverboard_unionval
        if (version >= 0x24f)
        {
            Serializer.Serialize(ref MaxBulletCount);
            Serializer.Serialize(ref AmmoFillFactor);
        }

        if (version >= 0x252)
            Serializer.Serialize(ref GunDirPrecisionMode);

        if (version >= 0x320)
        {
            Serializer.Serialize(ref FireRate);
            Serializer.Serialize(ref GunAccuracy);
            BulletEmitOffset = Serializer.Serialize(ref BulletEmitOffset);
            Serializer.Serialize(ref BulletEmitRotation);
            GunThing = Serializer.Reference(GunThing);
            GunTrigger = Serializer.Reference(GunTrigger);
            Serializer.Serialize(ref LastGunTriggerUid);
        }

        if (version >= 0x272)
            Serializer.Serialize(ref AirTimeLeft);

        if (version >= 0x2c9 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_SUBMERGED))
        {
            Serializer.Serialize(ref AmountBodySubmerged);
            Serializer.Serialize(ref AmountHeadSubmerged);
        }

        if (version >= 0x289 || revision.IsLeerdammer())
            Serializer.Serialize(ref HasScubaGear);

        if (version is >= 0x289 and < 0x2c8 || revision.Before(Branch.Leerdammer,
                (int)Revisions.LD_REMOVED_HEAD_PIECE))
            Serializer.Serialize(ref HeadPiece, HeadPiece, ResourceType.Plan);

        if (version >= 0x289 || revision.IsLeerdammer())
            Serializer.Serialize(ref OutOfWaterJumpBoost);

        if (version >= 0x2a9)
            Serializer.Serialize(ref HandPiece, HandPiece, ResourceType.Plan);

        if (version >= 0x273)
        {
            Head = Serializer.Reference(Head);
            ToolTetherJoint = Serializer.Reference(ToolTetherJoint);
            Serializer.Serialize(ref ToolTetherWidth);
            Jetpack = Serializer.Reference(Jetpack);
            Serializer.Serialize(ref WallJumpDir);
            WallJumpPos = Serializer.Serialize(ref WallJumpPos);

            if (!Serializer.IsWriting())
                BootContactForceList = new Vector3[Serializer.GetInput().I32()];
            else
            {
                BootContactForceList ??= [];
                Serializer.GetOutput().I32(BootContactForceList.Length);
            }

            for (var i = 0; i < BootContactForceList.Length; ++i)
                Serializer.Serialize(ref BootContactForceList[i]).Value;


            Serializer.Serialize(ref GunType);
            Serializer.Serialize(ref WallJumpMat);
        }

        if (version is >= 0x29e and < 0x336)
            LastDirectControlPrompt = Serializer.Reference(LastDirectControlPrompt);

        if (version > 0x2e4)
            DirectControlPrompt = Serializer.Reference(DirectControlPrompt);

        if (version is >= 0x29e and < 0x336)
        {
            SmoothedDirectControlStick =
                Serializer.Serialize(ref SmoothedDirectControlStick);
            Serializer.Serialize(ref DirectControlAnimFrame);
            Serializer.Serialize(ref DirectControlAnimState);
        }

        switch (version)
        {
            case >= 0x29f and < 0x2c1:
                Serializer.Serialize(ref DirectControlMode);
                break;
            case >= 0x2c1 and < 0x336:
                Serializer.Serialize(ref 0);
                break;
        }

        if (version >= 0x2a5)
        {
            Serializer.Serialize(ref ResponsiblePlayer);
            Serializer.Serialize(ref ResponsibleFramesLeft);
        }

        if (version >= 0x32c)
            Serializer.Serialize(ref CanDropPowerup);

        if (version >= 0x3f0)
            Serializer.Serialize(ref CapeExtraMaxVelocityCap);

        if (version >= 0x35a)
            Serializer.Serialize(ref Behavior);

        if (version >= 0x373)
            Serializer.Serialize(ref EffectDestroy);

        if (version >= 0x3c0)
            Serializer.Serialize(ref WhipSim);

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            switch (vita)
            {
                case < 0x53:
                    Serializer.Serialize(ref 0);
                    break;
                case >= 0x53:
                    Serializer.Serialize(ref ShootAtTouch);
                    break;
            }
        }

        switch (subVersion)
        {
            case >= 0x88 and <= 0xa4:
                Serializer.Serialize(ref null, ResourceType.Plan);
                break;
            case >= 0xaa:
                AlternateFormWorld = Serializer.Reference(AlternateFormWorld);
                break;
        }

        if (subVersion >= 0xd7)
            Serializer.Serialize(ref HookHatState);

        if (subVersion is >= 0xd7 and < 0xea)
        {
            Serializer.Reference(null);
            Serializer.Reference(null);
        }

        if (subVersion >= 0xdf)
            HookHatBogey = Serializer.Reference(HookHatBogey);

        if (subVersion >= 0x196)
        {
            Serializer.Serialize(ref FlyingState);
            Serializer.Serialize(ref FlyingTimer);
            Serializer.Serialize(ref FlyingFlumpFrame);
            Serializer.Serialize(ref FlyingImpulseFrame);
            Serializer.Serialize(ref FlyingFlapButtonTimer);
            Serializer.Serialize(ref FlyingBrakeTimer);
            Serializer.Serialize(ref FlyingGrabFallTimer);
            Serializer.Serialize(ref FlyingLegScale);
            Serializer.Serialize(ref FlyingVels);
            Serializer.Serialize(ref FlyingFlapLockout);
            Serializer.Serialize(ref FlyingFallLockout);
            Serializer.Serialize(ref FlyingInWind);
            Serializer.Serialize(ref FlyingThrustLatched);
            Serializer.Serialize(ref GlidingTime);
        }

        if (subVersion >= 0x20c)
        {
            Serializer.Serialize(ref SpringState);
            Serializer.Serialize(ref SpringHasSprung);
            Serializer.Serialize(ref CurrentSpringData);
            Serializer.Serialize(ref SpringPower);
            Serializer.Serialize(ref SpringSeparateForces);
            Serializer.Serialize(ref SpringForce);
            Serializer.Serialize(ref SpringStateTimer);
        }
    }

    // TODO: Actually implement

    public int GetAllocatedSize()
    {
        return 0;
    }
}