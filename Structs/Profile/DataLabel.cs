using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Profile
{
    public class DataLabel : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x8;
        public int labelIndex;
        public string name;
        public virtual void Serialize(Serializer serializer)
        {
            labelIndex = serializer.Serialize(labelIndex);
            name = serializer.Serialize(name);
        }

        public virtual int GetAllocatedSize()
        {
            int size = DataLabel.BASE_ALLOCATION_SIZE;
            if (this.name != null)
                size += (this.name.Length() * 2);
            return size;
        }
    }
}