using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Things.Components;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

public class PDecorations: ISerializable
{
    public Decoration[]? Decorations;

    public PDecorations() { }

    public PDecorations(Decoration decor)
    {
        Decorations = [decor];
    }

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Decorations);
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}