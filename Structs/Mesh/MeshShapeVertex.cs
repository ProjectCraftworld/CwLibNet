using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Mesh
{
    public class MeshShapeVertex : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x24;
        public Vector4 localPos, localNormal;
        public int boneIndex;
        public override void Serialize(Serializer serializer)
        {
            localPos = serializer.Serialize(localPos);
            localNormal = serializer.Serialize(localNormal);
            boneIndex = serializer.Serialize(boneIndex);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}