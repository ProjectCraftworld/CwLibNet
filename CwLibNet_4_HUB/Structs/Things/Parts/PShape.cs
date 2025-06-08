using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Things.Components.Shapes;
using CwLibNet.Types.Data;
using CwLibNet.Util;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PShape: ISerializable
{
    public const int BaseAllocationSize = 0x100;

    /**
     * Polygon defining the collision of this Thing.
     */
    public Polygon Polygon = new();

    /**
     * Physical properties of this shape.
     */
    public ResourceDescriptor Material = new(10724, ResourceType.Material);

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

    
    public ContactCache ContactCache = new();

    
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
        Polygon.Loops = [vertices.Length];
        Polygon.RequiresZ = true;
    }

    public PShape(float massDepth, float thickness, Vector3?[] vertices): this(vertices)
    {
        MassDepth = massDepth;
        Thickness = thickness;
    }

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Serializer.Serialize(ref Polygon);
        Serializer.Serialize(ref Material, ResourceType.Material, false, true, false);
        switch (version)
        {
            case >= 0x15c:
                Serializer.Serialize(ref OldMaterial, ResourceType.Material, false, true, false);
                break;
            case < 0x13c:
                Serializer.Serialize(ref temp_int);
                break;
        }

        Serializer.Serialize(ref Thickness);
        if (version >= 0x227)
            Serializer.Serialize(ref MassDepth);

        if (version <= 0x389)
        {
            if (Serializer.IsWriting())
                Serializer.GetCurrentSerializer().GetOutput().V4(Colors.FromARGB((int)Color));
            else
            {
                var color = Serializer.GetCurrentSerializer().GetInput().V4();
                Color = (uint)Colors.GetARGB(color);
            }
        }
        else 
        {
            int colorInt = (int)Color;
            Serializer.Serialize(ref colorInt);
            Color = (uint)colorInt;
        }

        switch (version)
        {
            case < 0x13c:
                ResourceDescriptor? nullTexture = null;
                Serializer.Serialize(ref nullTexture, ResourceType.Texture);
                break;
            case >= 0x301:
                Serializer.Serialize(ref Brightness);
                break;
        }

        Serializer.Serialize(ref BevelSize);

        if (version < 0x13c)
            Serializer.Serialize(ref temp_int);

        if (version is <= 0x340 or >= 0x38e)
            Serializer.Serialize(ref Com);

        if (version <= 0x306)
        {
            Serializer.Serialize(ref InteractPlayMode);
            Serializer.Serialize(ref InteractEditMode);
        }

        if (version >= 0x303)
            Serializer.Serialize(ref Behavior);


        if (version >= 0x303)
        {
            if (version < 0x38a)
            {
                if (Serializer.IsWriting())
                    Serializer.GetCurrentSerializer().GetOutput().V4(Colors.FromARGB(ColorOff));
                else
                {
                    var color = Serializer.GetCurrentSerializer().GetInput().V4();
                    ColorOff = Colors.GetARGB(color);
                }
            }
            else Serializer.Serialize(ref ColorOff);
            Serializer.Serialize(ref BrightnessOff);
        }

        if (version <= 0x345)
            LethalType = Serializer.GetCurrentSerializer().Enum32(LethalType, true);
        else
        {
            if (Serializer.IsWriting())
                Serializer.GetCurrentSerializer().GetOutput().I16((short)LethalType);
            else LethalType = (LethalType)Serializer.GetCurrentSerializer().GetInput().U16();
        }

        if (version < 0x2b5)
        {
            if (!Serializer.IsWriting())
            {
                var stream = Serializer.GetCurrentSerializer().GetInput();
                Flags = 0;
                if (stream.Boole()) Flags |= (short)ShapeFlags.COLLIDABLE_GAME;
                if (version >= 0x224 && stream.Boole())
                    Flags |= (short)ShapeFlags.COLLIDABLE_POPPET;
                if (stream.Boole()) Flags |= (short)ShapeFlags.COLLIDABLE_WITH_PARENT;
            }
            else
            {
                var stream = Serializer.GetCurrentSerializer().GetOutput();
                stream.Boole((Flags & ShapeFlags.COLLIDABLE_GAME) != 0);
                if (version >= 0x224)
                    stream.Boole((Flags & ShapeFlags.COLLIDABLE_POPPET) != 0);
                stream.Boole((Flags & ShapeFlags.COLLIDABLE_WITH_PARENT) != 0);
            }
        }

        if (version < 0x13c)
        {
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_int); // Is this a float, or is it just because it's an early
            // revision?
        }

        Serializer.Serialize(ref SoundEnumOverride);

        if (version is >= 0x29d and < 0x30c)
        {
            Serializer.Serialize(ref temp_int); // restitution
            if (version < 0x2b5)
                Serializer.Serialize(ref temp_int); // unk
        }

        if (version >= 0x2a3)
        {
            if (version <= 0x367)
                Serializer.Serialize(ref PlayerNumberColor);
            else
                Serializer.Serialize(ref PlayerNumberColor);
        }

        if (version >= 0x2b5)
        {
            if (version <= 0x345)
            {
                byte flagsByte = (byte) Flags;
                Serializer.Serialize(ref flagsByte);
                Flags = flagsByte;
            }
            else
            {
                Serializer.Serialize(ref Flags);
            }
        }

        if (version >= 0x307)
            Serializer.Serialize(ref ContactCache);

        if (version >= 0x3bd)
        {
            Serializer.Serialize(ref Stickiness);
            Serializer.Serialize(ref Grabbability);
            Serializer.Serialize(ref GrabFilter);
        }

        if (version >= 0x3c1)
        {
            Serializer.Serialize(ref ColorOpacity);
            Serializer.Serialize(ref ColorOffOpacity);
        }

        // head > 0x3c0
        if (revision.Has(Branch.Double11, 0x5))
            Serializer.Serialize(ref Touchability);

        if (subVersion >= 0x12c)
            Serializer.Serialize(ref ColorShininess);

        if (version >= 0x3e2)
            Serializer.Serialize(ref CanCollect);

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();
            if (vita >= 0x26)
                Serializer.Serialize(ref InvisibleTouch);
            if (vita >= 0x34)
                Serializer.Serialize(ref BouncePadBehavior);
            if (vita >= 0x5f)
                Serializer.Serialize(ref ZBiasVita);
            if (vita >= 0x7a)
                Serializer.Serialize(ref TouchWhenInvisible);
        }

        if (subVersion is >= 0x42 and < 0xc6)
            Serializer.Serialize(ref temp_int);

        if (subVersion >= 0x42)
            Serializer.Serialize(ref Ghosty);

        if (subVersion >= 0x186)
            Serializer.Serialize(ref DefaultClimbable);
        if (subVersion >= 0x4b)
            Serializer.Serialize(ref CurrentlyClimbable);

        if (subVersion >= 0x63)
            Serializer.Serialize(ref HeadDucking);
        if (subVersion >= 0x82)
            Serializer.Serialize(ref IsLbp2Shape);
        if (subVersion >= 0x8a)
            Serializer.Serialize(ref IsStatic);
        if (subVersion >= 0xe6)
            Serializer.Serialize(ref CollidableSackboy);
        if (subVersion >= 0x11a)
        {
            Serializer.Serialize(ref PartOfPowerUp);
            Serializer.Serialize(ref CameraExcluderIsSticky);
        }

        switch (subVersion)
        {
            case >= 0x19a:
                Serializer.Serialize(ref Ethereal);
                break;
            // Unknown value
            case >= 0x120 and < 0x135:
                Serializer.Serialize(ref temp_int);
                break;
        }

        if (subVersion >= 0x149)
            Serializer.Serialize(ref ZBias);

        if (subVersion < 0x14c) return;
        Serializer.Serialize(ref FireDensity);
        Serializer.Serialize(ref FireLifetime);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}