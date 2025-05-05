using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;

namespace CwLibNet.Structs.Things.Parts;

public class PEnemy: ISerializable
{
    public const int BASE_ALLOCATION_SIZE = 0x100;


    public EnemyPart partType = EnemyPart.LEG;

    
    public float radius;

    
    public int snapVertex;

    
    public Vector3? centerOffset;

    
    public Thing animThing;

    
    public float animSpeed;

    
    public int sourcePlayerNumber;

    
    public bool newWalkConstraintMass;

    
    public int smokeColor;

    
    public float smokeBrightness;

    /* Vita */
    
    public bool touchSensitive;
    
    public int touchType;

    
    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();

        if (version >= 0x15d)
            partType = serializer.Enum32(partType, true);

        if (version > 0x15c && version < 0x19f)
            serializer.Thing(null);

        if (version >= 0x16d)
            radius = serializer.F32(radius);

        switch (version)
        {
            case < 0x19e and < 0x1a9:
                serializer.Thing(null);
                break;
            case >= 0x19f:
                snapVertex = serializer.I32(snapVertex);
                break;
        }

        if (version >= 0x1a9)
        {
            centerOffset = serializer.V3(centerOffset);
            animThing = serializer.Thing(animThing);
            animSpeed = serializer.F32(animSpeed);
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
                sourcePlayerNumber = serializer.I32(sourcePlayerNumber);
                break;
        }

        if (version >= 0x265)
            newWalkConstraintMass = serializer.Bool(newWalkConstraintMass);
        if (version >= 0x31e)
            smokeColor = serializer.I32(smokeColor);
        if (version >= 0x39a)
            smokeBrightness = serializer.F32(smokeBrightness);

        if (!revision.IsVita()) return; // 0x3c0
        int vita = revision.GetBranchRevision();
        if (vita >= 0x2)
            touchSensitive = serializer.Bool(touchSensitive);
        if (vita >= 0x3)
            touchType = serializer.I32(touchType);
    }

    
    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }


}