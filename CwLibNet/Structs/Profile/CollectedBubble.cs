using CwLibNet.Enums;

using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class CollectedBubble : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x50;

        public ResourceDescriptor level;
        public int thingUID;
        public ResourceDescriptor plan;

        public void Serialize(Serializer serializer)
        {
            level = serializer.Resource(level, ResourceType.Level, true);
            thingUID = serializer.I32(thingUID);
            plan = serializer.Resource(plan, ResourceType.Plan, true);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}
