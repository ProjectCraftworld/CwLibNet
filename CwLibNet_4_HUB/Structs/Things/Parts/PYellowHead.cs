using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.Structs.Things.Components;
using static net.torutheredfox.craftworld.serialization.Serializer;

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
    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();
        var subVersion = Serializer.GetRevision().GetSubVersion();

        Head = Serializer.Reference(Head);
        if (version > 0x1fc)
            LegacyToolTetherJoint = Serializer.Reference(LegacyToolTetherJoint);
        else
            Serializer.Serialize(ref 0);

        if (version < 0x13c) Serializer.Serialize(ref null);
        if (version < 0x8c) Serializer.Serialize(ref 0);
        if (version < 0x1d6) Serializer.Serialize(ref null, ResourceType.GfxMaterial);

        Serializer.Serialize(ref LegacyToolTetherWidth);
        Serializer.Serialize(ref PlayerNumber);

        if (version < 0x155) Serializer.Serialize(ref 0);
        if (version < 0x20c) Serializer.Serialize(ref false);
        switch (version)
        {
            case < 0x161:
            case 0x170:
                Serializer.Serialize(ref false);
                break;
        }

        if (version is < 0x17f or > 0x184 and < 0x192 or > 0x1b5)
            Serializer.Serialize(ref Poppet);

        if (subVersion is >= 0xc and < 0x66)
            throw new SerializationException("Unknown serialization object in PYellowHead!");

        if (version < 0x3da)
            Serializer.Serialize(ref SensorHistory);

        if (version < 0x203) Serializer.Serialize(ref 0.0f);

        if (version < 0x3da)
            Serializer.Serialize(ref NewestSense);

        switch (version)
        {
            case >= 0x146 and < 0x16b:
                Serializer.Serialize(ref 0);
                break;
            case >= 0x16b:
                Serializer.Serialize(ref RequestedSuicide);
                break;
        }

        LegacyJetpack = Serializer.Reference(LegacyJetpack);

        switch (version)
        {
            case > 0x134 and < 0x164:
                Serializer.Serialize(ref 0); // Primary
                Serializer.Serialize(ref 0); // Secondary
                Serializer.Serialize(ref 0); // Tertiary
                break;
            case > 0x193:
                Serializer.Serialize(ref OnScreenCounter);
                break;
        }

        if (version is > 0x1ba and < 0x1fd)
            Serializer.Serialize(ref 0);

        if (version > 0x1d0)
            Serializer.Serialize(ref OnScreenStatus);

        if (version > 0x1d3)
            Serializer.Serialize(ref EditJetpack);

        if (version > 0x272)
        {
            if (version < 0x2df)
                Serializer.Serialize(ref Recording);
            Recordee = Serializer.Reference(Recordee);
        }

        if (version > 0x359)
            Serializer.Serialize(ref LastTimeSlappedAPlayer);

        if (subVersion is >= 0x83 and < 0x8b) Serializer.Serialize(ref 0);
        AnimSetKey = subVersion switch
        {
            >= 0x88 and < 0xa3 => throw new SerializationException("Unknown serialization object in PYellowHead!"),
            > 0xa5 => Serializer.Serialize(ref AnimSetKey),
            _ => AnimSetKey
        };

        if (subVersion > 0xd2)
            Serializer.Serialize(ref MonstrousHeadScale);

        if (subVersion <= 0x12a) return;
        Serializer.Serialize(ref CreatureToSpawnAs);
        Serializer.Serialize(ref SpawnAsAlternateForm);
    }

    public int GetAllocatedSize()
    {
        return 0;
    }
}