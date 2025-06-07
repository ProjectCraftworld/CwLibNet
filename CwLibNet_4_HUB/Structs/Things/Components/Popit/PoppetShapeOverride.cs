using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Popit;

public class PoppetShapeOverride: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public Vector3?[]? Polygon;
    public int[]? Loops;

    public int Back, Front;
    public float Scale, Angle;

    public Matrix4x4? WorldMatrix;
    public void Serialize()
    {
        if (!Serializer.IsWriting()) Polygon = new Vector3?[Serializer.GetInput().I32()];
        else
        {
            Polygon ??= [];
            Serializer.GetOutput().I32(Polygon.Length);
        }
        for (var i = 0; i < Polygon.Length; ++i)
            Serializer.Serialize(ref Polygon[i]);
        Serializer.Serialize(ref Loops);
        Serializer.Serialize(ref Back);
        Serializer.Serialize(ref Front);
        Serializer.Serialize(ref Scale);
        Serializer.Serialize(ref Angle);
        if (Serializer.GetRevision().GetVersion() > 0x317)
            Serializer.Serialize(ref WorldMatrix);
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