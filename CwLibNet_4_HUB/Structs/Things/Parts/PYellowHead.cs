using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components;

namespace CwLibNet.Structs.Things.Parts;

public class PYellowHead: ISerializable
{
    public Thing? Head;
    public Thing? LegacyToolTetherJoint;
    public float LegacyToolTetherWidth;
    public int PlayerNumber;
    public Poppet? Poppet;

    public Vector4[]? SensorHistory;
    public int NewestSense;
    public bool RequestedSuicide;

    public Thing? LegacyJetpack;
    public float OnScreenCounter;
    public byte OnScreenStatus;
    public bool EditJetpack;
    public bool Recording;
    public Thing? Recordee;
    public int LastTimeSlappedAPlayer;
    public int AnimSetKey;
    public bool MonstrousHeadScale;
    public int CreatureToSpawnAs;
    public bool SpawnAsAlternateForm;
    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();
        var subVersion = serializer.GetRevision().GetSubVersion();

        Head = serializer.Thing(Head);
        if (version > 0x1fc)
            LegacyToolTetherJoint = serializer.Thing(LegacyToolTetherJoint);
        else
            serializer.I32(0);

        if (version < 0x13c) serializer.V4(null);
        if (version < 0x8c) serializer.I32(0);
        if (version < 0x1d6) serializer.Resource(null, ResourceType.GfxMaterial);

        LegacyToolTetherWidth = serializer.F32(LegacyToolTetherWidth);
        PlayerNumber = serializer.S32(PlayerNumber);

        if (version < 0x155) serializer.I32(0);
        if (version < 0x20c) serializer.Bool(false);
        switch (version)
        {
            case < 0x161:
            case 0x170:
                serializer.Bool(false);
                break;
        }

        if (version is < 0x17f or > 0x184 and < 0x192 or > 0x1b5)
            Poppet = serializer.Reference(Poppet);

        if (subVersion is >= 0xc and < 0x66)
            throw new SerializationException("Unknown serialization object in PYellowHead!");

        if (version < 0x3da)
            SensorHistory = serializer.Vectorarray(SensorHistory);

        if (version < 0x203) serializer.F32(0.0f);

        if (version < 0x3da)
            NewestSense = serializer.I32(NewestSense);

        switch (version)
        {
            case >= 0x146 and < 0x16b:
                serializer.I32(0);
                break;
            case >= 0x16b:
                RequestedSuicide = serializer.Bool(RequestedSuicide);
                break;
        }

        LegacyJetpack = serializer.Thing(LegacyJetpack);

        switch (version)
        {
            case > 0x134 and < 0x164:
                serializer.I32(0); // Primary
                serializer.I32(0); // Secondary
                serializer.I32(0); // Tertiary
                break;
            case > 0x193:
                OnScreenCounter = serializer.F32(OnScreenCounter);
                break;
        }

        if (version is > 0x1ba and < 0x1fd)
            serializer.I32(0);

        if (version > 0x1d0)
            OnScreenStatus = serializer.I8(OnScreenStatus);

        if (version > 0x1d3)
            EditJetpack = serializer.Bool(EditJetpack);

        if (version > 0x272)
        {
            if (version < 0x2df)
                Recording = serializer.Bool(Recording);
            Recordee = serializer.Thing(Recordee);
        }

        if (version > 0x359)
            LastTimeSlappedAPlayer = serializer.I32(LastTimeSlappedAPlayer);

        if (subVersion is >= 0x83 and < 0x8b) serializer.U8(0);
        AnimSetKey = subVersion switch
        {
            >= 0x88 and < 0xa3 => throw new SerializationException("Unknown serialization object in PYellowHead!"),
            > 0xa5 => serializer.I32(AnimSetKey),
            _ => AnimSetKey
        };

        if (subVersion > 0xd2)
            MonstrousHeadScale = serializer.Bool(MonstrousHeadScale);

        if (subVersion <= 0x12a) return;
        CreatureToSpawnAs = serializer.I32(CreatureToSpawnAs);
        SpawnAsAlternateForm = serializer.Bool(SpawnAsAlternateForm);
    }

    public int GetAllocatedSize()
    {
        return 0;
    }
}