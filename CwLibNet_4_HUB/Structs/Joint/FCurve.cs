using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Joint;

public class FCurve: ISerializable
{
    public const int BaseAllocationSize = 0x40;

    public Vector4? X = Vector4.Zero;
    public Vector4? Y = Vector4.Zero;
    public Vector4? P = Vector4.Zero;
    public CurveType Type = CurveType.CONSTANT;

    
    public void Serialize()
    {
        Serializer.Serialize(ref X);
        Serializer.Serialize(ref Y);
        Serializer.Serialize(ref P);
        Serializer.Serialize(ref Type);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}