using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Parts;

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


    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Name);
    }


    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null)
            size += Name.Length;
        return size;
    }
}