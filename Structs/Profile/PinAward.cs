using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Profile
{
    public class PinAward : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public int pinID, awardCount;
        public override void Serialize(Serializer serializer)
        {
            pinID = serializer.Serialize(pinID);
            awardCount = serializer.Serialize(awardCount);
        }

        public virtual int GetAllocatedSize()
        {
            return PinAward.BASE_ALLOCATION_SIZE;
        }
    }
}