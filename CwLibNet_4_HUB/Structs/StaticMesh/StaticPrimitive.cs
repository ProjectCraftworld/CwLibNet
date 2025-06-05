using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using System.Numerics;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.StaticMesh;

public class StaticPrimitive: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public Vector4? Min, Max;
    public ResourceDescriptor? Gmat;
    public int VertexStart, IndexStart;
    public int NumIndices;
    public CellGcmPrimitive Type = CellGcmPrimitive.TRIANGLES;

    /* Not actually serialized, just used for exporting */
    public int NumVerts;

    public void Serialize(Serializer serializer)
    {
        Min = serializer.V4(Min);
        Max = serializer.V4(Max);
        Gmat = serializer.Resource(Gmat, ResourceType.GfxMaterial);
        VertexStart = serializer.I32(VertexStart);
        IndexStart = serializer.I32(IndexStart);
        NumIndices = serializer.I32(NumIndices);
        Type = serializer.Enum8(Type);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}