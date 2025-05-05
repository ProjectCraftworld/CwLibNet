using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Streaming;

namespace CwLibNet.Structs.Level;

public class StartPoint: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public SlotID Slot;
    public List<StreamingID> Ids = [];

    
    public void Serialize(Serializer serializer)
    {
        Slot = serializer.Struct(Slot);
        Ids = serializer.Arraylist(Ids);
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