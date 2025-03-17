using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using CwLibNet.Types.Things;

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

    public byte InteractPlayMode = 0, InteractEditMode = 2;

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
    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        A = serializer.Reference(A);
        B = serializer.Reference(B);
        AContact = serializer.V3(AContact);
        BContact = serializer.V3(BContact);
        Length = serializer.F32(Length);
        Angle = serializer.F32(Angle);
        OffsetTime = serializer.F32(OffsetTime);
        InvertAngle = serializer.Bool(InvertAngle);
        Settings = serializer.Resource(Settings, ResourceType.Joint);
        BoneIdx = serializer.Intarray(BoneIdx);
        BoneLengths = serializer.V4(BoneLengths);
        Type = serializer.I32(Type);
        Strength = serializer.F32(Strength);

        if (version < 0x18d)
            serializer.F32(0); // Unknown
        else
        {
            Stiff = serializer.Bool(Stiff);
            SlideDir = serializer.V3(SlideDir);
        }

        AnimationPattern = serializer.I32(AnimationPattern);
        AnimationRange = serializer.F32(AnimationRange);
        AnimationTime = serializer.F32(AnimationTime);
        AnimationPhase = serializer.F32(AnimationPhase);
        AnimationSpeed = serializer.F32(AnimationSpeed);
        AnimationPause = serializer.F32(AnimationPause);

        AAngleOffset = serializer.F32(AAngleOffset);
        BAngleOffset = serializer.F32(BAngleOffset);

        ModStartFrames = serializer.F32(ModStartFrames);
        ModDeltaFrames = serializer.F32(ModDeltaFrames);
        ModScale = serializer.F32(ModScale);

        if (version < 0x2c4)
            ModDriven = serializer.Bool(ModDriven);

        if (version < 0x307)
        {
            InteractPlayMode = serializer.I8(InteractPlayMode);
            InteractEditMode = serializer.I8(InteractEditMode);
        }

        RenderScale = serializer.F32(RenderScale);

        if (version > 0x169)
            JointSoundEnum = serializer.I32(JointSoundEnum);

        if (version > 0x280)
        {
            TweakTargetMaxLength = serializer.F32(TweakTargetMaxLength);
            TweakTargetMinLength = serializer.F32(TweakTargetMinLength);
        }
        else if (version > 0x21c)
        {
            TweakTargetMaxLength = serializer.I32((int)Math.Round(TweakTargetMaxLength));
            TweakTargetMinLength = serializer.I32((int)Math.Round(TweakTargetMinLength));
        }

        if (version > 0x21e)
            CurrentlyEditing = serializer.Bool(CurrentlyEditing);

        if (version > 0x22f && version < 0x2c4)
            ModScaleActive = serializer.Bool(ModScaleActive);

        if (version > 0x25c)
            HideInPlayMode = serializer.Bool(HideInPlayMode);

        if (version > 0x2c3)
            Behaviour = serializer.I32(Behaviour);

        if (subVersion >= 0xed)
        {
            if (serializer.IsWriting())
            {
                MemoryOutputStream stream = serializer.GetOutput();
                if (RailKnotVector != null)
                {
                    stream.I32(RailKnotVector.Length);
                    foreach (Vector3 vector in RailKnotVector)
                        serializer.V3(vector);
                }
                else stream.I32(0);
            }
            else
            {
                MemoryInputStream stream = serializer.GetInput();
                RailKnotVector = new Vector3[stream.I32()];
                for (int i = 0; i < RailKnotVector.Length; ++i)
                    RailKnotVector[i] = stream.V3();
            }
        }

        if (subVersion >= 0x197)
            RailInteractions = serializer.I8(RailInteractions);

        if (subVersion >= 0x19f)
        {
            CanBeTweakedByPoppetPowerup = serializer.Bool(CanBeTweakedByPoppetPowerup);
            CreatedByPoppetPowerup = serializer.Bool(CreatedByPoppetPowerup);
        }

        if (subVersion >= 0x1a4)
            OldJointOutputBehavior = serializer.Bool(OldJointOutputBehavior);
    }
    
    public PJoint() { }

    public PJoint(Thing? a, Thing? b)
    {
        this.A = a;
        this.B = b;
    }

    public int GetAllocatedSize()
    {
        int size = PJoint.BaseAllocationSize;
        if (this.RailKnotVector != null) size += (this.RailKnotVector.Length * 0xC);
        if (this.BoneIdx != null) size += (this.BoneIdx.Length * 0x4);
        return size;
    }
}