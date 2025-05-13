using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class InventoryCollection : ISerializable 
    {
        public const int BaseAllocationSize = 0x10;

        public int InventoryCollectionIndex;
        public InventoryPage[]? InventoryPageInstances;
        public int CurrentPageNumber;

        public bool PoppetPowerupSelection;

        public void Serialize(Serializer serializer)
        {
            InventoryCollectionIndex = serializer.I32(InventoryCollectionIndex);
            InventoryPageInstances = serializer.Array(InventoryPageInstances);
            CurrentPageNumber = serializer.I32(CurrentPageNumber);
            if (serializer.GetRevision().GetSubVersion() >= (int)Revisions.COLLECTION_POPPET_POWERUP)
                PoppetPowerupSelection = serializer.Bool(PoppetPowerupSelection);
        }

        public int GetAllocatedSize() 
        {
            var size = BaseAllocationSize;
            if (InventoryPageInstances != null)
            {
                size += InventoryPageInstances.Length * InventoryPage.BASE_ALLOCATION_SIZE;
            }
            return size;
        }
    }
}