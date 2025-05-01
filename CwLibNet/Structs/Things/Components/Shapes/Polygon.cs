using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

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

    
    public void Serialize(Serializer serializer)
    {
        if (serializer.GetRevision().GetVersion() < 0x341)
        {
            RequiresZ = true;
            if (serializer.IsWriting())
            {
                if (Vertices == null)
                    serializer.GetOutput().I32(0);
                else serializer.I32(Vertices.Length);
            }
            else Vertices = new Vector3?[serializer.GetInput().I32()];
            if (Vertices != null)
                for (var i = 0; i < Vertices.Length; ++i)
                    Vertices[i] = serializer.V3(Vertices[i]);
            Loops = serializer.Intvector(Loops);
            return;
        }

        if (serializer.IsWriting())
        {
            var _stream = serializer.GetOutput();
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
            Loops = serializer.Intvector(Loops);
            return;
        }

        var stream = serializer.GetInput();
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

        Loops = serializer.Intvector(Loops);
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