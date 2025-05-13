using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;

namespace CwLibNet.Structs.Things.Parts;

public class PPhysicsTweak: ISerializable
{
    
    
    public float Activation;

    public float TweakGravity;

    
    public float TweakBuoyancy;

    
    
    public float LegacyTweakDampening;

    
    public Vector3? TweakDampening;

    public Vector3? Input;
    public Vector3? MiddleVel;


    public Vector3 LegacyVelRange;
    public Vector3? LegacyAcceleration;
    public Vector3? LegacyDeceleration;

    
    
    public int LegacyTurnToFace;

    
    public float VelRange, AccelStrength;

    
    public float DecelStrength;

    
    public byte DirectionModifier, MovementModifier;

    public bool LocalSpace;

    
    public int Configuration;

    
    public bool HideInPlayMode;

    
    public int ColorIndex;

    
    public String Name;

    
    public int FollowerPlayerMode;

    
    
    public int TeamFilter;

    
    public int Behavior;

    
    public bool AllowInOut, AllowUpDown;

    
    public float MinRange, MaxRange;

    
    public bool FollowKey;

    
    public float AngleRange;

    
    public bool Flee;

    
    public bool Stabiliser;

    
    public bool ShardDephysicalised, ShardPhysicsAudio;

    
    public bool IsLbp2PhysicsTweak;

    
    public float MaximumMass;

    
    public bool CanPush;

    
    public int ZBehavior;

    
    public float LastKnownActivation;

    
    public bool WaitingToMove;

    
    public byte ZPhase;

    
    public short GridSnap, GridStrength;

    
    public float GridGoalW;

    
    public Vector3? GridGoal;

    /* Vita */
    
    public int UsePanel;

    
    public int FollowType;

    
    public float FollowerDeceleration;

    
    public byte PlayerFilter;

    
    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        if (version < 0x2c4)
            Activation = serializer.F32(Activation);

        TweakGravity = serializer.F32(TweakGravity);
        if (version > 0x2ff)
            TweakBuoyancy = serializer.F32(TweakBuoyancy);

        if (version < 0x281)
            LegacyTweakDampening = serializer.F32(LegacyTweakDampening);

        if (version > 0x280)
            TweakDampening = serializer.V3(TweakDampening);
        Input = serializer.V3(Input);
        MiddleVel = serializer.V3(MiddleVel);

        if (version < 0x281)
            LegacyDeceleration = serializer.V3(LegacyDeceleration);
        if (version > 0x280)
            VelRange = serializer.F32(VelRange);
        if (version < 0x281)
            LegacyAcceleration = serializer.V3(LegacyAcceleration);

        if (version > 0x280)
        {
            AccelStrength = serializer.F32(AccelStrength);
            if (version > 0x285)
                DecelStrength = serializer.F32(DecelStrength);
        }

        if (version < 0x281)
        {
            LegacyDeceleration = serializer.V3(LegacyDeceleration);
            LegacyTurnToFace = serializer.I32(LegacyTurnToFace);
        }

        if (subVersion > 0xa3)
        {
            DirectionModifier = serializer.I8(DirectionModifier);
            MovementModifier = serializer.I8(MovementModifier);
        }

        LocalSpace = serializer.Bool(LocalSpace);

        if (!serializer.IsWriting() && version < 0x38a && VelRange != 0.0f)
        {
            AccelStrength /= VelRange;
            DecelStrength /= VelRange;
        }

        if (version > 0x278)
            Configuration = serializer.I32(Configuration);

        if (version >= 0x27a && version < 0x327)
            serializer.Thing(null);

        if (version > 0x282)
            HideInPlayMode = serializer.Bool(HideInPlayMode);

        if (version > 0x28d)
            ColorIndex = serializer.I32(ColorIndex);
        if (version > 0x2db)
            Name = serializer.Wstr(Name);

        if (version < 0x3e8)
        {
            if (version > 0x2ad)
                TeamFilter = serializer.U8(TeamFilter);
        }
        else
        {
            switch (subVersion)
            {
                case < 0x132:
                {
                    if (version >= 0x2ae)
                        TeamFilter = serializer.U8(TeamFilter);
                    break;
                }
                case > 0x131:
                    FollowerPlayerMode = serializer.I32(FollowerPlayerMode);
                    break;
            }
        }


