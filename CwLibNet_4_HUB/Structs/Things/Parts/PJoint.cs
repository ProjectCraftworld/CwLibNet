using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

public class PJoint: ISerializable
{
    public const int BaseAllocationSize = 0x100;

    public Thing? A;
    public Thing? B;
    public Vector3? AContact;
    public Vector3? BContact;
    public float Length = 1.01f, Angle, OffsetTime;
    public bool InvertAngle;
    public ResourceDescriptor? Settings;
    public int[]? BoneIdx = [-1, -1];
    public Vector4? BoneLengths = new Vector4(0.07920773f, 0.07920773f, 0.07920773f, 1.0f);
    public int Type = 10;
    public float Strength = 0.027f;

    public bool Stiff;
    public Vector3? SlideDir;

    public int AnimationPattern;
    public float AnimationRange = 1.0f, AnimationTime = 60.0f,
        AnimationPhase, AnimationSpeed, AnimationPause;
    public float AAngleOffset, BAngleOffset;
    public float ModStartFrames = -1.0f, ModDeltaFrames, ModScale = 1.0f;

    public bool ModDriven;

    public byte InteractPlayMode, InteractEditMode = 2;

    public float RenderScale = 1.0f;

    public int JointSoundEnum = 1;

    public float TweakTargetMaxLength, TweakTargetMinLength;

    public bool CurrentlyEditing;
    public bool ModScaleActive;

    public bool HideInPlayMode;

    public int Behaviour;

    public Vector3[]? RailKnotVector;

    public byte RailInteractions;

    public bool CanBeTweakedByPoppetPowerup, CreatedByPoppetPowerup;

    public bool OldJointOutputBehavior;
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Serializer.Serialize(ref A);
        Serializer.Serialize(ref B);
        Serializer.Serialize(ref AContact);
        Serializer.Serialize(ref BContact);
        Serializer.Serialize(ref Length);
        Serializer.Serialize(ref Angle);
        Serializer.Serialize(ref OffsetTime);
        Serializer.Serialize(ref InvertAngle);
        Serializer.Serialize(ref Settings, ResourceType.Joint, false, true, false);
        Serializer.Serialize(ref BoneIdx);
        Serializer.Serialize(ref BoneLengths);
        Serializer.Serialize(ref Type);
        Serializer.Serialize(ref Strength);

        if (version < 0x18d)
            Serializer.Serialize(ref temp_int); // Unknown
        else
        {
            Serializer.Serialize(ref Stiff);
            Serializer.Serialize(ref SlideDir);
        }

        Serializer.Serialize(ref AnimationPattern);
        Serializer.Serialize(ref AnimationRange);
        Serializer.Serialize(ref AnimationTime);
        Serializer.Serialize(ref AnimationPhase);
        Serializer.Serialize(ref AnimationSpeed);
        Serializer.Serialize(ref AnimationPause);

        Serializer.Serialize(ref AAngleOffset);
        Serializer.Serialize(ref BAngleOffset);

        Serializer.Serialize(ref ModStartFrames);
        Serializer.Serialize(ref ModDeltaFrames);
        Serializer.Serialize(ref ModScale);

        if (version < 0x2c4)
            Serializer.Serialize(ref ModDriven);

        if (version < 0x307)
        {
            Serializer.Serialize(ref InteractPlayMode);
            Serializer.Serialize(ref InteractEditMode);
        }

        Serializer.Serialize(ref RenderScale);

        if (version > 0x169)
            Serializer.Serialize(ref JointSoundEnum);

        switch (version)
        {
            case > 0x280:
                Serializer.Serialize(ref TweakTargetMaxLength);
                Serializer.Serialize(ref TweakTargetMinLength);
                break;
            case > 0x21c:
                var maxLengthInt = (int)Math.Round(TweakTargetMaxLength);
                var minLengthInt = (int)Math.Round(TweakTargetMinLength);
                Serializer.Serialize(ref maxLengthInt);
                Serializer.Serialize(ref minLengthInt);
                TweakTargetMaxLength = maxLengthInt;
                TweakTargetMinLength = minLengthInt;
                break;
        }

        if (version > 0x21e)
            Serializer.Serialize(ref CurrentlyEditing);

        if (version is > 0x22f and < 0x2c4)
            Serializer.Serialize(ref ModScaleActive);

        if (version > 0x25c)
            Serializer.Serialize(ref HideInPlayMode);

        if (version > 0x2c3)
            Serializer.Serialize(ref Behaviour);

        if (subVersion >= 0xed)
        {
            if (Serializer.IsWriting())
            {
                var stream = Serializer.GetCurrentSerializer().GetOutput();
                if (RailKnotVector != null)
                {
                    stream.I32(RailKnotVector.Length);
                    for (int i = 0; i < RailKnotVector.Length; i++)
                    {
                        Vector3? vector = RailKnotVector[i];
                        Serializer.Serialize(ref vector);
                        RailKnotVector[i] = vector ?? Vector3.Zero;
                    }
                }
                else stream.I32(0);
            }
            else
            {
                var stream = Serializer.GetCurrentSerializer().GetInput();
                RailKnotVector = new Vector3[stream.I32()];
                for (var i = 0; i < RailKnotVector.Length; ++i)
                {
                    Vector3? vector = null;
                    Serializer.Serialize(ref vector);
                    RailKnotVector[i] = vector ?? Vector3.Zero;
                }
            }
        }

        if (subVersion >= 0x197)
            Serializer.Serialize(ref RailInteractions);

        if (subVersion >= 0x19f)
        {
            Serializer.Serialize(ref CanBeTweakedByPoppetPowerup);
            Serializer.Serialize(ref CreatedByPoppetPowerup);
        }

        if (subVersion >= 0x1a4)
            Serializer.Serialize(ref OldJointOutputBehavior);
    }
    
    public PJoint() { }

    public PJoint(Thing? a, Thing? b)
    {
        A = a;
        B = b;
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (RailKnotVector != null) size += RailKnotVector.Length * 0xC;
        if (BoneIdx != null) size += BoneIdx.Length * 0x4;
        return size;
    }
}