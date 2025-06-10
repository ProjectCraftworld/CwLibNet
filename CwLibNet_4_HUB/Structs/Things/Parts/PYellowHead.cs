using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.EX;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Things.Components;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using CwLibNet4Hub.Types.Data;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

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
    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        Head = Serializer.SerializeReference(Head);
        if (version > 0x1fc)
            LegacyToolTetherJoint = Serializer.SerializeReference(LegacyToolTetherJoint);
        else
        {
            int tempZero = 0;
            Serializer.Serialize(ref tempZero);
        }

        if (version < 0x13c) 
        {
            ResourceDescriptor? tempNull = null;
            Serializer.Serialize(ref tempNull, ResourceType.FileOfBytes);
        }
        if (version < 0x8c) 
        {
            int tempZero = 0;
            Serializer.Serialize(ref tempZero);
        }
        if (version < 0x1d6) 
        {
            ResourceDescriptor? tempNull = null;
            Serializer.Serialize(ref tempNull, ResourceType.GfxMaterial, false, false, false);
        }

        Serializer.Serialize(ref LegacyToolTetherWidth);
        Serializer.Serialize(ref PlayerNumber);

        if (version < 0x155) 
        {
            int tempZero = 0;
            Serializer.Serialize(ref tempZero);
        }
        if (version < 0x20c) 
        {
            bool tempFalse = false;
            Serializer.Serialize(ref tempFalse);
        }
        switch (version)
        {
            case < 0x161:
            case 0x170:
                bool tempFalse = false;
                Serializer.Serialize(ref tempFalse);
                break;
        }

        if (version is < 0x17f or > 0x184 and < 0x192 or > 0x1b5)
            Serializer.Serialize(ref Poppet);

        if (subVersion is >= 0xc and < 0x66)
            throw new SerializationException("Unknown serialization object in PYellowHead!");

        if (version < 0x3da)
            Serializer.Serialize(ref SensorHistory);

        if (version < 0x203) 
        {
            float tempZeroFloat = 0.0f;
            Serializer.Serialize(ref tempZeroFloat);
        }

        if (version < 0x3da)
            Serializer.Serialize(ref NewestSense);

        switch (version)
        {
            case >= 0x146 and < 0x16b:
                int tempZero = 0;
                Serializer.Serialize(ref tempZero);
                break;
            case >= 0x16b:
                Serializer.Serialize(ref RequestedSuicide);
                break;
        }

        LegacyJetpack = Serializer.SerializeReference(LegacyJetpack);

        switch (version)
        {
            case > 0x134 and < 0x164:
                int tempPrimary = 0;
                int tempSecondary = 0;
                int tempTertiary = 0;
                Serializer.Serialize(ref tempPrimary); // Primary
                Serializer.Serialize(ref tempSecondary); // Secondary
                Serializer.Serialize(ref tempTertiary); // Tertiary
                break;
            case > 0x193:
                Serializer.Serialize(ref OnScreenCounter);
                break;
        }

        if (version is > 0x1ba and < 0x1fd)
        {
            int tempZero = 0;
            Serializer.Serialize(ref tempZero);
        }

        if (version > 0x1d0)
            Serializer.Serialize(ref OnScreenStatus);

        if (version > 0x1d3)
            Serializer.Serialize(ref EditJetpack);

        if (version > 0x272)
        {
            if (version < 0x2df)
                Serializer.Serialize(ref Recording);
            Recordee = Serializer.SerializeReference(Recordee);
        }

        if (version > 0x359)
            Serializer.Serialize(ref LastTimeSlappedAPlayer);

        if (subVersion is >= 0x83 and < 0x8b) 
        {
            int tempZero = 0;
            Serializer.Serialize(ref tempZero);
        }
        AnimSetKey = subVersion switch
        {
            >= 0x88 and < 0xa3 => throw new SerializationException("Unknown serialization object in PYellowHead!"),
            > 0xa5 => AnimSetKey,
            _ => AnimSetKey
        };
        
        if (subVersion > 0xa5)
        {
            Serializer.Serialize(ref AnimSetKey);
        }

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