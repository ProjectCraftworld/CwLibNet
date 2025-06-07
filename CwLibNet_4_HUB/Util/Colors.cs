using System.Numerics;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Util;

public class Colors
{
    public static int GetARGB(Vector4 rgba) {
        if (rgba == null) 
            throw new NullReferenceException("Can't convert null color!");
        if (rgba.W != 1.0f) rgba.W = 1.0f;
        return  ((int)Math.Round(rgba.W * 255.0f) << 24) |
                ((int)Math.Round(rgba.X * 255.0f) << 16) |
                ((int)Math.Round(rgba.Y * 255.0f) << 8) |
                ((int)Math.Round(rgba.Z * 255.0f) << 0);
    }

    public static Vector4 FromARGB(int color) {
        return new Vector4(
            ((color >>> 16) & 0xFF) / 255.0f,
            ((color >>> 8) & 0xFF) / 255.0f,
            ((color >>> 0) & 0xFF) / 255.0f,
            ((color >>> 24) & 0xFF) / 255.0f
        );
    }
}