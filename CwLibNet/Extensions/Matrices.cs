using System.Numerics;

namespace CwLibNet.Extensions;

public static class Matrices
{
    public static Matrix4x4 Invert(this Matrix4x4 matrix)
    {
        Matrix4x4.Invert(matrix, out Matrix4x4 result);
        return result;
    }
}