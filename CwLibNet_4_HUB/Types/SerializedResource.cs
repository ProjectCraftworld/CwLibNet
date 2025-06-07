using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Streams;
using CwLibNet.Resources;
using CwLibNet.Structs.StaticMesh;
using CwLibNet.Structs.Texture;
using CwLibNet.Structs.Things;
using CwLibNet.Types.Data;
using CwLibNet.Util;
using Xxtea;
using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Types;

public class SerializedResource
{
    /**
     * Type of resource.
     */
    private ResourceType type = ResourceType.Invalid;

    /**
     * Method of serialization, binary, text, or texture resources.
     */
    private SerializationType method = SerializationType.UNKNOWN;

    /**
     * Texture metadata for texture resources.
     */
    private CellGcmTexture textureInfo;

    /**
     * Static mesh metadata
     */
    private StaticMeshInfo meshInfo;

    /**
     * Revision of the resource.
     */
    private Revision revision;

    /**
     * Whether the resource is compressed
     */
    private bool isCompressed = true;

    /**
     * Controls which data types get compressed during serialization.
     */
    private byte compressionFlags = CompressionFlags.USE_NO_COMPRESSION;

    /**
     * Decompressed data from this resource.
     */
    private byte[]? data;

    /**
     * Resources this resource depends on.
     */
    private HashSet<ResourceDescriptor> dependencies = [];

    /**
     * Constructs a new resource from path
     *
     * @param path Path to read resource from
     */
    public SerializedResource(string path)
    {
        Process(new MemoryInputStream(path));
    }

    /**
     * Constructs a new resource from buffer
     *
     * @param data Data to read resource from
     */
    public SerializedResource(byte[]? data)
    {
        Process(new MemoryInputStream(data));
    }

    /**
     * Processes the resource stream.
     *
     * @param stream Memory input stream to read from
     */
    private void Process(MemoryInputStream stream)
    {
        if (stream == null || stream.GetLength() < 0xb) return;
        type = ResourceType.FromMagic(stream.Str(3));
        if (type.Value == ResourceType.Invalid.Value)
            throw new SerializationException("Invalid Resource type!");
        method = SerializationType.FromValue(stream.Str(1));
        switch (method.GetValue())
        {
            case "b":
            case "e":
                var head = stream.I32();
                short branchId = 0, branchRevision = 0;
                var dependencyTableOffset = -1;
                if (head >= 0x109)
                {
                    dependencyTableOffset = ProcessDependencies(stream);
                    if (head >= 0x189)
                    {
                        if (type.Value != ResourceType.StaticMesh.Value)
                        {
                            if (head >= 0x271)
                            {
                                branchId = stream.I16();
                                branchRevision = stream.I16();
                            }
                            if (head >= 0x297 || head == Branch.Leerdammer.Head && branchId == Branch.Leerdammer.Id && branchRevision >= (int)Revisions.LD_RESOURCES)
                                compressionFlags = stream.I8();
                            isCompressed = stream.Boole();
                        }
                        else
                            meshInfo =
                                new Serializer(stream, new Revision(head)).Struct<StaticMeshInfo>(null);
                    }
                }

                if (method.Equals(SerializationType.ENCRYPTED_BINARY))
                {
                    int size = stream.I32(), padding = 0;
                    if (size % 4 != 0)
                        padding = 4 - size % 4;
                    stream =
                        new MemoryInputStream(XXTEA.Decrypt(stream.Bytes(size + padding),
                            XXTEA.TEA_KEY()));
                    stream.Seek(padding);
                }

                revision = new Revision(head, branchId, branchRevision);

                if (isCompressed)
                    data = Compressor.DecompressData(stream, dependencyTableOffset);
                else if (dependencyTableOffset != -1)
                    data = stream.Bytes(dependencyTableOffset - stream.GetOffset());
                else
                    data = stream.Bytes(stream.GetLength() - stream.GetOffset());
                break;
            case "t":
                data = stream.Bytes(stream.GetLength() - stream.GetOffset());
                break;
            case " ":
            case "s":
            case "S":
                if (type.Value != ResourceType.Texture.Value)
                    textureInfo = new CellGcmTexture(stream, method);
                data = Compressor.DecompressData(stream, stream.GetLength());
                break;
            case null:
                throw new SerializationException("Invalid serialization method!");
        }
    }

