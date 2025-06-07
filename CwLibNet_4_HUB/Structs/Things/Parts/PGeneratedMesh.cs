using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PGeneratedMesh : ISerializable
{
    public const int BaseAllocationSize = 0x80;

    /**
     * The material used to render this mesh.
     */
    public ResourceDescriptor? GfxMaterial = new(11166, ResourceType.GfxMaterial);

    /**
     * The bevel used with this mesh.
     */
    public ResourceDescriptor? Bevel;

    /**
     * How much the UVs of the texture are offset.
     * (UV tool in-game)
     */
    public Vector4? UvOffset;

    /**
     * The plan this generated mesh came from.
     */
    public GUID? PlanGuid;

    /**
     * Flags controlling the visibility of this mesh.
     */
    public byte VisibilityFlags = (byte)Enums.VisibilityFlags.PLAY_MODE | (byte)Enums.VisibilityFlags.EDIT_MODE;

    /**
     * Speed of this material's animation,
     * animations are defined in the RGfxMaterial.
     */
    public float TextureAnimationSpeed = 1.0f;

    /**
     * The speed of this material's animation when it's off,
     * animations are defined in the RGfxMaterial.
     */
    public float TextureAnimationSpeedOff = 1.0f;


    /* Vita texture parameters */

    public int UvMode;

    public float TextureScale;

    /**
     * Indicates that this material should have no bevel.
     */
    public bool NoBevel;

    /**
     * Whether this material has been sharded.
     */
    public bool Sharded;

    /**
     * Whether the sides should be rendered(?)
     */
    public bool IncludeSides = true;


    public byte SlideImpactDamping = 50;

    /**
     * Speed the player moves when steering on a slide.
     */
    public byte SlideSteer = 100;

    /**
     * Speed the player descends this slide.
     */
    public byte SlideSpeed = 50;

    public PGeneratedMesh()
    {
    }

    public PGeneratedMesh(ResourceDescriptor? material, ResourceDescriptor? bevel)
    {
        GfxMaterial = material;
        Bevel = bevel;
    }

    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Serializer.Serialize(ref GfxMaterial, GfxMaterial, ResourceType.GfxMaterial);
        Serializer.Serialize(ref Bevel, Bevel, ResourceType.Bevel);
        Serializer.Serialize(ref UvOffset);
        if (version >= 0x258)
            Serializer.Serialize(ref PlanGuid);

        if (version >= 0x27c && subVersion < 0xfb)
        {
            if (Serializer.IsWriting())
                Serializer.GetCurrentSerializer().GetOutput().Boole((VisibilityFlags & (byte)Enums.VisibilityFlags.PLAY_MODE) != 0);
            else
            {
                VisibilityFlags = (byte)Enums.VisibilityFlags.EDIT_MODE;
                if (Serializer.GetCurrentSerializer().GetInput().Boole())
                    VisibilityFlags |= (byte)Enums.VisibilityFlags.PLAY_MODE;
            }
        }

        if (subVersion >= 0xfb)
            Serializer.Serialize(ref VisibilityFlags);

        if (version >= 0x305)
        {
            Serializer.Serialize(ref TextureAnimationSpeed);
            Serializer.Serialize(ref TextureAnimationSpeedOff);
        }

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            if (vita >= 0x6b)
                Serializer.Serialize(ref UvMode);
            if (vita == 0x76)
                Serializer.Serialize(ref TextureScale);
        }

        if (subVersion >= 0x34)
            Serializer.Serialize(ref NoBevel);
        if (subVersion >= 0x97)
            Serializer.Serialize(ref Sharded);
        if (subVersion >= 0x13d)
            Serializer.Serialize(ref IncludeSides);
        switch (subVersion)
        {
            case >= 0x155:
                Serializer.Serialize(ref SlideImpactDamping);
                break;
            case < 0x13d:
                return;
        }

        Serializer.Serialize(ref SlideSteer);
        Serializer.Serialize(ref SlideSpeed);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}