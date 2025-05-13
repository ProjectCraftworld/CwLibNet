using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class Treasure : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int treasureID;
        public int planID;
        public int timestamp;

        public void Serialize(Serializer serializer) 
        {
            treasureID = serializer.I32(treasureID);
            planID = serializer.I32(planID);
            timestamp = serializer.I32(timestamp);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}