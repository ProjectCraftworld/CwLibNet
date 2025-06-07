using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        Serializer.Serialize(ref Angle);
        Serializer.Serialize(ref LastDecoration);
        LastDecoratedThing = Serializer.Reference(LastDecoratedThing);
        Serializer.Serialize(ref Decoration, Decoration, ResourceType.Mesh);
        Serializer.Serialize(ref Scale);
        Serializer.Serialize(ref Reversed);
        if (version > 0x147)
            Serializer.Serialize(ref Stamping);
        if (version > 0x177)
            Serializer.Serialize(ref Plan, Plan, ResourceType.Plan, true);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}