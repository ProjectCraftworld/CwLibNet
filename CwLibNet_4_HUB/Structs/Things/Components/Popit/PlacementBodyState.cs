using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components.Popit;

public class PlacementBodyState: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public Thing? Thing;
    public Thing? OldParent;
    public int Frozen;
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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