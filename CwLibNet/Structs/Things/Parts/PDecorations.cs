using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components;

namespace CwLibNet.Structs.Things.Parts;

public class PDecorations: ISerializable
{
    public Decoration[] decorations;

    public PDecorations() { }

    public PDecorations(Decoration decor)
    {
        this.decorations = [decor];
    }

    
    public void Serialize(Serializer serializer)
    {
        decorations = serializer.Array(decorations);
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}