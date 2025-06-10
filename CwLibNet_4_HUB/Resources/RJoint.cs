using CwLibNet4Hub.Structs.Joint;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Enums;

using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Resources;

public class RJoint: Resource
{
    public const int BASE_ALLOCATION_SIZE = 0x80 + FCurve.BaseAllocationSize * 0x2;

    public bool allowExpand, allowContract, contractFreely;
    public float lengthElasticity, lengthPlasticity;
    public FCurve lengthFunc = new();
    public float angleDeviation, angleElasticity, anglePlasticity = 0.01f;
    public float angleVelocity;
    public FCurve angleFunc = new();
    public bool normalizedForces = true, dontRotateA, dontRotateB;
    public float breakResistance;
    public ResourceDescriptor gfxMaterial;
    public float gfxWidth = 20.0f;
    public string eventNameAngle, eventNameLength;
    public ResourceDescriptor mesh;

    
    public override void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref allowExpand);
        Serializer.Serialize(ref allowContract);
        Serializer.Serialize(ref contractFreely);

        Serializer.Serialize(ref lengthElasticity);
        Serializer.Serialize(ref lengthPlasticity);

        Serializer.Serialize(ref lengthFunc);

        Serializer.Serialize(ref angleDeviation);
        Serializer.Serialize(ref angleElasticity);
        Serializer.Serialize(ref anglePlasticity);
        Serializer.Serialize(ref angleVelocity);

        Serializer.Serialize(ref angleFunc);

        Serializer.Serialize(ref normalizedForces);
        Serializer.Serialize(ref dontRotateA);
        Serializer.Serialize(ref dontRotateB);

        Serializer.Serialize(ref breakResistance);

        Serializer.Serialize(ref gfxMaterial, ResourceType.GfxMaterial, true, false, false);

        Serializer.Serialize(ref gfxWidth);

        Serializer.Serialize(ref eventNameAngle);
        Serializer.Serialize(ref eventNameLength);

        Serializer.Serialize(ref mesh, ResourceType.Mesh, true, false, false);
    }

    
    public override int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE + eventNameAngle.Length + eventNameLength.Length;
    }

    
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        var serializer = new Serializer(GetAllocatedSize(), revision,
            compressionFlags);
        Serializer.SetCurrentSerializer(serializer);
        Serialize(serializer);
        return new SerializationData(
            Serializer.GetCurrentSerializer().GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Joint,
            SerializationType.BINARY,
            Serializer.GetCurrentSerializer().GetDependencies()
        );
    }


}