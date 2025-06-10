using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Mesh;

public class MeshShapeVertex: ISerializable
{
    public const int BaseAllocationSize = 0x24;

    public Vector4? LocalPos, LocalNormal;
    public int BoneIndex;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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