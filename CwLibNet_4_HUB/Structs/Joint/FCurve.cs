using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Joint;

public class FCurve: ISerializable
{
    public const int BaseAllocationSize = 0x40;

    public Vector4? X = Vector4.Zero;
    public Vector4? Y = Vector4.Zero;
    public Vector4? P = Vector4.Zero;
    public CurveType Type = CurveType.CONSTANT;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref X);
        Serializer.Serialize(ref Y);
        Serializer.Serialize(ref P);
        Serializer.SerializeEnum32(ref Type);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}