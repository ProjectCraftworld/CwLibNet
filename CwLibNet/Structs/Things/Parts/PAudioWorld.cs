using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

public class PAudioWorld: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public string? SoundName;
    public float InitialVolume, InitialPitch, InitialParam1;
    public float MaxFalloff, ImpactTolerance;

    public bool TriggerBySwitch;

    public PlayMode PlayMode = PlayMode.TRIGGER_BY_FALLOFF;

    public bool ParamAffectVol;
    public bool ParamAffectPitch;
    public bool ParamAffectParam;

    public bool IsLocal;
    public bool HideInPlayMode;


    public int Behavior;

    public GUID? SoundNames;
    public int MeshColor;

    public int CategoryGuid;
    public bool ActivatedLastFrame;
    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();
        int subVersion = serializer.GetRevision().GetSubVersion();

        SoundName = serializer.Str(SoundName);

        InitialVolume = serializer.F32(InitialVolume);
        InitialPitch = serializer.F32(InitialPitch);
        InitialParam1 = serializer.F32(InitialParam1);

        MaxFalloff = serializer.F32(MaxFalloff);
        ImpactTolerance = serializer.F32(ImpactTolerance);

        if (version < 0x2c4)
        {
            bool triggerByFalloff = PlayMode == PlayMode.TRIGGER_BY_FALLOFF;
            bool triggerByImpact = PlayMode == PlayMode.TRIGGER_BY_IMPACT;
            bool triggerByDestroy = PlayMode == PlayMode.TRIGGER_BY_DESTROY;

            triggerByFalloff = serializer.Bool(triggerByFalloff);
            triggerByImpact = serializer.Bool(triggerByImpact);
            if (version < 0x165)
                serializer.Bool(false); // unk
            TriggerBySwitch = serializer.Bool(TriggerBySwitch);
            if (version < 0x165)
                serializer.Bool(false);
            if (version >= 0x1ad)
                triggerByDestroy = serializer.Bool(triggerByDestroy);

            if (!serializer.IsWriting())
            {
                if (triggerByFalloff) PlayMode = PlayMode.TRIGGER_BY_FALLOFF;
                if (triggerByImpact) PlayMode = PlayMode.TRIGGER_BY_IMPACT;
                if (triggerByDestroy) PlayMode = PlayMode.TRIGGER_BY_DESTROY;
            }
        }
        else PlayMode = serializer.Enum32(PlayMode);

        ParamAffectVol = serializer.Bool(ParamAffectVol);
        ParamAffectPitch = serializer.Bool(ParamAffectPitch);
        ParamAffectParam = serializer.Bool(ParamAffectParam);

        if (version >= 0x165)
            IsLocal = serializer.Bool(IsLocal);
        if (version >= 0x198)
            HideInPlayMode = serializer.Bool(HideInPlayMode);

        if (version >= 0x2c4)
            Behavior = serializer.I32(Behavior);

        if (version >= 0x380)
            SoundNames = serializer.Guid(SoundNames);

        if (version >= 0x380)
            MeshColor = serializer.I32(MeshColor);

        if (subVersion >= 0x178)
            CategoryGuid = serializer.I32(CategoryGuid);
        if (subVersion >= 0x191)
            ActivatedLastFrame = serializer.Bool(ActivatedLastFrame);
    }

    public int GetAllocatedSize()
    {
        int size = PAudioWorld.BaseAllocationSize;
        if (this.SoundName != null)
            size += (this.SoundName.Length);
        return size;
    }
}