using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Slot 
{
    public class Collectabubble : ISerializable
    {
        public static readonly int BaseAllocationSize = 0x8;

        public ResourceDescriptor plan;
        public int count;

        public void Serialize(Serializer serializer)
        {
            plan = serializer.Resource(plan, ResourceType.Plan, true);
            count = serializer.I32(count);
        }

        public int GetAllocatedSize()
        {
            return Collectabubble.BaseAllocationSize;
        }
    }
}