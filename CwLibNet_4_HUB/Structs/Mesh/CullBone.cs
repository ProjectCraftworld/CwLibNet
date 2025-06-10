using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Mesh;

public class CullBone: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public Matrix4x4? InvSkinPoseMatrix;
    public Vector4? BoundBoxMin;
    public Vector4? BoundBoxMax;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref InvSkinPoseMatrix);
        Serializer.Serialize(ref BoundBoxMin);
        Serializer.Serialize(ref BoundBoxMax);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}