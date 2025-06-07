using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Streaming;

public class StreamingID: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public string? Name;
    public int Type;

    
    public void Serialize()
    {
        Serializer.Serialize(ref Name);
        Serializer.Serialize(ref Type);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null)
            size += Name.Length * 0x2;
        return size;
    }


}