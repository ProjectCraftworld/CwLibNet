using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public enum CurveType
{
    // CONSTANT(0)
    CONSTANT,
    // LINEAR(1)
    LINEAR,
    // QUADRATIC(2)
    QUADRATIC,
    // CUBIC(3)
    CUBIC,
    // WAVE(4)
    WAVE,
    // BOX(5)
    BOX,
    // SAW(6)
    SAW,
    // MAX(7)
    MAX 
}

public sealed class CurveBodyMembers
{
    private readonly CurveType value;

    public CurveBodyMembers(int value)
    {
        this.value = (CurveType)value;
    }

    public CurveType getType()
    {
        return value;
    }
    /// <summary>
    /// Attempts to get a f-curve type from value.
    /// </summary>
    /// <param name="value">curve type value</param>
    /// <returns>f-curve type</returns>
    public static CurveBodyMembers fromValue(int value)
    {
        if (Enum.IsDefined(typeof(CurveType), value))
        {
            return new CurveBodyMembers(value);
        }
        return default(CurveBodyMembers);
    }
}