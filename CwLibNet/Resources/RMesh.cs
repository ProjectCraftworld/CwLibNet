using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.Extensions;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Custom;
using CwLibNet.Structs.Mesh;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using CwLibNet.Util;

namespace CwLibNet.Resources;

public class RMesh: Resource
{
    public const int StreamPosBoneindices = 0x0;
    public const int StreamBoneweightsNormTangentSmoothNorm = 0x1;
    public static readonly int STREAM_MORPHS0 = 0x2;

    public const int MaxMorphs = 0x20;

    private static readonly int BaseAllocationSize =
        0x400 + SoftbodyClusterData.BASE_ALLOCATION_SIZE;

    /**
     * The number of vertices this mesh has.
     */
    public int NumVerts;

    /**
     * The number of face indices this mesh has.
     */
    public int NumIndices;

    /**
     * The number of edge indices his mesh has.
     */
    public int NumEdgeIndices;

    /**
     * The number of triangles this mesh has.
     */
    public int NumTris;

    /**
     * The number of vertex streams this model has.
     */
    public int StreamCount;

    /**
     * How number of UVs this model has.
     */
    public int AttributeCount;

    /**
     * The number of morphs this model has.
     */
    public int MorphCount;

    /**
     * Names of the morphs in-use by this this.
     */
    public string[] MorphNames = new string[MaxMorphs];

    public float[] MinUv = [0.0f, 0.0f];
    public float[] MaxUv = [1.0f, 1.0f];
    public float AreaScaleFactor = 500;

    /**
     * STREAM[0] = v3 Pos, u32 Bone Indices
     * STREAM[1] = u8 w2, u8 w1, u8 w0, u8 b1, u24 norm, u8 b2, u24 tangent, u8 b3, u24
     * smoothnormal, b4
     */
    public byte[][] Streams;

    /**
     * Texture coordinate (UV) buffer.
     */
    public byte[] Attributes;

    /**
     * Face indices buffer.
     */
    public byte[] Indices;

    /**
     * Triangle adjacency buffer used for
     * geometry shaders in LittleBigPlanet 3.
     */
    public byte[] TriangleAdjacencyInfos;

    /**
     * Primitive objects controlling how each part of this
     * mesh gets rendered.
     */
    public List<Primitive> Primitives = [];

    /**
     * The skeleton of this this.
     */
    public Bone[]? Bones;

    /**
     * Each index refers to a bone, the value at each index
     * refers to the bone of which the first is a mirror.
     */
    public short[] MirrorBones;

    /**
     * The type of mirroring between the bones.
     */
    public FlipType[] MirrorBoneFlipTypes;

    /**
     * Each index refers to a morph, the value at each index
     * refers to the morph of which the first is a mirror.
     */
    public short[] MirrorMorphs;

    /**
     * How this mesh will get rendered.
     */
    public CellGcmPrimitive PrimitiveType = CellGcmPrimitive.TRIANGLE_STRIP;

    public SoftbodyClusterData SoftbodyCluster;
    public SoftbodySpring[]? SoftbodySprings;

    /**
     * Vertices that are equivalent, but only separate for texturing reasons.
     */
    public SoftbodyVertEquivalence[]? SoftbodyEquivs;

    /**
     * Mass of each vertex, used for softbody physics.
     */
    public float[] Mass;

    public ImplicitEllipsoid[]? ImplicitEllipsoids = [];
    public Matrix4x4[] ClusterImplicitEllipsoids = [];
    public ImplicitEllipsoid[]? InsideImplicitEllipsoids = [];
    public ImplicitPlane[]? ImplicitPlanes = [];

    /**
     * Settings for how this mesh's softbody physics behave.
     */
    public ResourceDescriptor SoftPhysSettings;

    /**
     * Min and max vertices where this mesh contains springs.
     */
    public int MinSpringVert, MaxSpringVert;

    public int MinUnalignedSpringVert;

    /**
     * Indices of the mesh that are springy.
     */
    public short[] SpringyTriIndices;

    /**
     * Whether the mesh's spring indices
     * are tri-stripped or not.
     * (Stored as an integer, but only used as a boolean)
     */
    public bool SpringTrisStripped;

    public Vector4? SoftbodyContainingBoundBoxMin;
    public Vector4? SoftbodyContainingBoundBoxMax;

    /**
     * Bones that control render culling behavior.
     */
    public CullBone[]? CullBones;

    /**
     * Parts of the player mesh that gets hidden
     * when you wear this mesh as a costume piece.
     */
    public int[] RegionIDsToHide;

    /**
     * Costume slots this mesh takes up.
     */
    public int CostumeCategoriesUsed;

    /**
     * How this costume piece hides a player's
     * hair when it's equipped.
     */
    public HairMorph HairMorphs = HairMorph.HAT;

    public int BevelVertexCount;
    public bool ImplicitBevelSprings;

    public uint[] VertexColors;

    /**
     * Which character this mesh is for.
     */
    public SkeletonType SkeletonType = SkeletonType.SACKBOY;

    /* Creates an empty mesh, used for serialization. */
    public RMesh() { }

    public RMesh(byte[][] streams, byte[] attributes, byte[] indices, Bone[] bones)
    {
        // We're not using triangle strips, we're using triangle lists,
        // so treat the indices buffer as such.
        PrimitiveType = CellGcmPrimitive.TRIANGLES;
        if (indices != null)
        {
            if (indices.Length % 0x6 != 0)
                throw new ArgumentException("Indices buffer must be divisible by " +
                                            "0x6 since" +
                                            " it contains triangle data!");
            NumTris = indices.Length / 0x6;
            NumIndices = indices.Length / 0x2;
        }
        Init(streams, attributes, indices, bones);
    }

