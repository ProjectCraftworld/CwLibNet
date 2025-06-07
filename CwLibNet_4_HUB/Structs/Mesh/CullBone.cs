using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Mesh;

public class CullBone: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public Matrix4x4? InvSkinPoseMatrix;
    public Vector4? BoundBoxMin;
    public Vector4? BoundBoxMax;

    
    public void Serialize()
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