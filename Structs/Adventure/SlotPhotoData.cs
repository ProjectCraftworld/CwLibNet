using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace Cwlib.Structs.Adventure
{
    public class SlotPhotoData : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        SlotID id;
        ResourceDescriptor[] photos;
        public override void Serialize(Serializer serializer)
        {
            id = serializer.Struct(id, typeof(SlotID));
            int numPhotos = serializer.Serialize(photos != null ? photos.length : 0);
            if (!serializer.IsWriting())
                photos = new ResourceDescriptor[numPhotos];
            for (int i = 0; i < numPhotos; ++i)
                photos[i] = serializer.Resource(photos[i], ResourceType.PLAN, true);
        }

        public int GetAllocatedSize()
        {
            int size = SlotPhotoData.BASE_ALLOCATION_SIZE;
            if (this.photos != null)
                size += (this.photos.length * 0x24);
            return size;
        }
    }
}