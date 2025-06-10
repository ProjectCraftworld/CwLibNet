using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;

namespace CwLibNet4Hub.IO;

/// <summary>
/// Interface that specifies that an object can be serialized as a pure data binary structure.
/// </summary>
public interface ISerializable
{
    /// <summary>
    /// (De)serializes a structure that implements Serializable.
    /// </summary>
    /// <param name="serializer">Serializer instance</param>
    void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer);
    
    /// <summary>
    /// Calculates the size necessary to store this structure
    /// </summary>
    /// <returns>Size of this structure</returns>
    int GetAllocatedSize();
}