    /**
     * Creates a new mesh from raw streams.
     * NOTE: Edge indices are not included.
     * NOTE: Primitive type is assumed to be GL_TRIANGLES.
     *
     * @param streams    All vertex streams, including geometry, skinning/lighting, and morphs.
     * @param attributes Texture coordinate (UV) data stream.
     * @param indices    Face indices stream
     * @param bones      Skeleton of this mesh
     */
    public RMesh(byte[][] streams, byte[] attributes, byte[] indices, Bone[] bones,
                 int numIndices, int numEdgeIndices, int numTris)
    {
        NumIndices = numIndices;
        NumEdgeIndices = numEdgeIndices;
        NumTris = numTris;
        Init(streams, attributes, indices, bones);
    }

    private void Init(byte[][] streams, byte[] attributes, byte[] indices, Bone[] bones)
    {
        // Return an empty mesh if the streams are null or empty,
        // it's effectively as such anyway.
        if (streams == null || streams.Length == 0) return;

        if (streams.Length > 34)
            throw new ArgumentException("Meshes cannot have more than 32 morphs!");

        // Validate the vertex streams, we won't check
        // if the data in the streams are valid, because that's
        // the user's problem!
        var size = streams[0].Length;
        foreach (var stream in streams)
        {
            if (stream.Length != size)
                throw new ArgumentException("All vertex streams must be the same " +
                                                   "size!");
            if (stream.Length % 0x10 != 0)
                throw new ArgumentException("All vertex streams must be divisible " +
                                                   "by 0x10!");
        }

        Streams = streams;
        StreamCount = streams.Length;
        NumVerts = size / 0x10;

        // This won't be used if the mesh doesn't have softbody
        // data, but we'll still create the array anway.
        Mass = new float[NumVerts];

        // If the stream Length is 2, it means we
        // only have geometry and skinning data, no morph data.
        if (Streams.Length > 2)
        {
            MorphCount = Streams.Length - 2;
            MirrorMorphs = new short[MorphCount];
        }
        else MorphCount = 0;

        if (attributes != null)
        {
            // A single attribute stream should consist of
            // UV0 (8 bytes per vertex).
            var attributeSize = NumVerts * 0x8;

            if (attributes.Length % attributeSize != 0)
                throw new ArgumentException("Attribute buffer doesn't match vertex" +
                                                   " count!");

            AttributeCount = attributes.Length / attributeSize;
        }
        Attributes = attributes;
        Indices = indices;

        // Initialize the mirror arrays if the bones aren't null.
        if (bones != null)
        {
            MirrorBones = new short[bones.Length];
            for (var i = 0; i < MirrorBones.Length; ++i)
                MirrorBones[i] = (short) i;
            MirrorBoneFlipTypes = new FlipType[bones.Length];
            Array.Fill(MirrorBoneFlipTypes, FlipType.MAX);
        }
        Bones = bones;
        CullBones = new CullBone[Bones.Length];
        for (var i = 0; i < Bones.Length; ++i)
        {
            var bone = new CullBone
            {
                BoundBoxMax = Bones[i].BoundBoxMax,
                BoundBoxMin = Bones[i].BoundBoxMin,
                InvSkinPoseMatrix = Bones[i].InvSkinPoseMatrix
            };
            CullBones[i] = bone;
        }

        VertexColors = new uint[NumVerts];
        for (var i = 0; i < NumVerts; ++i)
            VertexColors[i] = 0xFFFFFFFF;
    }

    public override void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        NumVerts = serializer.I32(NumVerts);
        NumIndices = serializer.I32(NumIndices);
        NumEdgeIndices = serializer.I32(NumEdgeIndices);
        NumTris = serializer.I32(NumTris);
        StreamCount = serializer.I32(StreamCount);
        AttributeCount = serializer.I32(AttributeCount);
        MorphCount = serializer.I32(MorphCount);

        if (!serializer.IsWriting())
            MorphNames = new string[MaxMorphs];
        for (var i = 0; i < MaxMorphs; ++i)
            MorphNames[i] = serializer.Str(MorphNames[i], 0x10);

        if (version >= (int)Revisions.MESH_MINMAX_UV)
        {
            MinUv = serializer.Floatarray(MinUv);
            MaxUv = serializer.Floatarray(MaxUv);
            AreaScaleFactor = serializer.F32(AreaScaleFactor);
        }

        if (serializer.IsWriting())
        {
            var offset = 0;
            var stream = serializer.GetOutput();
            stream.I32(offset);
            for (var i = 0; i < Streams.Length; ++i)
            {
                offset += Streams[i].Length;
                stream.I32(offset);
            }
            stream.I32(offset);
            foreach (var vertexStream in Streams)
                stream.Bytes(vertexStream);
        }
        else
        {
            var stream = serializer.GetInput();
            // We're skipping source stream offsets.
            for (var i = 0; i < StreamCount + 1; ++i)
                stream.I32();
            stream.I32();

            Streams = new byte[StreamCount][];
            for (var i = 0; i < StreamCount; ++i)
                Streams[i] = stream.Bytes(NumVerts * 0x10);
        }

