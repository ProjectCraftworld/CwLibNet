using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types.Data;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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
    
    public override void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        base.Serialize(serializer);

        if (version >= 0x153)
            Serializer.Serialize(ref Presets);

        if (version is > 0x152 and < 0x15a)
            Serializer.Serialize(ref temp_int);

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
                ResourceDescriptor? nullTexture = null;
                Serializer.Serialize(ref nullTexture, ResourceType.Texture);
                break;
            case >= 0x2f3:
                Serializer.Serialize(ref BackdropMesh, ResourceType.StaticMesh);
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