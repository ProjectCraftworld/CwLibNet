using CwLibNet.IO.Serializer;
using CwLibNet.Types;

namespace CwLibNet.IO
{
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

        /// <summary>
        /// Performs necessary fixes to a resource after serializing is finished.
        /// </summary>
        /// <param name="revision">The revision of the loaded resource</param>
        public virtual void OnLoadFinished(Revision revision)
        {
            // Using a default implementation since I'm not adding this to every resource right now.
        }

        /// <summary>
        /// Performs necessary fixes to a resource for serialization for a specified revision.
        /// </summary>
        /// <param name="revision">The target revision</param>
        public virtual void OnStartSave(Revision revision)
        {
            // Using a default implementation since I'm not adding this to every resource right now.
        }
        
        public abstract void Serialize(Serializer.Serializer serializer);
        
        public abstract int GetAllocatedSize();
    }
}