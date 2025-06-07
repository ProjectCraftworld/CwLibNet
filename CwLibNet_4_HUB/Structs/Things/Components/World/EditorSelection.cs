using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.World;

public class EditorSelection: ISerializable
{
    public const int BaseAllocationSize = 0x8;
    
    public string? Name;
    public List<Thing>? Things = [];
    public void Serialize()
    {
        Serializer.Serialize(ref Name);
        Things = Serializer.Serialize(ref Things);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null) size += Name.Length;
        return size;
    }
}