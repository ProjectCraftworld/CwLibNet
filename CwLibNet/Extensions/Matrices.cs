using System.Numerics;

namespace CwLibNet.Extensions;

public static class Matrices
{
    public static Matrix4x4 Invert(this Matrix4x4 matrix)
    {
        Matrix4x4.Invert(matrix, out Matrix4x4 result);
        return result;
    }

    public static Matrix4x4 TranslationRotationScale(this Matrix4x4 matrix, Vector3 translation, Quaternion rotation,
        Vector3 scale)
    {
        Matrix4x4 _a = Matrix4x4.CreateTranslation(translation) * matrix;
        Matrix4x4 _b = Matrix4x4.Transform(_a, rotation);
        Matrix4x4 _c = Matrix4x4.CreateScale(scale) * _b;
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
        
        float t = 1f - (q.X * q.X + q.Y * q.Y + q.Z * q.Z);
        q.W = (float)Math.Sqrt(t > 0f ? t : 0f);
        
        return q;
    }

    public static float[] Linearize(this Matrix4x4 amatrix)
    {
        Matrix4x4 matrix = Matrix4x4.Transpose(amatrix);
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
        Quaternion localRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, angle);
        Quaternion result = q * localRotation;
        return result;  // Quaternion.Normalize(result);
    }
    
    public static Quaternion GetNormalizedRotation(this Matrix4x4 matrix)
    {
        Quaternion q;
        q.X = matrix.M11;
        q.Y = matrix.M21;
        q.Z = matrix.M31;
        float t = 1f - (q.X * q.X + q.Y * q.Y + q.Z * q.Z);
        q.W = (float)Math.Sqrt(t > 0f ? t : 0f);
        q = Quaternion.Normalize(q);
        return q;
    }
    
    public static Matrix4x4 GetMatrix(this Quaternion q)
    {
        float d = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
        float s = d == 0f ? 0f : 2f / d;
        
        float xs = q.X * s;
        float ys = q.Y * s;
        float zs = q.Z * s;
        
        float wx = q.W * xs;
        float wy = q.W * ys;
        float wz = q.W * zs;
        
        float xx = q.X * xs;
        float xy = q.X * ys;
        float xz = q.X * zs;
        
        float yy = q.Y * ys;
        float yz = q.Y * zs;
        float zz = q.Z * zs;
        
        Matrix4x4 m = new Matrix4x4();
        
        m.M11 = 1f - (yy + zz);
        m.M12 = xy - wz;
        m.M13 = xz + wy;
        m.M14 = 0f;
        
        m.M21 = xy + wz;
        m.M22 = 1f - (xx + zz);
        m.M23 = yz - wx;
        m.M24 = 0f;
        
        m.M31 = xz - wy;
        m.M32 = yz + wx;
        m.M33 = 1f - (xx + yy);
        m.M34 = 0f;
        
        m.M41 = 0f;
        m.M42 = 0f;
        m.M43 = 0f;
        m.M44 = 1f;
        
        return m;
    }
}