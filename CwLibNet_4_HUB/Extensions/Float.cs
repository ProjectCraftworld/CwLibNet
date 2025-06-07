using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Extensions;

public static class Float
{
    public static float ToRadians (this float angleIn10thofaDegree) {
        // Angle in 10th of a degree
        return (float)(angleIn10thofaDegree * Math.PI/1800); 
    }

    public static float IntBitsToFloat(this int value)
    {
        return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
    }

    public static int FloatToIntBits(this float value)
    {
        return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
    }
}