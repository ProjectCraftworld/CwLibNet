using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Mesh;

public class MeshShapeVertex: ISerializable
{
    public const int BaseAllocationSize = 0x24;

    public Vector4? LocalPos, LocalNormal;
    public int BoneIndex;

    
    public void Serialize(Serializer serializer)
    {
        LocalPos = serializer.V4(LocalPos);
        LocalNormal = serializer.V4(LocalNormal);
        BoneIndex = serializer.I32(BoneIndex);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}