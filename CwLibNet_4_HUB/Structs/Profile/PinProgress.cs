using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Profile
{
    public class PinProgress : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int progressType, progressCount;

        public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer) 
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