using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Mesh
{
    public class ImplicitEllipsoid : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x50;
        public Matrix4x4 transform;
        public int parentBone, affectWorldOnly;
        public override void Serialize(Serializer serializer)
        {
            transform = serializer.Serialize(transform);
            parentBone = serializer.Serialize(parentBone);
            affectWorldOnly = serializer.Serialize(affectWorldOnly);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}