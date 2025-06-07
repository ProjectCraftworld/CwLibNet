using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Streaming;

public class StreamingID: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public string? Name;
    public int Type;

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
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