        Attributes = serializer.Bytearray(Attributes);
        Indices = serializer.Bytearray(Indices);
        if (subVersion >= (int)Revisions.FUZZ)
            TriangleAdjacencyInfos = serializer.Bytearray(TriangleAdjacencyInfos);

        Primitives = serializer.Arraylist(Primitives);
        Bones = serializer.Array(Bones);

        MirrorBones = serializer.Shortarray(MirrorBones);
        MirrorBoneFlipTypes = serializer.Enumarray(MirrorBoneFlipTypes);
        MirrorMorphs = serializer.Shortarray(MirrorMorphs);

        PrimitiveType = serializer.Enum8(PrimitiveType);

        SoftbodyCluster = serializer.Struct(SoftbodyCluster);
        SoftbodySprings = serializer.Array(SoftbodySprings);
        SoftbodyEquivs = serializer.Array(SoftbodyEquivs);

        // Don't write mass field if there's no softbody data on the this.
        if (serializer.IsWriting())
        {
            if (HasSoftbodyData()) serializer.Floatarray(Mass);
            else serializer.GetOutput().I32(0);
        }
        else Mass = serializer.Floatarray(Mass);

        ImplicitEllipsoids = serializer.Array(ImplicitEllipsoids);

        if (!serializer.IsWriting())
            ClusterImplicitEllipsoids = new Matrix4x4[serializer.GetInput().I32()];
        else serializer.GetOutput().I32(ClusterImplicitEllipsoids.Length);
        for (var i = 0; i < ClusterImplicitEllipsoids.Length; ++i)
            ClusterImplicitEllipsoids[i] =
                serializer.M44(ClusterImplicitEllipsoids[i])!.Value;

        InsideImplicitEllipsoids = serializer.Array(InsideImplicitEllipsoids);
        ImplicitPlanes = serializer.Array(ImplicitPlanes);
        SoftPhysSettings = serializer.Resource(SoftPhysSettings,
            ResourceType.SettingsSoftPhys);
        MinSpringVert = serializer.I32(MinSpringVert);
        MaxSpringVert = serializer.I32(MaxSpringVert);
        MinUnalignedSpringVert = serializer.I32(MinUnalignedSpringVert);
        SpringyTriIndices = serializer.Shortarray(SpringyTriIndices);
        SpringTrisStripped = serializer.Intbool(SpringTrisStripped);
        SoftbodyContainingBoundBoxMin = serializer.V4(SoftbodyContainingBoundBoxMin);
        SoftbodyContainingBoundBoxMax = serializer.V4(SoftbodyContainingBoundBoxMax);

        CullBones = serializer.Array(CullBones);
        RegionIDsToHide = serializer.Intvector(RegionIDsToHide);

        CostumeCategoriesUsed = serializer.I32(CostumeCategoriesUsed);
        if (version >= 0x141)
            HairMorphs = serializer.Enum32(HairMorphs);
        BevelVertexCount = serializer.I32(BevelVertexCount);
        ImplicitBevelSprings = serializer.Bool(ImplicitBevelSprings);

        if (revision.Has(Branch.Double11, (int)Revisions.D1_VERTEX_COLORS))
        {
            if (!serializer.IsWriting())
                VertexColors = new uint[serializer.GetInput().I32()];
            else serializer.GetOutput().I32(VertexColors.Length);
            for (var i = 0; i < VertexColors.Length; ++i)
                VertexColors[i] = (uint)serializer.I32((int)VertexColors[i], true);
        }
        else if (!serializer.IsWriting())
        {
            VertexColors = new uint[NumVerts];
            for (var i = 0; i < NumVerts; ++i)
                VertexColors[i] = 0xFFFFFFFF;
        }

