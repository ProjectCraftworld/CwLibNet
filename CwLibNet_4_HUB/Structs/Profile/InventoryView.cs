using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Profile 
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

        public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref type);
            Serializer.Serialize(ref subType);
            Serializer.Serialize(ref title);
            Serializer.Serialize(ref heartedOnly);
            Serializer.Serialize(ref customID);
            currentSortMode = serializer.Enum32(currentSortMode);
            desiredSortMode = serializer.Enum32(desiredSortMode);
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