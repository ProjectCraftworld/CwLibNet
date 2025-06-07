using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Things.Components.World;

public class EditorSelection: ISerializable
{
    public const int BaseAllocationSize = 0x8;
    
    public string? Name;
    public List<Thing>? Things = [];
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Name);
        Serializer.Serialize(ref Things);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null) size += Name.Length;
        return size;
    }
}