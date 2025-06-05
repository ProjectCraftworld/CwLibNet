using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Mesh;

public class ImplicitEllipsoid: ISerializable
{
    public const int BaseAllocationSize = 0x50;

    public Matrix4x4? Transform;
    public int ParentBone, AffectWorldOnly;

    
    public void Serialize(Serializer serializer)
    {
        Transform = serializer.M44(Transform);
        ParentBone = serializer.I32(ParentBone);
        AffectWorldOnly = serializer.I32(AffectWorldOnly);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}