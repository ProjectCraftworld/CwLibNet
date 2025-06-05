using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Enums;
using System.Runtime.Serialization;
using CwLibNet.Structs.Slot;

namespace CwLibNet.Structs.Adventure;

public class SlotPhotoData : ISerializable
{
    public const int BaseAllocationSize = 0x10; // for the SlotID
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
        
    public SlotID? Id;
    public ResourceDescriptor?[]? Photos;

    public virtual void Serialize(Serializer serializer)
    {
        Id = serializer.Struct<SlotID>(Id);
        var numPhotos = serializer.I32(Photos?.Length ?? 0);
        if (!serializer.IsWriting()) Photos = new ResourceDescriptor[numPhotos];
        for (var i = 0; i < numPhotos; ++i)
        {
            Photos[i] = serializer.Resource(Photos[i], ResourceType.Plan, true);
        }
    }

    public virtual int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Photos != null)
            size += Photos.Length * 0x24;
        return size;
    }
}