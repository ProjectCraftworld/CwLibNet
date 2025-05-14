using System.Numerics;

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
        Offsets = offsets;
        Normals = normals;
    }

    public Vector3[] GetOffsets()
    {
        return Offsets;
    }

    public Vector3[] GetNormals()
    {
        return Normals;
    }


}