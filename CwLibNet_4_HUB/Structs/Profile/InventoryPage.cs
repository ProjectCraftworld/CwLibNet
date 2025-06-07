using CwLibNet.IO;
using CwLibNet.Enums;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class InventoryPage : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int inventoryPageTitleKey;
        public InventorySortMode desiredSortMode = InventorySortMode.INVALID;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref inventoryPageTitleKey);
            Serializer.Serialize(ref desiredSortMode);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}