using System.Numerics;
using static net.torutheredfox.craftworld.serialization.Serializer;

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
            0 => v with { X = value },
            1 => v with { Y = value },
            2 => v with { Z = value },
            3 => v with { W = value },
            _ => throw new IndexOutOfRangeException()
        };
    }

    public static Vector4 Lerp(this Vector4 a, Vector4 b, float amount)
    {
        return Vector4.Lerp(a, b, amount);
    }
}