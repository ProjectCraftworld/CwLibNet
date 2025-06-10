using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

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