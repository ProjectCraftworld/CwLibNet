using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using CwLibNet.Types.Data;

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
        this.GfxMaterial = material;
        this.Bevel = bevel;
    }

    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        GfxMaterial = serializer.Resource(GfxMaterial, ResourceType.GfxMaterial);
        Bevel = serializer.Resource(Bevel, ResourceType.Bevel);
        UvOffset = serializer.V4(UvOffset);
        if (version >= 0x258)
            PlanGuid = serializer.Guid(PlanGuid);

        if (version >= 0x27c && subVersion < 0xfb)
        {
            if (serializer.IsWriting())
                serializer.GetOutput().Boole((VisibilityFlags & (byte)Enums.VisibilityFlags.PLAY_MODE) != 0);
            else
            {
                VisibilityFlags = (byte)Enums.VisibilityFlags.EDIT_MODE;
                if (serializer.GetInput().Boole())
                    VisibilityFlags |= (byte)Enums.VisibilityFlags.PLAY_MODE;
            }
        }

        if (subVersion >= 0xfb)
            VisibilityFlags = serializer.I8(VisibilityFlags);

        if (version >= 0x305)
        {
            TextureAnimationSpeed = serializer.F32(TextureAnimationSpeed);
            TextureAnimationSpeedOff = serializer.F32(TextureAnimationSpeedOff);
        }

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            if (vita >= 0x6b)
                UvMode = serializer.I32(UvMode);
            if (vita == 0x76)
                TextureScale = serializer.F32(TextureScale);
        }

        if (subVersion >= 0x34)
            NoBevel = serializer.Bool(NoBevel);
        if (subVersion >= 0x97)
            Sharded = serializer.Bool(Sharded);
        if (subVersion >= 0x13d)
            IncludeSides = serializer.Bool(IncludeSides);
        if (subVersion >= 0x155)
            SlideImpactDamping = serializer.I8(SlideImpactDamping);
        if (subVersion >= 0x13d)
        {
            SlideSteer = serializer.I8(SlideSteer);
            SlideSpeed = serializer.I8(SlideSpeed);
        }
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}