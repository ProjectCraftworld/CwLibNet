using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Types.Things.Components.World;

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
        int size = EditorSelection.BaseAllocationSize;
        if (this.Name != null) size += (this.Name.Length);
        return size;
    }
}