using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PScriptName : ISerializable
{
    public const int BaseAllocationSize = 0x4;

    public string? Name;

    public PScriptName()
    {
    }

    public PScriptName(string? name)
    {
        Name = name;
    }


    public void Serialize(Serializer serializer)
    {
        Name = serializer.Str(Name);
    }


    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null)
            size += Name.Length;
        return size;
    }
}