        if (version > 0x2c5)
            Behavior = serializer.I32(Behavior);
        if (version > 0x2dc)
        {
            AllowInOut = serializer.Bool(AllowInOut);
            AllowUpDown = serializer.Bool(AllowUpDown);
            MinRange = serializer.F32(MinRange);
            MaxRange = serializer.F32(MaxRange);
            FollowKey = serializer.Bool(FollowKey);
        }

        if (version > 0x381)
            AngleRange = serializer.F32(AngleRange);

        if (version > 0x2fe)
            Flee = serializer.Bool(Flee);

        if (subVersion > 0x6f)
            Stabiliser = serializer.Bool(Stabiliser);
        if (subVersion > 0x12f)
        {
            ShardDephysicalised = serializer.Bool(ShardDephysicalised);
            ShardPhysicsAudio = serializer.Bool(ShardPhysicsAudio);
        }
        if (subVersion > 0x43)
            IsLbp2PhysicsTweak = serializer.Bool(IsLbp2PhysicsTweak);

        if (version > 0x36a)
        {
            MaximumMass = serializer.F32(MaximumMass);
            CanPush = serializer.Bool(CanPush);
            ZBehavior = serializer.S32(ZBehavior);
            LastKnownActivation = serializer.F32(LastKnownActivation);
            WaitingToMove = serializer.Bool(WaitingToMove);
        }

        if (subVersion > 0xf6)
            ZPhase = serializer.I8(ZPhase);

        // move recording, again will figure out the fields later
        // RecordingPlayer
        if (version > 0x3b8 && Configuration == 0xd)
        {
            serializer.Resource(null, ResourceType.ThingRecording); // recording
            serializer.F32(0); // playHead
            if (version < 0x3c4) serializer.U8(0);
            serializer.U8(0); // type
            serializer.U8(0); // dir
            serializer.V3(null); // prevDesiredPos
            serializer.U8(0); // prevDesiredPosSet
            serializer.M44(null); // startOrientation
            serializer.F32(0); // speed
            if (version > 0x3c4)
                serializer.U8(0); // pathIsAbsolute
        }

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            if (vita >= 0x11) // 0x3c0
                UsePanel = serializer.I32(UsePanel);
            if (vita is >= 0x11 and < 0x4b) serializer.U8(0); // 0x3c0
            if (vita >= 0x1f) // 0x3d4
                FollowType = serializer.I32(FollowType);
            if (vita >= 0x4b)
                FollowerDeceleration = serializer.F32(FollowerDeceleration);
            if (vita >= 0x55)
                PlayerFilter = serializer.I8(PlayerFilter);
        }

        if (subVersion >= 0x1 && subVersion < 0x17)
        {
            serializer.S32(0);
            serializer.S32(0);
            serializer.S32(0);
            serializer.S32(0);
            serializer.S32(0);
            serializer.S32(0);
            serializer.S32(0);
            serializer.S32(0);
            serializer.S32(0);
        }

        if (subVersion >= 0x5 && subVersion < 0x17)
        {
            serializer.F32(0);
            serializer.U8(0);
            serializer.U8(0);
        }

        // attract-o-gel, ill figure out the fields later
        // attractorData
        if (Configuration == 0xe && version > 0x3e3)
        {
            serializer.F32(0); // attractDistance
            serializer.I32(0);
            serializer.F32(0); // visualEffectBrightness_On
            serializer.F32(0); // visualEffectSpeed_On
            serializer.I32(0);
            serializer.F32(0); // visualEffectBrightnessOff
            serializer.F32(0); // visualEffectSpeed_Off
            serializer.U8(0); // attractorManualDetach
            serializer.U8(0); // attractorSoundEffects
            serializer.U8(0); // attractorVisualEffect
            serializer.U8(0); // attractorAffectConnected
        }

        if (subVersion > 0x56)
        {
            GridSnap = serializer.I16(GridSnap);
            GridStrength = serializer.I16(GridStrength);
        }

        if (subVersion > 0x99)
            GridGoalW = serializer.F32(GridGoalW);

        if (subVersion > 0x98)
            GridGoal = serializer.V3(GridGoal);
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}