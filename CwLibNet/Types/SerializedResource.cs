using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Resources;
using CwLibNet.Structs.StaticMesh;
using CwLibNet.Structs.Texture;
using CwLibNet.Types.Data;
using CwLibNet.Types.Things;
using CwLibNet.Util;
using Xxtea;

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
    private byte[]? data = null;

    /**
     * Resources this resource depends on.
     */
    private HashSet<ResourceDescriptor> dependencies = [];

    /**
     * Constructs a new resource from path
     *
     * @param path Path to read resource from
     */
    public SerializedResource(String path)
    {
        this.process(new MemoryInputStream(path));
    }

    /**
     * Constructs a new resource from buffer
     *
     * @param data Data to read resource from
     */
    public SerializedResource(byte[]? data)
    {
        this.process(new MemoryInputStream(data));
    }

    /**
     * Processes the resource stream.
     *
     * @param stream Memory input stream to read from
     */
    private void process(MemoryInputStream stream)
    {
        if (stream == null || stream.GetLength() < 0xb) return;
        this.type = ResourceType.FromMagic(stream.Str(3));
        if (this.type.Value == ResourceType.Invalid.Value)
            throw new SerializationException("Invalid Resource type!");
        this.method = SerializationType.FromValue(stream.Str(1));
        switch (this.method.GetValue())
        {
            case "b":
            case "e":
                int head = stream.I32();
                short branchID = 0, branchRevision = 0;
                int dependencyTableOffset = -1;
                if (head >= 0x109)
                {
                    dependencyTableOffset = this.processDependencies(stream);
                    if (head >= 0x189)
                    {
                        if (this.type.Value != ResourceType.StaticMesh.Value)
                        {
                            if (head >= 0x271)
                            {
                                branchID = stream.I16();
                                branchRevision = stream.I16();
                            }
                            if (head >= 0x297 || (head == Branch.Leerdammer.Head && branchID == Branch.Leerdammer.Id) && branchRevision >= (int)Revisions.LdResources)
                                this.compressionFlags = stream.I8();
                            this.isCompressed = stream.Boole();
                        }
                        else
                            this.meshInfo =
                                new Serializer(stream, new Revision(head)).Struct<StaticMeshInfo>(null);
                    }
                }

                if (this.method.Equals(SerializationType.ENCRYPTED_BINARY))
                {
                    int size = stream.I32(), padding = 0;
                    if (size % 4 != 0)
                        padding = 4 - (size % 4);
                    stream =
                        new MemoryInputStream(XXTEA.Decrypt(stream.Bytes(size + padding),
                            XXTEA.TEA_KEY()));
                    stream.Seek(padding);
                }

                this.revision = new Revision(head, branchID, branchRevision);

                if (this.isCompressed)
                    this.data = Compressor.DecompressData(stream, dependencyTableOffset);
                else if (dependencyTableOffset != -1)
                    this.data = stream.Bytes(dependencyTableOffset - stream.GetOffset());
                else
                    this.data = stream.Bytes(stream.GetLength() - stream.GetOffset());
                break;
            case "t":
                this.data = stream.Bytes(stream.GetLength() - stream.GetOffset());
                break;
            case " ":
            case "s":
            case "S":
                if (this.type.Value != ResourceType.Texture.Value)
                    this.textureInfo = new CellGcmTexture(stream, this.method);
                this.data = Compressor.DecompressData(stream, stream.GetLength());
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
    public Serializer getSerializer()
    {
        Serializer serializer = new Serializer(this.data, this.revision, this.compressionFlags);
        foreach (ResourceDescriptor descriptor in this.dependencies)
            serializer.AddDependency(descriptor);
        return serializer;
    }

    /**
     * Constructs a new memory input stream from this resource's data.
     *
     * @return Data stream from current resource data
     */
    public MemoryInputStream getStream()
    {
        return new MemoryInputStream(this.data, this.compressionFlags);
    }

    /**
     * Deserializes a resource from this instance.
     *
     * @param <T>   Resource type that implements Serializable
     * @param clazz Resource class reference that implements Serializable
     * @return Deserialized resource
     */
    public T loadResource<T>() where T : ISerializable
    {
        Serializer serializer = this.getSerializer();
        return serializer.Struct<T>(default);
    }

    /**
     * Reads the dependency table from the current resource stream.
     *
     * @param stream Memory input stream to read from
     * @return The offset of the dependency table
     */
    private int processDependencies(MemoryInputStream stream)
    {
        int dependencyTableOffset = stream.I32();
        int originalOffset = stream.GetOffset();
        stream.Seek(dependencyTableOffset, SeekMode.Begin);

        int count = stream.I32();
        this.dependencies = new HashSet<ResourceDescriptor>(count);
        for (int i = 0; i < count; ++i)
        {
            ResourceDescriptor descriptor = null;
            byte flags = stream.I8();

            GUID? guid = null;
            SHA1? sha1 = null;

            if ((flags & 2) != 0)
                guid = stream.Guid();
            if ((flags & 1) != 0)
                sha1 = stream.Sha1();

            descriptor = new ResourceDescriptor(guid, sha1,
                ResourceType.FromType(stream.I32()));
            if (descriptor.IsValid())
                this.dependencies.Add(descriptor);
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
     * @param preferCompressed Whether or not this resource should be compressed, if possible
     * @return Resource container
     */
    public static byte[]? compress(Resource resource, Revision revision, byte compressionFlags
        , bool preferCompressed)
    {
        return SerializedResource.compress(resource.Build(revision, compressionFlags),
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
    public static byte[]? compress(Resource resource, Revision revision, byte compressionFlags)
    {
        return SerializedResource.compress(resource.Build(revision, compressionFlags), true);
    }

    /**
     * Wraps a resource in a container, preferring compression if possible.
     *
     * @param data Serialization data to wrap
     * @return Resource container
     */
    public static byte[]? compress(SerializationData data)
    {
        return SerializedResource.compress(data, true);
    }

    public byte[]? compress(byte[]? data)
    {
        return SerializedResource.compress(
            new SerializationData(
                data,
                this.revision,
                this.compressionFlags,
                this.type,
                this.method,
                this.getDependencies()
            ),
            true
        );
    }

    public byte[]? compress()
    {
        return SerializedResource.compress(
            new SerializationData(
                this.data,
                this.revision,
                this.compressionFlags,
                this.type,
                this.method,
                this.getDependencies()
            ),
            true
        );
    }

    /**
     * Wraps a resource in a container, with optional compression.
     *
     * @param data             Serialization data to wrap
     * @param preferCompressed Whether or not this resource should be compressed, if possible
     * @return Resource container
     */
    public static byte[]? compress(SerializationData data, bool preferCompressed)
    {
        ResourceType? type = data.Type;
        StaticMeshInfo meshInfo = data.StaticMeshInfo;
        bool isStaticMesh = type.Value.Value == ResourceType.StaticMesh.Value;

        byte[]? buffer = data.Buffer;
        ResourceDescriptor[]? dependencies = data.Dependencies;

        int size = buffer.Length + 0x50;
        if (dependencies != null)
            size += dependencies.Length * 0x1c;
        if (data.TextureInfo != null) size += 0x24;
        else if (meshInfo != null) size += meshInfo.GetAllocatedSize();

        MemoryOutputStream stream = new MemoryOutputStream(size);

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
                data.TextureInfo.Write(stream);
            stream.Bytes(Compressor.GetCompressedStream(data.Buffer, preferCompressed));

            stream.Shrink();
            return stream.GetBuffer();
        }

        stream.Str(type.Value.Header + data.Method.GetValue(), 4);

        Revision? revision = data.Revision;
        int head = revision.Value.Head;

        bool isCompressed = head < 0x189;
        isCompressed = preferCompressed;

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

                if (head >= 0x297 || (revision.Value.Has(Branch.Leerdammer,
                    (int)Revisions.LdResources)))
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
            int dependencyTableOffset = stream.GetOffset();
            stream.Seek(8, SeekMode.Begin);
            stream.I32(dependencyTableOffset);
            stream.Seek(dependencyTableOffset, SeekMode.Begin);

            // Writing dependencies
            stream.I32(dependencies.Length);
            foreach (ResourceDescriptor dependency in dependencies)
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

    public static byte[]? changeRevision(byte[]? data, Revision revision)
    {
        int version = revision.GetVersion();
        byte compressionFlags = CompressionFlags.USE_NO_COMPRESSION;
        if (version >= 0x297 || (version == 0x272 && (revision.GetBranchRevision() == 0x4c44) && (revision.GetBranchRevision() > 1)))
            compressionFlags = CompressionFlags.USE_ALL_COMPRESSION;

        Resource compressable = null;
        try
        {
            SerializedResource resource = new SerializedResource(data);
            Serializer serializer = resource.getSerializer();
//            Object _struct = serializer.Struct(null,
//                resource.getResourceType().Compressable);
            object? _struct = serializer.GetType().GetMethod("Struct")?.MakeGenericMethod(resource.getResourceType().Compressable).Invoke(serializer, null);
            if (_struct is RPlan plan)
            {
                Thing[]? things = null;
                try { things = plan.GetThings(); }
                catch (Exception ex) { return null; }
                plan.Revision = revision;
                plan.CompressionFlags = compressionFlags;
                plan.SetThings(things);
            }
            compressable = (Resource) _struct;
        }
        catch (Exception ex)
        {
            return null;
        }

        return SerializedResource.compress(compressable.Build(revision, compressionFlags));
    }

    public void replaceDependency(ResourceDescriptor oldDescriptor,
                                  ResourceDescriptor newDescriptor)
    {
        if (oldDescriptor.Equals(newDescriptor)) return;
        if (!this.dependencies.Contains(oldDescriptor)) return;

        if (this.type.Value != ResourceType.StaticMesh.Value)
        {
            ResourceType type = oldDescriptor.GetResourceType();
            bool isFSB = type.Equals(ResourceType.Filename);
            byte[]? oldDescBuffer;
            byte[]? newDescBuffer;

            // Music dependencies are actually the GUID dependencies of a script,
            // so they don't have the same structure for referencing.
            if (type.Equals(ResourceType.MusicSettings) || type.Equals(ResourceType.FileOfBytes) || type.Equals(ResourceType.Sample) || isFSB)
            {
                if (oldDescriptor.IsGUID() && newDescriptor.IsGUID())
                {
                    oldDescBuffer =
                        Bytes.GetIntegerBuffer(oldDescriptor.GetGUID().Value.Value,
                            this.compressionFlags);
                    newDescBuffer =
                        Bytes.GetIntegerBuffer(newDescriptor.GetGUID().Value.Value,
                            this.compressionFlags);
                }
                else return;
            }
            else
            {
                oldDescBuffer = Bytes.GetResourceReference(oldDescriptor, this.revision,
                    this.compressionFlags);
                newDescBuffer = Bytes.GetResourceReference(newDescriptor, this.revision,
                    this.compressionFlags);
            }


            if (this.type.Value == ResourceType.Plan.Value)
            {
                RPlan plan = this.loadResource<RPlan>();
                plan.ThingData = Bytes.Replace(plan.ThingData, oldDescBuffer,
                    newDescBuffer);
                if (isFSB && plan.InventoryData != null)
                {
                    if (oldDescriptor.GetGUID().Equals(plan.InventoryData.HighlightSound))
                        plan.InventoryData.HighlightSound = newDescriptor.GetGUID();
                }
                this.data = plan.Build(this.revision, this.compressionFlags).Buffer;
            }

            this.data = Bytes.Replace(this.data, oldDescBuffer, newDescBuffer);
        }
        else
        {
            if (this.meshInfo.fallmap.Equals(oldDescriptor))
                this.meshInfo.fallmap = newDescriptor;
            if (this.meshInfo.lightmap.Equals(oldDescriptor))
                this.meshInfo.lightmap = newDescriptor;
            if (this.meshInfo.risemap.Equals(oldDescriptor))
                this.meshInfo.risemap = newDescriptor;
            foreach (StaticPrimitive primitive in this.meshInfo.primitives)
                if (primitive.gmat.Equals(oldDescriptor))
                    primitive.gmat = newDescriptor;
        }

        this.dependencies.Remove(oldDescriptor);
        if (newDescriptor != null)
            this.dependencies.Add(newDescriptor);
    }

    public ResourceDescriptor[] getDependencies()
    {
        return this.dependencies.ToArray();
    }

    public byte getCompressionFlags()
    {
        return this.compressionFlags;
    }

    public Revision getRevision()
    {
        return this.revision;
    }

    public ResourceType getResourceType()
    {
        return this.type;
    }

    public SerializationType getSerializationType()
    {
        return this.method;
    }

    public CellGcmTexture getTextureInfo()
    {
        return this.textureInfo;
    }

    public StaticMeshInfo getMeshInfo()
    {
        return this.meshInfo;
    }
}