using CwLibNet.IO;
using CwLibNet.IO.Serialization;
namespace CwLibNet.Structs.Profile
{
    public class LegacyInventoryCollection : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public InventoryView[] inventoryViews;
        public int currentPageNumber;
        public int collectionID;
        public int actionOnItemSelect;
        public override void Serialize(Serializer serializer)
        {
            inventoryViews = serializer.Serialize(inventoryViews, typeof(InventoryView), true);
            currentPageNumber = serializer.Serialize(currentPageNumber);
            collectionID = serializer.Serialize(collectionID);
            actionOnItemSelect = serializer.Serialize(actionOnItemSelect);
        }

        public virtual int GetAllocatedSize()
        {
            int size = LegacyInventoryCollection.BASE_ALLOCATION_SIZE;
            if (this.inventoryViews != null)
                foreach (InventoryView view in this.inventoryViews)
                    size += view.GetAllocatedSize();
            return size;
        }
    }
}