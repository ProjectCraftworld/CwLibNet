using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Slot;
using CwLibNet4Hub.Structs.Streaming;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Level;

public class StartPoint: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public SlotID Slot;
    public List<StreamingID> Ids = [];

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Slot);
        Serializer.Serialize(ref Ids);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Ids != null)
        {
            size += Ids.Sum(id => id.GetAllocatedSize());
        }
        return size;
    }


}