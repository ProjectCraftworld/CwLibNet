using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Enums;

public enum ParameterType
{
    // INVALID(-1)
    INVALID,
    // TEXTURE0(0x0)
    TEXTURE0,
    // TEXTURE1(0x1)
    TEXTURE1,
    // TEXTURE2(0x2)
    TEXTURE2,
    // TEXTURE3(0x3)
    TEXTURE3,
    // TEXTURE4(0x4)
    TEXTURE4,
    // TEXTURE5(0x5)
    TEXTURE5,
    // TEXTURE6(0x6)
    TEXTURE6,
    // TEXTURE7(0x7)
    TEXTURE7,
    // ALPHA_TEST_LEVEL(0x8)
    ALPHA_TEST_LEVEL,
    // BUMP_LEVEL(0x9)
    BUMP_LEVEL,
    // COSINE_POWER(0xa)
    COSINE_POWER,
    // SPECULAR_COLOR(0xb)
    SPECULAR_COLOR 
}

public sealed class ParameterBody(byte value)
{
    private readonly ParameterType value = (ParameterType)value;

    public ParameterType GetValue()
    {
        return value;
    }

    public static ParameterBody? FromValue(int value)
    {
        return Enum.IsDefined(typeof(ParameterType), value) ? new ParameterBody((byte)value) : null;
    }        
}