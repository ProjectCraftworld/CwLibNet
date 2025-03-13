using Cwlib.Io.Serializer;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using System.Numerics;
namespace CwLibNet.Structs.StaticMesh
{
    public class UnknownStruct : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;

        public Vector3 min, max;
        public short structIndexA, structIndexB, firstPrimitive, numPrimitives;

        public void Serialize(Serializer serializer)
        {
            min = serializer.V3(min);
            structIndexA = serializer.I16(structIndexA);
            structIndexB = serializer.I16(structIndexB);

            // If structIndexA/structIndexB is -1
            // then firstPrimitive and numPrimitives is set

            // If structIndexA/structIndexB is set
            // then firstPrimitive and numPrimitives is 0

            // Does -1 indicate an instance of a submesh
            // and otherwise a group of submeshes?

            max = serializer.V3(max);
            firstPrimitive = serializer.I16(firstPrimitive);
            numPrimitives = serializer.I16(numPrimitives);
        }

        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }


    public class StaticMeshInfo : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x80;


        public ResourceDescriptor? lightmap, risemap, fallmap;
        public int primitiveCount, unknownStructCount, indexBufferSize, vertexStreamSize;

        public StaticPrimitive[] primitives;
        public UnknownStruct[] unknown;

        public void Serialize(Serializer serializer)
        {
            lightmap = serializer.Resource(lightmap, ResourceType.Texture);
            risemap = serializer.Resource(risemap, ResourceType.Texture);
            fallmap = serializer.Resource(fallmap, ResourceType.Texture);

            primitiveCount = serializer.i32(primitiveCount);
            unknownStructCount = serializer.i32(unknownStructCount);
            indexBufferSize = serializer.i32(indexBufferSize);
            vertexStreamSize = serializer.i32(vertexStreamSize);

            primitives = serializer.Array(primitives);
            unknown = serializer.Array(unknown);

            serializer.i32(0x48454c50); // "HELP", no idea, used as a marker?
        }

        public int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (primitives != null)
                size += (primitives.Length * StaticPrimitive.BASE_ALLOCATION_SIZE);
            if (unknown != null)
                size += (unknown.Length * UnknownStruct.BASE_ALLOCATION_SIZE);
            return size;
        }
    }
}
