using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components;

namespace CwLibNet.Structs.Things.Parts;

public class PDecorations: ISerializable
{
    public Decoration[]? Decorations;

    public PDecorations() { }

    public PDecorations(Decoration decor)
    {
        Decorations = [decor];
    }

    
    public void Serialize(Serializer serializer)
    {
        Decorations = serializer.Array(Decorations);
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}