        if (subVersion >= (int)Revisions.MESH_SKELETON_TYPE)
            SkeletonType = serializer.Enum8(SkeletonType);
    }

    public override int GetAllocatedSize()
    {
        var size = BaseAllocationSize;

        if (Streams != null) size += Streams.Sum(stream => stream.Length);
        if (Attributes != null) size += Attributes.Length;
        if (Indices != null) size += Indices.Length;
        if (TriangleAdjacencyInfos != null) size += TriangleAdjacencyInfos.Length;

        if (Primitives != null)
            size += Primitives.Count * Primitive.BaseAllocationSize;

        if (Bones != null)
            foreach (var bone in Bones)
                size += bone.GetAllocatedSize();

        if (MirrorBones != null) size += MirrorBones.Length * 2;
        if (MirrorBoneFlipTypes != null) size += MirrorBoneFlipTypes.Length;
        if (MirrorMorphs != null) size += MirrorMorphs.Length * 2;
        if (SoftbodyCluster != null) size += SoftbodyCluster.GetAllocatedSize();
        if (SoftbodySprings != null)
            size += SoftbodySprings.Length * SoftbodySpring.BaseAllocationSize;
        if (SoftbodyEquivs != null)
            size += SoftbodyEquivs.Length * SoftbodyVertEquivalence.BaseAllocationSize;
        if (Mass != null) size += Mass.Length * 4;
        if (ImplicitEllipsoids != null)
            size += ImplicitEllipsoids.Length * ImplicitEllipsoid.BaseAllocationSize;
        if (ClusterImplicitEllipsoids != null)
            size += ClusterImplicitEllipsoids.Length * 0x40;
        if (InsideImplicitEllipsoids != null)
            size += InsideImplicitEllipsoids.Length * ImplicitEllipsoid.BaseAllocationSize;
        if (ImplicitPlanes != null)
            size += ImplicitPlanes.Length * ImplicitPlane.BaseAllocationSize;
        if (SpringyTriIndices != null) size += SpringyTriIndices.Length * 2;
        if (CullBones != null)
            size += CullBones.Length * CullBone.BaseAllocationSize;
        if (RegionIDsToHide != null) size += RegionIDsToHide.Length * 0x8;

        return size;
    }
    
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        var serializer = new Serializer(GetAllocatedSize() + 0x8000, revision,
            compressionFlags);
        serializer.Struct(this);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Mesh,
            SerializationType.BINARY,
            serializer.GetDependencies()
        );
    }

    /**
     * Checks if this mesh uses tri-strips.
     *
     * @return True if the mesh uses tri-strips, false otherwise.
     */
    public bool IsStripped()
    {
        return PrimitiveType.Equals(CellGcmPrimitive.TRIANGLE_STRIP);
    }

    /**
     * Checks if this mesh uses softbody simulation.
     *
     * @return True if the mesh uses softbody simulation, false otherwise.
     */
    public bool HasSoftbodyData()
    {
        if (SoftbodyCluster == null) return false;
        return SoftbodyCluster.Clusters.Count != 0;
    }

    public Vector2 GetMinUv()
    {
        if (MinUv is not { Length: 2 })
            return Vector2.Zero;
        return new Vector2(MinUv[0], MinUv[1]);
    }

    public Vector2 GetMaxUv()
    {
        if (MaxUv is not { Length: 2 })
            return Vector2.Zero;
        return new Vector2(MaxUv[0], MaxUv[1]);
    }

    /**
     * Gets a stream at specified index.
     *
     * @param index Index of stream
     * @return Stream at index
     */
    public byte[] GetVertexStream(int index)
    {
        if (Streams == null || index < 0 || index >= Streams.Length)
            throw new NullReferenceException("Vertex stream at position " + index + " does " +
                                             "not " +
                                             "exist!");
        return Streams[index];
    }

    /**
     * Gets the main vertex stream.
     *
     * @return Main vertex stream.
     */
    public byte[] GetVertexStream()
    {
        if (Streams.Length <= StreamPosBoneindices)
            throw new NullReferenceException("Vertex stream doesn't exist on this mesh!");
        return Streams[StreamPosBoneindices];
    }

    /**
     * Gets the stream that contains skinning data,
     * as well as normals, tangents, and smooth normals.
     *
     * @return Skinning/Normal stream.
     */
    public byte[] GetSkinningStream()
    {
        if (Streams.Length <= StreamBoneweightsNormTangentSmoothNorm)
            throw new NullReferenceException("Skinning stream doesn't exist on this mesh!");
        return Streams[StreamBoneweightsNormTangentSmoothNorm];
    }

    /**
     * Gets all streams containing morph data.
     *
     * @return Morph data streams
     */
    public byte[][] GetMorphStreams()
    {
        byte[][] streams = new byte[MorphCount][];
        if (StreamCount - 2 >= 0)
            Array.Copy(Streams, 2, streams, 0, StreamCount - 2);
        return streams;
    }

    public void SetMorphNames(string[] names)
    {
        if (names == null)
            throw new NullReferenceException("Morph names cannot be null!");
        if (names.Length != MaxMorphs)
            throw new ArgumentException("Morph name array must have Length of 32!");
        MorphNames = names;
    }

    /**
     * Sets the morph name at index.
     *
     * @param name  Morph name
     * @param index Index of morph
     */
    public void SetMorphName(string name, int index)
    {
        MorphNames[index] = name;
    }

    public void SetMinUv(Vector2 minUv)
    {
        if (minUv == null)
        {
            MinUv = null;
            return;
        }
        MinUv = [minUv.X, minUv.Y];
    }

    public void SetMaxUv(Vector2 maxUv)
    {
        if (maxUv == null)
        {
            MaxUv = null;
            return;
        }
        MaxUv = [maxUv.X, maxUv.Y];
    }

    public void SetAreaScaleFactor(float factor)
    {
        AreaScaleFactor = factor;
    }

    public void SetPrimitives(List<Primitive> primitives)
    {
        Primitives = primitives;
    }

    /**
     * Sets a bone's mirror.
     *
     * @param index  Bone index
     * @param mirror Bone mirror index
     */
    public void SetBoneMirror(int index, int mirror)
    {
        if (MirrorBones == null || index < 0 || index >= MirrorBones.Length)
            throw new NullReferenceException("Bone at position " + index + " does not exist!");
        if (MirrorBones == null || mirror < 0 || mirror >= MirrorBones.Length)
            throw new NullReferenceException("Bone at position " + mirror + " does not exist!");
        MirrorBones[index] = (short) (mirror & 0xFFFF);
    }

    /**
     * Sets a bone's flip type.
     *
     * @param index Bone index
     * @param type  Bone flip type
     */
    public void SetBoneFlipType(int index, FlipType type)
    {
        if (MirrorBones == null || index < 0 || index >= MirrorBones.Length)
            throw new NullReferenceException("Bone at position " + index + " does not exist!");
        MirrorBoneFlipTypes[index] = type;
    }

    public void SetMorphMirror(int index, int mirror)
    {
        if (MirrorMorphs == null || index < 0 || index >= MirrorMorphs.Length)
            throw new NullReferenceException("Morph at position " + index + " does not exist!");
        if (MirrorMorphs == null || mirror < 0 || mirror >= MirrorMorphs.Length)
            throw new NullReferenceException("Morph at position " + mirror + " does not " +
                                           "exist!");
        MirrorMorphs[index] = (short) (mirror & 0xFFFF);
    }
    
    /**
     * Sets the mass of a vertex at a given position,
     * this is used for softbody simulations.
     *
     * @param index Index of the vertex
     * @param mass  New mass of the vertex
     */
    public void SetVertexMass(int index, float mass)
    {
        if (Mass == null || Mass.Length != NumVerts)
        {
            Mass = new float[NumVerts];
            for (var i = 0; i < NumVerts; ++i)
                Mass[i] = 1.0f;
        }

        if (index < 0 || index >= Mass.Length)
            throw new NullReferenceException("Vertex at position " + index + " does not " +
                                           "exist!");
        Mass[index] = mass;
    }

    /**
     * Gets all sub-meshes contained in this this.
     *
     * @return Sub-meshes
     */
    public Primitive[][] GetSubmeshes()
    {
        Dictionary<int, List<Primitive>> meshes = new();
        foreach (var primitive in Primitives)
        {
            var region = primitive.Region;
            if (!meshes.ContainsKey(region))
            {
                List<Primitive> primitives =
                [
                    primitive
                ];
                meshes.Add(region, primitives);
            }
            else meshes[region].Add(primitive);
        }

        Primitive[][] submeshes = new Primitive[meshes.Values.Count][];

        var index = 0;
        foreach (List<Primitive> primitiveList in meshes.Values)
        {
            submeshes[index] = primitiveList.ToArray();
            ++index;
        }

        return submeshes;
    }

    /**
     * Gets all the vertices in a specified range
     *
     * @param start First vertex
     * @param count Number of vertices
     * @return Vertices of range
     */
    public Vector3[] GetVertices(int start, int count)
    {
        var stream = new MemoryInputStream(GetVertexStream());
        stream.Seek(start * 0x10);
        var vertices = new Vector3[count];
        for (var i = 0; i < count; ++i)
        {
            vertices[i] = stream.V3();

            // stream.i8(cluster_index * 2);
            // stream.i8(0)
            // stream.i8(0)
            // stream.i8(vertex_weight)

            stream.I32(true);
        }
        return vertices;
    }

    /**
     * Gets all the softbody weights in a specified range
     *
     * @param start First vertex
     * @param count Number of vertices
     * @return Softbody weights of range
     */
    public float[] GetSoftbodyWeights(int start, int count)
    {
        var stream = new MemoryInputStream(GetVertexStream());
        stream.Seek(start * 0x10);
        var weights = new float[count];
        for (var i = 0; i < count; ++i)
        {
            stream.Seek(0xC + 0x3);
            var c = stream.U8() / (float) 0xff;
            weights[i] = c;
        }
        return weights;
    }

    public byte[][] GetBlendIndices(int start, int count)
    {
        var stream = new MemoryInputStream(GetVertexStream());
        stream.Seek(start * 0x10);
        byte[][] vertices = new byte[count][];
        for (var i = 0; i < count; ++i)
        {
            stream.V3();

            // This is always 0x000000FF, the field is bone indices, but those seem
            // depreciated.
            // It's probably only kept for alignment reasons?
            vertices[i] = stream.Bytes(4);
        }
        return vertices;
    }

    /**
     * Gets all the vertices from a mesh primitive.
     *
     * @param primitive Mesh primitive to get vertices from
     * @return Vertices
     */
    public Vector3[] GetVertices(Primitive primitive)
    {
        return GetVertices(
            primitive.MinVert,
            primitive.MaxVert - primitive.MinVert + 1
        );
    }

    /**
     * Gets all the vertices from this this.
     *
     * @return Vertices
     */
    public Vector3[] GetVertices()
    {
        return GetVertices(0, NumVerts);
    }

    public void SwapUv01()
    {
        for (var i = 0; i < NumVerts; i++)
        {
            var uv0 = AttributeCount * 0x8 * i;
            var uv1 = uv0 + 0x8;

            var tmp = Attributes.Skip(uv0).Take(0x8).ToArray(); // copy uv0 into tmp
            Array.Copy(Attributes, uv1, Attributes, uv0, 0x8);
            Array.Copy(tmp, 0, Attributes, uv1, 0x8);
        }
    }

    /**
     * Gets the UVs in a specified range.
     *
     * @param start   First vertex to get UVs of
     * @param count   Number of UVs
     * @param channel UV channel
     * @return UVs
     */
    public Vector2[] GetUVs(int start, int count, int channel)
    {
        if (Attributes == null)
            throw new InvalidOperationException("This mesh doesn't have texture coordinates!");
        if (channel < 0 || channel + 1 > AttributeCount)
            throw new ArgumentException("Invalid UV channel!");
        var stream = new MemoryInputStream(Attributes);
        var uVs = new Vector2[count];
        for (var i = 0; i < count; ++i)
        {
            stream.Seek(start + AttributeCount * 0x8 * i + 0x8 * channel,
                SeekMode.Begin);
            uVs[i] = stream.V2();
        }
        return uVs;
    }


    /**
     * Gets the UVs of a mesh primitive from a specified channel
     *
     * @param primitive Primitive to get UVs from
     * @param channel   UV channel
     * @return UVs
     */
    public Vector2[] GetUVs(Primitive primitive, int channel)
    {
        return GetUVs(
            primitive.MinVert,
            primitive.MaxVert - primitive.MinVert + 1,
            channel
        );
    }

    /**
     * Gets the UVs of this mesh from a specified channel.
     *
     * @param channel UV channel
     * @return UVs
     */
    public Vector2[] GetUVs(int channel)
    {
        return GetUVs(0, NumVerts, channel);
    }

    /**
     * Gets the vertex normals in a specified range.
     *
     * @param start First vertex to get normals of
     * @param count Number of vertices
     * @return Vertex normals of range
     */
    public Vector3[] GetNormals(int start, int count)
    {
        var stream = new MemoryInputStream(GetSkinningStream());
        var normals = new Vector3[count];
        for (var i = 0; i < count; ++i)
        {
            stream.Seek(start * 0x10 + i * 0x10 + 0x4, SeekMode.Begin);
            normals[i] = Bytes.UnpackNormal24(stream.U24());
        }
        return normals;
    }

    /**
     * Gets all the vertex normals from this this.
     *
     * @return Vertex normals
     */
    public Vector3[] GetNormals()
    {
        return GetNormals(0, NumVerts);
    }

    /**
     * Gets all the vertex normals from a mesh primitive.
     *
     * @param primitive Primitive to get vertex normals from
     * @return Vertex normals
     */
    public Vector3[] GetNormals(Primitive primitive)
    {
        return GetNormals(
            primitive.MinVert,
            primitive.MaxVert - primitive.MinVert + 1
        );
    }

    /**
     * Gets all the vertex tangents from this this.
     *
     * @return Vertex tangents
     */
    public Vector4[] GetTangents()
    {
        return GetTangents(0, NumVerts);
    }

    /**
     * Gets the vertex tangents in a specified range.
     *
     * @param start First vertex to get tangents of
     * @param count Number of vertices
     * @return Vertex tangents of range
     */
    public Vector4[] GetTangents(int start, int count)
    {
        var stream = new MemoryInputStream(GetSkinningStream());
        var tangents = new Vector4[count];
        for (var i = 0; i < count; ++i)
        {
            stream.Seek(start * 0x10 + i * 0x10 + 0x8, SeekMode.Begin);
            var tangent = Bytes.UnpackNormal24(stream.U24());
            tangents[i] = new Vector4(tangent, 1.0f);
        }
        return tangents;
    }

    /**
     * Gets all the vertex tangents from a mesh primitive.
     *
     * @param primitive Primitive to get vertex tangents from
     * @return Vertex tangents
     */
    public Vector4[] GetTangents(Primitive primitive)
    {
        return GetTangents(
            primitive.MinVert,
            primitive.MaxVert - primitive.MinVert + 1
        );
    }

    /**
     * Gets the smooth vertex normals in a specified range.
     *
     * @param start First vertex to get smooth normals of
     * @param count Number of vertices
     * @return Smooth vertex normals of range
     */
    public Vector3[] GetSmoothNormals(int start, int count)
    {
        var stream = new MemoryInputStream(GetSkinningStream());
        var normals = new Vector3[count];
        for (var i = 0; i < count; ++i)
        {
            stream.Seek(start * 0x10 + i * 0x10 + 0xC, SeekMode.Begin);
            normals[i] = Bytes.UnpackNormal24(stream.U24());
        }
        return normals;
    }

    /**
     * Gets all the smooth vertex normals from this this.
     *
     * @return Smooth vertex normals
     */
    public Vector3[] GetSmoothNormals()
    {
        return GetSmoothNormals(0, NumVerts);
    }

    /**
     * Gets all the smooth vertex normals from a mesh primitive.
     *
     * @param primitive Primitive to get smooth vertex normals from
     * @return Smooth vertex normals
     */
    public Vector3[] GetSmoothNormals(Primitive primitive)
    {
        return GetSmoothNormals(
            primitive.MinVert,
            primitive.MaxVert - primitive.MinVert + 1
        );
    }

    /**
     * Gets the joints that have an influence on each vertex in a range.
     *
     * @param start First vertex to get joints of
     * @param count Number of vertices
     * @return Joint indices per vertex in range
     */
    public byte[][] GetJoints(int start, int count)
    {
        var stream = new MemoryInputStream(GetSkinningStream());
        stream.Seek(start * 0x10);
        byte[][] joints = new byte[count][];
        for (var i = 0; i < count; ++i)
        {
            var buffer = stream.Bytes(0x10);
            joints[i] =
            [
                buffer[3], buffer[7], buffer[0xB], buffer[0xF]
            ];
        }
        return joints;
    }

    /**
     * Gets the joints that have an influence on each vertex in a mesh primitive.
     *
     * @param primitive Primitive to get vertices from
     * @return Joint indices per vertex
     */
    public byte[][] GetJoints(Primitive primitive)
    {
        return GetJoints(
            primitive.MinVert,
            primitive.MaxVert - primitive.MinVert + 1
        );
    }

    /**
     * Gets the joints that have an influence on each vertex.
     *
     * @return Joint indices per vertex
     */
    public byte[][] GetJoints()
    {
        return GetJoints(0, NumVerts);
    }

    /**
     * Gets a number of weight influences from this skinned mesh,
     * starting from a certain position
     *
     * @param start First vertex to get weights of
     * @param count Number of vertices to get weights of
     * @return Weight influences of vertex range
     */
    public Vector4[] GetWeights(int start, int count)
    {
        var stream =
            new MemoryInputStream(GetSkinningStream());
        stream.Seek(start * 0x10);
        var weights = new Vector4[count];
        for (var i = 0; i < count; ++i)
        {
            var buffer = stream.Bytes(0x10);
            float[] rawWeights =
            [
                buffer[2] & 0xFF,
                buffer[1] & 0xFF,
                buffer[0] & 0xFF,
                0
            ];

            // If this vertex isn't weighted against a single bone,
            // we'll need to calculate the last weight.
            if (rawWeights[0] != 0xFF)
            {
                rawWeights[3] = 0xFF - rawWeights[2] - rawWeights[1] - rawWeights[0];
                weights[i] = new Vector4(
                    rawWeights[0] / 0xFF,
                    rawWeights[1] / 0xFF,
                    rawWeights[2] / 0xFF,
                    rawWeights[3] / 0xFF
                );
            }
            else weights[i] = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
        }
        return weights;
    }

    /**
     * Gets all weight influences from a mesh primitive.
     *
     * @param primitive Primitive to get weights of
     * @return Weight influences of given primitive
     */
    public Vector4[] GetWeights(Primitive primitive)
    {
        return GetWeights(
            primitive.MinVert,
            primitive.MaxVert - primitive.MinVert + 1
        );
    }

    /**
     * Gets all weight influences from this skinned this.
     *
     * @return Weight influences
     */
    public Vector4[] GetWeights()
    {
        return GetWeights(0, NumVerts);
    }

    /**
     * Parses all morphs from the morph streams.
     *
     * @return Parsed morphs
     */
    public Morph[] GetMorphs()
    {
        if (MorphCount == 0)
            throw new InvalidOperationException("Can't get morphs from mesh that has no morph " +
                                                "data!");
        Morph[] morphs = new Morph[MorphCount];
        for (var i = 0; i < MorphCount; ++i)
        {
            var stream = new MemoryInputStream(Streams[i + 2]);
            var offsets = new Vector3[NumVerts];
            var normals = new Vector3[NumVerts];
            for (var j = 0; j < NumVerts; ++j)
            {
                offsets[j] = stream.V3();
                normals[j] = Bytes.UnpackNormal32(stream.U32(true));
            }
            morphs[i] = new Morph(offsets, normals);
        }
        return morphs;
    }

    /**
     * Calculates a triangle list from a given range in the index buffer.
     *
     * @param start First face to include in list
     * @param count Number of faces from start
     * @return Mesh's triangle list
     */
    public int[] GetSpringyTriangles(int start, int count)
    {
        if (Indices == null)
            throw new InvalidOperationException("Can't get triangles from mesh without index " +
                                                "buffer!");

        var faces = new int[count];
        var stream = SpringyTriIndices;
        for (var i = start; i < count; ++i)
            faces[i] = stream[i] & 0xffff;

        if (!SpringTrisStripped) return faces;

        var triangles = new List<int>(NumVerts * 0x3);
        triangles.AddRange([faces[0], faces[1], faces[2]]);
        for (int i = 3, j = 1; i < faces.Length; ++i, ++j)
        {
            if (faces[i] == 65535)
            {
                triangles.AddRange([faces[i + 1], faces[i + 2], faces[i + 3]]);
                i += 3;
                j = 0;
                continue;
            }
            if ((j & 1) != 0)
                triangles.AddRange([faces[i - 2], faces[i], faces[i - 1]]);
            else triangles.AddRange([faces[i - 2], faces[i - 1], faces[i]]);
        }

        return triangles.ToArray();
    }

    /**
     * Calculates a triangle list from a given range in the index buffer.
     *
     * @param start First face to include in list
     * @param count Number of faces from start
     * @return Mesh's triangle list
     */
    public int[] GetTriangles(int start, int count)
    {
        if (Indices == null)
            throw new InvalidOperationException("Can't get triangles from mesh without index " +
                                                "buffer!");

        var faces = new int[count];
        var stream = new MemoryInputStream(Indices);
        stream.Seek(start * 0x2);
        for (var i = 0; i < count; ++i)
            faces[i] = stream.U16();

        if (!IsStripped()) return faces;

        List<int> triangles = new(NumVerts * 0x3);
        triangles.AddRange([faces[0], faces[1], faces[2]]);
        for (int i = 3, j = 1; i < faces.Length; ++i, ++j)
        {
            if (faces[i] == 65535)
            {
                if (i + 3 >= count) break;
                triangles.AddRange([faces[i + 1], faces[i + 2], faces[i + 3]]);
                i += 3;
                j = 0;
                continue;
            }
            if ((j & 1) != 0)
                triangles.AddRange([faces[i - 2], faces[i], faces[i - 1]]);
            else triangles.AddRange([faces[i - 2], faces[i - 1], faces[i]]);
        }

        return triangles.ToArray();
    }

    /**
     * Calcuates a triangle list from a given mesh primitive.
     *
     * @param primitive Primitive to get triangles from
     * @return Mesh's triangle list
     */
    public int[] GetTriangles(Primitive primitive)
    {
        if (primitive == null)
            throw new NullReferenceException("Can't get triangles from null primitive!");
        return GetTriangles(primitive.FirstIndex, primitive.NumIndices);
    }

    /**
     * Gets this mesh's triangle list, triangulating if necessary.
     *
     * @return Mesh's triangle list
     */
    public int[] GetTriangles()
    {
        return GetTriangles(0, NumIndices);
    }

    public void ApplySkeleton(Skeleton skeleton)
    {
        if (skeleton == null) return;
        Bones = skeleton.Bones;
        CullBones = skeleton.CullBones;
        MirrorBoneFlipTypes = skeleton.MirrorType;
        MirrorBones = skeleton.Mirror;

    }

    public void FixupSkinForExport()
    {
        var weights = GetWeights();
        int[] modulo = [0x3, 0x7, 0xb, 0xf];
        for (var i = 0; i < NumVerts; ++i)
        {
            var stream = GetSkinningStream();
            for (var j = 0; j < 4; ++j)
            {
                if (weights[i].GetComponent(j) == 0.0f)
                    stream[i * 0x10 + modulo[j]] = 0;
            }

            for (var j = 0; j < 4; ++j)
            {
                var joint = stream[i * 0x10 + modulo[j]] - 1;
                if (joint == -1) joint = 0;
                stream[i * 0x10 + modulo[j]] = (byte) joint;
            }
        }

        foreach (var bone in Bones)
        {
            var inverse = bone.InvSkinPoseMatrix.Value;
            inverse.M14 = 0.0f;
            inverse.M24 =0.0f;
            inverse.M34 = 0.0f;
            inverse.M44 = 1.0f;
        }
    }

    public Submesh[] FindAllMeshes()
    {
        List<Submesh> meshes = [];
        for (var boneIndex = 0; boneIndex < Bones.Length; ++boneIndex)
        {
            var bone = Bones[boneIndex];
            if (bone.Parent != -1) continue;

            var mesh = new Submesh
            {
                Transform = bone.SkinPoseMatrix,
                Locator = boneIndex,
                Skinned = bone.FirstChild != -1
            };

            List<Primitive> primitives = [];
            var stream = GetSkinningStream();
            foreach (var primitive in Primitives)
            {
                // Test the first vertex of each primitive to see
                // if they belong to this subthis.
                var joint = Bones[stream[primitive.MinVert * 0x10 + 0x3]];
                while (joint.Parent != -1)
                    joint = Bones[joint.Parent];

                if (joint == bone)
                    primitives.Add(primitive);
            }

            mesh.Primitives = primitives.ToArray();
            meshes.Add(mesh);
        }

        return meshes.ToArray();
    }

    /**
     * Recalculates the bounding boxes for each bone in this model.
     *
     * @param setOBB Whether to set the OBB field from AABB bounding boxes.
     */
    public void CalculateBoundBoxes(bool setObb)
    {
        var vertices = GetVertices();
        var weights = GetWeights();
        byte[][] joints = GetJoints();

        Dictionary<Bone, Vector3> minVert = new();
        Dictionary<Bone, Vector3> maxVert = new();

        foreach (var bone in Bones)
        {
            minVert.Add(bone, new Vector3(float.PositiveInfinity, float.PositiveInfinity,
                float.PositiveInfinity));
            maxVert.Add(bone, new Vector3(float.NegativeInfinity, float.NegativeInfinity,
                float.NegativeInfinity));
        }

        for (var i = 0; i < vertices.Length; ++i)
        {
            var v = vertices[i];
            var weightCache = weights[i];
            var jointCache = joints[i];

            for (var j = 0; j < 4; ++j)
            {
                if (weightCache.GetComponent(j) == 0.0f) continue;
                var max = maxVert[Bones[jointCache[j]]];
                var min = minVert[Bones[jointCache[j]]];


                if (v.X > max.X) max.X = v.X;
                if (v.Y > max.Y) max.Y = v.Y;
                if (v.Z > max.Z) max.Z = v.Z;

                if (v.X < min.X) min.X = v.X;
                if (v.Y < min.Y) min.Y = v.Y;
                if (v.Z < min.Z) min.Z = v.Z;
            }
        }

        var index = 0;
        foreach (var bone in Bones)
        {
            var max = new Vector4(maxVert[bone], 1.0f);
            var min = new Vector4(minVert[bone], 1.0f);

            min = float.IsPositiveInfinity(min.X) ? new Vector4(0.0f, 0.0f, 0.0f, 1.0f) : Vector4.Transform(min, bone.InvSkinPoseMatrix.Value);


            max = float.IsNegativeInfinity(max.X) ? new Vector4(0.0f, 0.0f, 0.0f, 1.0f) : Vector4.Transform(max, bone.InvSkinPoseMatrix.Value);

            for (var c = 0; c < 3; ++c)
            {
                if (!(min.GetComponent(c) > max.GetComponent(c))) continue;
                var u = min.GetComponent(c);
                var l = max.GetComponent(c);
                min.SetComponent(c, l);
                max.SetComponent(c, u);
            }

            bone.BoundBoxMax = max;
            bone.BoundBoxMin = min;
            if (setObb)
            {
                bone.ObbMax = bone.BoundBoxMax;
                bone.ObbMin = bone.BoundBoxMin;
            }

            var center = (max + min) / 2.0f;
            var minDist = Math.Abs(Vector4.Distance(center, min));
            var maxDist = Math.Abs(Vector4.Distance(center,max));
            center.W = minDist > maxDist ? minDist : maxDist;
            bone.BoundSphere = center;

            var culler = CullBones[index++];
            culler.BoundBoxMax = bone.BoundBoxMax;
            culler.BoundBoxMin = bone.BoundBoxMin;
            culler.InvSkinPoseMatrix = bone.InvSkinPoseMatrix;
        }
    }
}