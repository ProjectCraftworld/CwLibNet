using System.Numerics;

namespace CwLibNet.Structs.Mesh
{
    public class Submesh
    {
        public Matrix4x4 transform;
        public int locator;
        public bool skinned;
        public Primitive[] primitives;
    }
}