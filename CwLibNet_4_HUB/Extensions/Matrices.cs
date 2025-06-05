using System.Numerics;

namespace CwLibNet.Extensions;

public static class Matrices
{
    public static Matrix4x4 Invert(this Matrix4x4 matrix)
    {
        Matrix4x4.Invert(matrix, out var result);
        return result;
    }

    public static Matrix4x4 TranslationRotationScale(this Matrix4x4 matrix, Vector3 translation, Quaternion rotation,
        Vector3 scale)
    {
        var _a = Matrix4x4.CreateTranslation(translation) * matrix;
        var _b = Matrix4x4.Transform(_a, rotation);
        var _c = Matrix4x4.CreateScale(scale) * _b;
        return _c;
    }

    public static Vector3 GetTranslation(this Matrix4x4 matrix)
    {
        return new Vector3(matrix.M41, matrix.M42, matrix.M43);
    }

    public static Vector3 GetScale(this Matrix4x4 matrix)
    {
        return new Vector3
        {
            X = (float)Math.Sqrt(matrix.M11 * matrix.M11 + matrix.M12 * matrix.M12 + matrix.M13 * matrix.M13),
            Y = (float)Math.Sqrt(matrix.M21 * matrix.M21 + matrix.M22 * matrix.M22 + matrix.M23 * matrix.M23),
            Z = (float)Math.Sqrt(matrix.M31 * matrix.M31 + matrix.M32 * matrix.M32 + matrix.M33 * matrix.M33)
        };
    }

    public static Quaternion GetUnnormalizedRotation(this Matrix4x4 matrix)
    {
        Quaternion q;
        q.X = matrix.M11;
        q.Y = matrix.M21;
        q.Z = matrix.M31;
        
        var t = 1f - (q.X * q.X + q.Y * q.Y + q.Z * q.Z);
        q.W = (float)Math.Sqrt(t > 0f ? t : 0f);
        
        return q;
    }

    public static float[] Linearize(this Matrix4x4 amatrix)
    {
        var matrix = Matrix4x4.Transpose(amatrix);
        return
        [
            matrix.M11, matrix.M12, matrix.M13, matrix.M14,
            matrix.M21, matrix.M22, matrix.M23, matrix.M24,
            matrix.M31, matrix.M32, matrix.M33, matrix.M34,
            matrix.M41, matrix.M42, matrix.M43, matrix.M44
        ];
    }
    
    public static Quaternion RotateLocalX(this Quaternion q, float angle)
    {
        var localRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, angle);
        var result = q * localRotation;
        return result;  // Quaternion.Normalize(result);
    }
    
    public static Quaternion GetNormalizedRotation(this Matrix4x4 matrix)
    {
        Quaternion q;
        q.X = matrix.M11;
        q.Y = matrix.M21;
        q.Z = matrix.M31;
        var t = 1f - (q.X * q.X + q.Y * q.Y + q.Z * q.Z);
        q.W = (float)Math.Sqrt(t > 0f ? t : 0f);
        q = Quaternion.Normalize(q);
        return q;
    }
    
    public static Matrix4x4 GetMatrix(this Quaternion q)
    {
        var d = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
        var s = d == 0f ? 0f : 2f / d;
        
        var xs = q.X * s;
        var ys = q.Y * s;
        var zs = q.Z * s;
        
        var wx = q.W * xs;
        var wy = q.W * ys;
        var wz = q.W * zs;
        
        var xx = q.X * xs;
        var xy = q.X * ys;
        var xz = q.X * zs;
        
        var yy = q.Y * ys;
        var yz = q.Y * zs;
        var zz = q.Z * zs;
        
        var m = new Matrix4x4
        {
            M11 = 1f - (yy + zz),
            M12 = xy - wz,
            M13 = xz + wy,
            M14 = 0f,
            M21 = xy + wz,
            M22 = 1f - (xx + zz),
            M23 = yz - wx,
            M24 = 0f,
            M31 = xz - wy,
            M32 = yz + wx,
            M33 = 1f - (xx + yy),
            M34 = 0f,
            M41 = 0f,
            M42 = 0f,
            M43 = 0f,
            M44 = 1f
        };

        return m;
    }
}