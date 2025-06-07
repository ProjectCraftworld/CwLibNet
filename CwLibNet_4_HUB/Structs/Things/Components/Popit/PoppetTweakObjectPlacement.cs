using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Components.Popit;

public class PoppetTweakObjectPlacement: ISerializable
{
    public static int BaseAllocationSize = 0x100;

    public Thing[]? ObjectList;
    public Matrix4x4? StartMatrix;

    public GlobalThingDescriptor? Thing;
    public ResourceDescriptor? Plan;

    public Thing? ProxyObject;

    public int BackZ;
    public int Thickness;

    public float BackToZPos;
    public float Rotate;
    public float Scale;

    public int LastGridMoveFrame, LastGridRotateFrame, LastGridScaleFrame;
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        ObjectList = Serializer.Serialize(ref ObjectList);
        Serializer.Serialize(ref StartMatrix);

        if (version < 0x160)
            Serializer.Serialize(ref Thing);
        else
            Serializer.Serialize(ref Plan, Plan, ResourceType.Plan, true, false, false);

        ProxyObject = Serializer.Reference(ProxyObject);
        Serializer.Serialize(ref BackZ);
        Serializer.Serialize(ref Thickness);
        Serializer.Serialize(ref BackToZPos);
        Serializer.Serialize(ref Rotate);
        Serializer.Serialize(ref Scale);

        if (version <= 0x26b) return;
        Serializer.Serialize(ref LastGridMoveFrame);
        Serializer.Serialize(ref LastGridRotateFrame);
        Serializer.Serialize(ref LastGridScaleFrame);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}