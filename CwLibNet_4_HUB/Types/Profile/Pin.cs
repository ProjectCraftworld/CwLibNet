using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Types.Profile;

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
    
    public void Serialize()
    {
        Serializer.Serialize(ref Id);
        Serializer.Serialize(ref ProgressType);
        Serializer.Serialize(ref Category);
        Serializer.Serialize(ref TitleLamsKey);
        Serializer.Serialize(ref DescriptionLamsKey);
        Serializer.Serialize(ref Icon, Icon, ResourceType.Texture, true);
        Serializer.Serialize(ref InitialProgressValue);
        Serializer.Serialize(ref TargetValue);
        Serializer.Serialize(ref TrophyToUnlock);
        Serializer.Serialize(ref BehaviourFlags);
        if (Serializer.GetRevision().GetVersion() >= 0x3f7)
            Serializer.Serialize(ref TrophyToUnlockLbp1);
    }
    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}