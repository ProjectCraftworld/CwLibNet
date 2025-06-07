using System.Numerics;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Extensions;

public static class Quaternions
{
    private static void SetFromNormalized(Quaternion quaternion, float m00, float m01, float m02, float m10, float m11,
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