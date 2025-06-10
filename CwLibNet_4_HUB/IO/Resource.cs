using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.IO;

/// <summary>
/// Interface used to specify that an object can be compressed to a resource.
/// </summary>
public abstract class Resource : ISerializable
{
    /// <summary>
    /// Builds this resource using specified revision and compression flags and returns the data necessary to properly compress it.
    /// </summary>
    /// <param name="revision">Revision of the resource</param>
    /// <param name="compressionFlags">Compression flags used during resource serialization</param>
    /// <returns>Serialization data</returns>
    public abstract SerializationData Build(Revision revision, byte compressionFlags);

    public abstract void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer);
        
    public abstract int GetAllocatedSize();
}