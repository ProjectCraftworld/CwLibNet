using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.World;

public class EditorSelection: ISerializable
{
    public const int BaseAllocationSize = 0x8;
    
    public string? Name;
    public List<Thing>? Things = [];
    public void Serialize(Serializer serializer)
    {
        Name = serializer.Str(Name);
        Things = serializer.Arraylist(Things, true);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null) size += Name.Length;
        return size;
    }
}