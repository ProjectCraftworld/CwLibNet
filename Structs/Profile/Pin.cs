using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class Pin : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x50;
        public long id, progressType, category;
        public long titleLamsKey, descriptionLamsKey;
        public ResourceDescriptor icon;
        public long initialProgressValue, targetValue;
        public byte trophyToUnlock;
        public short behaviourFlags;
        public byte trophyToUnlockLBP1;
        public override void Serialize(Serializer serializer)
        {
            id = serializer.Serialize(id);
            progressType = serializer.Serialize(progressType);
            category = serializer.Serialize(category);
            titleLamsKey = serializer.Serialize(titleLamsKey);
            descriptionLamsKey = serializer.Serialize(descriptionLamsKey);
            icon = serializer.Serialize(icon, ResourceType.TEXTURE, true);
            initialProgressValue = serializer.Serialize(initialProgressValue);
            targetValue = serializer.Serialize(targetValue);
            trophyToUnlock = serializer.Serialize(trophyToUnlock);
            behaviourFlags = serializer.Serialize(behaviourFlags);
            if (serializer.GetRevision().GetVersion() >= 0x3f7)
                trophyToUnlockLBP1 = serializer.Serialize(trophyToUnlockLBP1);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}