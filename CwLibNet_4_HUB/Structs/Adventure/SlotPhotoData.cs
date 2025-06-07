using CwLibNet.Types.Data;
using CwLibNet.Enums;
using System.Runtime.Serialization;
using CwLibNet.Structs.Slot;
using static net.torutheredfox.craftworld.serialization.Serializer;

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
        Id = Serializer.Serialize(ref Id);
        var numPhotos = Serializer.Serialize(ref Photos?.Length ?? 0);
        if (!Serializer.IsWriting()) Photos = new ResourceDescriptor[numPhotos];
        for (var i = 0; i < numPhotos; ++i)
        {
            Serializer.Serialize(ref Photos[i]);
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