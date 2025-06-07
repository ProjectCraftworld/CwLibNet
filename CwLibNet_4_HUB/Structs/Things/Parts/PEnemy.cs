using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
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

    
    public void Serialize()
    {
        var revision = Serializer.GetRevision();
        var version = revision.GetVersion();

        if (version >= 0x15d)
            Serializer.Serialize(ref PartType);

        if (version is > 0x15c and < 0x19f)
            Serializer.Reference(null);

        if (version >= 0x16d)
            Serializer.Serialize(ref Radius);

        switch (version)
        {
            case < 0x19e and < 0x1a9:
                Serializer.Reference(null);
                break;
            case >= 0x19f:
                Serializer.Serialize(ref SnapVertex);
                break;
        }

        if (version >= 0x1a9)
        {
            CenterOffset = Serializer.Serialize(ref CenterOffset);
            AnimThing = Serializer.Reference(AnimThing);
            Serializer.Serialize(ref AnimSpeed);
        }

        switch (version)
        {
            case > 0x1f1 and < 0x21e:
                Serializer.Reference(null);
                Serializer.Reference(null);
                Serializer.Reference(null);
                Serializer.Reference(null);
                Serializer.Serialize(ref 0);
                Serializer.Reference(null);
                Serializer.Reference(null);
                Serializer.Serialize(ref null);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref null);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                break;
            case >= 0x246:
                Serializer.Serialize(ref SourcePlayerNumber);
                break;
        }

        if (version >= 0x265)
            Serializer.Serialize(ref NewWalkConstraintMass);
        if (version >= 0x31e)
            Serializer.Serialize(ref SmokeColor);
        if (version >= 0x39a)
            Serializer.Serialize(ref SmokeBrightness);

        if (!revision.IsVita()) return; // 0x3c0
        int vita = revision.GetBranchRevision();
        if (vita >= 0x2)
            Serializer.Serialize(ref TouchSensitive);
        if (vita >= 0x3)
            Serializer.Serialize(ref TouchType);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}