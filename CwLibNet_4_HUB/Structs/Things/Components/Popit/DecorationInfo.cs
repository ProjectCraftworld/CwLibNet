using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Components.Popit;

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
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
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