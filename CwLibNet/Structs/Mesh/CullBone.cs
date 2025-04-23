using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Mesh;

public class CullBone: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public Matrix4x4? InvSkinPoseMatrix;
    public Vector4? BoundBoxMin;
    public Vector4? BoundBoxMax;

    
    public void Serialize(Serializer serializer)
    {
        InvSkinPoseMatrix = serializer.M44(InvSkinPoseMatrix);
        BoundBoxMin = serializer.V4(BoundBoxMin);
        BoundBoxMax = serializer.V4(BoundBoxMax);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}