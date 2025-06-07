using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PLevelSettings: LevelSettings
{
    public new const int BaseAllocationSize = 0x100;

    public List<LevelSettings> Presets = [];

    public bool NonLinearFog;
    public string? BackdropAmbience;
    public ResourceDescriptor? BackdropMesh;
    public int BackgroundRepeatFlags;
    public float BackgroundSkyHeight;
    
    public override void Serialize()
    {
        var revision = Serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        base.Serialize(serializer);

        if (version >= 0x153)
            Serializer.Serialize(ref Presets);

        if (version is > 0x152 and < 0x15a)
            Serializer.Serialize(ref 0);

        if (revision.Has(Branch.Double11, 0x78))
            Serializer.Serialize(ref NonLinearFog);

        // 0x154 -> 0x155
        if (!(version is < 0x154 or > 0x155))
        {
            throw new SerializationException("Unsupported serialization object!");
            // matrix[0]
            // matrix[1]
            // colPick
            // vignetteTop
            // vignetteBottom
        }

        Serializer.Serialize(ref BackdropAmbience);

        switch (version)
        {
            case < 0x156:
                Serializer.Serialize(ref null, ResourceType.Texture);
                break;
            case >= 0x2f3:
                BackdropMesh = Serializer.Serialize(ref BackdropMesh, ResourceType.StaticMesh);
                break;
        }

        if (subVersion >= 0xaf)
        {
            Serializer.Serialize(ref BackgroundRepeatFlags);
            Serializer.Serialize(ref BackgroundSkyHeight);
        }
    }

    public override int GetAllocatedSize()
    {
        return BaseAllocationSize + Presets.Sum(p => p.GetAllocatedSize());
    }
}