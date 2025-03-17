using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types;
using CwLibNet.Types.Data;

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
    
    public override void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        base.Serialize(serializer);

        if (version >= 0x153)
            Presets = serializer.Arraylist(Presets);

        if (0x152 < version && version < 0x15a)
            serializer.F32(0);

        if (revision.Has(Branch.Double11, 0x78))
            NonLinearFog = serializer.Bool(NonLinearFog);

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

        BackdropAmbience = serializer.Str(BackdropAmbience);

        if (version < 0x156)
            serializer.Resource(null, ResourceType.Texture);

        if (version >= 0x2f3)
            BackdropMesh = serializer.Resource(BackdropMesh,
                ResourceType.StaticMesh);

        if (subVersion >= 0xaf)
        {
            BackgroundRepeatFlags = serializer.I32(BackgroundRepeatFlags);
            BackgroundSkyHeight = serializer.F32(BackgroundSkyHeight);
        }
    }

    public override int GetAllocatedSize()
    {
        return BaseAllocationSize + Presets.Sum(p => p.GetAllocatedSize());
    }
}