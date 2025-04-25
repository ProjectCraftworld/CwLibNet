using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Components.Decals;

public class Decal: ISerializable
{
    public const int BaseAllocationSize = 0x80;

    /**
     * The texture used for this decal.
     */
    public ResourceDescriptor Texture;

    /**
     * Coordinates of decal on the UV map.
     */
    public float U, V;

    /**
     * Rect of decal on the UV map.
     */
    public float Xvecu, Xvecv, Yvecu, Yvecv;

    /**
     * Color tint of this decal (RGB565).
     */
    
    public ushort Color = 0xad55;

    /**
     * Type of decal
     */
    
    public DecalType Type = DecalType.STICKER;

    /* No idea what this is actually used for, probably a reference to the thing containing the
    PMetaData for this sticker? */
    
    public short MetadataIndex = -1;

    
    public short NumMetadata;

    /**
     * Player that placed this decal.
     */
    
    public short PlacedBy = -1;

    /**
     * Number of frames that have passed in play mode.
     */
    
    public int PlayModeFrame;

    /**
     * If this decal has a scorch mark on it.
     */
    
    public bool ScorchMark;

    /**
     * The plan that this decal came from.
     */
    
    public ResourceDescriptor? Plan;

    /* Vita */
    
    public Vector4? LocalHitPoint;
    
    public bool DontWantGammaCorrection;

    public Decal() { }

    public Decal(ResourceDescriptor texture)
    {
        Texture = texture;
    }

    public Decal(ResourceDescriptor texture, float u, float v, float scale, float angle,
                 bool flipped)
    {
        Texture = texture;

        U = u;
        V = v;

        float sx = scale;
        float sy = scale;
        if (flipped)
            sx = -sx;

        Xvecu = (float) (sx * Math.Cos(angle));
        Xvecv = (float) (sx * Math.Sin(angle));

        Yvecu = (float) (-sy * Math.Sin(angle));
        Yvecv = (float) (sy * Math.Cos(angle));
    }

    
    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();

        Texture = serializer.Resource(Texture, ResourceType.Texture);
        U = serializer.F32(U);
        V = serializer.F32(V);
        Xvecu = serializer.F32(Xvecu);
        Xvecv = serializer.F32(Xvecv);
        Yvecu = serializer.F32(Yvecu);
        Yvecv = serializer.F32(Yvecv);

        if (version >= 0x14e && version < 0x25c)
        {
            if (serializer.IsWriting())
            {
                serializer.GetOutput().I32(
                    (int)((((Color & 0xffff) << 5) & 0xfc00) |
                          ((Color & 0xffff) << 8 & 0xf80000) |
                          ((Color & 0x1f) << 3) |
                          0xff000000)
                );
            }
            else
            {
                int color = serializer.GetInput().I32();
                color = (short) (
                    (((color >>> 10) & 0x3f) << 5) |
                    (((color >>> 0x13) & 0x1f) << 0xb) |
                    ((color >>> 3) & 0x1f)
                );
            }
        }

        if (version >= 0x260)
            Color = (ushort)serializer.I16((short)Color);

        if (version >= 0x158)
        {
            Type = serializer.Enum8(Type);
            MetadataIndex = serializer.I16(MetadataIndex);
            if (version <= 0x3ba)
                NumMetadata = serializer.I16(NumMetadata);
        }

        if (version >= 0x214)
            PlacedBy = serializer.I16(PlacedBy);
        if (version >= 0x215)
            PlayModeFrame = serializer.I32(PlayModeFrame);

        if (version >= 0x219)
            ScorchMark = serializer.Bool(ScorchMark);

        if (version >= 0x34c)
            Plan = serializer.Resource(Plan, ResourceType.Plan, true);
        else if (version >= 0x25b)
        {
            if (serializer.IsWriting())
            {
                if (Plan == null || Plan.IsHash())
                    serializer.GetOutput().I32(0);
                else if (Plan.IsGUID())
                    serializer.GetOutput().Guid(Plan.GetGUID());
            }
            else
            {
                GUID? guid = serializer.GetInput().Guid();
                Plan = guid == null ? null : new ResourceDescriptor(guid.Value, ResourceType.Plan);
            }
        }

        if (revision.Has(Branch.Double11, 0x3f))
            LocalHitPoint = serializer.V4(LocalHitPoint);
        if (revision.Has(Branch.Double11, 0x7e))
            DontWantGammaCorrection = serializer.Bool(DontWantGammaCorrection);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}