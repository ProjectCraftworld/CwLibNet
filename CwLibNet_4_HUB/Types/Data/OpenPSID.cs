using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Types.Data
{
    public class OpenPSID : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x8;

        private long high, low;

        public OpenPSID() { }

        public OpenPSID(long high, long low)
        {
            this.high = high;
            this.low = low;
        }

        public void Serialize()
        {
            Serializer.Serialize(ref high);
            Serializer.Serialize(ref low);
        }

        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }

        public long GetHigh()
        {
            return high;
        }

        public long GetLow()
        {
            return low;
        }
    }
}
