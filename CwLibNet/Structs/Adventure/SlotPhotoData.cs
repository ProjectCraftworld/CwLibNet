using System;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Enums;
using CwLibNet.Types.Things;
using CwLibNet.Structs;
using System.Runtime.Serialization;
using CwLibNet.Structs.Slot;

namespace CwLibNet.Structs.Adventure
{
    public class SlotPhotoData : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x10; // for the SlotID
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        
        SlotID id;
        ResourceDescriptor[] photos;

        public virtual void serialize(Serializer serializer)
        {
            id = serializer.Struct<SlotID>(id);
            int numPhotos = serializer.I32(photos?.Length ?? 0);
            if (!serializer.IsWriting()) photos = new ResourceDescriptor[numPhotos];
            for (int i = 0; i < numPhotos; ++i)
            {
                photos[i] = serializer.Resource(photos[i], ResourceType.Plan, true);
            }
        }

        public virtual int getAllocatedSize()
        {
            int size = SlotPhotoData.BASE_ALLOCATION_SIZE;
            if (this.photos != null)
                size += (this.photos.Length * 0x24);
            return size;
        }
    }
}