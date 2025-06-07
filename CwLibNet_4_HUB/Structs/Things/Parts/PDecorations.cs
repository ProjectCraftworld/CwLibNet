using CwLibNet.IO;
using CwLibNet.Structs.Things.Components;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PDecorations: ISerializable
{
    public Decoration[]? Decorations;

    public PDecorations() { }

    public PDecorations(Decoration decor)
    {
        Decorations = [decor];
    }

    
    public void Serialize()
    {
        Decorations = Serializer.Serialize(ref Decorations);
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}