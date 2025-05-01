using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

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
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();

        ObjectList = serializer.Thingarray(ObjectList);
        StartMatrix = serializer.M44(StartMatrix);

        if (version < 0x160)
            Thing = serializer.Struct(Thing);
        else
            Plan = serializer.Resource(Plan, ResourceType.Plan, true, false, false);

        ProxyObject = serializer.Thing(ProxyObject);
        BackZ = serializer.S32(BackZ);
        Thickness = serializer.S32(Thickness);
        BackToZPos = serializer.F32(BackToZPos);
        Rotate = serializer.F32(Rotate);
        Scale = serializer.F32(Scale);

        if (version <= 0x26b) return;
        LastGridMoveFrame = serializer.I32(LastGridMoveFrame);
        LastGridRotateFrame = serializer.I32(LastGridRotateFrame);
        LastGridScaleFrame = serializer.I32(LastGridScaleFrame);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}