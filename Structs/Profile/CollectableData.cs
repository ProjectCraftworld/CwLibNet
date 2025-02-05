using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;
namespace CwLibNet.Structs.Profile
{
    public class CollectableData : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x30;
        public ResourceDescriptor plan;
        public int source;
        // -1 =invalid
        // 0 = egg
        // 1 = award_complete
        // 2 = award_collect
        // 3 = award_ace
        public override void Serialize(Serializer serializer)
        {
            if (serializer.GetRevision().GetVersion() >= 0x1c2)
            {
                plan = serializer.Serialize(plan, ResourceType.PLAN, true);
                source = serializer.Serialize(source);
            }
        }

        // -1 =invalid
        // 0 = egg
        // 1 = award_complete
        // 2 = award_collect
        // 3 = award_ace
        public virtual int GetAllocatedSize()
        {
            return CollectableData.BASE_ALLOCATION_SIZE;
        }
    }
}