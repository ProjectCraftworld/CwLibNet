using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

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
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();

        Angle = serializer.F32(Angle);
        LastDecoration = serializer.I32(LastDecoration);
        LastDecoratedThing = serializer.Thing(LastDecoratedThing);
        Decoration = serializer.Resource(Decoration, ResourceType.Mesh);
        Scale = serializer.F32(Scale);
        Reversed = serializer.Bool(Reversed);
        if (version > 0x147)
            Stamping = serializer.Bool(Stamping);
        if (version > 0x177)
            Plan = serializer.Resource(Plan, ResourceType.Plan, true);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}