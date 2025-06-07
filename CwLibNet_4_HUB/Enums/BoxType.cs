using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public class BoxType
{
    public static readonly int OUTPUT = 0;
    public static readonly int TEXTURE_SAMPLE = 1;
    public static readonly int THING_COLOR = 2;
    public static readonly int COLOR = 3;
    public static readonly int CONSTANT = 4;
    public static readonly int CONSTANT2 = 5;
    public static readonly int CONSTANT3 = 6;
    public static readonly int CONSTANT4 = 7;
    // 8 is not used in any shader
    public static readonly int MULTIPLY_ADD = 9;
    public static readonly int MULTIPLY = 10;
    public static readonly int ADD = 11;
    public static readonly int MIX = 12;
    public static readonly int MAKE_FLOAT2 = 13;
    public static readonly int MAKE_FLOAT3 = 14;
    public static readonly int MAKE_FLOAT4 = 15;
    public static readonly int BLEND = 16;
    // 17, something with fur
    // 18 ???
    public static readonly int EXPONENT = 19;
}