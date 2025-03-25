using System.Numerics;

namespace CwLibNet.Extensions;

public static class Quaternions
{
    public static void SetFromUnnormalized(this Quaternion quaternion, Matrix4x4 matrix)
    {
        float nm00 = matrix.M11, nm01 = matrix.M12, nm02 = matrix.M13;
        float nm10 = matrix.M21, nm11 = matrix.M22, nm12 = matrix.M23;
        float nm20 = matrix.M31, nm21 = matrix.M32, nm22 = matrix.M33;
        float lenX = (float)Math.ReciprocalSqrtEstimate(nm00 * nm00 + nm01 * nm01 + nm02 * nm02);
        float lenY = (float)Math.ReciprocalSqrtEstimate(nm10 * nm10 + nm11 * nm11 + nm12 * nm12);
        float lenZ = (float)Math.ReciprocalSqrtEstimate(nm20 * nm20 + nm21 * nm21 + nm22 * nm22);
        nm00 *= lenX; nm01 *= lenX; nm02 *= lenX;
        nm10 *= lenY; nm11 *= lenY; nm12 *= lenY;
        nm20 *= lenZ; nm21 *= lenZ; nm22 *= lenZ;
        setFromNormalized(quaternion, nm00, nm01, nm02, nm10, nm11, nm12, nm20, nm21, nm22);
    }

    private static void setFromNormalized(Quaternion quaternion, float m00, float m01, float m02, float m10, float m11,
        float m12, float m20, float m21, float m22)
    {
        double t;
        double tr = m00 + m11 + m22;
        if (tr >= 0.0) {
            t = Math.Sqrt(tr + 1.0);
            quaternion.W = (float) (t * 0.5);
            t = 0.5 / t;
            quaternion.X = (float) ((m12 - m21) * t);
            quaternion.Y = (float) ((m20 - m02) * t);
            quaternion.Z = (float) ((m01 - m10) * t);
        } else {
            if (m00 >= m11 && m00 >= m22) {
                t = Math.Sqrt(m00 - (m11 + m22) + 1.0);
                quaternion.X = (float) (t * 0.5);
                t = 0.5 / t;
                quaternion.Y = (float) ((m10 + m01) * t);
                quaternion.Z = (float) ((m02 + m20) * t);
                quaternion.W = (float) ((m12 - m21) * t);
            } else if (m11 > m22) {
                t = Math.Sqrt(m11 - (m22 + m00) + 1.0);
                quaternion.Y = (float) (t * 0.5);
                t = 0.5 / t;
                quaternion.Z = (float) ((m21 + m12) * t);
                quaternion.X = (float) ((m10 + m01) * t);
                quaternion.W = (float) ((m20 - m02) * t);
            } else {
                t = Math.Sqrt(m22 - (m00 + m11) + 1.0);
                quaternion.Z = (float) (t * 0.5);
                t = 0.5 / t;
                quaternion.X = (float) ((m02 + m20) * t);
                quaternion.Y = (float) ((m21 + m12) * t);
                quaternion.W = (float) ((m01 - m10) * t);
            }
        }
    }
}