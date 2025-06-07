using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Level;

public class AdventureData: ISerializable
{
    public const int BASE_ALLOCATION_SIZE = 0x20;

    public int adventureFixedID;
    public int adventureItemPUIDCounter;
    public List<AdventureItem> adventureItems = [];
    public List<ResourceDescriptor> questDescriptors = [];
    public List<StartPoint> startPointList = [];

    
    public void Serialize()
    {
        var subVersion = Serializer.GetRevision().GetSubVersion();

        if (subVersion > 0xaf)
            Serializer.Serialize(ref adventureFixedID);

        if (subVersion > 0x93)
        {
            Serializer.Serialize(ref adventureItemPUIDCounter);
            Serializer.Serialize(ref adventureItems);
        }

        if (subVersion > 0xa6)
        {
            var numItems = Serializer.Serialize(ref questDescriptors != null ?
                questDescriptors.Count : 0);
            if (Serializer.IsWriting())
            {
                foreach (var descriptor in questDescriptors)
                    Serializer.Serialize(ref descriptor, ResourceType.Quest, true);
            }
            else
            {
                questDescriptors = new(numItems);
                for (var i = 0; i < numItems; ++i)
                    questDescriptors.Add(Serializer.Serialize(ref null, ResourceType.Quest));
            }
        }

        if (subVersion >= 0xd1)
            Serializer.Serialize(ref startPointList);
    }

    
    public int GetAllocatedSize()
    {
        var size = BASE_ALLOCATION_SIZE;
        if (adventureItems != null)
            size += adventureItems.Count * AdventureItem.BaseAllocationSize;
        if (questDescriptors != null)
            size += questDescriptors.Count * 0x24;
        if (startPointList != null)
            size += startPointList.Count * SlotID.BaseAllocationSize;
        return size;
    }


}