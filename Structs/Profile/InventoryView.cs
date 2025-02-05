using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class InventoryView : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;
        public int type;
        public int subType;
        public string title;
        public bool heartedOnly;
        public int customID;
        public InventorySortMode currentSortMode = InventorySortMode.INVALID;
        public InventorySortMode desiredSortMode = InventorySortMode.INVALID;
        public override void Serialize(Serializer serializer)
        {
            type = serializer.Serialize(type);
            subType = serializer.Serialize(subType);
            title = serializer.Serialize(title);
            heartedOnly = serializer.Serialize(heartedOnly);
            customID = serializer.Serialize(customID);
            currentSortMode = serializer.Serialize(currentSortMode);
            desiredSortMode = serializer.Serialize(desiredSortMode);
        }

        public virtual int GetAllocatedSize()
        {
            int size = InventoryView.BASE_ALLOCATION_SIZE;
            if (this.title != null)
                size += (this.title.Length() * 2);
            return size;
        }
    }
}