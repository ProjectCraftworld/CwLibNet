using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using System.Numerics;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.StaticMesh;

public class UnknownStruct : ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x20;

    public Vector3? Min, Max;
    public short StructIndexA, StructIndexB, FirstPrimitive, NumPrimitives;

    public void Serialize(Serializer serializer)
    {
        Min = serializer.V3(Min);
        StructIndexA = serializer.I16(StructIndexA);
        StructIndexB = serializer.I16(StructIndexB);

        // If structIndexA/structIndexB is -1
        // then firstPrimitive and numPrimitives is set

        // If structIndexA/structIndexB is set
        // then firstPrimitive and numPrimitives is 0

        // Does -1 indicate an instance of a submesh
        // and otherwise a group of submeshes?

        Max = serializer.V3(Max);
        FirstPrimitive = serializer.I16(FirstPrimitive);
        NumPrimitives = serializer.I16(NumPrimitives);
    }

    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }
}


public class StaticMeshInfo : ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x80;


    public ResourceDescriptor? Lightmap, Risemap, Fallmap;
    public int PrimitiveCount, UnknownStructCount, IndexBufferSize, VertexStreamSize;

    public StaticPrimitive[]? Primitives;
    public UnknownStruct[]? Unknown;

    public void Serialize(Serializer serializer)
    {
        Lightmap = serializer.Resource(Lightmap, ResourceType.Texture);
        Risemap = serializer.Resource(Risemap, ResourceType.Texture);
        Fallmap = serializer.Resource(Fallmap, ResourceType.Texture);

        PrimitiveCount = serializer.I32(PrimitiveCount);
        UnknownStructCount = serializer.I32(UnknownStructCount);
        IndexBufferSize = serializer.I32(IndexBufferSize);
        VertexStreamSize = serializer.I32(VertexStreamSize);

        Primitives = serializer.Array(Primitives);
        Unknown = serializer.Array(Unknown);

        serializer.I32(0x48454c50); // "HELP", no idea, used as a marker?
    }

    public int GetAllocatedSize()
    {
        var size = BASE_ALLOCATION_SIZE;
        if (Primitives != null)
            size += Primitives.Length * StaticPrimitive.BaseAllocationSize;
        if (Unknown != null)
            size += Unknown.Length * UnknownStruct.BASE_ALLOCATION_SIZE;
        return size;
    }
}