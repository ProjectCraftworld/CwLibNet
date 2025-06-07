using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using System.Numerics;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;
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

    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Min);
        Serializer.Serialize(ref Max);
        Serializer.Serialize(ref Gmat, ResourceType.GfxMaterial, false, false, false);
        Serializer.Serialize(ref VertexStart);
        Serializer.Serialize(ref IndexStart);
        Serializer.Serialize(ref NumIndices);
        Serializer.SerializeEnum32(ref Type);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}