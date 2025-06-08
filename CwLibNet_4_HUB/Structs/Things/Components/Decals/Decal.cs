using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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

        var sx = scale;
        if (flipped)
            sx = -sx;

        Xvecu = (float) (sx * Math.Cos(angle));
        Xvecv = (float) (sx * Math.Sin(angle));

        Yvecu = (float) (-scale * Math.Sin(angle));
        Yvecv = (float) (scale * Math.Cos(angle));
    }

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();

        Serializer.Serialize(ref Texture, ResourceType.Texture, false, true, false);
        Serializer.Serialize(ref U);
        Serializer.Serialize(ref V);
        Serializer.Serialize(ref Xvecu);
        Serializer.Serialize(ref Xvecv);
        Serializer.Serialize(ref Yvecu);
        Serializer.Serialize(ref Yvecv);

        switch (version)
        {
            case >= 0x14e and < 0x25c:
            {
                if (Serializer.IsWriting())
                {
                    Serializer.GetCurrentSerializer().GetOutput().I32(
                        (int)((((Color & 0xffff) << 5) & 0xfc00) |
                              ((Color & 0xffff) << 8 & 0xf80000) |
                              ((Color & 0x1f) << 3) |
                              0xff000000)
                    );
                }
                else
                {
                    var color = Serializer.GetCurrentSerializer().GetInput().I32();
/*
                    color = (short) (
                        (((color >>> 10) & 0x3f) << 5) |
                        (((color >>> 0x13) & 0x1f) << 0xb) |
                        ((color >>> 3) & 0x1f)
                    );
*/
                }

                break;
            }
            case >= 0x260:
                var tempColor = (short)Color;
                Serializer.Serialize(ref tempColor);
                Color = (ushort)tempColor;
                break;
        }

        if (version >= 0x158)
        {
            Type = serializer.Enum32(Type);
            Serializer.Serialize(ref MetadataIndex);
            if (version <= 0x3ba)
                Serializer.Serialize(ref NumMetadata);
        }

        if (version >= 0x214)
            Serializer.Serialize(ref PlacedBy);
        if (version >= 0x215)
            Serializer.Serialize(ref PlayModeFrame);

        if (version >= 0x219)
            Serializer.Serialize(ref ScorchMark);

        switch (version)
        {
            case >= 0x34c:
                Serializer.Serialize(ref Plan, ResourceType.Plan, true, true, false);
                break;
            case >= 0x25b:
            {
                if (Serializer.IsWriting())
                {
                    if (Plan == null || Plan.IsHash())
                        Serializer.GetCurrentSerializer().GetOutput().I32(0);
                    else if (Plan.IsGUID())
                        Serializer.GetCurrentSerializer().GetOutput().Guid(Plan.GetGUID());
                }
                else
                {
                    var guid = Serializer.GetCurrentSerializer().GetInput().Guid();
                    Plan = guid == null ? null : new ResourceDescriptor(guid.Value, ResourceType.Plan);
                }

                break;
            }
        }

        if (revision.Has(Branch.Double11, 0x3f))
            Serializer.Serialize(ref LocalHitPoint);
        if (revision.Has(Branch.Double11, 0x7e))
            Serializer.Serialize(ref DontWantGammaCorrection);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}