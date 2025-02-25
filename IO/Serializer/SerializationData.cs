using CwLibNet.Enums;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.IO.Serialization
{
    public class SerializationData
    {
        public byte[] Buffer;
        public readonly Revision Revision;
        public readonly byte CompressionFlags;
        public readonly ResourceType Type;
        public readonly SerializationType Method;
        public readonly ResourceDescriptor[] Dependencies;
        public readonly CellGcmTexture TextureInfo;
        public readonly StaticMeshInfo StaticMeshInfo;
    }
}