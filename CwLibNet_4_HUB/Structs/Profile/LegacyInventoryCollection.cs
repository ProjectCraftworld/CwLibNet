using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Profile 
{
    public class LegacyInventoryCollection : ISerializable 
    {
        public const int BaseAllocationSize = 0x10;

        public InventoryView[]? InventoryViews;
        public int CurrentPageNumber;
        public int CollectionId;
        public int ActionOnItemSelect;

        public void Serialize() 
        {
            InventoryViews = Serializer.Array(InventoryViews, true) ?? []; // Ensure non-null assignment
            Serializer.Serialize(ref CurrentPageNumber);
            Serializer.Serialize(ref CollectionId);
            Serializer.Serialize(ref ActionOnItemSelect);
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