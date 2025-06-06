using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Shapes;

public class Polygon: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    /**
     * Vertices that make up this polygon.
     */
    public Vector3?[]? Vertices =
    [
        new Vector3(-100, -100, 0),
        new Vector3(-100, 100, 0),
        new Vector3(100, 100, 0),
        new Vector3(100, -100, 0)
    ];

    /**
     * Whether this polygon needs the Z vertex.
     */
    
    public bool RequiresZ = true;

    /**
     * Controls which parts of the polygon are "loops",
     * if two loops intersect with each other, it counts as a cut.
     */
    public int[]? Loops = [4];

    
    public void Serialize()
    {
        if (Serializer.GetRevision().GetVersion() < 0x341)
        {
            RequiresZ = true;
            if (Serializer.IsWriting())
            {
                if (Vertices == null)
                    Serializer.GetOutput().I32(0);
                else Serializer.Serialize(ref Vertices.Length);
            }
            else Vertices = new Vector3?[Serializer.GetInput().I32()];
            if (Vertices != null)
                for (var i = 0; i < Vertices.Length; ++i)
                    Serializer.Serialize(ref Vertices[i]);
            Serializer.Serialize(ref Loops);
            return;
        }

        if (Serializer.IsWriting())
        {
            var _stream = Serializer.GetOutput();
            if (Vertices != null && Vertices.Length != 0)
            {
                _stream.I32(Vertices.Length);
                _stream.Boole(RequiresZ);
                if (RequiresZ)
                    foreach (Vector3 vertex in Vertices)
                        _stream.V3(vertex);
                else
                    foreach (Vector3 vertex in Vertices)
                        _stream.V2(new Vector2(vertex.X, vertex.Y));
            }
            else
            {
                _stream.I32(0);
                _stream.Boole(false);
            }
            Serializer.Serialize(ref Loops);
            return;
        }

        var stream = Serializer.GetInput();
        Vertices = new Vector3?[stream.I32()];
        RequiresZ = stream.Boole();
        if (Vertices.Length != 0)
        {
            for (var i = 0; i < Vertices.Length; ++i)
            {
                var vertex = RequiresZ ? stream.V3() : new Vector3(stream.F32(), stream.F32(), 0.0f);
                Vertices[i] = vertex;
            }
        }

        Serializer.Serialize(ref Loops);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Vertices != null)
            size += Vertices.Length * 0xC;
        if (Loops != null)
            size += Loops.Length * 0x4;
        return size;
    }


}