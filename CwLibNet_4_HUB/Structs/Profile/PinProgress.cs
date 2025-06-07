using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Profile
{
    public class PinProgress : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int progressType, progressCount;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer) 
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