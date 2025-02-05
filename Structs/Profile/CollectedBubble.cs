using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class CollectedBubble : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x50;
        public ResourceDescriptor level;
        public int thingUID;
        public ResourceDescriptor plan;
        public override void Serialize(Serializer serializer)
        {
            level = serializer.Serialize(level, ResourceType.LEVEL, true);
            thingUID = serializer.Serialize(thingUID);
            plan = serializer.Serialize(plan, ResourceType.PLAN, true);
        }

        public virtual int GetAllocatedSize()
        {
            return CollectedBubble.BASE_ALLOCATION_SIZE;
        }
    }
}