    /**
     * Constructs a new serializer from this resource's data.
     *
     * @return Data deserializer from current resource data
     */
    public Serializer GetSerializer()
    {
        var serializer = new Serializer(data, revision, compressionFlags);
        foreach (var descriptor in dependencies)
            Serializer.GetCurrentSerializer().AddDependency(descriptor);
        return serializer;
    }

    /**
     * Constructs a new memory input stream from this resource's data.
     *
     * @return Data stream from current resource data
     */
    public MemoryInputStream GetStream()
    {
        return new MemoryInputStream(data, compressionFlags);
    }

    /**
     * Deserializes a resource from this instance.
     *
     * @param T   Resource type that implements Serializable
     * @param clazz Resource class reference that implements Serializable
     * @return Deserialized resource
     */
    public T LoadResource<T>() where T : ISerializable
    {
        var serializer = GetSerializer();
        T? resource = default(T);
        Serializer.Serialize(ref resource);
        return resource;
    }

    /**
     * Reads the dependency table from the current resource stream.
     *
     * @param stream Memory input stream to read from
     * @return The offset of the dependency table
     */
    private int ProcessDependencies(MemoryInputStream stream)
    {
        var dependencyTableOffset = stream.I32();
        var originalOffset = stream.GetOffset();
        stream.Seek(dependencyTableOffset, SeekMode.Begin);

        var count = stream.I32();
        dependencies = new HashSet<ResourceDescriptor>(count);
        for (var i = 0; i < count; ++i)
        {
            var flags = stream.I8();

            GUID? guid = null;
            Sha1? sha1 = null;

            if ((flags & 2) != 0)
                guid = stream.Guid();
            if ((flags & 1) != 0)
                sha1 = stream.Sha1();

            var descriptor = new ResourceDescriptor(guid, sha1,
                ResourceType.FromType(stream.I32()));
            if (descriptor.IsValid())
                dependencies.Add(descriptor);
        }

        stream.Seek(originalOffset, SeekMode.Begin);

        return dependencyTableOffset;
    }

    /**
     * Wraps a resource in a container, preferring compression if possible.
     *
     * @param resource         Instance of compressible resource
     * @param revision         Revision of underlying resource stream.
     * @param compressionFlags Compression flags used during resource serialization
     * @param preferCompressed Whether this resource should be compressed, if possible
     * @return Resource container
     */
    public static byte[]? Compress(Resource resource, Revision revision, byte compressionFlags
        , bool preferCompressed)
    {
        return Compress(resource.Build(revision, compressionFlags),
            preferCompressed);
    }

    /**
     * Wraps a resource in a container, preferring compression if possible.
     *
     * @param resource         Instance of compressible resource
     * @param revision         Revision of underlying resource stream.
     * @param compressionFlags Compression flags used during resource serialization
     * @return Resource container
     */
    public static byte[]? Compress(Resource resource, Revision revision, byte compressionFlags)
    {
        return Compress(resource.Build(revision, compressionFlags), true);
    }

    /**
     * Wraps a resource in a container, preferring compression if possible.
     *
     * @param data Serialization data to wrap
     * @return Resource container
     */
    public static byte[]? Compress(SerializationData data)
    {
        return Compress(data, true);
    }

    public byte[]? Compress(byte[]? data)
    {
        return Compress(
            new SerializationData(
                data,
                revision,
                compressionFlags,
                type,
                method,
                GetDependencies()
            ),
            true
        );
    }

    public byte[]? Compress()
    {
        return Compress(
            new SerializationData(
                data,
                revision,
                compressionFlags,
                type,
                method,
                GetDependencies()
            ),
            true
        );
    }

