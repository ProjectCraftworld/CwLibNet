using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Structs.Things.Components.Switches;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

public class PEmitter: ISerializable
{
    public const int BaseAllocationSize = 0x120;


    public Vector3? PosVel;

    public float AngVel;
    public int Frequency, Phase, Lifetime;

    
    public bool RecycleEmittedObjects;

    
    
    public GlobalThingDescriptor Thing;

    
    
    public ResourceDescriptor Plan;

    public int MaxEmitted;

    
    public int MaxEmittedAtOnce = 1000;

    public float SpeedScaleStartFrame, SpeedScaleDeltaFrames;
    public SwitchSignal SpeedScale;

    
    public float LastUpdateFrame;

    
    public Vector4? WorldOffset;
    
    public float WorldRotation;

    
    public float WorldRotationForEditorEmitters;

    
    public float EmitScale, LinearVel;

    
    public Thing? ParentThing;

    
    public int CurrentEmitted;

    
    public bool EmitFlip;

    
    public Vector4? ParentRelativeOffset;
    
    public float ParentRelativeRotation;
    
    public float WorldZ, ZOffset, EmitFrontZ, EmitBackZ;

    
    public bool HideInPlayMode;


    
    
    public bool ModScaleActive = true;

    
    public int Behavior;

    
    public byte EffectCreate, EffectDestroy;

    
    public bool IgnoreParentsVelocity;

    
    public EmittedObjectSource EmittedObjectSource;

    
    public bool EditorEmitter;

    
    public bool IsLimboFlippedForGunEmitter;

    
    public float TheFuckingZOffset;

    
    public bool JustUseTheFuckingZOffset;

    
    public bool SoundEnabled;

    
    public bool EmitByReferenceInPlayMode;

    
    public bool EmitToNearestRearLayer;

    /* Vita */
    
    public byte EmitNode;
    
    public byte CleanUpOnDestroyed;

    
    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (version < 0x368)
            PosVel = serializer.V3(PosVel);
        AngVel = serializer.F32(AngVel);

        if (version < 0x137)
        {
            serializer.F32(0);
            serializer.F32(0);
        }

        Frequency = serializer.I32(Frequency);
        Phase = serializer.I32(Phase);
        Lifetime = serializer.I32(Lifetime);

        if (version >= 0x2fe && subVersion < 0x65)
            RecycleEmittedObjects = serializer.Bool(RecycleEmittedObjects);
        if (subVersion > 0x64)
            RecycleEmittedObjects = serializer.Bool(RecycleEmittedObjects); // ???

        if (version < 0x160)
            Thing = serializer.Struct(Thing);
        else
            Plan = serializer.Resource(Plan, ResourceType.Plan);

        MaxEmitted = serializer.I32(MaxEmitted);
        switch (version)
        {
            case >= 0x1c8:
                MaxEmittedAtOnce = serializer.I32(MaxEmittedAtOnce);
                break;
            case < 0x137:
                serializer.Bool(false);
                break;
        }

        SpeedScaleStartFrame = serializer.F32(SpeedScaleStartFrame);
        SpeedScaleDeltaFrames = serializer.F32(SpeedScaleDeltaFrames);
        SpeedScale = serializer.Struct(SpeedScale);

        if (version < 0x2c4)
            LastUpdateFrame = serializer.F32(LastUpdateFrame);
        else if (!serializer.IsWriting())
            LastUpdateFrame = SpeedScaleStartFrame + SpeedScaleDeltaFrames;

        if (version >= 0x137)
        {
            switch (version)
            {
                case < 0x314:
                    WorldOffset = serializer.V4(WorldOffset);
                    WorldRotation = serializer.F32(WorldRotation);
                    break;
                case >= 0x38e:
                    WorldRotationForEditorEmitters =
                        serializer.F32(WorldRotationForEditorEmitters);
                    break;
            }

            EmitScale = serializer.F32(EmitScale);
            LinearVel = serializer.F32(LinearVel);
            if (version < 0x314)
                ParentThing = serializer.Reference(ParentThing);
        }

