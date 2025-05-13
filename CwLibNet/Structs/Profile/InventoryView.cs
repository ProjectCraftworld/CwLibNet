using System;
using System.Collections.Generic;

using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class InventoryView : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x20;

        public int type;
        public int subType;
        public string title;
        public bool heartedOnly;
        public int customID;
        public InventorySortMode currentSortMode = InventorySortMode.INVALID;
        public InventorySortMode desiredSortMode = InventorySortMode.INVALID;

        public void Serialize(Serializer serializer)
        {
            type = serializer.I32(type);
            subType = serializer.I32(subType);
            title = serializer.Str(title);
            heartedOnly = serializer.Bool(heartedOnly);
            customID = serializer.I32(customID);
            currentSortMode = serializer.Enum32<InventorySortMode>(currentSortMode);
            desiredSortMode = serializer.Enum32<InventorySortMode>(desiredSortMode);
        }   

        public int GetAllocatedSize() 
        {
            int size = InventoryView.BASE_ALLOCATION_SIZE;
            if (title != null)
            {
                size += title.Length * 2; // 2 bytes per char
            }
            return size;
        }
    }
}