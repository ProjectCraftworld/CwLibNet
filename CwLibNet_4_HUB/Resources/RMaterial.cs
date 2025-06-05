using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

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


    public override void Serialize(Serializer serializer)
    {

        var head = serializer.GetRevision().GetVersion();

        traction = serializer.F32(traction);
        density = serializer.F32(density);

        if (head < 0x13c)
        {
            field000 = serializer.I8(field000);
            field001 = serializer.I8(field001);
        }

        // if (head >= 0x292)
        restitution = serializer.F32(restitution);
        // else
        //     serializer.F32(0); // Is this supposed to be restitution still?

        slidingFriction = serializer.F32(slidingFriction);

        if (head < 0x13c)
            field002 = serializer.F32(field002);

        rollingFriction = serializer.F32(rollingFriction);

        if (head < 0x13c)
        {
            field003 = serializer.I8(field003);
            field004 = serializer.I8(field004);
            field005 = serializer.I8(field005);
            field006 = serializer.I8(field006);
            field007 = serializer.I8(field007);
        }

        soundEnum = serializer.I32(soundEnum);

        grabbable = serializer.Bool(grabbable);

        if (head < 0x13c)
        {
            field008 = serializer.I8(field008);
            field009 = serializer.I8(field009);
        }

        gravityMultiplier = serializer.F32(gravityMultiplier);
        airResistanceMultiplier = serializer.F32(airResistanceMultiplier);

        if (head < 0x167)
            field010 = serializer.F32(field010); // breakResistance?

        limb = serializer.Bool(limb);

        if (head < 0x1d3)
            field011 = serializer.I8(field011); // creativeZone?

        shiftGrip = serializer.Bool(shiftGrip);

        collideType = serializer.I32(collideType);
        collideIgnore = serializer.I32(collideIgnore);

        dissolvable = serializer.Bool(dissolvable);
        explodable = serializer.Bool(explodable);
        cuttableByExplosion = serializer.Bool(cuttableByExplosion);

        if (head >= 0x167)
        {
            breakable = serializer.Bool(breakable);
            breakMinVel = serializer.F32(breakMinVel);
            breakMinForce = serializer.F32(breakMinForce);

            explosionMinRadius = serializer.F32(explosionMinRadius);
            explosionMaxRadius = serializer.F32(explosionMaxRadius);
            explosionMaxVel = serializer.F32(explosionMaxVel);
            if (head >= 0x168)
                explosionMaxAngVel = serializer.F32(explosionMaxAngVel);
            explosionMaxForce = serializer.F32(explosionMaxForce);
        }

        if (head >= 0x13c)
            maxForce = serializer.F32(maxForce);

        if (head >= 0x244)
            bullet = serializer.Bool(bullet);

        if (head >= 0x27b)
        {
            circuitBoard = serializer.Bool(circuitBoard);
            disableCSG = serializer.Bool(disableCSG);
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
        serializer.Struct(this);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Material,
            SerializationType.BINARY,
            serializer.GetDependencies()
        );
    }
}