        if (version >= 0x13f)
            CurrentEmitted = serializer.I32(CurrentEmitted);

        if (version >= 0x144 && subVersion < 0x64)
            EmitFlip = serializer.Bool(EmitFlip);
        if (subVersion > 0x64)
            EmitFlip = serializer.Bool(EmitFlip); // ???

        if (version >= 0x1ce)
        {
            ParentRelativeOffset = serializer.V4(ParentRelativeOffset);
            ParentRelativeRotation = serializer.F32(ParentRelativeRotation);
            WorldZ = serializer.F32(WorldZ);
            ZOffset = serializer.F32(ZOffset);
            EmitFrontZ = serializer.F32(EmitFrontZ);
            EmitBackZ = serializer.F32(EmitBackZ);
        }

        if (version >= 0x226 && subVersion < 0x64)
            HideInPlayMode = serializer.Bool(HideInPlayMode);
        if (subVersion > 0x64)
            HideInPlayMode = serializer.Bool(HideInPlayMode); // Literally what
        // is the point

        switch (version)
        {
            case >= 0x230 and < 0x2c4:
                ModScaleActive = serializer.Bool(ModScaleActive);
                break;
            case >= 0x2c4:
                Behavior = serializer.I32(Behavior);
                break;
        }

        if (version >= 0x308)
        {
            if (version >= 0x38d)
            {
                EffectCreate = serializer.I8(EffectCreate);
                EffectDestroy = serializer.I8(EffectDestroy);
            }
            else
            {
                EffectCreate = (byte) serializer.I32(EffectCreate);
                EffectDestroy = (byte) serializer.I32(EffectDestroy);
            }
        }

        if (version >= 0x32e && subVersion < 0x64)
            IgnoreParentsVelocity = serializer.Bool(IgnoreParentsVelocity);
        if (subVersion > 0x64)
            IgnoreParentsVelocity = serializer.Bool(IgnoreParentsVelocity); //
        // what the hell is happening here

        if (version >= 0x340)
            EmittedObjectSource = serializer.Reference(EmittedObjectSource);

        if (version >= 0x361 && subVersion < 0x64)
            EditorEmitter = serializer.Bool(EditorEmitter);
        if (subVersion > 0x64)
            EditorEmitter = serializer.Bool(EditorEmitter); // did every bool
        // just die or something wtf

        if (version >= 0x38d && subVersion < 0x64)
            IsLimboFlippedForGunEmitter =
                serializer.Bool(IsLimboFlippedForGunEmitter);
        if (subVersion > 0x64)
            IsLimboFlippedForGunEmitter =
                serializer.Bool(IsLimboFlippedForGunEmitter); // my god

        // they renamed this field to backupZOffset in Vita, not fans of swears? :fearful:
        if (version >= 0x3ae)
            TheFuckingZOffset = serializer.F32(TheFuckingZOffset);

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            if (vita >= 0xf)
                EmitNode = serializer.I8(EmitNode);
            if (vita >= 0x2f)
                CleanUpOnDestroyed = serializer.I8(CleanUpOnDestroyed);
        }

        switch (subVersion)
        {
            case >= 0x18e:
                JustUseTheFuckingZOffset = serializer.Bool(JustUseTheFuckingZOffset);
                break;
            case >= 0x31 and < 0x65:
                SoundEnabled = serializer.Bool(SoundEnabled);
                break;
        }

        if (subVersion >= 0x65)
            SoundEnabled = serializer.Bool(SoundEnabled); // seriously, for what
        // reason?

        if (subVersion is >= 0x41 and < 0x1a7)
            serializer.Bool(false);

        if (subVersion > 0x64)
            EmitByReferenceInPlayMode = serializer.Bool(EmitByReferenceInPlayMode);
        if (subVersion > 0x75)
            EmitToNearestRearLayer = serializer.Bool(EmitToNearestRearLayer);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}