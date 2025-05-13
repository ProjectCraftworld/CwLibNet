using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;

namespace CwLibNet.Structs.Things.Parts;

public class PEnemy: ISerializable
{
    public const int BaseAllocationSize = 0x100;


    public EnemyPart PartType = EnemyPart.LEG;

    
    public float Radius;

    
    public int SnapVertex;

    
    public Vector3? CenterOffset;

    
    public Thing AnimThing;

    
    public float AnimSpeed;

    
    public int SourcePlayerNumber;

    
    public bool NewWalkConstraintMass;

    
    public int SmokeColor;

    
    public float SmokeBrightness;

    /* Vita */
    
    public bool TouchSensitive;
    
    public int TouchType;

    
    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();

        if (version >= 0x15d)
            PartType = serializer.Enum32(PartType, true);

        if (version > 0x15c && version < 0x19f)
            serializer.Thing(null);

        if (version >= 0x16d)
            Radius = serializer.F32(Radius);

        switch (version)
        {
            case < 0x19e and < 0x1a9:
                serializer.Thing(null);
                break;
            case >= 0x19f:
                SnapVertex = serializer.I32(SnapVertex);
                break;
        }

        if (version >= 0x1a9)
        {
            CenterOffset = serializer.V3(CenterOffset);
            AnimThing = serializer.Thing(AnimThing);
            AnimSpeed = serializer.F32(AnimSpeed);
        }

        switch (version)
        {
            case > 0x1f1 and < 0x21e:
                serializer.Thing(null);
                serializer.Thing(null);
                serializer.Thing(null);
                serializer.Thing(null);
                serializer.I32(0);
                serializer.Thing(null);
                serializer.Thing(null);
                serializer.V3(null);
                serializer.S32(0);
                serializer.V3(null);
                serializer.F32(0);
                serializer.F32(0);
                serializer.F32(0);
                serializer.S32(0);
                serializer.S32(0);
                break;
            case >= 0x246:
                SourcePlayerNumber = serializer.I32(SourcePlayerNumber);
                break;
        }

        if (version >= 0x265)
            NewWalkConstraintMass = serializer.Bool(NewWalkConstraintMass);
        if (version >= 0x31e)
            SmokeColor = serializer.I32(SmokeColor);
        if (version >= 0x39a)
            SmokeBrightness = serializer.F32(SmokeBrightness);

        if (!revision.IsVita()) return; // 0x3c0
        int vita = revision.GetBranchRevision();
        if (vita >= 0x2)
            TouchSensitive = serializer.Bool(TouchSensitive);
        if (vita >= 0x3)
            TouchType = serializer.I32(TouchType);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}