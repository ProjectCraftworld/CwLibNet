using CwLibNet4Hub.Enums;

using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Profile
{
    public class CollectedBubble : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x50;

        public ResourceDescriptor level;
        public int thingUID;
        public ResourceDescriptor plan;

        public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref level, ResourceType.Level, true, false, false);
            Serializer.Serialize(ref thingUID);
            Serializer.Serialize(ref plan, ResourceType.Plan, true, false, false);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}
