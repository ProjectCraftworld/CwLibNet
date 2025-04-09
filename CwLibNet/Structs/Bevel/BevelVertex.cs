using CwlibNet.Enums;
using CwLibNet.IO.Serializable;

namespace CwLibNet.Structs.Bevel
{
    public class BevelVertex : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;

        public float y;
        public float z;
        public float rigidity = 1.0f;
        public byte smoothWithPrevious = 1;
        public byte gmatSlot;
        public MappingMode mappingMode = MappingMode.DAVE;

        public override void serialize(Serializer serializer)
        {
            serializer.Serialize(ref y);
            serializer.Serialize(ref z);
            serializer.Serialize(ref rigidity);
            serializer.Serialize(ref smoothWithPrevious);
            serializer.Serialize(ref gmatSlot);
            serializer.Serialize(ref mappingMode);
        }

        public override int getAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }

        public string toString() 
        {
            return string.Format("BevelVertex{{v2({0:F}, {1:F}), {2}}}", this.y, this.z, this.mappingMode);
        }
    }
}