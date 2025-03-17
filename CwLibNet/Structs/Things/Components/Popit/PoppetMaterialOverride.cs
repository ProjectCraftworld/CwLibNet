using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Components.Popit;

public class PoppetMaterialOverride: ISerializable
{
    public const int BaseAllocationSize = 0xA0;

    public ResourceDescriptor? Plan;
    public ResourceDescriptor? GfxMaterial;
    public ResourceDescriptor? Bevel;
    public ResourceDescriptor? PhysicsMaterial;
    public float BevelSize;

    public int SoundEnum;

    public bool HeadDucking;
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        if (version >= 0x2ed)
            Plan = serializer.Resource(Plan, ResourceType.Plan, true);

        GfxMaterial = serializer.Resource(GfxMaterial, ResourceType.GfxMaterial);
        Bevel = serializer.Resource(Bevel, ResourceType.Bevel);
        PhysicsMaterial = serializer.Resource(PhysicsMaterial,
            ResourceType.Material);
        SoundEnum = serializer.I32(SoundEnum);
        BevelSize = serializer.F32(BevelSize);

        if (version < 0x2ed)
            Plan = serializer.Resource(Plan, ResourceType.Plan, true);

        if (serializer.GetRevision().GetSubVersion() > 0x62)
            HeadDucking = serializer.Bool(HeadDucking);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}