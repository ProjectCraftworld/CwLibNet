using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using System.Numerics;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.StaticMesh;

public class UnknownStruct : ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x20;

    public Vector3? Min, Max;
    public short StructIndexA, StructIndexB, FirstPrimitive, NumPrimitives;

    public void Serialize()
    {
        Min = Serializer.Serialize(ref Min);
        Serializer.Serialize(ref StructIndexA);
        Serializer.Serialize(ref StructIndexB);

        // If structIndexA/structIndexB is -1
        // then firstPrimitive and numPrimitives is set

        // If structIndexA/structIndexB is set
        // then firstPrimitive and numPrimitives is 0

        // Does -1 indicate an instance of a submesh
        // and otherwise a group of submeshes?

        Max = Serializer.Serialize(ref Max);
        Serializer.Serialize(ref FirstPrimitive);
        Serializer.Serialize(ref NumPrimitives);
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

    public void Serialize()
    {
        Serializer.Serialize(ref Lightmap, Lightmap, ResourceType.Texture);
        Serializer.Serialize(ref Risemap, Risemap, ResourceType.Texture);
        Serializer.Serialize(ref Fallmap, Fallmap, ResourceType.Texture);

        Serializer.Serialize(ref PrimitiveCount);
        Serializer.Serialize(ref UnknownStructCount);
        Serializer.Serialize(ref IndexBufferSize);
        Serializer.Serialize(ref VertexStreamSize);

        Primitives = Serializer.Serialize(ref Primitives);
        Unknown = Serializer.Serialize(ref Unknown);

        Serializer.Serialize(ref 0x48454c50); // "HELP", no idea, used as a marker?
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