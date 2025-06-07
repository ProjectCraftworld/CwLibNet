using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Streaming;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Level;

public class StartPoint: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public SlotID Slot;
    public List<StreamingID> Ids = [];

    
    public void Serialize()
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