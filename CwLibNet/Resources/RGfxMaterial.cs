using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Gmat;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Resources;

public class RGfxMaterial: Resource
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x250;
    public static readonly int MAX_TEXTURES = 8;
    public static readonly int MAX_WRAPS = 8;
    public static readonly int UV_OFFSETS = 0x10;
    public static readonly int UV_SCALES = 0x8;
    public static readonly int PERF_DATA = 0x2;

    public static readonly Vector4 SPECULAR_COLOR = new Vector4(0.09f, 0.09f, 0.09f, 1.0f);

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

    
    public String Glsl;

    public byte[][] Shaders;
    public byte[] Code;

    public ResourceDescriptor[] Textures = new ResourceDescriptor[MAX_TEXTURES];

    public TextureWrap[] WrapS;
    public TextureWrap[] WrapT;

    public List<MaterialBox> Boxes = [];
    public List<MaterialWire> Wires = [];

    
    public int SoundEnum;

    public MaterialParameterAnimation[] ParameterAnimations;

    /* PS Vita specific fields */

    public float[] UvOffsets = new float[UV_OFFSETS];
    public float[] UvScales = new float[UV_SCALES];

    public byte[] CycleCount = new byte[PERF_DATA];
    public byte[] ConditionalTexLookups = new byte[PERF_DATA];
    public byte[] UnconditionalTexLookups = new byte[PERF_DATA];
    public byte[] NonDependentTexLookups = new byte[PERF_DATA];

    public RGfxMaterial()
    {
        this.WrapS = new TextureWrap[MAX_TEXTURES];
        this.WrapT = new TextureWrap[MAX_TEXTURES];
        for (int i = 0; i < 8; ++i)
        {
            this.WrapS[i] = TextureWrap.WRAP;
            this.WrapT[i] = TextureWrap.WRAP;
        }
    }

    
    public override void Serialize(Serializer serializer)
    {

        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        Flags = serializer.I32(Flags);
        AlphaTestLevel = serializer.F32(AlphaTestLevel);
        AlphaLayer = serializer.I8(AlphaLayer);
        if (version >= (int)Revisions.GfxmaterialAlphaMode)
            AlphaMode = serializer.I8(AlphaMode);
        ShadowCastMode = serializer.Enum8(ShadowCastMode);
        BumpLevel = serializer.F32(BumpLevel);
        CosinePower = serializer.F32(CosinePower);
        ReflectionBlur = serializer.F32(ReflectionBlur);
        RefractiveIndex = serializer.F32(RefractiveIndex);

        if (subVersion >= (int)Revisions.Fresnel)
        {
            RefractiveFresnelFalloffPower = serializer.F32(RefractiveFresnelFalloffPower);
            RefractiveFresnelMultiplier = serializer.F32(RefractiveFresnelMultiplier);
            RefractiveFresnelOffset = serializer.F32(RefractiveFresnelOffset);
            RefractiveFresnelShift = serializer.F32(RefractiveFresnelShift);
            if (subVersion >= (int)Revisions.Fuzz)
            {
                FuzzLengthAndRefractiveFlag = serializer.I8(FuzzLengthAndRefractiveFlag);
                if (subVersion >= (int)Revisions.FuzzLighting)
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

        if (revision.Has(Branch.Mizuki, (int)Revisions.MzGlslShaders))
            serializer.Str(Glsl);

        bool serializeCode = !revision.IsToolkit() || revision.Before(Branch.Mizuki,
                (int)Revisions.MzRemoveGfxCode);
        int sourceOffsets = GetBlobOffsetCount(revision);
        if (serializer.IsWriting())
        {
            if (serializeCode)
            {
                MemoryOutputStream stream = serializer.GetOutput();
                int offset = 0;
                for (int i = 0; i < sourceOffsets; ++i)
                {
                    byte[] shader = Shaders[i];
                    if (version >= 0x34f)
                        offset += shader.Length;
                    stream.I32(offset);
                    if (version < 0x34f)
                        offset += shader.Length;
                }
                if (this.Code != null) offset += this.Code.Length;
                stream.I32(offset);
                for (int i = 0; i < sourceOffsets; ++i)
                    stream.Bytes(Shaders[i]);
                if (this.Code != null)
                    stream.Bytes(this.Code);
            }
            for (int i = 0; i < MAX_TEXTURES; ++i)
                serializer.Resource(Textures[i], ResourceType.Texture);
        }
        else
        {
            if (serializeCode)
            {
                MemoryInputStream stream = serializer.GetInput();
                int[] blobOffsets = new int[sourceOffsets];
                for (int i = 0; i < sourceOffsets; ++i)
                    blobOffsets[i] = stream.I32();

                byte[] code = stream.Bytearray();

                if (!revision.IsVita())
                {
                    Shaders = new byte[sourceOffsets][];
                    if (version < 0x34f)
                    {
                        for (int i = 1; i < sourceOffsets; ++i)
                            Shaders[i - 1] = code.Skip(
                                    blobOffsets[i - 1]).Take(
                                    blobOffsets[i] - blobOffsets[i-1]).ToArray();
                        Shaders[sourceOffsets - 1] = code.Skip(
                                blobOffsets[sourceOffsets - 1]).ToArray();
                    }
                    else
                    {
                        int offset = 0;

                        for (int i = 0; i < sourceOffsets; ++i)
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
                    for (int i = 0; i < 4; i++)
                        Shaders[i] = new byte[0x500];
                }
            }

            Textures = new ResourceDescriptor[MAX_TEXTURES];
            for (int i = 0; i < MAX_TEXTURES; ++i)
                Textures[i] = serializer.Resource(null, ResourceType.Texture);
        }

        WrapS = serializer.Enumarray(WrapS);
        WrapT = serializer.Enumarray(WrapT);
        Boxes = serializer.Arraylist(Boxes);
        Wires = serializer.Arraylist(Wires);

        if (version >= (int)Revisions.GfxmaterialAlphaMode)
            SoundEnum = serializer.I32(SoundEnum);

        if (version >= (int)Revisions.ParameterAnimations)
            ParameterAnimations = serializer.Array(ParameterAnimations);

        if (revision.Has(Branch.Double11, (int)Revisions.D1UvOffscale))
        {
            for (int i = 0; i < UV_OFFSETS; ++i)
                UvOffsets[i] = serializer.F16(UvOffsets[i]);
            for (int i = 0; i < UV_SCALES; ++i)
                UvScales[i] = serializer.F16(UvScales[i]);
        }

        if (revision.Has(Branch.Double11, (int)Revisions.D1Perfdata))
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
        int size = BASE_ALLOCATION_SIZE;
        if (this.Shaders != null) size += this.Shaders.Sum(shader => shader.Length);
        if (this.Code != null)
            size += this.Code.Length;
        if (this.Boxes != null)
            foreach (MaterialBox box in this.Boxes)
                size += box.GetAllocatedSize();
        if (this.Wires != null)
            foreach (MaterialWire wire in this.Wires)
                size += wire.GetAllocatedSize();
        if (this.ParameterAnimations != null)
            foreach (MaterialParameterAnimation animation in ParameterAnimations)
                size += animation.GetAllocatedSize();
        return size;
    }

    
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        Serializer serializer = new Serializer(this.GetAllocatedSize(), revision,
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
        int head = revision.GetVersion();
        int sourceOffsets = 0xC;
        if ((this.Flags & 0x10000) != 0)
            sourceOffsets = 0x18;
        if (head < 0x3c1 || !revision.IsVita() || revision.Before(Branch.Double11,
                (int)Revisions.D1Shader))
            sourceOffsets = 0xA;
        if (head < 0x393)
            sourceOffsets = 0x8;
        if (head < 0x34f)
            sourceOffsets = 0x4;
        if ((head < 0x2d0 && !revision.IsLeerdammer()) || revision.Before(Branch.Leerdammer,
                (int)Revisions.LdShader))
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
        foreach (MaterialWire wire in this.Wires)
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
        for (int i = 0; i < this.Boxes.Count; ++i)
        {
            MaterialBox box = this.Boxes[i];
            if (box.Type == BoxType.OUTPUT)
                return i;
        }
        return -1;
    }

    public int GetBoxIndex(MaterialBox box)
    {
        for (int i = 0; i < this.Boxes.Count; ++i)
            if (box == this.Boxes[i]) return i;
        return -1;
    }

    public MaterialWire GetWireConnectedToPort(MaterialBox box, int port)
    {
        return this.GetWireConnectedToPort(this.GetBoxIndex(box), port);
    }

    public MaterialWire GetWireConnectedToPort(int box, int port)
    {
        foreach (MaterialWire wire in this.Wires)
        {
            if (wire.BoxTo == box && (wire.PortTo & 0xff) == port)
                return wire;
        }
        return null;
    }

    public MaterialBox[] GetBoxesConnected(MaterialBox box)
    {
        return this.GetBoxesConnected(this.GetBoxIndex(box));
    }

    public MaterialBox[] GetBoxesConnected(int box)
    {
        List<MaterialBox> boxes = [];
        foreach (MaterialWire wire in this.Wires)
        {
            if (wire.BoxTo == box)
                boxes.Add(this.Boxes[wire.BoxFrom]);
        }
        return boxes.ToArray();
    }

    public MaterialBox GetBoxConnectedToPort(MaterialBox box, int port)
    {
        return this.GetBoxConnectedToPort(this.GetBoxIndex(box), port);
    }

    public MaterialBox GetBoxConnectedToPort(int box, int port)
    {
        foreach (MaterialWire wire in this.Wires)
        {
            if (wire.BoxTo == box && (wire.PortTo & 0xff) == port)
                return this.Boxes[wire.BoxFrom];
        }
        return null;
    }

    public MaterialBox GetBoxFrom(MaterialWire wire)
    {
        return this.Boxes[wire.BoxFrom];
    }

    public MaterialBox GetBoxTo(MaterialWire wire)
    {
        return this.Boxes[wire.BoxTo];
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
        RGfxMaterial gfx = new RGfxMaterial();
        gfx.Textures[0] = diffuse;
        gfx.Textures[1] = bump;
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
        RGfxMaterial gfx = new RGfxMaterial();
        gfx.Textures[0] = texture;
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