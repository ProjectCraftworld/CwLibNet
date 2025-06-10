using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Mesh;

public class ImplicitEllipsoid: ISerializable
{
    public const int BaseAllocationSize = 0x50;

    public Matrix4x4? Transform;
    public int ParentBone, AffectWorldOnly;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Transform);
        Serializer.Serialize(ref ParentBone);
        Serializer.Serialize(ref AffectWorldOnly);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}