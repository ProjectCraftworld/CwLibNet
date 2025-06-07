using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Popit;

public class PlacementBodyState: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public Thing? Thing;
    public Thing? OldParent;
    public int Frozen;
    public void Serialize()
    {
        Thing = Serializer.Reference(Thing);
        OldParent = Serializer.Reference(OldParent);
        Serializer.Serialize(ref Frozen);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}