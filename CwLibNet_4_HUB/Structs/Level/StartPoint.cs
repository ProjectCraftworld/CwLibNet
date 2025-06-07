using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Streaming;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Level;

public class StartPoint: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public SlotID Slot;
    public List<StreamingID> Ids = [];

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
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