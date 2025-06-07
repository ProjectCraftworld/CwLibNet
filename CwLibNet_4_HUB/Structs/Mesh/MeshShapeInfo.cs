using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Mesh;

public class MeshShapeInfo: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public int NumVerts;
    public bool IsPointCloud;

    
    public void Serialize()
    {
        Serializer.Serialize(ref NumVerts);
        Serializer.Serialize(ref IsPointCloud);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}