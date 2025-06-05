using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Level;

public class AdventureData: ISerializable
{
    public const int BASE_ALLOCATION_SIZE = 0x20;

    public int adventureFixedID;
    public int adventureItemPUIDCounter;
    public List<AdventureItem> adventureItems = [];
    public List<ResourceDescriptor> questDescriptors = [];
    public List<StartPoint> startPointList = [];

    
    public void Serialize(Serializer serializer)
    {
        var subVersion = serializer.GetRevision().GetSubVersion();

        if (subVersion > 0xaf)
            adventureFixedID = serializer.I32(adventureFixedID);

        if (subVersion > 0x93)
        {
            adventureItemPUIDCounter = serializer.I32(adventureItemPUIDCounter);
            adventureItems = serializer.Arraylist(adventureItems);
        }

        if (subVersion > 0xa6)
        {
            var numItems = serializer.I32(questDescriptors != null ?
                questDescriptors.Count : 0);
            if (serializer.IsWriting())
            {
                foreach (var descriptor in questDescriptors)
                    serializer.Resource(descriptor, ResourceType.Quest, true);
            }
            else
            {
                questDescriptors = new(numItems);
                for (var i = 0; i < numItems; ++i)
                    questDescriptors.Add(serializer.Resource(null, ResourceType.Quest,
                        true));
            }
        }

        if (subVersion >= 0xd1)
            startPointList = serializer.Arraylist(startPointList);
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