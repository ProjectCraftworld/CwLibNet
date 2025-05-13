using System;
using System.Collections.Generic;

using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Enums;

namespace CwLibNet.Structs.Profile 
{
    public class InventoryPage : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int inventoryPageTitleKey;
        public InventorySortMode desiredSortMode = InventorySortMode.INVALID;

        public void Serialize(Serializer serializer)
        {
            inventoryPageTitleKey = serializer.I32(inventoryPageTitleKey);
            desiredSortMode = serializer.Enum32<InventorySortMode>(desiredSortMode);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}