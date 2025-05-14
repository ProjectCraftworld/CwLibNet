using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.Popit;

public class PoppetShapeOverride: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public Vector3?[]? Polygon;
    public int[]? Loops;

    public int Back, Front;
    public float Scale, Angle;

    public Matrix4x4? WorldMatrix;
    public void Serialize(Serializer serializer)
    {
        if (!serializer.IsWriting()) Polygon = new Vector3?[serializer.GetInput().I32()];
        else
        {
            Polygon ??= [];
            serializer.GetOutput().I32(Polygon.Length);
        }
        for (var i = 0; i < Polygon.Length; ++i)
            Polygon[i] = serializer.V3(Polygon[i]);
        Loops = serializer.Intvector(Loops);
        Back = serializer.S32(Back);
        Front = serializer.S32(Front);
        Scale = serializer.F32(Scale);
        Angle = serializer.F32(Angle);
        if (serializer.GetRevision().GetVersion() > 0x317)
            WorldMatrix = serializer.M44(WorldMatrix);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Polygon != null)
            size += Polygon.Length * 0xC;
        if (Loops != null)
            size += Loops.Length * 0x4;
        return size;
    }
}