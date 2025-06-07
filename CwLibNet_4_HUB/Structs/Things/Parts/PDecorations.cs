using CwLibNet.IO;
using CwLibNet.Structs.Things.Components;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PDecorations: ISerializable
{
    public Decoration[]? Decorations;

    public PDecorations() { }

    public PDecorations(Decoration decor)
    {
        Decorations = [decor];
    }

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Decorations);
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}