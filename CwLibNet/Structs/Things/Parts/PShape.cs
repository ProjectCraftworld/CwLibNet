using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Things.Components.Shapes;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using CwLibNet.Util;

namespace CwLibNet.Structs.Things.Parts;

public class PShape: ISerializable
{
    public const int BaseAllocationSize = 0x100;

    /**
     * Polygon defining the collision of this Thing.
     */
    public Polygon Polygon = new Polygon();

    /**
     * Physical properties of this shape.
     */
    public ResourceDescriptor Material =
        new ResourceDescriptor(10724, ResourceType.Material);

    /**
     * Old physical properties of this shape.
     */
    
    public ResourceDescriptor OldMaterial;

    public float Thickness = 90.0f;

    
    public float MassDepth = 1.0f;

    /**
     * RGBA color of the shape.
     */
    public uint Color = 0xFFFFFFFF;

    /**
     * Brightness of the color of the shape.
     */
    
    public float Brightness;

    public float BevelSize = 10.0f;

    public Matrix4x4? Com = Matrix4x4.Identity;

    
    public int Behavior;

    
    public int ColorOff;
    
    public float BrightnessOff;

    
    public byte InteractPlayMode, InteractEditMode = 1;

    public LethalType LethalType = LethalType.NOT;


    
    public int SoundEnumOverride;

    
    public byte PlayerNumberColor;

    public short Flags = (short)ShapeFlags.DEFAULT_FLAGS;

    
    public ContactCache ContactCache = new ContactCache();

    
    public byte Stickiness, Grabbability, GrabFilter;

    
    public byte ColorOpacity, ColorOffOpacity;

    
    public byte ColorShininess;

    
    public bool CanCollect;
    
    public bool Ghosty;

    
    public bool DefaultClimbable;
    
    public bool CurrentlyClimbable;

    
    public bool HeadDucking;
    
    public bool IsLbp2Shape;
    
    public bool IsStatic;
    
    public bool CollidableSackboy;

    
    public bool PartOfPowerUp, CameraExcluderIsSticky;

    
    public bool Ethereal;
    
    public byte ZBias;

    
    public byte FireDensity, FireLifetime;

    /* Vita fields */

    
    public byte Touchability;
    
    public bool InvisibleTouch;
    
    public byte BouncePadBehavior;
    
    public float ZBiasVita;
    
    public bool TouchWhenInvisible;

    public PShape() { }

    public PShape(Vector3?[] vertices)
    {
        Polygon.Vertices = vertices;
        Polygon.Loops = new int[] { vertices.Length };
        Polygon.RequiresZ = true;
    }

    public PShape(float massDepth, float thickness, Vector3?[] vertices): this(vertices)
    {
        MassDepth = massDepth;
        Thickness = thickness;
    }

    
    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        Polygon = serializer.Struct(Polygon);
        Material = serializer.Resource(Material, ResourceType.Material);
        if (version >= 0x15c)
            OldMaterial = serializer.Resource(OldMaterial, ResourceType.Material);

        if (version < 0x13c)
            serializer.U8(0);

        Thickness = serializer.F32(Thickness);
        if (version >= 0x227)
            MassDepth = serializer.F32(MassDepth);

        if (version <= 0x389)
        {
            if (serializer.IsWriting())
                serializer.GetOutput().V4(Colors.FromARGB((int)Color));
            else
            {
                Vector4 color = serializer.GetInput().V4();
                Color = (uint)Colors.GetARGB(color);
            }
        }
        else Color = (uint)serializer.I32((int)Color);

        if (version < 0x13c)
            serializer.Resource(null, ResourceType.Texture);

        if (version >= 0x301)
            Brightness = serializer.F32(Brightness);

        BevelSize = serializer.F32(BevelSize);

        if (version < 0x13c)
            serializer.I32(0);

        if (version is <= 0x340 or >= 0x38e)
            Com = serializer.M44(Com);

        if (version <= 0x306)
        {
            InteractPlayMode = serializer.I8(InteractPlayMode);
            InteractEditMode = serializer.I8(InteractEditMode);
        }

        if (version >= 0x303)
            Behavior = serializer.I32(Behavior);


        if (version >= 0x303)
        {
            if (version < 0x38a)
            {
                if (serializer.IsWriting())
                    serializer.GetOutput().V4(Colors.FromARGB(ColorOff));
                else
                {
                    Vector4 color = serializer.GetInput().V4();
                    ColorOff = Colors.GetARGB(color);
                }
            }
            else ColorOff = serializer.I32(ColorOff);
            BrightnessOff = serializer.F32(BrightnessOff);
        }

