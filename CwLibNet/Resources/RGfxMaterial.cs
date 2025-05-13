using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Gmat;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Resources;

public class RGfxMaterial: Resource
{
    public const int BaseAllocationSize = 0x250;
    public const int MaxTextures = 8;
    public const int MaxWraps = 8;
    public const int UV_OFFSETS = 0x10;
    public const int UV_SCALES = 0x8;
    public const int PerfData = 0x2;

    public static readonly Vector4 SPECULAR_COLOR = new(0.09f, 0.09f, 0.09f, 1.0f);

    public int Flags = GfxMaterialFlags.DEFAULT;
    public float AlphaTestLevel = 0.5f;
    public byte AlphaLayer, AlphaMode;
    public ShadowCastMode ShadowCastMode = ShadowCastMode.ON;
    public float BumpLevel = 0.2f, CosinePower = 1.0f;
    public float ReflectionBlur = 1.0f, RefractiveIndex = 0.01f;

    public float RefractiveFresnelFalloffPower = 1.0f;
    public float RefractiveFresnelMultiplier = 1.0f;
    public float RefractiveFresnelOffset;
    public float RefractiveFresnelShift;

    public byte FuzzLengthAndRefractiveFlag;
    public byte TranslucencyDensity = 0xFF;
    public byte FuzzSwirlAngle = 0xFF;
    public byte FuzzSwirlAmplitude;
    public byte FuzzLightingBias = 127;
    public byte FuzzLightingScale = 127;
    public byte IridesenceRoughness;

    
    public string Glsl;

    public byte[][] Shaders;
    public byte[] Code;

    public ResourceDescriptor[] Textures = new ResourceDescriptor[MaxTextures];

    public TextureWrap[] WrapS;
    public TextureWrap[] WrapT;

    public List<MaterialBox> Boxes = [];
    public List<MaterialWire> Wires = [];

    
    public int SoundEnum;

    public MaterialParameterAnimation[]? ParameterAnimations;

    /* PS Vita specific fields */

    public float[] UvOffsets = new float[UV_OFFSETS];
    public float[] UvScales = new float[UV_SCALES];

    public byte[] CycleCount = new byte[PerfData];
    public byte[] ConditionalTexLookups = new byte[PerfData];
    public byte[] UnconditionalTexLookups = new byte[PerfData];
    public byte[] NonDependentTexLookups = new byte[PerfData];

