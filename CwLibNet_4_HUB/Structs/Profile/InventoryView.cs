using CwLibNet.Enums;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Profile 
{
    public class InventoryView : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x20;

        public int type;
        public int subType;
        public string title;
        public bool heartedOnly;
        public int customID;
        public InventorySortMode currentSortMode = InventorySortMode.INVALID;
        public InventorySortMode desiredSortMode = InventorySortMode.INVALID;

        public void Serialize()
        {
            Serializer.Serialize(ref type);
            Serializer.Serialize(ref subType);
            Serializer.Serialize(ref title);
            Serializer.Serialize(ref heartedOnly);
            Serializer.Serialize(ref customID);
            currentSortMode = Serializer.Serialize(ref currentSortMode);
            desiredSortMode = Serializer.Serialize(ref desiredSortMode);
        }   

        public int GetAllocatedSize() 
        {
            var size = BASE_ALLOCATION_SIZE;
            if (title != null)
            {
                size += title.Length * 2; // 2 bytes per char
            }
            return size;
        }
    }
}