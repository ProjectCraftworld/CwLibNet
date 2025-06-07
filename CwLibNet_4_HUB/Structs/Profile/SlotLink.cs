using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class SlotLink : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public SlotID from = new();
        public SlotID to = new();

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref from);
            Serializer.Serialize(ref to);
        }

        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}