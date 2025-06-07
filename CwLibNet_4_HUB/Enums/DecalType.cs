using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public enum DecalType
{
    // STICKER(0)
    STICKER,
    // PAINT(1)
    PAINT,
    // EYETOY(2)
    EYETOY 
}

public sealed class DecalBody
{
    private readonly int value;

    private DecalBody(int value)
    {
        this.value = value & 0xFF;
    }

    public DecalType getValue()
    {
        return (DecalType)value;
    }

    public static DecalBody fromValue(byte value)
    {
        if (Enum.IsDefined(typeof(DecalType), value))
            return new DecalBody(value);
        return default(DecalBody);
    }
}