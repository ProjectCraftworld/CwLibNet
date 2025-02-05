using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Mesh
{
    /// <summary>
    /// Bones that control the render culling of a model.
    /// </summary>
    public class CullBone : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x60;
        public Matrix4x4 invSkinPoseMatrix;
        public Vector4 boundBoxMin;
        public Vector4 boundBoxMax;
        public override void Serialize(Serializer serializer)
        {
            invSkinPoseMatrix = serializer.Serialize(invSkinPoseMatrix);
            boundBoxMin = serializer.Serialize(boundBoxMin);
            boundBoxMax = serializer.Serialize(boundBoxMax);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}