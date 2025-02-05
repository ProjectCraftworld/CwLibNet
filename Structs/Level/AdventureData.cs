using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Level
{
    public class AdventureData : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;
        public int adventureFixedID;
        public int adventureItemPUIDCounter;
        public List<AdventureItem> adventureItems = new List();
        public List<ResourceDescriptor> questDescriptors = new List();
        public List<StartPoint> startPointList = new List();
        public override void Serialize(Serializer serializer)
        {
            int subVersion = serializer.GetRevision().GetSubVersion();
            if (subVersion > 0xaf)
                adventureFixedID = serializer.Serialize(adventureFixedID);
            if (subVersion > 0x93)
            {
                adventureItemPUIDCounter = serializer.Serialize(adventureItemPUIDCounter);
                adventureItems = serializer.Serialize(adventureItems, typeof(AdventureItem));
            }

            if (subVersion > 0xa6)
            {
                int numItems = serializer.Serialize(questDescriptors != null ? questDescriptors.Count : 0);
                if (serializer.IsWriting())
                {
                    foreach (ResourceDescriptor descriptor in questDescriptors)
                        serializer.Resource(descriptor, ResourceType.QUEST, true);
                }
                else
                {
                    questDescriptors = new List(numItems);
                    for (int i = 0; i < numItems; ++i)
                        questDescriptors.Add(serializer.Resource(null, ResourceType.QUEST, true));
                }
            }

            if (subVersion >= 0xd1)
                startPointList = serializer.Serialize(startPointList, typeof(StartPoint));
        }

        public virtual int GetAllocatedSize()
        {
            int size = AdventureData.BASE_ALLOCATION_SIZE;
            if (this.adventureItems != null)
                size += (this.adventureItems.Count * AdventureItem.BASE_ALLOCATION_SIZE);
            if (this.questDescriptors != null)
                size += (this.questDescriptors.Count * 0x24);
            if (this.startPointList != null)
                size += (this.startPointList.Count * SlotID.BASE_ALLOCATION_SIZE);
            return size;
        }
    }
}