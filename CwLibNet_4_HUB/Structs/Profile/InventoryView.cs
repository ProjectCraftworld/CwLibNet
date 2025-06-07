using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;
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

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref type);
            Serializer.Serialize(ref subType);
            Serializer.Serialize(ref title);
            Serializer.Serialize(ref heartedOnly);
            Serializer.Serialize(ref customID);
            Serializer.Serialize(ref currentSortMode);
            Serializer.Serialize(ref desiredSortMode);
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