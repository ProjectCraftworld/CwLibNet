using CwLibNet4Hub.IO;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Profile 
{
    public class InventoryPage : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int inventoryPageTitleKey;
        public InventorySortMode desiredSortMode = InventorySortMode.INVALID;

        public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref inventoryPageTitleKey);
            desiredSortMode = serializer.Enum32(desiredSortMode);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}