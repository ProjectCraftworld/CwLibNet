using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Mesh;

public class ImplicitPlane: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public Vector4? PlaneNormal;
    public Vector4? PointInPlane;
    public int ParentBone;

    
    public void Serialize()
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