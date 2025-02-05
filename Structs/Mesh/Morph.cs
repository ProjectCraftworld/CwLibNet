using System.Numerics;
namespace Cwlib.Structs.Mesh
{
    /// <summary>
    /// Represents mesh deformations.
    /// </summary>
    public class Morph
    {
        /// <summary>
        /// Relative offset of each vertex.
        /// </summary>
        public Vector3[] offsets;
        /// <summary>
        /// New normals for each vertex.
        /// </summary>
        public Vector3[] normals;
        /// <summary>
        /// Creates a morph from offset and normal data.
        /// </summary>
        /// <param name="offsets">Relative offsets of each vertex</param>
        /// <param name="normals">New normals for each vertex</param>
        public Morph(Vector3[] offsets, Vector3[] normals)
        {
            this.offsets = offsets;
            this.normals = normals;
        }

        public virtual Vector3[] GetOffsets()
        {
            return this.offsets;
        }

        public virtual Vector3[] GetNormals()
        {
            return this.normals;
        }
    }
}