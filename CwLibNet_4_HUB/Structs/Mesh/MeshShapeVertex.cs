using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Mesh;

public class MeshShapeVertex: ISerializable
{
    public const int BaseAllocationSize = 0x24;

    public Vector4? LocalPos, LocalNormal;
    public int BoneIndex;

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref LocalPos);
        Serializer.Serialize(ref LocalNormal);
        Serializer.Serialize(ref BoneIndex);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}