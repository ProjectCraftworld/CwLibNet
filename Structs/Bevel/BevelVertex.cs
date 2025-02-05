using System;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Bevel
{
    public class BevelVertex : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public float y;
        public float z;
        public float rigidity = 1F;
        public byte smoothWithPrevious = 1;
        public byte gmatSlot;
        public MappingMode mappingMode = MappingMode.DAVE;
        public override void Serialize(Serializer serializer)
        {
            y = serializer.Serialize(y);
            z = serializer.Serialize(z);
            rigidity = serializer.Serialize(rigidity);
            smoothWithPrevious = serializer.I8(smoothWithPrevious);
            gmatSlot = serializer.I8(gmatSlot);
            mappingMode = serializer.Enum8(mappingMode);
        }

        public virtual int GetAllocatedSize()
        {
            return BevelVertex.BASE_ALLOCATION_SIZE;
        }

        public virtual string ToString()
        {
            return String.Format(Locale.ROOT, "BevelVertex{v2(%f, %f), %s}", this.y, this.z, this.mappingMode);
        }
    }
}