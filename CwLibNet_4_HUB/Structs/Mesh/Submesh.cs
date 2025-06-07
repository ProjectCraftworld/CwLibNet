using System.Numerics;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Mesh;

public class Submesh
{
    public Matrix4x4? Transform;
    public int Locator;
    public bool Skinned;
    public Primitive[]? Primitives;
}