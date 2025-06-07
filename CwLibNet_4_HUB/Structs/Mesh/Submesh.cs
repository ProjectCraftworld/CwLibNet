using System.Numerics;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Mesh;

public class Submesh
{
    public Matrix4x4? Transform;
    public int Locator;
    public bool Skinned;
    public Primitive[]? Primitives;
}