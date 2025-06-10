using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.Enums;
using System.Runtime.Serialization;
using CwLibNet4Hub.Structs.Slot;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Adventure;

public class SlotPhotoData : CwLibNet4Hub.IO.ISerializable
{
    public const int BaseAllocationSize = 0x10; // for the SlotID
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
        
    public SlotID? Id;
    public ResourceDescriptor?[]? Photos;

    public virtual void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Id);
        var numPhotos = Photos?.Length ?? 0;
        Serializer.Serialize(ref numPhotos);
        if (!Serializer.IsWriting()) Photos = new ResourceDescriptor[numPhotos];
        for (var i = 0; i < numPhotos; ++i)
        {
            Serializer.Serialize(ref Photos[i], ResourceType.Texture);
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