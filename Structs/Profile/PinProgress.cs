using CwLibNet.IO;
using CwLibNet.IO.Serialization;
namespace CwLibNet.Structs.Profile
{
    public class PinProgress : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public int progressType, progressCount;
        public virtual void Serialize(Serializer serializer)
        {
            progressType = serializer.Serialize(progressType);
            progressCount = serializer.Serialize(progressCount);
        }

        public virtual int GetAllocatedSize()
        {
            return PinProgress.BASE_ALLOCATION_SIZE;
        }
    }
}