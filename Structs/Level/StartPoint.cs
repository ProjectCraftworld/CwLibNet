using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Streaming;

namespace CwLibNet.Structs.Level
{
    public class StartPoint : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public SlotID slot;
        public List<StreamingID> ids = new List();
        public override void Serialize(Serializer serializer)
        {
            slot = serializer.Serialize(slot, typeof(SlotID));
            ids = serializer.Serialize(ids, typeof(StreamingID));
        }

        public virtual int GetAllocatedSize()
        {
            int size = StartPoint.BASE_ALLOCATION_SIZE;
            if (this.ids != null)
            {
                foreach (StreamingID id in this.ids)
                    size += id.GetAllocatedSize();
            }

            return size;
        }
    }
}