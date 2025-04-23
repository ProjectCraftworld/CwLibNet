using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Joint;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Resources;

public class RJoint: Resource
{
    public const int BASE_ALLOCATION_SIZE = 0x80 + (FCurve.BaseAllocationSize * 0x2);

    public bool allowExpand, allowContract, contractFreely;
    public float lengthElasticity, lengthPlasticity;
    public FCurve lengthFunc = new FCurve();
    public float angleDeviation, angleElasticity, anglePlasticity = 0.01f;
    public float angleVelocity = 0.0f;
    public FCurve angleFunc = new FCurve();
    public bool normalizedForces = true, dontRotateA, dontRotateB;
    public float breakResistance;
    public ResourceDescriptor gfxMaterial;
    public float gfxWidth = 20.0f;
    public String eventNameAngle, eventNameLength;
    public ResourceDescriptor mesh;

    
    public override void Serialize(Serializer serializer)
    {
        allowExpand = serializer.Bool(allowExpand);
        allowContract = serializer.Bool(allowContract);
        contractFreely = serializer.Bool(contractFreely);

        lengthElasticity = serializer.F32(lengthElasticity);
        lengthPlasticity = serializer.F32(lengthPlasticity);

        lengthFunc = serializer.Struct(lengthFunc);

        angleDeviation = serializer.F32(angleDeviation);
        angleElasticity = serializer.F32(angleElasticity);
        anglePlasticity = serializer.F32(anglePlasticity);
        angleVelocity = serializer.F32(angleVelocity);

        angleFunc = serializer.Struct(angleFunc);

        normalizedForces = serializer.Bool(normalizedForces);
        dontRotateA = serializer.Bool(dontRotateA);
        dontRotateB = serializer.Bool(dontRotateB);

        breakResistance = serializer.F32(breakResistance);

        gfxMaterial = serializer.Resource(gfxMaterial, ResourceType.GfxMaterial);

        gfxWidth = serializer.F32(gfxWidth);

        eventNameAngle = serializer.Str(eventNameAngle);
        eventNameLength = serializer.Str(eventNameLength);

        mesh = serializer.Resource(mesh, ResourceType.Mesh);
    }

    
    public override int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE + eventNameAngle.Length + eventNameLength.Length;
    }

    
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        Serializer serializer = new Serializer(this.GetAllocatedSize(), revision,
            compressionFlags);
        serializer.Struct(this);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Joint,
            SerializationType.BINARY,
            serializer.GetDependencies()
        );
    }


}