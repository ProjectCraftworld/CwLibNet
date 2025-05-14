using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.Popit;

public class PlacementBodyState: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public Thing? Thing;
    public Thing? OldParent;
    public int Frozen;
    public void Serialize(Serializer serializer)
    {
        Thing = serializer.Thing(Thing);
        OldParent = serializer.Thing(OldParent);
        Frozen = serializer.I32(Frozen);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}