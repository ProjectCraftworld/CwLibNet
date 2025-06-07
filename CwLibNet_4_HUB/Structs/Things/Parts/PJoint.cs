using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

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
    public void Serialize()
    {
        var revision = Serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Serializer.Serialize(ref A);
        Serializer.Serialize(ref B);
        AContact = Serializer.Serialize(ref AContact);
        BContact = Serializer.Serialize(ref BContact);
        Serializer.Serialize(ref Length);
        Serializer.Serialize(ref Angle);
        Serializer.Serialize(ref OffsetTime);
        Serializer.Serialize(ref InvertAngle);
        Serializer.Serialize(ref Settings, Settings, ResourceType.Joint);
        Serializer.Serialize(ref BoneIdx);
        Serializer.Serialize(ref BoneLengths);
        Serializer.Serialize(ref Type);
        Serializer.Serialize(ref Strength);

        if (version < 0x18d)
            Serializer.Serialize(ref 0); // Unknown
        else
        {
            Serializer.Serialize(ref Stiff);
            SlideDir = Serializer.Serialize(ref SlideDir);
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
                TweakTargetMaxLength = Serializer.Serialize(ref (int)Math.Round(TweakTargetMaxLength));
                TweakTargetMinLength = Serializer.Serialize(ref (int)Math.Round(TweakTargetMinLength));
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
                var stream = Serializer.GetOutput();
                if (RailKnotVector != null)
                {
                    stream.I32(RailKnotVector.Length);
                    foreach (var vector in RailKnotVector)
                        Serializer.Serialize(ref vector);
                }
                else stream.I32(0);
            }
            else
            {
                var stream = Serializer.GetInput();
                RailKnotVector = new Vector3[stream.I32()];
                for (var i = 0; i < RailKnotVector.Length; ++i)
                    RailKnotVector[i] = stream.V3();
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