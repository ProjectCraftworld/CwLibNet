using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Mesh;

public class ImplicitPlane: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public Vector4? PlaneNormal;
    public Vector4? PointInPlane;
    public int ParentBone;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref PlaneNormal);
        Serializer.Serialize(ref PointInPlane);
        Serializer.Serialize(ref ParentBone);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}