        if (version <= 0x345)
            LethalType = serializer.Enum32(LethalType, true);
        else
        {
            if (serializer.IsWriting())
                serializer.GetOutput().I16((short)LethalType);
            else LethalType = (LethalType)(serializer.GetInput().U16());
        }

        if (version < 0x2b5)
        {
            if (!serializer.IsWriting())
            {
                MemoryInputStream stream = serializer.GetInput();
                Flags = 0;
                if (stream.Boole()) Flags |= (short)ShapeFlags.COLLIDABLE_GAME;
                if (version >= 0x224 && stream.Boole())
                    Flags |= (short)ShapeFlags.COLLIDABLE_POPPET;
                if (stream.Boole()) Flags |= (short)ShapeFlags.COLLIDABLE_WITH_PARENT;
            }
            else
            {
                MemoryOutputStream stream = serializer.GetOutput();
                stream.Boole((Flags & ShapeFlags.COLLIDABLE_GAME) != 0);
                if (version >= 0x224)
                    stream.Boole((Flags & ShapeFlags.COLLIDABLE_POPPET) != 0);
                stream.Boole((Flags & ShapeFlags.COLLIDABLE_WITH_PARENT) != 0);
            }
        }

        if (version < 0x13c)
        {
            serializer.U8(0);
            serializer.F32(0); // Is this a float, or is it just because it's an early
            // revision?
        }

        SoundEnumOverride = serializer.I32(SoundEnumOverride);

        if (version >= 0x29d && version < 0x30c)
        {
            serializer.F32(0); // restitution
            if (version < 0x2b5)
                serializer.U8(0); // unk
        }

        if (version >= 0x2a3)
        {
            if (version <= 0x367)
                PlayerNumberColor = (byte) serializer.I32(PlayerNumberColor);
            else
                PlayerNumberColor = serializer.I8(PlayerNumberColor);
        }

        if (version >= 0x2b5)
        {
            if (version <= 0x345)
                Flags = serializer.I8((byte) (Flags));
            else
                Flags = serializer.I16(Flags);
        }

        if (version >= 0x307)
            ContactCache = serializer.Struct(ContactCache);

        if (version >= 0x3bd)
        {
            Stickiness = serializer.I8(Stickiness);
            Grabbability = serializer.I8(Grabbability);
            GrabFilter = serializer.I8(GrabFilter);
        }

        if (version >= 0x3c1)
        {
            ColorOpacity = serializer.I8(ColorOpacity);
            ColorOffOpacity = serializer.I8(ColorOffOpacity);
        }

        // head > 0x3c0
        if (revision.Has(Branch.Double11, 0x5))
            Touchability = serializer.I8(Touchability);

        if (subVersion >= 0x12c)
            ColorShininess = serializer.I8(ColorShininess);

        if (version >= 0x3e2)
            CanCollect = serializer.Bool(CanCollect);

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            if (vita >= 0x26)
                InvisibleTouch = serializer.Bool(InvisibleTouch);
            if (vita >= 0x34)
                BouncePadBehavior = serializer.I8(BouncePadBehavior);
            if (vita >= 0x5f)
                ZBiasVita = serializer.F32(ZBiasVita);
            if (vita >= 0x7a)
                TouchWhenInvisible = serializer.Bool(TouchWhenInvisible);
        }

        if (subVersion is >= 0x42 and < 0xc6)
            serializer.U8(0);

        if (subVersion >= 0x42)
            Ghosty = serializer.Bool(Ghosty);

        if (subVersion >= 0x186)
            DefaultClimbable = serializer.Bool(DefaultClimbable);
        if (subVersion >= 0x4b)
            CurrentlyClimbable = serializer.Bool(CurrentlyClimbable);

        if (subVersion >= 0x63)
            HeadDucking = serializer.Bool(HeadDucking);
        if (subVersion >= 0x82)
            IsLbp2Shape = serializer.Bool(IsLbp2Shape);
        if (subVersion >= 0x8a)
            IsStatic = serializer.Bool(IsStatic);
        if (subVersion >= 0xe6)
            CollidableSackboy = serializer.Bool(CollidableSackboy);
        if (subVersion >= 0x11a)
        {
            PartOfPowerUp = serializer.Bool(PartOfPowerUp);
            CameraExcluderIsSticky = serializer.Bool(CameraExcluderIsSticky);
        }

        if (subVersion >= 0x19a)
            Ethereal = serializer.Bool(Ethereal);

        // Unknown value
        if (subVersion is >= 0x120 and < 0x135)
            serializer.U8(0);

        if (subVersion >= 0x149)
            ZBias = serializer.I8(ZBias);

        if (subVersion >= 0x14c)
        {
            FireDensity = serializer.I8(FireDensity);
            FireLifetime = serializer.I8(FireLifetime);
        }
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}