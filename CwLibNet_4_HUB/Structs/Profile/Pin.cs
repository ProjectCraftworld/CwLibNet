using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class Pin : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x50;

        public long id, progressType, category;
        public long titleLamsKey, descriptionLamsKey;
        public ResourceDescriptor icon;
        public long initialProgressValue, targetValue;
        public byte trophyToUnlock;
        public short behaviorFlags;
        public byte trophyToUnlockLBP1;

        public void Serialize() 
        {
            Serializer.Serialize(ref id);
            Serializer.Serialize(ref progressType);
            Serializer.Serialize(ref category);
            Serializer.Serialize(ref titleLamsKey);
            Serializer.Serialize(ref descriptionLamsKey);
            Serializer.Serialize(ref icon, icon, ResourceType.Texture);
            Serializer.Serialize(ref initialProgressValue);
            Serializer.Serialize(ref targetValue);
            Serializer.Serialize(ref trophyToUnlock);
            Serializer.Serialize(ref behaviorFlags);
            if (Serializer.GetRevision().GetVersion() >= 0x3f7)
                Serializer.Serialize(ref trophyToUnlockLBP1);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}