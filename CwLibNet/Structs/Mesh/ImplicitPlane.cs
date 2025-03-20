using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Mesh;

public class ImplicitPlane: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public Vector4? PlaneNormal;
    public Vector4? PointInPlane;
    public int ParentBone;

    
    public void Serialize(Serializer serializer)
    {
        PlaneNormal = serializer.V4(PlaneNormal);
        PointInPlane = serializer.V4(PointInPlane);
        ParentBone = serializer.I32(ParentBone);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}