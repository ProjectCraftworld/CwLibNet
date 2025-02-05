using CwLibNet.Resources;
using CwLibNet.Singleton;
using CwLibNet.Enums;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Profile
{
    public class InventoryCollection : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public int inventoryCollectionIndex;
        public InventoryPage[] inventoryPageInstances;
        public int currentPageNumber;
        public bool poppetPowerupSelection;
        public override void Serialize(Serializer serializer)
        {
            inventoryCollectionIndex = serializer.Serialize(inventoryCollectionIndex);
            inventoryPageInstances = serializer.Serialize(inventoryPageInstances, typeof(InventoryPage), true);
            currentPageNumber = serializer.Serialize(currentPageNumber);
            if (serializer.GetRevision().GetSubVersion() >= Revisions.COLLECTION_POPPET_POWERUP)
                poppetPowerupSelection = serializer.Serialize(poppetPowerupSelection);
        }

        public virtual int GetAllocatedSize()
        {
            int size = InventoryCollection.BASE_ALLOCATION_SIZE;
            if (this.inventoryPageInstances != null)
                size += (this.inventoryPageInstances.length * InventoryPage.BASE_ALLOCATION_SIZE);
            return size;
        }
    }
}