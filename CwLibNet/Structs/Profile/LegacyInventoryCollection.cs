using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class LegacyInventoryCollection : ISerializable 
    {
        public const int BaseAllocationSize = 0x10;

        public InventoryView[]? InventoryViews;
        public int CurrentPageNumber;
        public int CollectionId;
        public int ActionOnItemSelect;

        public void Serialize(Serializer serializer) 
        {
            InventoryViews = serializer.Array(InventoryViews, true) ?? []; // Ensure non-null assignment
            CurrentPageNumber = serializer.I32(CurrentPageNumber);
            CollectionId = serializer.I32(CollectionId);
            ActionOnItemSelect = serializer.I32(ActionOnItemSelect);
        }

        public int GetAllocatedSize() 
        {
            var size = BaseAllocationSize;
            if (InventoryViews == null) return size;
            size += InventoryViews.Sum(view => view.GetAllocatedSize());
            return size;
        }
    }
}