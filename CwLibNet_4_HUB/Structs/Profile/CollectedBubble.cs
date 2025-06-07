using CwLibNet.Enums;

using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class CollectedBubble : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x50;

        public ResourceDescriptor level;
        public int thingUID;
        public ResourceDescriptor plan;

        public void Serialize()
        {
            Serializer.Serialize(ref level, level, ResourceType.Level, true);
            Serializer.Serialize(ref thingUID);
            Serializer.Serialize(ref plan, plan, ResourceType.Plan, true);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}
