using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class InventoryPage : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public int inventoryPageTitleKey;
        public InventorySortMode desiredSortMode = InventorySortMode.INVALID;
        public override void Serialize(Serializer serializer)
        {
            inventoryPageTitleKey = serializer.Serialize(inventoryPageTitleKey);
            desiredSortMode = serializer.Serialize(desiredSortMode);
        }

        public virtual int GetAllocatedSize()
        {
            return InventoryPage.BASE_ALLOCATION_SIZE;
        }
    }
}