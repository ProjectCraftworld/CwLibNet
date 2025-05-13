using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class InventoryCollection : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int inventoryCollectionIndex;
        public InventoryPage[] inventoryPageInstances;
        public int currentPageNumber;

        public bool poppetPowerupSelection;

        public void Serialize(Serializer serializer)
        {
            inventoryCollectionIndex = serializer.I32(inventoryCollectionIndex);
            inventoryPageInstances = serializer.Array<InventoryPage>(inventoryPageInstances);
            currentPageNumber = serializer.I32(currentPageNumber);
            if (serializer.GetRevision().GetSubVersion() >= (int)Revisions.CollectionPoppetPowerup)
                poppetPowerupSelection = serializer.Bool(poppetPowerupSelection);
        }

        public int GetAllocatedSize() 
        {
            int size = BASE_ALLOCATION_SIZE;
            if (inventoryPageInstances != null)
            {
                size += inventoryPageInstances.Length * InventoryPage.BASE_ALLOCATION_SIZE;
            }
            return size;
        }
    }
}