using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

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

    
    public void Serialize(Serializer serializer)
    {
        if (serializer.GetRevision().GetVersion() < 0x1c2) return;
        Plan = serializer.Resource(Plan, ResourceType.Plan, true);
        Source = serializer.S32(Source);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}