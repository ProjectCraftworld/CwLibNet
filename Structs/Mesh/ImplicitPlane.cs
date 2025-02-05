using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Mesh
{
    public class ImplicitPlane : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x30;
        public Vector4 planeNormal, pointInPlane;
        public int parentBone;
        public override void Serialize(Serializer serializer)
        {
            planeNormal = serializer.Serialize(planeNormal);
            pointInPlane = serializer.Serialize(pointInPlane);
            parentBone = serializer.Serialize(parentBone);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}