    /**
     * Wraps a resource in a container, with optional compression.
     *
     * @param data             Serialization data to wrap
     * @param preferCompressed Whether this resource should be compressed, if possible
     * @return Resource container
     */
    public static byte[]? Compress(SerializationData data, bool preferCompressed)
    {
        var type = data.Type;
        var meshInfo = data.StaticMeshInfo;
        var isStaticMesh = type.Value.Value == ResourceType.StaticMesh.Value;

        var buffer = data.Buffer;
        ResourceDescriptor?[] dependencies = data.Dependencies;

        var size = buffer.Length + 0x50;
        size += dependencies.Length * 0x1c;
        if (data.TextureInfo != null) size += 0x24;
        else if (meshInfo != null) size += meshInfo.GetAllocatedSize();

        var stream = new MemoryOutputStream(size);

        if (data.Method.GetValue() == SerializationType.TEXT.GetValue())
        {
            stream.Str(type.Value.Header + data.Method.GetValue() + '\n', 5);
            stream.Bytes(buffer);
            stream.Shrink();
            return stream.GetBuffer();
        }

        if (type.Value.Value == ResourceType.Texture.Value || type.Value.Value == ResourceType.GtfTexture.Value)
        {
            stream.Str(type.Value.Header + data.Method.GetValue(), 4);

            if (!type.Equals(ResourceType.Texture))
                data.TextureInfo?.Write(stream);
            stream.Bytes(Compressor.GetCompressedStream(data.Buffer, preferCompressed));

            stream.Shrink();
            return stream.GetBuffer();
        }

        stream.Str(type.Value.Header + data.Method.GetValue(), 4);

        var revision = data.Revision;
        var head = revision!.Value.Head;

        var isCompressed = preferCompressed;

        stream.I32(head);
        if (head >= 0x109 || isStaticMesh)
        {
            stream.I32(0); // Dummy value for dependency table offset.
            if (head >= 0x189 && !isStaticMesh)
            {
                if (head >= 0x271)
                {
                    stream.I16(revision.Value.BranchId);
                    stream.I16(revision.Value.BranchRevision);
                }

                if (head >= 0x297 || revision.Value.Has(Branch.Leerdammer,
                        (int)Revisions.LD_RESOURCES))
                    stream.I8(data.CompressionFlags.Value);

                if (preferCompressed)
                    isCompressed = true;
                stream.Boole(isCompressed);
            }
            else if (isStaticMesh)
                new Serializer(stream, revision.Value).Struct(data.StaticMeshInfo);
        }

        if (isCompressed || head < 0x189)
            buffer = Compressor.GetCompressedStream(buffer, isCompressed);

            // Tell the game there are no streams in the zlib data,
            // technically we don't have to waste memory concatenating the streams,
            // since these resources can't be encrypted anyway, but whatever
        else if (isStaticMesh && !preferCompressed)
            buffer = Bytes.Combine([0x00, 0x00, 0x00, 0x00], buffer);

        if (data.Method.GetValue() == SerializationType.ENCRYPTED_BINARY.GetValue())
        {
            stream.I32(buffer.Length);
            buffer = XXTEA.Encrypt(buffer, XXTEA.TEA_KEY());
        }
        stream.Bytes(buffer);

        if (head >= 0x109 || isStaticMesh)
        {
            // Setting dependency table offset
            var dependencyTableOffset = stream.GetOffset();
            stream.Seek(8, SeekMode.Begin);
            stream.I32(dependencyTableOffset);
            stream.Seek(dependencyTableOffset, SeekMode.Begin);

            // Writing dependencies
            stream.I32(dependencies.Length);
            foreach (var dependency in dependencies)
            {
                byte flags = 0;

                if (dependency != null)
                {
                    if (dependency.IsGUID()) flags |= 2;
                    if (dependency.IsHash()) flags |= 1;
                }

                stream.I8(flags);
                if (flags != 0)
                {
                    if ((flags & 2) != 0)
                        stream.Guid(dependency.GetGUID());
                    if ((flags & 1) != 0)
                        stream.Sha1(dependency.GetSHA1());
                }

                stream.I32(dependency != null ? dependency.GetResourceType().Value : 0);
            }
        }

        stream.Shrink();
        return stream.GetBuffer();
    }

