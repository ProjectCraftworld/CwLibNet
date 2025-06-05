using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class MysteryPodEventSeen : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int id;
        public int type;

        public void Serialize(Serializer serializer)
        {
            id = serializer.I32(id);
            type = serializer.I32(type);
        }

        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}