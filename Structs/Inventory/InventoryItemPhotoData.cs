using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Inventory
{
    public class InventoryItemPhotoData : Serializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x80;
        public ResourceDescriptor icon, sticker;
        private PhotoMetadata photoMetadata = new PhotoMetadata();
        public ResourceDescriptor painting;
        public override void Serialize(Serializer serializer)
        {
            icon = serializer.Serialize(icon, ResourceType.TEXTURE, true);
            sticker = serializer.Serialize(sticker, ResourceType.TEXTURE, true);
            photoMetadata = serializer.Serialize(photoMetadata, typeof(PhotoMetadata));
            if (serializer.GetRevision().GetVersion() > 0x3c7)
                painting = serializer.Serialize(painting, ResourceType.PAINTING, true);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE + this.photoMetadata.GetAllocatedSize();
        }

        public virtual PhotoMetadata GetPhotoMetadata()
        {
            return this.photoMetadata;
        }

        public virtual void SetPhotoMetadata(PhotoMetadata value)
        {
            if (value == null)
                throw new NullReferenceException("Photo metadata cannot be nulL!");
            this.photoMetadata = value;
        }
    }
}