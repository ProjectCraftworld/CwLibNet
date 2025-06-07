using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Resources;

public class RMaterial: Resource
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x60;

    public float traction = 1.0f, density = 20.0f;

    public byte field000, field001;
    public float restitution = 0.3f, slidingFriction = 0.8f;

    public float field002;
    public float rollingFriction = 1.8f;

    public byte field003, field004, field005, field006, field007;


    public int soundEnum;

    public bool grabbable;

    public byte field008, field009;
    public float gravityMultiplier = 1.0f, airResistanceMultiplier = 1.0f;

    public float field010;
    public bool limb;

    public byte field011;
    public bool shiftGrip;
    public int collideType = CollideType.NORMAL, collideIgnore = CollideType.NONE;
    public bool dissolvable, explodable, cuttableByExplosion = true;

    public bool breakable;

    public float breakMinVel, breakMinForce;

    public float explosionMinRadius, explosionMaxRadius, explosionMaxVel;

    public float explosionMaxAngVel;

    public float explosionMaxForce;

    public float maxForce = float.NaN;

    public bool bullet;

    public bool circuitBoard, disableCSG;


    public override void Serialize()
    {

        var head = Serializer.GetRevision().GetVersion();

        Serializer.Serialize(ref traction);
        Serializer.Serialize(ref density);

        if (head < 0x13c)
        {
            Serializer.Serialize(ref field000);
            Serializer.Serialize(ref field001);
        }

        // if (head >= 0x292)
        Serializer.Serialize(ref restitution);
        // else
        //     Serializer.Serialize(ref 0); // Is this supposed to be restitution still?

        Serializer.Serialize(ref slidingFriction);

        if (head < 0x13c)
            Serializer.Serialize(ref field002);

        Serializer.Serialize(ref rollingFriction);

        if (head < 0x13c)
        {
            Serializer.Serialize(ref field003);
            Serializer.Serialize(ref field004);
            Serializer.Serialize(ref field005);
            Serializer.Serialize(ref field006);
            Serializer.Serialize(ref field007);
        }

        Serializer.Serialize(ref soundEnum);

        Serializer.Serialize(ref grabbable);

        if (head < 0x13c)
        {
            Serializer.Serialize(ref field008);
            Serializer.Serialize(ref field009);
        }

        Serializer.Serialize(ref gravityMultiplier);
        Serializer.Serialize(ref airResistanceMultiplier);

        if (head < 0x167)
            Serializer.Serialize(ref field010); // breakResistance?

        Serializer.Serialize(ref limb);

        if (head < 0x1d3)
            Serializer.Serialize(ref field011); // creativeZone?

        Serializer.Serialize(ref shiftGrip);

        Serializer.Serialize(ref collideType);
        Serializer.Serialize(ref collideIgnore);

        Serializer.Serialize(ref dissolvable);
        Serializer.Serialize(ref explodable);
        Serializer.Serialize(ref cuttableByExplosion);

        if (head >= 0x167)
        {
            Serializer.Serialize(ref breakable);
            Serializer.Serialize(ref breakMinVel);
            Serializer.Serialize(ref breakMinForce);

            Serializer.Serialize(ref explosionMinRadius);
            Serializer.Serialize(ref explosionMaxRadius);
            Serializer.Serialize(ref explosionMaxVel);
            if (head >= 0x168)
                Serializer.Serialize(ref explosionMaxAngVel);
            Serializer.Serialize(ref explosionMaxForce);
        }

        if (head >= 0x13c)
            Serializer.Serialize(ref maxForce);

        if (head >= 0x244)
            Serializer.Serialize(ref bullet);

        if (head >= 0x27b)
        {
            Serializer.Serialize(ref circuitBoard);
            Serializer.Serialize(ref disableCSG);
        }
    }


    public override int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }


    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        Serializer serializer = new(GetAllocatedSize(), revision,
            compressionFlags);
        Serializer.Serialize(ref this);
        return new SerializationData(
            Serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Material,
            SerializationType.BINARY,
            Serializer.GetDependencies()
        );
    }
}