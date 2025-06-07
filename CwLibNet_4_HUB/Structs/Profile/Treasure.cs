using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Profile;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Profile
{
    public class Treasure : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int treasureID;
        public int planID;
        public int timestamp;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer) 
        {
            Serializer.Serialize(ref treasureID);
            Serializer.Serialize(ref planID);
            Serializer.Serialize(ref timestamp);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}