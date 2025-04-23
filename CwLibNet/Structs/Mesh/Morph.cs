using System.Numerics;
using CwLibNet.IO;

namespace CwLibNet.Structs.Mesh;

public class Morph
{
    /**
     * Relative offset of each vertex.
     */
    public readonly Vector3[] Offsets;

    /**
     * New normals for each vertex.
     */
    public Vector3[] Normals;

    /**
     * Creates a morph from offset and normal data.
     *
     * @param offsets Relative offsets of each vertex
     * @param normals New normals for each vertex
     */
    public Morph(Vector3[] offsets, Vector3[] normals)
    {
        this.Offsets = offsets;
        this.Normals = normals;
    }

    public Vector3[] GetOffsets()
    {
        return this.Offsets;
    }

    public Vector3[] GetNormals()
    {
        return this.Normals;
    }


}