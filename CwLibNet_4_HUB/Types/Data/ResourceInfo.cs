using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Types.Data;

public class ResourceInfo
{
    private const int MaxDependencyDepth = 2;
    private object Resource;
    public Revision Revision;
    public ResourceType Type = ResourceType.Invalid;
    public SerializationType Method = SerializationType.UNKNOWN;
    public byte CompressionFlags = Enums.CompressionFlags.USE_NO_COMPRESSION;
    public ResourceDescriptor[] Dependencies = [];
    public bool IsMissingDependencies;
    
    public bool IsResource()
    {
        return Type.Value != ResourceType.Invalid.Value;
    }
    
    public bool IsCompressedResource()
    {
        return IsResource() &&
               (Method == SerializationType.BINARY || Method == SerializationType.ENCRYPTED_BINARY);
    }
    
    public T GetResource<T>()
    {
        return (T) Resource;
    }

}