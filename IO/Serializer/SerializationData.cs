using Cwlib.Enums;
using Cwlib.Io.Serializer;
using CwLibNet.Enums;
using CwLibNet.Structs.Texture;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.IO.Serialization
{
    public class SerializationData
    {
        public byte[]? Buffer;
        public readonly Revision? Revision;
        public readonly byte? CompressionFlags;
        public readonly ResourceType? Type;
        public readonly SerializationType? Method;
        public readonly ResourceDescriptor[]? Dependencies;
        public readonly CellGcmTexture? TextureInfo;
        public readonly StaticMeshInfo? StaticMeshInfo;

        public SerializationData(
        byte[] Buffer,
        Revision Revision,
        byte CompressionFlags,
        ResourceType Type,
        SerializationType Method,
        ResourceDescriptor[] Dependencies)
        {
            this.Buffer = Buffer;
            this.Revision = Revision;
            this.CompressionFlags = CompressionFlags;
            this.Type = Type;
            this.Method = Method;
            this.Dependencies = Dependencies;
            this.TextureInfo = null;
            this.StaticMeshInfo = null;
        }

        /**
         * Creates a serialization data structure for texture data.
         *
         * @param Buffer Decompressed texture data
         */
        public SerializationData(byte[] Buffer)
        {
            this.Buffer = Buffer;
            this.Revision = null;
            this.CompressionFlags = Cwlib.Enums.CompressionFlags.USE_NO_COMPRESSION;
            this.Type = ResourceType.Texture;
            this.Method = SerializationType.COMPRESSED_TEXTURE;
            this.Dependencies = null;
            this.TextureInfo = null;
            this.StaticMeshInfo = null;
        }

        /**
         * Creates a serialization data structure for GTF texture data.
         *
         * @param Buffer Decompressed texture data
         * @param info   Texture metadata
         */
        public SerializationData(byte[] Buffer, CellGcmTexture info)
        {
            this.Buffer = Buffer;
            this.Revision = null;
            this.CompressionFlags = Cwlib.Enums.CompressionFlags.USE_NO_COMPRESSION;
            this.Type = ResourceType.GtfTexture;
            this.Method = info.GetMethod();
            this.Dependencies = null;
            this.TextureInfo = info;
            this.StaticMeshInfo = null;
        }

        /**
         * Creates a serialization data structure for static mesh data
         *
         * @param Buffer   Decompressed static mesh
         * @param Revision Revision of the serialized resource
         * @param info     Static mesh metadata
         */
        public SerializationData(byte[] Buffer, Revision Revision, StaticMeshInfo info)
        {
            this.Buffer = Buffer;
            this.Revision = Revision;
            this.CompressionFlags = Cwlib.Enums.CompressionFlags.USE_NO_COMPRESSION;
            this.Type = ResourceType.StaticMesh;
            this.Method = SerializationType.BINARY;

            // Maybe I need a Method for gathering Dependencies without actually
            // serializing the data, can probably use reflection for it,
            // as to avoid serializing twice.

            Serializer? serializer = new(info.getAllocatedSize(), Revision,
                Cwlib.Enums.CompressionFlags.USE_NO_COMPRESSION);
            serializer.Struct<StaticMeshInfo>(info, typeof(StaticMeshInfo));
            this.Dependencies = serializer.GetDependencies();

            this.TextureInfo = null;
            this.StaticMeshInfo = info;
    }
}
}