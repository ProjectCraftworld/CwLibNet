using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        if (version >= 0x2ed)
            Serializer.Serialize(ref Plan, Plan, ResourceType.Plan, true);

        Serializer.Serialize(ref GfxMaterial, GfxMaterial, ResourceType.GfxMaterial);
        Serializer.Serialize(ref Bevel, Bevel, ResourceType.Bevel);
        PhysicsMaterial = Serializer.Serialize(ref PhysicsMaterial, ResourceType.Material);
        Serializer.Serialize(ref SoundEnum);
        Serializer.Serialize(ref BevelSize);

        if (version < 0x2ed)
            Serializer.Serialize(ref Plan, Plan, ResourceType.Plan, true);

        if (Serializer.GetRevision().GetSubVersion() > 0x62)
            Serializer.Serialize(ref HeadDucking);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}