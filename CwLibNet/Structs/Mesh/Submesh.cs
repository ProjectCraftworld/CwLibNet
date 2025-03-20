using System.Numerics;

namespace CwLibNet.Structs.Mesh;

public class Submesh
{
    public Matrix4x4? Transform;
    public int Locator;
    public bool Skinned;
    public Primitive[]? Primitives;
}