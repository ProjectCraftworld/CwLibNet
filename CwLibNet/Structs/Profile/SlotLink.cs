using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;

namespace CwLibNet.Structs.Profile
{
    public class SlotLink : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public SlotID from = new();
        public SlotID to = new();

        public void Serialize(Serializer serializer)
        {
            from = serializer.Struct<SlotID>(from);
            to = serializer.Struct<SlotID>(to);
        }

        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}