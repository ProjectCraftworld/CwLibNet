using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Profile
{
    public class MysteryPodEventSeen : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int id;
        public int type;

        public void Serialize()
        {
            Serializer.Serialize(ref id);
            Serializer.Serialize(ref type);
        }

        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}