using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Profile 
{
    public class PinAward : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int pinID, awardCount;

        public void Serialize() 
        {
            Serializer.Serialize(ref pinID);
            Serializer.Serialize(ref awardCount);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}