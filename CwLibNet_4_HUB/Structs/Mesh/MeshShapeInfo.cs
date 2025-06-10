using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Mesh;

public class MeshShapeInfo: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public int NumVerts;
    public bool IsPointCloud;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref NumVerts);
        Serializer.Serialize(ref IsPointCloud);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}