    public static byte[]? ChangeRevision(byte[]? data, Revision revision)
    {
        var version = revision.GetVersion();
        var compressionFlags = CompressionFlags.USE_NO_COMPRESSION;
        if (version >= 0x297 || (version == 0x272 && revision.GetBranchRevision() == 0x4c44 && revision.GetBranchRevision() > 1))
            compressionFlags = CompressionFlags.USE_ALL_COMPRESSION;

        Resource? compressable;
        try
        {
            var resource = new SerializedResource(data);
            var serializer = resource.GetSerializer();
//            Object _struct = Serializer.Serialize(ref null,
//                resource.getResourceType().Compressable);
            var @struct = serializer.GetType().GetMethod("Struct")?.MakeGenericMethod(resource.GetResourceType().Compressable).Invoke(serializer, null);
            if (@struct is RPlan plan)
            {
                Thing[]? things;
                try { things = plan.GetThings(); }
                catch (Exception) { return null; }
                plan.Revision = revision;
                plan.CompressionFlags = compressionFlags;
                plan.SetThings(things);
            }
            compressable = (Resource) @struct!;
        }
        catch (Exception)
        {
            return null;
        }

        return Compress(compressable.Build(revision, compressionFlags));
    }

    public void ReplaceDependency(ResourceDescriptor oldDescriptor,
                                  ResourceDescriptor newDescriptor)
    {
        if (oldDescriptor.Equals(newDescriptor)) return;
        if (!dependencies.Contains(oldDescriptor)) return;

        if (this.type.Value != ResourceType.StaticMesh.Value)
        {
            var type = oldDescriptor.GetResourceType();
            var isFsb = type.Equals(ResourceType.Filename);
            byte[]? oldDescBuffer;
            byte[]? newDescBuffer;

            // Music dependencies are actually the GUID dependencies of a script,
            // so they don't have the same structure for referencing.
            if (type.Equals(ResourceType.MusicSettings) || type.Equals(ResourceType.FileOfBytes) || type.Equals(ResourceType.Sample) || isFsb)
            {
                if (oldDescriptor.IsGUID() && newDescriptor.IsGUID())
                {
                    oldDescBuffer =
                        Bytes.GetIntegerBuffer(oldDescriptor.GetGUID().Value.Value,
                            compressionFlags);
                    newDescBuffer =
                        Bytes.GetIntegerBuffer(newDescriptor.GetGUID().Value.Value,
                            compressionFlags);
                }
                else return;
            }
            else
            {
                oldDescBuffer = Bytes.GetResourceReference(oldDescriptor, revision,
                    compressionFlags);
                newDescBuffer = Bytes.GetResourceReference(newDescriptor, revision,
                    compressionFlags);
            }


            if (this.type.Value == ResourceType.Plan.Value)
            {
                var plan = LoadResource<RPlan>();
                plan.ThingData = Bytes.Replace(plan.ThingData, oldDescBuffer,
                    newDescBuffer);
                if (isFsb && plan.InventoryData != null)
                {
                    if (oldDescriptor.GetGUID().Equals(plan.InventoryData.HighlightSound))
                        plan.InventoryData.HighlightSound = newDescriptor.GetGUID();
                }
                data = plan.Build(revision, compressionFlags).Buffer;
            }

            data = Bytes.Replace(data, oldDescBuffer, newDescBuffer);
        }
        else
        {
            if (meshInfo.Fallmap.Equals(oldDescriptor))
                meshInfo.Fallmap = newDescriptor;
            if (meshInfo.Lightmap.Equals(oldDescriptor))
                meshInfo.Lightmap = newDescriptor;
            if (meshInfo.Risemap.Equals(oldDescriptor))
                meshInfo.Risemap = newDescriptor;
            foreach (var primitive in meshInfo.Primitives)
                if (primitive.Gmat.Equals(oldDescriptor))
                    primitive.Gmat = newDescriptor;
        }

        dependencies.Remove(oldDescriptor);
        if (newDescriptor != null)
            dependencies.Add(newDescriptor);
    }

    public ResourceDescriptor[] GetDependencies()
    {
        return dependencies.ToArray();
    }

    public byte GetCompressionFlags()
    {
        return compressionFlags;
    }

    public Revision GetRevision()
    {
        return revision;
    }

    public ResourceType GetResourceType()
    {
        return type;
    }

    public SerializationType GetSerializationType()
    {
        return method;
    }

    public CellGcmTexture GetTextureInfo()
    {
        return textureInfo;
    }

    public StaticMeshInfo GetMeshInfo()
    {
        return meshInfo;
    }
}