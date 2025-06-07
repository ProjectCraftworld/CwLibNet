using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Profile 
{
    public class InventoryCollection : ISerializable 
    {
        public const int BaseAllocationSize = 0x10;

        public int InventoryCollectionIndex;
        public InventoryPage[]? InventoryPageInstances;
        public int CurrentPageNumber;

        public bool PoppetPowerupSelection;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref InventoryCollectionIndex);
            Serializer.Serialize(ref InventoryPageInstances);
            Serializer.Serialize(ref CurrentPageNumber);
            if (Serializer.GetCurrentSerializer().GetRevision().GetSubVersion() >= (int)Revisions.COLLECTION_POPPET_POWERUP)
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