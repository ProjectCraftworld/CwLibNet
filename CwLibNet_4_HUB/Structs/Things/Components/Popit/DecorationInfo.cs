using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components.Popit;

public class DecorationInfo: ISerializable
{
    public const int BaseAllocationSize = 0x150;

    public float Angle;
    public int LastDecoration;
    public Thing? LastDecoratedThing;
    public ResourceDescriptor? Decoration;
    public float Scale;
    public bool Reversed;
    public bool Stamping;

    public ResourceDescriptor? Plan;
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref Angle);
        Serializer.Serialize(ref LastDecoration);
        LastDecoratedThing = Serializer.SerializeReference(LastDecoratedThing);
        Serializer.Serialize(ref Decoration, ResourceType.Mesh, false, true, false);
        Serializer.Serialize(ref Scale);
        Serializer.Serialize(ref Reversed);
        if (version > 0x147)
            Serializer.Serialize(ref Stamping);
        if (version > 0x177)
            Serializer.Serialize(ref Plan, ResourceType.Plan, true, true, false);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}