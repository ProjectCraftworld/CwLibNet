using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using System.Numerics;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.StaticMesh
{
    public class StaticPrimitive: ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x60;

        public Vector4? min, max;
        public ResourceDescriptor? gmat;
        public int vertexStart, indexStart;
        public int numIndices;
        public CellGcmPrimitive type = CellGcmPrimitive.TRIANGLES;

        /* Not actually serialized, just used for exporting */
        public int numVerts;

        public void Serialize(Serializer serializer)
        {
            min = serializer.V4(min);
            max = serializer.V4(max);
            gmat = serializer.Resource(gmat, ResourceType.GfxMaterial);
            vertexStart = serializer.I32(vertexStart);
            indexStart = serializer.I32(indexStart);
            numIndices = serializer.I32(numIndices);
            type = serializer.Enum8(type);
        }

        public int GetAllocatedSize()
        {
            return StaticPrimitive.BASE_ALLOCATION_SIZE;
        }
    }
}
