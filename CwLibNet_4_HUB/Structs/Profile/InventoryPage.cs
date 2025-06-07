using CwLibNet.IO;
using CwLibNet.Enums;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class InventoryPage : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int inventoryPageTitleKey;
        public InventorySortMode desiredSortMode = InventorySortMode.INVALID;

        public void Serialize()
        {
            Serializer.Serialize(ref inventoryPageTitleKey);
            desiredSortMode = Serializer.Serialize(ref desiredSortMode);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}