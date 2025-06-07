using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Profile 
{
    public class PinAward : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int pinID, awardCount;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer) 
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