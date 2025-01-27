using CwLibNet.IO;

namespace CwLibNet.Types.Profile;

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
        Id = serializer.u32(Id);
        ProgressType = serializer.u32(ProgressType);
        Category = serializer.u32(Category);
        TitleLamsKey = serializer.u32(TitleLamsKey);
        DescriptionLamsKey = serializer.u32(DescriptionLamsKey);
        Icon = serializer.resource(Icon, ResourceType.Texture, true);
        InitialProgressValue = serializer.u32(InitialProgressValue);
        TargetValue = serializer.u32(TargetValue);
        TrophyToUnlock = serializer.i8(TrophyToUnlock);
        BehaviourFlags = serializer.i16(BehaviourFlags);
        if (serializer.getRevision().getVersion() >= 0x3f7)
            TrophyToUnlockLbp1 = serializer.i8(TrophyToUnlockLbp1);
    }
    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}