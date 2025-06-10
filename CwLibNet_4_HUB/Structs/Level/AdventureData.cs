using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Slot;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Level;

public class AdventureData: ISerializable
{
    public const int BASE_ALLOCATION_SIZE = 0x20;

    public int adventureFixedID;
    public int adventureItemPUIDCounter;
    public List<AdventureItem> adventureItems = [];
    public List<ResourceDescriptor> questDescriptors = [];
    public List<StartPoint> startPointList = [];

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        if (subVersion > 0xaf)
            Serializer.Serialize(ref adventureFixedID);

        if (subVersion > 0x93)
        {
            Serializer.Serialize(ref adventureItemPUIDCounter);
            Serializer.Serialize(ref adventureItems);
        }

        if (subVersion > 0xa6)
        {
            int numItems = questDescriptors?.Count ?? 0;
            Serializer.Serialize(ref numItems);
            if (Serializer.IsWriting())
            {
                foreach (var descriptor in questDescriptors)
                {
                    var temp = descriptor;
                    Serializer.Serialize(ref temp, ResourceType.Quest, true, false, false);
                }
            }
            else
            {
                questDescriptors = new(numItems);
                for (var i = 0; i < numItems; ++i)
                {
                    ResourceDescriptor? nullQuest = null;
                    Serializer.Serialize(ref nullQuest, ResourceType.Quest);
                    questDescriptors.Add(nullQuest);
                }
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