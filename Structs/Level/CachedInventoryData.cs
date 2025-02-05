using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Level
{
    public class CachedInventoryData : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;
        public long category, location;
        public int cachedToolType;
        // CachedUVs
        public byte U0, U1, V0, V1;
        // CachedUVs
        public override void Serialize(Serializer serializer)
        {
            int version = serializer.GetRevision().GetVersion();
            category = serializer.Serialize(category);
            location = serializer.Serialize(location);
            if (version > 0x39d)
            {
                cachedToolType = serializer.Serialize(cachedToolType);
                U0 = serializer.Serialize(U0);
                U1 = serializer.Serialize(U1);
                V0 = serializer.Serialize(V0);
                V1 = serializer.Serialize(V1);
            }
        }

        // CachedUVs
        public virtual int GetAllocatedSize()
        {
            return CachedInventoryData.BASE_ALLOCATION_SIZE;
        }
    }
}