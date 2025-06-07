using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Enums;

public enum LethalType
{
    // NOT(0)
    NOT,
    // FIRE(1)
    FIRE,
    // ELECTRIC(2)
    ELECTRIC,
    // @Deprecated
    // ICE(3)
    ICE,
    // CRUSH(4)
    CRUSH,
    // SPIKE(5)
    SPIKE,
    // GAS(6)
    GAS,
    // GAS2(7)
    GAS2,
    // GAS3(8)
    GAS3,
    // GAS4(9)
    GAS4,
    // GAS5(10)
    GAS5,
    // GAS6(11)
    GAS6,
    // NO_STAND(12)
    NO_STAND,
    // BULLET(13)
    BULLET,
    // DROWNED(14)
    DROWNED 
}

public sealed class LethalBody
{
    private readonly LethalType type;

    public LethalBody(int type)
    {
        this.type = (LethalType)type;
    }

    public LethalType GetBodyType()
    {
        return type;
    }
    public static LethalBody? FromValue(int type)
    {
        return Enum.IsDefined(typeof(LethalType), type) ? new LethalBody(type) : null;
    }
}