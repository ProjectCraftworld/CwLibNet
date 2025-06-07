using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Profile
{
    public class PinProgress : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int progressType, progressCount;

        public void Serialize() 
        {
            Serializer.Serialize(ref progressType);
            Serializer.Serialize(ref progressCount);
        }
        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}