using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Mesh;

public class MeshShapeInfo: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public int NumVerts;
    public bool IsPointCloud;

    
    public void Serialize(Serializer serializer)
    {
        NumVerts = serializer.I32(NumVerts);
        IsPointCloud = serializer.Intbool(IsPointCloud);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}