using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Things.Components;
using CwLibNet4Hub.Structs.Things.Components.Switches;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (version < 0x368)
            Serializer.Serialize(ref PosVel);
        Serializer.Serialize(ref AngVel);

        if (version < 0x137)
        {
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int);
        }

        Serializer.Serialize(ref Frequency);
        Serializer.Serialize(ref Phase);
        Serializer.Serialize(ref Lifetime);

        if (version >= 0x2fe && subVersion < 0x65)
            Serializer.Serialize(ref RecycleEmittedObjects);
        if (subVersion > 0x64)
            Serializer.Serialize(ref RecycleEmittedObjects); // ???

        if (version < 0x160)
            Serializer.Serialize(ref Thing);
        else
            Serializer.Serialize(ref Plan, ResourceType.Plan, false, true, false);

        Serializer.Serialize(ref MaxEmitted);
        switch (version)
        {
            case >= 0x1c8:
                Serializer.Serialize(ref MaxEmittedAtOnce);
                break;
            case < 0x137:
                Serializer.Serialize(ref temp_bool_false);
                break;
        }

        Serializer.Serialize(ref SpeedScaleStartFrame);
        Serializer.Serialize(ref SpeedScaleDeltaFrames);
        Serializer.Serialize(ref SpeedScale);

        if (version < 0x2c4)
            Serializer.Serialize(ref LastUpdateFrame);
        else if (!Serializer.IsWriting())
            LastUpdateFrame = SpeedScaleStartFrame + SpeedScaleDeltaFrames;

        if (version >= 0x137)
        {
            switch (version)
            {
                case < 0x314:
                    Serializer.Serialize(ref WorldOffset);
                    Serializer.Serialize(ref WorldRotation);
                    break;
                case >= 0x38e:
                    Serializer.Serialize(ref WorldRotationForEditorEmitters);
                    break;
            }

            Serializer.Serialize(ref EmitScale);
            Serializer.Serialize(ref LinearVel);
            if (version < 0x314)
                Serializer.Serialize(ref ParentThing);
        }

        if (version >= 0x13f)
            Serializer.Serialize(ref CurrentEmitted);

        if (version >= 0x144 && subVersion < 0x64)
            Serializer.Serialize(ref EmitFlip);
        if (subVersion > 0x64)
            Serializer.Serialize(ref EmitFlip); // ???

        if (version >= 0x1ce)
        {
            Serializer.Serialize(ref ParentRelativeOffset);
            Serializer.Serialize(ref ParentRelativeRotation);
            Serializer.Serialize(ref WorldZ);
            Serializer.Serialize(ref ZOffset);
            Serializer.Serialize(ref EmitFrontZ);
            Serializer.Serialize(ref EmitBackZ);
        }

        if (version >= 0x226 && subVersion < 0x64)
            Serializer.Serialize(ref HideInPlayMode);
        if (subVersion > 0x64)
            Serializer.Serialize(ref HideInPlayMode); // Literally what
        // is the point

        switch (version)
        {
            case >= 0x230 and < 0x2c4:
                Serializer.Serialize(ref ModScaleActive);
                break;
            case >= 0x2c4:
                Serializer.Serialize(ref Behavior);
                break;
        }

        if (version >= 0x308)
        {
            if (version >= 0x38d)
            {
                Serializer.Serialize(ref EffectCreate);
                Serializer.Serialize(ref EffectDestroy);
            }
            else
            {
                EffectCreate = serializer.I8(EffectCreate);
                EffectDestroy = serializer.I8(EffectDestroy);
            }
        }

        if (version >= 0x32e && subVersion < 0x64)
            Serializer.Serialize(ref IgnoreParentsVelocity);
        if (subVersion > 0x64)
            Serializer.Serialize(ref IgnoreParentsVelocity); //
        // what the hell is happening here

        if (version >= 0x340)
            Serializer.Serialize(ref EmittedObjectSource);

        if (version >= 0x361 && subVersion < 0x64)
            Serializer.Serialize(ref EditorEmitter);
        if (subVersion > 0x64)
            Serializer.Serialize(ref EditorEmitter); // did every bool
        // just die or something wtf

        if (version >= 0x38d && subVersion < 0x64)
            Serializer.Serialize(ref IsLimboFlippedForGunEmitter);
        if (subVersion > 0x64)
            Serializer.Serialize(ref IsLimboFlippedForGunEmitter); // my god

        // they renamed this field to backupZOffset in Vita, not fans of swears? :fearful:
        if (version >= 0x3ae)
            Serializer.Serialize(ref TheFuckingZOffset);

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            if (vita >= 0xf)
                Serializer.Serialize(ref EmitNode);
            if (vita >= 0x2f)
                Serializer.Serialize(ref CleanUpOnDestroyed);
        }

        switch (subVersion)
        {
            case >= 0x18e:
                Serializer.Serialize(ref JustUseTheFuckingZOffset);
                break;
            case >= 0x31 and < 0x65:
                Serializer.Serialize(ref SoundEnabled);
                break;
        }

        if (subVersion >= 0x65)
            Serializer.Serialize(ref SoundEnabled); // seriously, for what
        // reason?

        if (subVersion is >= 0x41 and < 0x1a7)
            Serializer.Serialize(ref temp_bool_false);

        if (subVersion > 0x64)
            Serializer.Serialize(ref EmitByReferenceInPlayMode);
        if (subVersion > 0x75)
            Serializer.Serialize(ref EmitToNearestRearLayer);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}