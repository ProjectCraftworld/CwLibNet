using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Profile;

public class CollectableData: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor Plan;
    public int Source;

    // -1 =invalid
    // 0 = egg
    // 1 = award_complete
    // 2 = award_collect
    // 3 = award_ace

    
    public void Serialize()
    {
        if (Serializer.GetRevision().GetVersion() < 0x1c2) return;
        Serializer.Serialize(ref Plan, Plan, ResourceType.Plan, true);
        Serializer.Serialize(ref Source);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}