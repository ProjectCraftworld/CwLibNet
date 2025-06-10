using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Profile;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() < 0x1c2) return;
        Serializer.Serialize(ref Plan, ResourceType.Plan, true, false, false);
        Serializer.Serialize(ref Source);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}