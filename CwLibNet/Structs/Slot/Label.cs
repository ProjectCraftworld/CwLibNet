using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Slot 
{
    public class Label : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x8;

        public int key;
        public int order;

        public Label() { }

        public Label(int key, int order)
        {
            this.key = key;
            this.order = order;
        }

        public void Serialize(Serializer serializer)
        {
            key = serializer.I32(key);
            order = serializer.I32(order);
        }

        public int GetAllocatedSize()
        {
            return Label.BASE_ALLOCATION_SIZE;
        }

    }
}