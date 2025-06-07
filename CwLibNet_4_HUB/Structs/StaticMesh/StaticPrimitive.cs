using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using System.Numerics;
using static net.torutheredfox.craftworld.serialization.Serializer;
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

    public void Serialize()
    {
        Serializer.Serialize(ref Min);
        Serializer.Serialize(ref Max);
        Serializer.Serialize(ref Gmat, Gmat, ResourceType.GfxMaterial);
        Serializer.Serialize(ref VertexStart);
        Serializer.Serialize(ref IndexStart);
        Serializer.Serialize(ref NumIndices);
        Serializer.Serialize(ref Type);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}