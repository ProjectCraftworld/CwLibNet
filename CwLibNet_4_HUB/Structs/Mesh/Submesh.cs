using System.Numerics;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Mesh;

public class Submesh
{
    public Matrix4x4? Transform;
    public int Locator;
    public bool Skinned;
    public Primitive[]? Primitives;
}