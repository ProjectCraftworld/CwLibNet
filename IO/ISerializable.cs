namespace CwLibNet.IO
{
    /// <summary>
    /// Interface that specifies that an object can be serialized as a pure data binary structure.
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// (De)serializes a structure that implements Serializable.
        /// </summary>
        /// <param name="serializer">Serializer instance</param>
        void Serialize(Serializer.Serializer serializer);
    
        /// <summary>
        /// Calculates the size necessary to store this structure
        /// </summary>
        /// <returns>Size of this structure</returns>
        int GetAllocatedSize();
    }
}