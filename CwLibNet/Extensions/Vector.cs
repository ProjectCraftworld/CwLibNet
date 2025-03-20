using System.Numerics;

namespace CwLibNet.Extensions;

public static class Vector
{
    public static float GetComponent(this Vector4 v, int index)
    {
        return index switch
        {
            0 => v.X,
            1 => v.Y,
            2 => v.Z,
            3 => v.W,
            _ => throw new IndexOutOfRangeException()
        };
    }

    public static Vector4 SetComponent(this Vector4 v, int index, float value)
    {
        return index switch
        {
            0 => new Vector4(value, v.Y, v.Z, v.W),
            1 => new Vector4(v.X, value, v.Z, v.W),
            2 => new Vector4(v.X, v.Y, value, v.W),
            3 => new Vector4(v.X, v.Y, v.Z, value),
            _ => throw new IndexOutOfRangeException()
        };
    }
}