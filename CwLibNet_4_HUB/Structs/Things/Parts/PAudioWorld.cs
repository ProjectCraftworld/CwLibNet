using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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
    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();
        var subVersion = Serializer.GetRevision().GetSubVersion();

        Serializer.Serialize(ref SoundName);

        Serializer.Serialize(ref InitialVolume);
        Serializer.Serialize(ref InitialPitch);
        Serializer.Serialize(ref InitialParam1);

        Serializer.Serialize(ref MaxFalloff);
        Serializer.Serialize(ref ImpactTolerance);

        if (version < 0x2c4)
        {
            var triggerByFalloff = PlayMode == PlayMode.TRIGGER_BY_FALLOFF;
            var triggerByImpact = PlayMode == PlayMode.TRIGGER_BY_IMPACT;
            var triggerByDestroy = PlayMode == PlayMode.TRIGGER_BY_DESTROY;

            Serializer.Serialize(ref triggerByFalloff);
            Serializer.Serialize(ref triggerByImpact);
            if (version < 0x165)
                Serializer.Serialize(ref false); // unk
            Serializer.Serialize(ref TriggerBySwitch);
            switch (version)
            {
                case < 0x165:
                    Serializer.Serialize(ref false);
                    break;
                case >= 0x1ad:
                    Serializer.Serialize(ref triggerByDestroy);
                    break;
            }

            if (!Serializer.IsWriting())
            {
                if (triggerByFalloff) PlayMode = PlayMode.TRIGGER_BY_FALLOFF;
                if (triggerByImpact) PlayMode = PlayMode.TRIGGER_BY_IMPACT;
                if (triggerByDestroy) PlayMode = PlayMode.TRIGGER_BY_DESTROY;
            }
        }
        else Serializer.Serialize(ref PlayMode);

        Serializer.Serialize(ref ParamAffectVol);
        Serializer.Serialize(ref ParamAffectPitch);
        Serializer.Serialize(ref ParamAffectParam);

        if (version >= 0x165)
            Serializer.Serialize(ref IsLocal);
        if (version >= 0x198)
            Serializer.Serialize(ref HideInPlayMode);

        if (version >= 0x2c4)
            Serializer.Serialize(ref Behavior);

        if (version >= 0x380)
            SoundNames = Serializer.Serialize(ref SoundNames);

        if (version >= 0x380)
            Serializer.Serialize(ref MeshColor);

        if (subVersion >= 0x178)
            Serializer.Serialize(ref CategoryGuid);
        if (subVersion >= 0x191)
            Serializer.Serialize(ref ActivatedLastFrame);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (SoundName != null)
            size += SoundName.Length;
        return size;
    }
}