    public RGfxMaterial()
    {
        WrapS = new TextureWrap[MaxTextures];
        WrapT = new TextureWrap[MaxTextures];
        for (var i = 0; i < 8; ++i)
        {
            WrapS[i] = TextureWrap.WRAP;
            WrapT[i] = TextureWrap.WRAP;
        }
    }

    
    public override void Serialize(Serializer serializer)
    {

        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Flags = serializer.I32(Flags);
        AlphaTestLevel = serializer.F32(AlphaTestLevel);
        AlphaLayer = serializer.I8(AlphaLayer);
        if (version >= (int)Revisions.GFXMATERIAL_ALPHA_MODE)
            AlphaMode = serializer.I8(AlphaMode);
        ShadowCastMode = serializer.Enum8(ShadowCastMode);
        BumpLevel = serializer.F32(BumpLevel);
        CosinePower = serializer.F32(CosinePower);
        ReflectionBlur = serializer.F32(ReflectionBlur);
        RefractiveIndex = serializer.F32(RefractiveIndex);

        if (subVersion >= (int)Revisions.FRESNEL)
        {
            RefractiveFresnelFalloffPower = serializer.F32(RefractiveFresnelFalloffPower);
            RefractiveFresnelMultiplier = serializer.F32(RefractiveFresnelMultiplier);
            RefractiveFresnelOffset = serializer.F32(RefractiveFresnelOffset);
            RefractiveFresnelShift = serializer.F32(RefractiveFresnelShift);
            if (subVersion >= (int)Revisions.FUZZ)
            {
                FuzzLengthAndRefractiveFlag = serializer.I8(FuzzLengthAndRefractiveFlag);
                if (subVersion >= (int)Revisions.FUZZ_LIGHTING)
                {
                    TranslucencyDensity = serializer.I8(TranslucencyDensity);
                    FuzzSwirlAngle = serializer.I8(FuzzSwirlAngle);
                    FuzzSwirlAmplitude = serializer.I8(FuzzSwirlAmplitude);
                    FuzzLightingBias = serializer.I8(FuzzLightingBias);
                    FuzzLightingScale = serializer.I8(FuzzLightingScale);
                    IridesenceRoughness = serializer.I8(IridesenceRoughness);
                }
            }
        }

        if (revision.Has(Branch.Mizuki, (int)Revisions.MZ_GLSL_SHADERS))
            serializer.Str(Glsl);

        var serializeCode = !revision.IsToolkit() || revision.Before(Branch.Mizuki,
                (int)Revisions.MZ_REMOVE_GFX_CODE);
        var sourceOffsets = GetBlobOffsetCount(revision);
        if (serializer.IsWriting())
        {
            if (serializeCode)
            {
                var stream = serializer.GetOutput();
                var offset = 0;
                for (var i = 0; i < sourceOffsets; ++i)
                {
                    var shader = Shaders[i];
                    if (version >= 0x34f)
                        offset += shader.Length;
                    stream.I32(offset);
                    if (version < 0x34f)
                        offset += shader.Length;
                }
                if (Code != null) offset += Code.Length;
                stream.I32(offset);
                for (var i = 0; i < sourceOffsets; ++i)
                    stream.Bytes(Shaders[i]);
                if (Code != null)
                    stream.Bytes(Code);
            }
            for (var i = 0; i < MaxTextures; ++i)
                serializer.Resource(Textures[i], ResourceType.Texture);
        }
        else
        {
            if (serializeCode)
            {
                var stream = serializer.GetInput();
                var blobOffsets = new int[sourceOffsets];
                for (var i = 0; i < sourceOffsets; ++i)
                    blobOffsets[i] = stream.I32();

                var code = stream.Bytearray();

                if (!revision.IsVita())
                {
                    Shaders = new byte[sourceOffsets][];
                    if (version < 0x34f)
                    {
                        for (var i = 1; i < sourceOffsets; ++i)
                            Shaders[i - 1] = code.Skip(
                                    blobOffsets[i - 1]).Take(
                                    blobOffsets[i] - blobOffsets[i-1]).ToArray();
                        Shaders[sourceOffsets - 1] = code.Skip(
                                blobOffsets[sourceOffsets - 1]).ToArray();
                    }
                    else
                    {
                        var offset = 0;

                        for (var i = 0; i < sourceOffsets; ++i)
                        {
                            Shaders[i] = code.Skip(
                                    offset % code.Length).Take(
                                    blobOffsets[i] - offset % code.Length).ToArray();
                            offset += Shaders[i].Length;
                        }

                        if (offset != code.Length)
                            code = code.Skip(offset).ToArray();
                    }
                }
                else
                {
                    Shaders = new byte[4][];
                    for (var i = 0; i < 4; i++)
                        Shaders[i] = new byte[0x500];
                }
            }

            Textures = new ResourceDescriptor[MaxTextures];
            for (var i = 0; i < MaxTextures; ++i)
                Textures[i] = serializer.Resource(null, ResourceType.Texture);
        }

        WrapS = serializer.Enumarray(WrapS);
        WrapT = serializer.Enumarray(WrapT);
        Boxes = serializer.Arraylist(Boxes);
        Wires = serializer.Arraylist(Wires);

        if (version >= (int)Revisions.GFXMATERIAL_ALPHA_MODE)
            SoundEnum = serializer.I32(SoundEnum);

        if (version >= (int)Revisions.PARAMETER_ANIMATIONS)
            ParameterAnimations = serializer.Array(ParameterAnimations);

        if (revision.Has(Branch.Double11, (int)Revisions.D1_UV_OFFSCALE))
        {
            for (var i = 0; i < UV_OFFSETS; ++i)
                UvOffsets[i] = serializer.F16(UvOffsets[i]);
            for (var i = 0; i < UV_SCALES; ++i)
                UvScales[i] = serializer.F16(UvScales[i]);
        }

        if (revision.Has(Branch.Double11, (int)Revisions.D1_PERFDATA))
        {
            CycleCount[0] = serializer.I8(CycleCount[0]);
            CycleCount[1] = serializer.I8(CycleCount[1]);

            ConditionalTexLookups[0] = serializer.I8(ConditionalTexLookups[0]);
            ConditionalTexLookups[1] = serializer.I8(ConditionalTexLookups[1]);

            UnconditionalTexLookups[0] = serializer.I8(UnconditionalTexLookups[0]);
            UnconditionalTexLookups[1] = serializer.I8(UnconditionalTexLookups[1]);

            NonDependentTexLookups[0] = serializer.I8(NonDependentTexLookups[0]);
            NonDependentTexLookups[1] = serializer.I8(NonDependentTexLookups[1]);
        }
    }

    
    public override int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Shaders != null) size += Shaders.Sum(shader => shader.Length);
        if (Code != null)
            size += Code.Length;
        if (Boxes != null) size += Boxes.Sum(box => box.GetAllocatedSize());
        if (Wires != null) size += Wires.Sum(wire => wire.GetAllocatedSize());
        if (ParameterAnimations != null) size += ParameterAnimations.Sum(animation => animation.GetAllocatedSize());
        return size;
    }

    
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        var serializer = new Serializer(GetAllocatedSize(), revision,
                compressionFlags);
        serializer.Struct(this);
        return new SerializationData(
                serializer.GetBuffer(),
                revision,
                compressionFlags,
                ResourceType.GfxMaterial,
                SerializationType.BINARY,
                serializer.GetDependencies()
        );
    }

    /**
     * Gets the number of shader blob offsets in this material.
     *
     * @param revision Revision of the resource
     * @return Number of blob offsets
     */
    public int GetBlobOffsetCount(Revision revision)
    {
        var head = revision.GetVersion();
        var sourceOffsets = 0xC;
        if ((Flags & 0x10000) != 0)
            sourceOffsets = 0x18;
        if (head < 0x3c1 || !revision.IsVita() || revision.Before(Branch.Double11,
                (int)Revisions.D1_SHADER))
            sourceOffsets = 0xA;
        if (head < 0x393)
            sourceOffsets = 0x8;
        if (head < 0x34f)
            sourceOffsets = 0x4;
        if ((head < 0x2d0 && !revision.IsLeerdammer()) || revision.Before(Branch.Leerdammer,
                (int)Revisions.LD_SHADER))
            sourceOffsets = 0x3;
        return sourceOffsets;
    }

    /**
     * Finds the output wire of a specified box
     *
     * @param box Index of box
     * @return Output wire if it exists
     */
    public MaterialWire FindWireFrom(int box)
    {
        foreach (var wire in Wires)
            if (wire.BoxFrom == box)
                return wire;
        return null;
    }

    /**
     * Gets the index of the shader output box
     *
     * @return Index of output box
     */
    public int GetOutputBox()
    {
        for (var i = 0; i < Boxes.Count; ++i)
        {
            var box = Boxes[i];
            if (box.Type == BoxType.OUTPUT)
                return i;
        }
        return -1;
    }

    public int GetBoxIndex(MaterialBox box)
    {
        for (var i = 0; i < Boxes.Count; ++i)
            if (box == Boxes[i]) return i;
        return -1;
    }

    public MaterialWire GetWireConnectedToPort(MaterialBox box, int port)
    {
        return GetWireConnectedToPort(GetBoxIndex(box), port);
    }

    public MaterialWire GetWireConnectedToPort(int box, int port)
    {
        foreach (var wire in Wires)
        {
            if (wire.BoxTo == box && (wire.PortTo & 0xff) == port)
                return wire;
        }
        return null;
    }

    public MaterialBox[] GetBoxesConnected(MaterialBox box)
    {
        return GetBoxesConnected(GetBoxIndex(box));
    }

    public MaterialBox[] GetBoxesConnected(int box)
    {
        List<MaterialBox> boxes = [];
        foreach (var wire in Wires)
        {
            if (wire.BoxTo == box)
                boxes.Add(Boxes[wire.BoxFrom]);
        }
        return boxes.ToArray();
    }

    public MaterialBox GetBoxConnectedToPort(MaterialBox box, int port)
    {
        return GetBoxConnectedToPort(GetBoxIndex(box), port);
    }

    public MaterialBox GetBoxConnectedToPort(int box, int port)
    {
        foreach (var wire in Wires)
        {
            if (wire.BoxTo == box && (wire.PortTo & 0xff) == port)
                return Boxes[wire.BoxFrom];
        }
        return null;
    }

    public MaterialBox GetBoxFrom(MaterialWire wire)
    {
        return Boxes[wire.BoxFrom];
    }

    public MaterialBox GetBoxTo(MaterialWire wire)
    {
        return Boxes[wire.BoxTo];
    }

    public static RGfxMaterial GetBumpLayout(
            Vector4 diffuseTransform,
            Vector4 bumpTransform,
            ResourceDescriptor diffuse,
            ResourceDescriptor bump,
            bool doubleSided,
            bool alphaClip
    )
    {
        var gfx = new RGfxMaterial
        {
            Textures =
            {
                [0] = diffuse,
                [1] = bump
            }
        };
        gfx.Boxes.Add(new MaterialBox());
        gfx.Boxes.Add(new MaterialBox(diffuseTransform, 0, 0));
        gfx.Boxes.Add(new MaterialBox(SPECULAR_COLOR));
        gfx.Boxes.Add(new MaterialBox(bumpTransform, 0, 1));

        gfx.Wires.Add(new MaterialWire(1, 0, 0, BrdfPort.DIFFUSE));
        gfx.Wires.Add(new MaterialWire(2, 0, 0, BrdfPort.SPECULAR));
        gfx.Wires.Add(new MaterialWire(3, 0, 0, BrdfPort.BUMP));

        gfx.Flags = GfxMaterialFlags.DEFAULT;
        if (doubleSided)
            gfx.Flags |= GfxMaterialFlags.TWO_SIDED;

        if (alphaClip)
            gfx.Wires.Add(new MaterialWire(1, 0, 0, BrdfPort.ALPHA_CLIP));

        return gfx;
    }

    public static RGfxMaterial GetDiffuseLayout(Vector4 transform, ResourceDescriptor texture,
                                                bool doubleSided, bool alphaClip)
    {
        var gfx = new RGfxMaterial
        {
            Textures =
            {
                [0] = texture
            }
        };
        gfx.Boxes.Add(new MaterialBox());
        gfx.Boxes.Add(new MaterialBox(transform, 0, 0));
        gfx.Boxes.Add(new MaterialBox(SPECULAR_COLOR));

        gfx.Wires.Add(new MaterialWire(1, 0, 0, BrdfPort.DIFFUSE));
        gfx.Wires.Add(new MaterialWire(2, 0, 0, BrdfPort.SPECULAR));

        gfx.Flags = GfxMaterialFlags.DEFAULT;
        if (doubleSided)
            gfx.Flags |= GfxMaterialFlags.TWO_SIDED;

        if (alphaClip)
            gfx.Wires.Add(new MaterialWire(1, 0, 0, BrdfPort.ALPHA_CLIP));

        return gfx;
    }


}