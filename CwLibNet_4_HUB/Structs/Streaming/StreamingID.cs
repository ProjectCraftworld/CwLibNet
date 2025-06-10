using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Streaming;

public class StreamingID: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public string? Name;
    public int Type;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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