using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Joint;

public class FCurve
{
    public const int BaseAllocationSize = 0x40;

    public Vector4? X = Vector4.Zero;
    public Vector4? Y = Vector4.Zero;
    public Vector4? P = Vector4.Zero;
    public CurveType Type = CurveType.CONSTANT;

    
    public void Serialize(Serializer serializer)
    {
        X = serializer.V4(X);
        Y = serializer.V4(Y);
        P = serializer.V4(P);
        Type = serializer.Enum32(Type);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}