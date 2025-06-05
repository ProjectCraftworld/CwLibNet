using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

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

        public void Serialize(Serializer serializer) 
        {
            id = serializer.U32(id);
            progressType = serializer.U32(progressType);
            category = serializer.U32(category);
            titleLamsKey = serializer.U32(titleLamsKey);
            descriptionLamsKey = serializer.U32(descriptionLamsKey);
            icon = serializer.Resource(icon, ResourceType.Texture);
            initialProgressValue = serializer.U32(initialProgressValue);
            targetValue = serializer.U32(targetValue);
            trophyToUnlock = serializer.I8(trophyToUnlock);
            behaviorFlags = serializer.I16(behaviorFlags);
            if (serializer.GetRevision().GetVersion() >= 0x3f7)
                trophyToUnlockLBP1 = serializer.I8(trophyToUnlockLBP1);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}