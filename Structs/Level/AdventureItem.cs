using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Level
{
    public class AdventureItem : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x30;
        public int PUID;
        public ResourceDescriptor descriptor;
        public int flags;
        public int iconPUID;
        public override void Serialize(Serializer serializer)
        {
            int subVersion = serializer.GetRevision().GetSubVersion();
            PUID = serializer.Serialize(PUID);
            descriptor = serializer.Serialize(descriptor, ResourceType.PLAN, true);
            if (subVersion < 0xae)
            {
                serializer.Serialize(0);
                serializer.Serialize(0);
                serializer.Serialize(0);
                serializer.Serialize(0);
            }

            if (subVersion > 0xbe)
                flags = serializer.Serialize(flags);
            if (subVersion > 0xe0)
                iconPUID = serializer.Serialize(iconPUID);
        }

        public virtual int GetAllocatedSize()
        {
            return AdventureItem.BASE_ALLOCATION_SIZE;
        }
    }
}