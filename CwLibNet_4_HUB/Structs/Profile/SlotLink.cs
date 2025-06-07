using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class SlotLink : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public SlotID from = new();
        public SlotID to = new();

        public void Serialize()
        {
            from = Serializer.Serialize(ref from);
            to = Serializer.Serialize(ref to);
        }

        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}