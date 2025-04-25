using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Streaming;

public class StreamingID: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public string? Name;
    public int Type;

    
    public void Serialize(Serializer serializer)
    {
        Name = serializer.Wstr(Name);
        Type = serializer.I32(Type);
    }

    
    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (Name != null)
            size += Name.Length * 0x2;
        return size;
    }


}