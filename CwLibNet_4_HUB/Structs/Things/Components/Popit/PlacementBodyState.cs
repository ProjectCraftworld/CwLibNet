using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Things.Components.Popit;

public class PlacementBodyState: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public Thing? Thing;
    public Thing? OldParent;
    public int Frozen;
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Thing = Serializer.SerializeReference(Thing);
        OldParent = Serializer.SerializeReference(OldParent);
        Serializer.Serialize(ref Frozen);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}