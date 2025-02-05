using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Mesh
{
    public class MeshShapeInfo : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x8;
        public int numVerts;
        public bool isPointCloud;
        public override void Serialize(Serializer serializer)
        {
            numVerts = serializer.Serialize(numVerts);
            isPointCloud = serializer.Serialize(isPointCloud);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}