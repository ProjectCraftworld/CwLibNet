using System;
using System.Collections.Generic;

using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class LegacyInventoryCollection : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public InventoryView[] inventoryViews;
        public int currentPageNumber;
        public int collectionID;
        public int actionOnItemSelect;

        public void Serialize(Serializer serializer) 
        {
            inventoryViews = serializer.Array(inventoryViews, true) ?? Array.Empty<InventoryView>(); // Ensure non-null assignment
            currentPageNumber = serializer.I32(currentPageNumber);
            collectionID = serializer.I32(collectionID);
            actionOnItemSelect = serializer.I32(actionOnItemSelect);
        }

        public int GetAllocatedSize() 
        {
            int size = BASE_ALLOCATION_SIZE;
            if (inventoryViews != null) 
            {
                foreach (var view in inventoryViews) 
                {
                    size += view.GetAllocatedSize();
                }
            }
            return size;
        }
    }
}