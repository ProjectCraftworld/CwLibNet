using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Types.Profile;

public class Pin : ISerializable
{
    public static int BaseAllocationSize = 0x50;

    public long Id, ProgressType, Category;
    public long TitleLamsKey, DescriptionLamsKey;
    public ResourceDescriptor? Icon;
    public long InitialProgressValue, TargetValue;
    public byte TrophyToUnlock;
    public short BehaviourFlags;
    public byte TrophyToUnlockLbp1;
    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Id);
        Serializer.Serialize(ref ProgressType);
        Serializer.Serialize(ref Category);
        Serializer.Serialize(ref TitleLamsKey);
        Serializer.Serialize(ref DescriptionLamsKey);
        Serializer.Serialize(ref Icon, ResourceType.Texture, true, false, false);
        Serializer.Serialize(ref InitialProgressValue);
        Serializer.Serialize(ref TargetValue);
        Serializer.Serialize(ref TrophyToUnlock);
        Serializer.Serialize(ref BehaviourFlags);
        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() >= 0x3f7)
            Serializer.Serialize(ref TrophyToUnlockLbp1);
    }
    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}