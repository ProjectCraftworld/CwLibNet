using System.Collections;
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
    public Bone[] Bones;

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
    public SoftbodySpring[] SoftbodySprings;

    /**
     * Vertices that are equivalent, but only separate for texturing reasons.
     */
    public SoftbodyVertEquivalence[] SoftbodyEquivs;

    /**
     * Mass of each vertex, used for softbody physics.
     */
    public float[] Mass;

    public ImplicitEllipsoid[] ImplicitEllipsoids = new ImplicitEllipsoid[0];
    public Matrix4x4[] ClusterImplicitEllipsoids = new Matrix4x4[0];
    public ImplicitEllipsoid[] InsideImplicitEllipsoids = new ImplicitEllipsoid[0];
    public ImplicitPlane[] ImplicitPlanes = new ImplicitPlane[0];

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
    public CullBone[] CullBones;

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
        this.PrimitiveType = CellGcmPrimitive.TRIANGLES;
        if (indices != null)
        {
            if (indices.Length % 0x6 != 0)
                throw new ArgumentException("Indices buffer must be divisible by " +
                                            "0x6 since" +
                                            " it contains triangle data!");
            this.NumTris = indices.Length / 0x6;
            this.NumIndices = indices.Length / 0x2;
        }
        this.Init(streams, attributes, indices, bones);
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
        this.NumIndices = numIndices;
        this.NumEdgeIndices = numEdgeIndices;
        this.NumTris = numTris;
        this.Init(streams, attributes, indices, bones);
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
        int size = streams[0].Length;
        foreach (byte[] stream in streams)
        {
            if (stream.Length != size)
                throw new ArgumentException("All vertex streams must be the same " +
                                                   "size!");
            if ((stream.Length % 0x10) != 0)
                throw new ArgumentException("All vertex streams must be divisible " +
                                                   "by 0x10!");
        }

        this.Streams = streams;
        this.StreamCount = streams.Length;
        this.NumVerts = size / 0x10;

        // This won't be used if the mesh doesn't have softbody
        // data, but we'll still create the array anway.
        this.Mass = new float[this.NumVerts];

        // If the stream Length is 2, it means we
        // only have geometry and skinning data, no morph data.
        if (this.Streams.Length > 2)
        {
            this.MorphCount = this.Streams.Length - 2;
            this.MirrorMorphs = new short[this.MorphCount];
        }
        else this.MorphCount = 0;

        if (attributes != null)
        {
            // A single attribute stream should consist of
            // UV0 (8 bytes per vertex).
            int attributeSize = this.NumVerts * 0x8;

            if ((attributes.Length % attributeSize) != 0)
                throw new ArgumentException("Attribute buffer doesn't match vertex" +
                                                   " count!");

            this.AttributeCount = attributes.Length / attributeSize;
        }
        this.Attributes = attributes;
        this.Indices = indices;

        // Initialize the mirror arrays if the bones aren't null.
        if (bones != null)
        {
            this.MirrorBones = new short[bones.Length];
            for (int i = 0; i < this.MirrorBones.Length; ++i)
                this.MirrorBones[i] = (short) i;
            this.MirrorBoneFlipTypes = new FlipType[bones.Length];
            Array.Fill(this.MirrorBoneFlipTypes, FlipType.MAX);
        }
        this.Bones = bones;
        this.CullBones = new CullBone[this.Bones.Length];
        for (int i = 0; i < this.Bones.Length; ++i)
        {
            CullBone bone = new CullBone();
            bone.BoundBoxMax = this.Bones[i].boundBoxMax;
            bone.BoundBoxMin = this.Bones[i].boundBoxMin;
            bone.InvSkinPoseMatrix = this.Bones[i].invSkinPoseMatrix;
            this.CullBones[i] = bone;
        }

        this.VertexColors = new uint[this.NumVerts];
        for (int i = 0; i < this.NumVerts; ++i)
            this.VertexColors[i] = 0xFFFFFFFF;
    }

    public override void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        this.NumVerts = serializer.I32(this.NumVerts);
        this.NumIndices = serializer.I32(this.NumIndices);
        this.NumEdgeIndices = serializer.I32(this.NumEdgeIndices);
        this.NumTris = serializer.I32(this.NumTris);
        this.StreamCount = serializer.I32(this.StreamCount);
        this.AttributeCount = serializer.I32(this.AttributeCount);
        this.MorphCount = serializer.I32(this.MorphCount);

        if (!serializer.IsWriting())
            this.MorphNames = new string[MaxMorphs];
        for (int i = 0; i < MaxMorphs; ++i)
            this.MorphNames[i] = serializer.Str(this.MorphNames[i], 0x10);

        if (version >= (int)Revisions.MeshMinmaxUv)
        {
            this.MinUv = serializer.Floatarray(this.MinUv);
            this.MaxUv = serializer.Floatarray(this.MaxUv);
            this.AreaScaleFactor = serializer.F32(this.AreaScaleFactor);
        }

        if (serializer.IsWriting())
        {
            int offset = 0;
            MemoryOutputStream stream = serializer.GetOutput();
            stream.I32(offset);
            for (int i = 0; i < this.Streams.Length; ++i)
            {
                offset += this.Streams[i].Length;
                stream.I32(offset);
            }
            stream.I32(offset);
            foreach (byte[] vertexStream in this.Streams)
                stream.Bytes(vertexStream);
        }
        else
        {
            MemoryInputStream stream = serializer.GetInput();
            // We're skipping source stream offsets.
            for (int i = 0; i < this.StreamCount + 1; ++i)
                stream.I32();
            stream.I32();

            this.Streams = new byte[this.StreamCount][];
            for (int i = 0; i < this.StreamCount; ++i)
                this.Streams[i] = stream.Bytes(this.NumVerts * 0x10);
        }

        this.Attributes = serializer.Bytearray(this.Attributes);
        this.Indices = serializer.Bytearray(this.Indices);
        if (subVersion >= (int)Revisions.Fuzz)
            this.TriangleAdjacencyInfos = serializer.Bytearray(this.TriangleAdjacencyInfos);

        this.Primitives = serializer.Arraylist(this.Primitives);
        this.Bones = serializer.Array(this.Bones);

        this.MirrorBones = serializer.Shortarray(this.MirrorBones);
        this.MirrorBoneFlipTypes = serializer.Enumarray(this.MirrorBoneFlipTypes);
        this.MirrorMorphs = serializer.Shortarray(this.MirrorMorphs);

        this.PrimitiveType = serializer.Enum8(this.PrimitiveType);

        this.SoftbodyCluster = serializer.Struct(this.SoftbodyCluster);
        this.SoftbodySprings = serializer.Array(this.SoftbodySprings);
        this.SoftbodyEquivs = serializer.Array(this.SoftbodyEquivs);

        // Don't write mass field if there's no softbody data on the this.
        if (serializer.IsWriting())
        {
            if (this.HasSoftbodyData()) serializer.Floatarray(this.Mass);
            else serializer.GetOutput().I32(0);
        }
        else this.Mass = serializer.Floatarray(this.Mass);

        this.ImplicitEllipsoids = serializer.Array(this.ImplicitEllipsoids);

        if (!serializer.IsWriting())
            this.ClusterImplicitEllipsoids = new Matrix4x4[serializer.GetInput().I32()];
        else serializer.GetOutput().I32(this.ClusterImplicitEllipsoids.Length);
        for (int i = 0; i < this.ClusterImplicitEllipsoids.Length; ++i)
            this.ClusterImplicitEllipsoids[i] =
                serializer.M44(this.ClusterImplicitEllipsoids[i])!.Value;

        this.InsideImplicitEllipsoids = serializer.Array(this.InsideImplicitEllipsoids);
        this.ImplicitPlanes = serializer.Array(this.ImplicitPlanes);
        this.SoftPhysSettings = serializer.Resource(this.SoftPhysSettings,
            ResourceType.SettingsSoftPhys);
        this.MinSpringVert = serializer.I32(this.MinSpringVert);
        this.MaxSpringVert = serializer.I32(this.MaxSpringVert);
        this.MinUnalignedSpringVert = serializer.I32(this.MinUnalignedSpringVert);
        this.SpringyTriIndices = serializer.Shortarray(this.SpringyTriIndices);
        this.SpringTrisStripped = serializer.Intbool(this.SpringTrisStripped);
        this.SoftbodyContainingBoundBoxMin = serializer.V4(this.SoftbodyContainingBoundBoxMin);
        this.SoftbodyContainingBoundBoxMax = serializer.V4(this.SoftbodyContainingBoundBoxMax);

        this.CullBones = serializer.Array(this.CullBones);
        this.RegionIDsToHide = serializer.Intvector(this.RegionIDsToHide);

        this.CostumeCategoriesUsed = serializer.I32(this.CostumeCategoriesUsed);
        if (version >= 0x141)
            this.HairMorphs = serializer.Enum32(this.HairMorphs);
        this.BevelVertexCount = serializer.I32(this.BevelVertexCount);
        this.ImplicitBevelSprings = serializer.Bool(this.ImplicitBevelSprings);

        if (revision.Has(Branch.Double11, (int)Revisions.D1VertexColors))
        {
            if (!serializer.IsWriting())
                this.VertexColors = new uint[serializer.GetInput().I32()];
            else serializer.GetOutput().I32(this.VertexColors.Length);
            for (int i = 0; i < this.VertexColors.Length; ++i)
                this.VertexColors[i] = (uint)serializer.I32((int)this.VertexColors[i], true);
        }
        else if (!serializer.IsWriting())
        {
            this.VertexColors = new uint[this.NumVerts];
            for (int i = 0; i < this.NumVerts; ++i)
                this.VertexColors[i] = 0xFFFFFFFF;
        }

        if (subVersion >= (int)Revisions.MeshSkeletonType)
            this.SkeletonType = serializer.Enum8(this.SkeletonType);
    }

    public override int GetAllocatedSize()
    {
        int size = BaseAllocationSize;

        if (this.Streams != null) size += this.Streams.Sum(stream => stream.Length);
        if (this.Attributes != null) size += this.Attributes.Length;
        if (this.Indices != null) size += this.Indices.Length;
        if (this.TriangleAdjacencyInfos != null) size += this.TriangleAdjacencyInfos.Length;

        if (this.Primitives != null)
            size += (this.Primitives.Count * Primitive.BaseAllocationSize);

        if (this.Bones != null)
            foreach (Bone bone in this.Bones)
                size += bone.GetAllocatedSize();

        if (this.MirrorBones != null) size += (this.MirrorBones.Length * 2);
        if (this.MirrorBoneFlipTypes != null) size += this.MirrorBoneFlipTypes.Length;
        if (this.MirrorMorphs != null) size += (this.MirrorMorphs.Length * 2);
        if (this.SoftbodyCluster != null) size += this.SoftbodyCluster.GetAllocatedSize();
        if (this.SoftbodySprings != null)
            size += (this.SoftbodySprings.Length * SoftbodySpring.BaseAllocationSize);
        if (this.SoftbodyEquivs != null)
            size += (this.SoftbodyEquivs.Length * SoftbodyVertEquivalence.BaseAllocationSize);
        if (this.Mass != null) size += (this.Mass.Length * 4);
        if (this.ImplicitEllipsoids != null)
            size += (this.ImplicitEllipsoids.Length * ImplicitEllipsoid.BaseAllocationSize);
        if (this.ClusterImplicitEllipsoids != null)
            size += (this.ClusterImplicitEllipsoids.Length * 0x40);
        if (this.InsideImplicitEllipsoids != null)
            size += (this.InsideImplicitEllipsoids.Length * ImplicitEllipsoid.BaseAllocationSize);
        if (this.ImplicitPlanes != null)
            size += (this.ImplicitPlanes.Length * ImplicitPlane.BaseAllocationSize);
        if (this.SpringyTriIndices != null) size += (this.SpringyTriIndices.Length * 2);
        if (this.CullBones != null)
            size += (this.CullBones.Length * CullBone.BaseAllocationSize);
        if (this.RegionIDsToHide != null) size += (this.RegionIDsToHide.Length * 0x8);

        return size;
    }
    
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        Serializer serializer = new Serializer(this.GetAllocatedSize() + 0x8000, revision,
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
        return this.PrimitiveType.Equals(CellGcmPrimitive.TRIANGLE_STRIP);
    }

    /**
     * Checks if this mesh uses softbody simulation.
     *
     * @return True if the mesh uses softbody simulation, false otherwise.
     */
    public bool HasSoftbodyData()
    {
        if (this.SoftbodyCluster == null) return false;
        return this.SoftbodyCluster.Clusters.Count != 0;
    }

    public Vector2 GetMinUv()
    {
        if (this.MinUv is not { Length: 2 })
            return Vector2.Zero;
        return new Vector2(this.MinUv[0], this.MinUv[1]);
    }

    public Vector2 GetMaxUv()
    {
        if (this.MaxUv is not { Length: 2 })
            return Vector2.Zero;
        return new Vector2(this.MaxUv[0], this.MaxUv[1]);
    }

    /**
     * Gets a stream at specified index.
     *
     * @param index Index of stream
     * @return Stream at index
     */
    public byte[] GetVertexStream(int index)
    {
        if (this.Streams == null || index < 0 || index >= this.Streams.Length)
            throw new NullReferenceException("Vertex stream at position " + index + " does " +
                                             "not " +
                                             "exist!");
        return this.Streams[index];
    }

    /**
     * Gets the main vertex stream.
     *
     * @return Main vertex stream.
     */
    public byte[] GetVertexStream()
    {
        if (this.Streams.Length <= StreamPosBoneindices)
            throw new NullReferenceException("Vertex stream doesn't exist on this mesh!");
        return this.Streams[StreamPosBoneindices];
    }

    /**
     * Gets the stream that contains skinning data,
     * as well as normals, tangents, and smooth normals.
     *
     * @return Skinning/Normal stream.
     */
    public byte[] GetSkinningStream()
    {
        if (this.Streams.Length <= StreamBoneweightsNormTangentSmoothNorm)
            throw new NullReferenceException("Skinning stream doesn't exist on this mesh!");
        return this.Streams[StreamBoneweightsNormTangentSmoothNorm];
    }

    /**
     * Gets all streams containing morph data.
     *
     * @return Morph data streams
     */
    public byte[][] GetMorphStreams()
    {
        byte[][] streams = new byte[this.MorphCount][];
        if (StreamCount - 2 >= 0)
            Array.Copy(this.Streams, 2, streams, 0, StreamCount - 2);
        return streams;
    }

    public void SetMorphNames(string[] names)
    {
        if (names == null)
            throw new NullReferenceException("Morph names cannot be null!");
        if (names.Length != MaxMorphs)
            throw new ArgumentException("Morph name array must have Length of 32!");
        this.MorphNames = names;
    }

    /**
     * Sets the morph name at index.
     *
     * @param name  Morph name
     * @param index Index of morph
     */
    public void SetMorphName(string name, int index)
    {
        this.MorphNames[index] = name;
    }

    public void SetMinUv(Vector2 minUv)
    {
        if (minUv == null)
        {
            this.MinUv = null;
            return;
        }
        this.MinUv = [minUv.X, minUv.Y];
    }

    public void SetMaxUv(Vector2 maxUv)
    {
        if (maxUv == null)
        {
            this.MaxUv = null;
            return;
        }
        this.MaxUv = [maxUv.X, maxUv.Y];
    }

    public void SetAreaScaleFactor(float factor)
    {
        this.AreaScaleFactor = factor;
    }

    public void SetPrimitives(List<Primitive> primitives)
    {
        this.Primitives = primitives;
    }

    /**
     * Sets a bone's mirror.
     *
     * @param index  Bone index
     * @param mirror Bone mirror index
     */
    public void SetBoneMirror(int index, int mirror)
    {
        if (this.MirrorBones == null || index < 0 || index >= this.MirrorBones.Length)
            throw new NullReferenceException("Bone at position " + index + " does not exist!");
        if (this.MirrorBones == null || mirror < 0 || mirror >= this.MirrorBones.Length)
            throw new NullReferenceException("Bone at position " + mirror + " does not exist!");
        this.MirrorBones[index] = (short) (mirror & 0xFFFF);
    }

    /**
     * Sets a bone's flip type.
     *
     * @param index Bone index
     * @param type  Bone flip type
     */
    public void SetBoneFlipType(int index, FlipType type)
    {
        if (this.MirrorBones == null || index < 0 || index >= this.MirrorBones.Length)
            throw new NullReferenceException("Bone at position " + index + " does not exist!");
        this.MirrorBoneFlipTypes[index] = type;
    }

    public void SetMorphMirror(int index, int mirror)
    {
        if (this.MirrorMorphs == null || index < 0 || index >= this.MirrorMorphs.Length)
            throw new NullReferenceException("Morph at position " + index + " does not exist!");
        if (this.MirrorMorphs == null || mirror < 0 || mirror >= this.MirrorMorphs.Length)
            throw new NullReferenceException("Morph at position " + mirror + " does not " +
                                           "exist!");
        this.MirrorMorphs[index] = (short) (mirror & 0xFFFF);
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
        if (this.Mass == null || this.Mass.Length != this.NumVerts)
        {
            this.Mass = new float[this.NumVerts];
            for (int i = 0; i < this.NumVerts; ++i)
                this.Mass[i] = 1.0f;
        }

        if (index < 0 || index >= this.Mass.Length)
            throw new NullReferenceException("Vertex at position " + index + " does not " +
                                           "exist!");
        this.Mass[index] = mass;
    }

    /**
     * Gets all sub-meshes contained in this this.
     *
     * @return Sub-meshes
     */
    public Primitive[][] GetSubmeshes()
    {
        Dictionary<int, List<Primitive>> meshes = new();
        foreach (Primitive primitive in this.Primitives)
        {
            int region = primitive.Region;
            if (!meshes.ContainsKey(region))
            {
                List<Primitive> primitives = [];
                primitives.Add(primitive);
                meshes.Add(region, primitives);
            }
            else meshes[region].Add(primitive);
        }

        Primitive[][] submeshes = new Primitive[meshes.Values.Count][];

        int index = 0;
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
        MemoryInputStream stream = new MemoryInputStream(this.GetVertexStream());
        stream.Seek(start * 0x10);
        Vector3[] vertices = new Vector3[count];
        for (int i = 0; i < count; ++i)
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
        MemoryInputStream stream = new MemoryInputStream(this.GetVertexStream());
        stream.Seek(start * 0x10);
        float[] weights = new float[count];
        for (int i = 0; i < count; ++i)
        {
            stream.Seek(0xC + 0x3);
            float c = ((float) stream.U8()) / ((float) 0xff);
            weights[i] = c;
        }
        return weights;
    }

    public byte[][] GetBlendIndices(int start, int count)
    {
        MemoryInputStream stream = new MemoryInputStream(this.GetVertexStream());
        stream.Seek(start * 0x10);
        byte[][] vertices = new byte[count][];
        for (int i = 0; i < count; ++i)
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
        return this.GetVertices(
            primitive.MinVert,
            (primitive.MaxVert - primitive.MinVert) + 1
        );
    }

    /**
     * Gets all the vertices from this this.
     *
     * @return Vertices
     */
    public Vector3[] GetVertices()
    {
        return this.GetVertices(0, this.NumVerts);
    }

    public void SwapUv01()
    {
        for (int i = 0; i < this.NumVerts; i++)
        {
            int uv0 = (this.AttributeCount * 0x8 * i);
            int uv1 = uv0 + 0x8;

            byte[] tmp = this.Attributes.Skip(uv0).Take(0x8).ToArray(); // copy uv0 into tmp
            Array.Copy(this.Attributes, uv1, this.Attributes, uv0, 0x8);
            Array.Copy(tmp, 0, this.Attributes, uv1, 0x8);
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
        if (this.Attributes == null)
            throw new InvalidOperationException("This mesh doesn't have texture coordinates!");
        if (channel < 0 || (channel + 1 > this.AttributeCount))
            throw new ArgumentException("Invalid UV channel!");
        MemoryInputStream stream = new MemoryInputStream(this.Attributes);
        Vector2[] uVs = new Vector2[count];
        for (int i = 0; i < count; ++i)
        {
            stream.Seek(start + (this.AttributeCount * 0x8 * i) + (0x8 * channel),
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
        return this.GetUVs(
            primitive.MinVert,
            (primitive.MaxVert - primitive.MinVert) + 1,
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
        return this.GetUVs(0, this.NumVerts, channel);
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
        MemoryInputStream stream = new MemoryInputStream(this.GetSkinningStream());
        Vector3[] normals = new Vector3[count];
        for (int i = 0; i < count; ++i)
        {
            stream.Seek((start * 0x10) + (i * 0x10) + 0x4, SeekMode.Begin);
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
        return this.GetNormals(0, this.NumVerts);
    }

    /**
     * Gets all the vertex normals from a mesh primitive.
     *
     * @param primitive Primitive to get vertex normals from
     * @return Vertex normals
     */
    public Vector3[] GetNormals(Primitive primitive)
    {
        return this.GetNormals(
            primitive.MinVert,
            (primitive.MaxVert - primitive.MinVert) + 1
        );
    }

    /**
     * Gets all the vertex tangents from this this.
     *
     * @return Vertex tangents
     */
    public Vector4[] GetTangents()
    {
        return this.GetTangents(0, this.NumVerts);
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
        MemoryInputStream stream = new MemoryInputStream(this.GetSkinningStream());
        Vector4[] tangents = new Vector4[count];
        for (int i = 0; i < count; ++i)
        {
            stream.Seek((start * 0x10) + (i * 0x10) + 0x8, SeekMode.Begin);
            Vector3 tangent = Bytes.UnpackNormal24(stream.U24());
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
        return this.GetTangents(
            primitive.MinVert,
            (primitive.MaxVert - primitive.MinVert) + 1
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
        MemoryInputStream stream = new MemoryInputStream(this.GetSkinningStream());
        Vector3[] normals = new Vector3[count];
        for (int i = 0; i < count; ++i)
        {
            stream.Seek((start * 0x10) + (i * 0x10) + 0xC, SeekMode.Begin);
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
        return this.GetSmoothNormals(0, this.NumVerts);
    }

    /**
     * Gets all the smooth vertex normals from a mesh primitive.
     *
     * @param primitive Primitive to get smooth vertex normals from
     * @return Smooth vertex normals
     */
    public Vector3[] GetSmoothNormals(Primitive primitive)
    {
        return this.GetSmoothNormals(
            primitive.MinVert,
            (primitive.MaxVert - primitive.MinVert) + 1
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
        MemoryInputStream stream = new MemoryInputStream(this.GetSkinningStream());
        stream.Seek(start * 0x10);
        byte[][] joints = new byte[count][];
        for (int i = 0; i < count; ++i)
        {
            byte[] buffer = stream.Bytes(0x10);
            joints[i] = new byte[] {
                buffer[3], buffer[7], buffer[0xB], buffer[0xF]
            };
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
        return this.GetJoints(
            primitive.MinVert,
            (primitive.MaxVert - primitive.MinVert) + 1
        );
    }

    /**
     * Gets the joints that have an influence on each vertex.
     *
     * @return Joint indices per vertex
     */
    public byte[][] GetJoints()
    {
        return this.GetJoints(0, this.NumVerts);
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
        MemoryInputStream stream =
            new MemoryInputStream(this.GetSkinningStream());
        stream.Seek(start * 0x10);
        Vector4[] weights = new Vector4[count];
        for (int i = 0; i < count; ++i)
        {
            byte[] buffer = stream.Bytes(0x10);
            float[] rawWeights = {
                (float) ((int) buffer[2] & 0xFF),
                (float) ((int) buffer[1] & 0xFF),
                (float) ((int) buffer[0] & 0xFF),
                0
            };

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
        return this.GetWeights(
            primitive.MinVert,
            (primitive.MaxVert - primitive.MinVert) + 1
        );
    }

    /**
     * Gets all weight influences from this skinned this.
     *
     * @return Weight influences
     */
    public Vector4[] GetWeights()
    {
        return this.GetWeights(0, this.NumVerts);
    }

    /**
     * Parses all morphs from the morph streams.
     *
     * @return Parsed morphs
     */
    public Morph[] GetMorphs()
    {
        if (this.MorphCount == 0)
            throw new InvalidOperationException("Can't get morphs from mesh that has no morph " +
                                                "data!");
        Morph[] morphs = new Morph[this.MorphCount];
        for (int i = 0; i < this.MorphCount; ++i)
        {
            MemoryInputStream stream = new MemoryInputStream(this.Streams[i + 2]);
            Vector3[] offsets = new Vector3[this.NumVerts];
            Vector3[] normals = new Vector3[this.NumVerts];
            for (int j = 0; j < this.NumVerts; ++j)
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
        if (this.Indices == null)
            throw new InvalidOperationException("Can't get triangles from mesh without index " +
                                                "buffer!");

        int[] faces = new int[count];
        short[] stream = this.SpringyTriIndices;
        for (int i = start; i < count; ++i)
            faces[i] = stream[i] & 0xffff;

        if (!this.SpringTrisStripped) return faces;

        List<int> triangles = new List<int>(this.NumVerts * 0x3);
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
        if (this.Indices == null)
            throw new InvalidOperationException("Can't get triangles from mesh without index " +
                                                "buffer!");

        int[] faces = new int[count];
        MemoryInputStream stream = new MemoryInputStream(this.Indices);
        stream.Seek(start * 0x2);
        for (int i = 0; i < count; ++i)
            faces[i] = stream.U16();

        if (!this.IsStripped()) return faces;

        List<int> triangles = new(this.NumVerts * 0x3);
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
        return this.GetTriangles(primitive.FirstIndex, primitive.NumIndices);
    }

    /**
     * Gets this mesh's triangle list, triangulating if necessary.
     *
     * @return Mesh's triangle list
     */
    public int[] GetTriangles()
    {
        return this.GetTriangles(0, this.NumIndices);
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
        Vector4[] weights = GetWeights();
        int[] modulo = [0x3, 0x7, 0xb, 0xf];
        for (int i = 0; i < NumVerts; ++i)
        {
            byte[] stream = GetSkinningStream();
            for (int j = 0; j < 4; ++j)
            {
                if (weights[i].GetComponent(j) == 0.0f)
                    stream[(i * 0x10) + modulo[j]] = 0;
            }

            for (int j = 0; j < 4; ++j)
            {
                int joint = ((int) stream[(i * 0x10) + modulo[j]]) - 1;
                if (joint == -1) joint = 0;
                stream[(i * 0x10) + modulo[j]] = (byte) (joint);
            }
        }

        foreach (Bone bone in Bones)
        {
            Matrix4x4 inverse = bone.invSkinPoseMatrix.Value;
            inverse.M14 = 0.0f;
            inverse.M24 =(0.0f);
            inverse.M34 = 0.0f;
            inverse.M44 = 1.0f;
        }
    }

    public Submesh[] FindAllMeshes()
    {
        List<Submesh> meshes = [];
        for (int boneIndex = 0; boneIndex < Bones.Length; ++boneIndex)
        {
            Bone bone = Bones[boneIndex];
            if (bone.parent != -1) continue;

            Submesh mesh = new Submesh();
            mesh.Transform = bone.skinPoseMatrix;
            mesh.Locator = boneIndex;
            mesh.Skinned = bone.firstChild != -1;

            List<Primitive> primitives = [];
            byte[] stream = GetSkinningStream();
            foreach (Primitive primitive in this.Primitives)
            {
                // Test the first vertex of each primitive to see
                // if they belong to this subthis.
                Bone joint = Bones[stream[(primitive.MinVert * 0x10) + 0x3]];
                while (joint.parent != -1)
                    joint = Bones[joint.parent];

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
        Vector3[] vertices = this.GetVertices();
        Vector4[] weights = this.GetWeights();
        byte[][] joints = this.GetJoints();

        Dictionary<Bone, Vector3> minVert = new();
        Dictionary<Bone, Vector3> maxVert = new();

        foreach (Bone bone in this.Bones)
        {
            minVert.Add(bone, new Vector3(float.PositiveInfinity, float.PositiveInfinity,
                float.PositiveInfinity));
            maxVert.Add(bone, new Vector3(float.NegativeInfinity, float.NegativeInfinity,
                float.NegativeInfinity));
        }

        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 v = vertices[i];
            Vector4 weightCache = weights[i];
            byte[] jointCache = joints[i];

            for (int j = 0; j < 4; ++j)
            {
                if (weightCache.GetComponent(j) == 0.0f) continue;
                Vector3 max = maxVert[this.Bones[jointCache[j]]];
                Vector3 min = minVert[this.Bones[jointCache[j]]];


                if (v.X > max.X) max.X = v.X;
                if (v.Y > max.Y) max.Y = v.Y;
                if (v.Z > max.Z) max.Z = v.Z;

                if (v.X < min.X) min.X = v.X;
                if (v.Y < min.Y) min.Y = v.Y;
                if (v.Z < min.Z) min.Z = v.Z;
            }
        }

        int index = 0;
        foreach (Bone bone in this.Bones)
        {
            Vector4 max = new Vector4(maxVert[bone], 1.0f);
            Vector4 min = new Vector4(minVert[bone], 1.0f);

            if (float.IsPositiveInfinity(min.X)) min = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            else min = Vector4.Transform(min, bone.invSkinPoseMatrix.Value);


            if (float.IsNegativeInfinity(max.X)) max = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            else max = Vector4.Transform(max, bone.invSkinPoseMatrix.Value);

            for (int c = 0; c < 3; ++c)
            {
                if (min.GetComponent(c) > max.GetComponent(c))
                {
                    float u = min.GetComponent(c);
                    float l = max.GetComponent(c);
                    min.SetComponent(c, l);
                    max.SetComponent(c, u);
                }
            }

            bone.boundBoxMax = max;
            bone.boundBoxMin = min;
            if (setObb)
            {
                bone.obbMax = bone.boundBoxMax;
                bone.obbMin = bone.boundBoxMin;
            }

            Vector4 center = (max + (min)) / (2.0f);
            float minDist = Math.Abs(Vector4.Distance(center, min));
            float maxDist = Math.Abs(Vector4.Distance(center,(max)));
            center.W = (minDist > maxDist) ? minDist : maxDist;
            bone.boundSphere = center;

            CullBone culler = this.CullBones[index++];
            culler.BoundBoxMax = bone.boundBoxMax;
            culler.BoundBoxMin = bone.boundBoxMin;
            culler.InvSkinPoseMatrix = bone.invSkinPoseMatrix;
        }
    }
}