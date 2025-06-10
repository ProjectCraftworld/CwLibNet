using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components.Popit;

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
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        if (version >= 0x2ed)
            Serializer.Serialize(ref Plan, ResourceType.Plan, true, true, false);

        Serializer.Serialize(ref GfxMaterial, ResourceType.GfxMaterial, false, true, false);
        Serializer.Serialize(ref Bevel, ResourceType.Bevel, false, true, false);
        Serializer.Serialize(ref PhysicsMaterial, ResourceType.Material, false, true, false);
        Serializer.Serialize(ref SoundEnum);
        Serializer.Serialize(ref BevelSize);

        if (version < 0x2ed)
            Serializer.Serialize(ref Plan, ResourceType.Plan, true, true, false);

        if (Serializer.GetCurrentSerializer().GetRevision().GetSubVersion() > 0x62)
            Serializer.Serialize(ref HeadDucking);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}