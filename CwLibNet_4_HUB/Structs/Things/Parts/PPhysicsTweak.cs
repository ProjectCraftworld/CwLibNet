using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;
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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (version < 0x2c4)
            Serializer.Serialize(ref Activation);

        Serializer.Serialize(ref TweakGravity);
        if (version > 0x2ff)
            Serializer.Serialize(ref TweakBuoyancy);

        if (version < 0x281)
            Serializer.Serialize(ref LegacyTweakDampening);

        if (version > 0x280)
            Serializer.Serialize(ref TweakDampening);
        Serializer.Serialize(ref Input);
        Serializer.Serialize(ref MiddleVel);

        if (version < 0x281)
            Serializer.Serialize(ref LegacyDeceleration);
        if (version > 0x280)
            Serializer.Serialize(ref VelRange);
        if (version < 0x281)
            Serializer.Serialize(ref LegacyAcceleration);

        if (version > 0x280)
        {
            Serializer.Serialize(ref AccelStrength);
            if (version > 0x285)
                Serializer.Serialize(ref DecelStrength);
        }

        if (version < 0x281)
        {
            Serializer.Serialize(ref LegacyDeceleration);
            Serializer.Serialize(ref LegacyTurnToFace);
        }

        if (subVersion > 0xa3)
        {
            Serializer.Serialize(ref DirectionModifier);
            Serializer.Serialize(ref MovementModifier);
        }

        Serializer.Serialize(ref LocalSpace);

        if (!Serializer.IsWriting() && version < 0x38a && VelRange != 0.0f)
        {
            AccelStrength /= VelRange;
            DecelStrength /= VelRange;
        }

        if (version > 0x278)
            Serializer.Serialize(ref Configuration);

        if (version is >= 0x27a and < 0x327)
            Serializer.SerializeReference(null);

        if (version > 0x282)
            Serializer.Serialize(ref HideInPlayMode);

        if (version > 0x28d)
            Serializer.Serialize(ref ColorIndex);
        if (version > 0x2db)
            Serializer.Serialize(ref Name);

        if (version < 0x3e8)
        {
            if (version > 0x2ad)
                Serializer.Serialize(ref TeamFilter);
        }
        else
        {
            switch (subVersion)
            {
                case < 0x132:
                {
                    if (version >= 0x2ae)
                        Serializer.Serialize(ref TeamFilter);
                    break;
                }
                case > 0x131:
                    Serializer.Serialize(ref FollowerPlayerMode);
                    break;
            }
        }


        if (version > 0x2c5)
            Serializer.Serialize(ref Behavior);
        if (version > 0x2dc)
        {
            Serializer.Serialize(ref AllowInOut);
            Serializer.Serialize(ref AllowUpDown);
            Serializer.Serialize(ref MinRange);
            Serializer.Serialize(ref MaxRange);
            Serializer.Serialize(ref FollowKey);
        }

        if (version > 0x381)
            Serializer.Serialize(ref AngleRange);

        if (version > 0x2fe)
            Serializer.Serialize(ref Flee);

        if (subVersion > 0x6f)
            Serializer.Serialize(ref Stabiliser);
        if (subVersion > 0x12f)
        {
            Serializer.Serialize(ref ShardDephysicalised);
            Serializer.Serialize(ref ShardPhysicsAudio);
        }
        if (subVersion > 0x43)
            Serializer.Serialize(ref IsLbp2PhysicsTweak);

        if (version > 0x36a)
        {
            Serializer.Serialize(ref MaximumMass);
            Serializer.Serialize(ref CanPush);
            Serializer.Serialize(ref ZBehavior);
            Serializer.Serialize(ref LastKnownActivation);
            Serializer.Serialize(ref WaitingToMove);
        }

        if (subVersion > 0xf6)
            Serializer.Serialize(ref ZPhase);

        // move recording, again will figure out the fields later
        // RecordingPlayer
        if (version > 0x3b8 && Configuration == 0xd)
        {
            Serializer.Serialize(ref null, ResourceType.ThingRecording); // recording
            Serializer.Serialize(ref temp_int); // playHead
            if (version < 0x3c4) Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int); // type
            Serializer.Serialize(ref temp_int); // dir
            Serializer.Serialize(ref null); // prevDesiredPos
            Serializer.Serialize(ref temp_int); // prevDesiredPosSet
            Serializer.Serialize(ref null); // startOrientation
            Serializer.Serialize(ref temp_int); // speed
            if (version > 0x3c4)
                Serializer.Serialize(ref temp_int); // pathIsAbsolute
        }

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            if (vita >= 0x11) // 0x3c0
                Serializer.Serialize(ref UsePanel);
            if (vita is >= 0x11 and < 0x4b) Serializer.Serialize(ref temp_int); // 0x3c0
            if (vita >= 0x1f) // 0x3d4
                Serializer.Serialize(ref FollowType);
            if (vita >= 0x4b)
                Serializer.Serialize(ref FollowerDeceleration);
            if (vita >= 0x55)
                Serializer.Serialize(ref PlayerFilter);
        }

        if (subVersion is >= 0x1 and < 0x17)
        {
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
        }

        if (subVersion is >= 0x5 and < 0x17)
        {
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
        }

        // attract-o-gel, ill figure out the fields later
        // attractorData
        if (Configuration == 0xe && version > 0x3e3)
        {
            Serializer.Serialize(ref temp_int); // attractDistance
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int); // visualEffectBrightness_On
            Serializer.Serialize(ref temp_int); // visualEffectSpeed_On
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int); // visualEffectBrightnessOff
            Serializer.Serialize(ref temp_int); // visualEffectSpeed_Off
            Serializer.Serialize(ref temp_int); // attractorManualDetach
            Serializer.Serialize(ref temp_int); // attractorSoundEffects
            Serializer.Serialize(ref temp_int); // attractorVisualEffect
            Serializer.Serialize(ref temp_int); // attractorAffectConnected
        }

        if (subVersion > 0x56)
        {
            Serializer.Serialize(ref GridSnap);
            Serializer.Serialize(ref GridStrength);
        }

        if (subVersion > 0x99)
            Serializer.Serialize(ref GridGoalW);

        if (subVersion > 0x98)
            Serializer.Serialize(ref GridGoal);
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}