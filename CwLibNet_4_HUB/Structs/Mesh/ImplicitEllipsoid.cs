using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Mesh;

public class ImplicitEllipsoid: ISerializable
{
    public const int BaseAllocationSize = 0x50;

    public Matrix4x4? Transform;
    public int ParentBone, AffectWorldOnly;

    
    public void Serialize()
    {
        Serializer.Serialize(ref Transform);
        Serializer.Serialize(ref ParentBone);
        Serializer.Serialize(ref AffectWorldOnly);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}