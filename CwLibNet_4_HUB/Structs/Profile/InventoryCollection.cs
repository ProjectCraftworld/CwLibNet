using CwLibNet.Enums;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Profile 
{
    public class InventoryCollection : ISerializable 
    {
        public const int BaseAllocationSize = 0x10;

        public int InventoryCollectionIndex;
        public InventoryPage[]? InventoryPageInstances;
        public int CurrentPageNumber;

        public bool PoppetPowerupSelection;

        public void Serialize()
        {
            Serializer.Serialize(ref InventoryCollectionIndex);
            InventoryPageInstances = Serializer.Serialize(ref InventoryPageInstances);
            Serializer.Serialize(ref CurrentPageNumber);
            if (Serializer.GetRevision().GetSubVersion() >= (int)Revisions.COLLECTION_POPPET_POWERUP)
                Serializer.Serialize(ref PoppetPowerupSelection);
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