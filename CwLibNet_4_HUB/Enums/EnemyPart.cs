using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public enum EnemyPart
{
    // LEG(0)
    LEG,
    // EYE(1)
    EYE,
    // BRAIN(2)
    BRAIN,
    // WHEEL(3)
    WHEEL,
    // ROCKET(4)
    ROCKET,
    // PAINT(5)
    PAINT 
}

public sealed class EnemyBody
{
    private readonly EnemyPart value;

    private EnemyBody(int value)
    {
        this.value = (EnemyPart)value;
    }

    public EnemyPart getValue()
    {
        return value;
    }

    public static EnemyBody fromValue(int value)
    {
        if (Enum.IsDefined(typeof(EnemyPart), value))
            return new EnemyBody(value);
        return default(EnemyBody);
    }
}