using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Enums;

public enum FlipType
{
    // /**
    //  * X.pos, X.rot
    //  */
    // MAX(0)
    MAX,
    // /**
    //  * Y.pos, Y.rot + prerot
    //  */
    // BIP_ROOT(1)
    BIP_ROOT,
    // /**
    //  * Z.pos, Z.rot + prerot
    //  */
    // BIP_PELVIS(2)
    BIP_PELVIS,
    // /**
    //  * Z.pos, Z.rot
    //  */
    // BIP_BONE(3)
    BIP_BONE,
    // /**
    //  * Z.pos, Z.rot + null prerot
    //  */
    // PARENT_WAS_BIP_BONE(4)
    PARENT_WAS_BIP_BONE,
    // /**
    //  * X.pos, X.rot + prerot
    //  */
    // GRANDPARENT_WAS_BIP_BONE(5)
    GRANDPARENT_WAS_BIP_BONE,
    // /**
    //  * No flip, straight copy from mirror bone.
    //  */
    // COPY(6)
    COPY 
}

public sealed class FlipBody
{
    private readonly FlipType value;

    private FlipBody(int value)
    {
        this.value = (FlipType)(value & 0xFF);
    }

    public FlipType getValue()
    {
        return value;
    }

    public static FlipBody fromValue(int value)
    {
        if (Enum.IsDefined(typeof(FlipType), value))
            return new FlipBody(value);
        return default(FlipBody);
    }
}