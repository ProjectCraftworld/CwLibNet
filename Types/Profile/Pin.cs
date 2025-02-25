using Cwlib.Io.Serializer;
using CwLibNet.Enums;
using CwLibNet.IO;

namespace CwLibNet.Types.Profile
{
    public class Pin : ISerializable
    {
        public static int BaseAllocationSize = 0x50;

        public long Id, ProgressType, Category;
        public long TitleLamsKey, DescriptionLamsKey;
        public ResourceDescriptor Icon;
        public long InitialProgressValue, TargetValue;
        public byte TrophyToUnlock;
        public short BehaviourFlags;
        public byte TrophyToUnlockLbp1;
    
        public void Serialize(Serializer serializer)
        {
            Id = serializer.U32(Id);
            ProgressType = serializer.U32(ProgressType);
            Category = serializer.U32(Category);
            TitleLamsKey = serializer.U32(TitleLamsKey);
            DescriptionLamsKey = serializer.U32(DescriptionLamsKey);
            Icon = serializer.Resource(Icon, ResourceType.Texture, true);
            InitialProgressValue = serializer.U32(InitialProgressValue);
            TargetValue = serializer.U32(TargetValue);
            TrophyToUnlock = serializer.I8(TrophyToUnlock);
            BehaviourFlags = serializer.I16(BehaviourFlags);
            if (serializer.GetRevision().GetVersion() >= 0x3f7)
                TrophyToUnlockLbp1 = serializer.I8(TrophyToUnlockLbp1);
        }
    
        public int GetAllocatedSize()
        {
            return BaseAllocationSize;
        }
    }
}