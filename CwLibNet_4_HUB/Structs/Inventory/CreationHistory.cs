using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Inventory;

public class CreationHistory : ISerializable
{
    public const int BaseAllocationSize = 0x4;
    public string?[]? Creators;

    public CreationHistory() { }

    public CreationHistory(string? creator)
    {
        Creators = [creator];
    }

    public CreationHistory(string?[] creators)
    {
        Creators = creators;
    }

    public void Serialize()
    {
        var isFixed = Serializer.GetRevision().GetVersion() > 0x37c;
            
        if (Serializer.IsWriting())
        {
            var ostream = Serializer.GetOutput();
            if (Creators != null)
            {
                ostream.I32(Creators.Length);
                foreach (var editor in Creators)
                {
                    if (isFixed) ostream.Str(editor, 0x14);
                    else ostream.Wstr(editor);
                }
            }
            else ostream.I32(0);
            return;
        }

        var stream = Serializer.GetInput();
        Creators = new string[stream.I32()];
        for (var i = 0; i < Creators.Length; ++i)
        {
            if (isFixed) Creators[i] = stream.Str(0x14);
            else Creators[i] = stream.Wstr();
        }
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Creators == null) return size;
        foreach (var editor in Creators)
            size += editor!.Length * 2 + 4;
        return size;
    }
}