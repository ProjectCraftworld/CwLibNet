using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Mesh
{
    public class SoftbodyVertEquivalence : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x4;
        public short first, count;
        public SoftbodyVertEquivalence()
        {
        }

        public SoftbodyVertEquivalence(int first, int count)
        {
            this.first = (short)first;
            this.count = (short)count;
        }

        public override void Serialize(Serializer serializer)
        {
            first = serializer.Serialize(first);
            count = serializer.Serialize(count);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}