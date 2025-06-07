using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Enums;

public enum BuiltinType
{
    // /**
    //  * If a type doesn't exist, or if it's associated
    //  * machine type is a reference, this type will be used.
    //  */
    // VOID(0x0)
    VOID,
    // BOOL(0x1)
    BOOL,
    // CHAR(0x2)
    CHAR,
    // S32(0x3)
    S32,
    // F32(0x4)
    F32,
    // /* All the vector types are still vector4's natively */
    // V2(0x5)
    V2,
    // V3(0x6)
    V3,
    // V4(0x7)
    V4,
    // M44(0x8)
    M44,
    // /**
    //  * Functionally the same as a s32.
    //  */
    // GUID(0x9)
    GUID,
    // S64(0xa)
    S64,
    // F64(0xb)
    F64
}

public sealed class BuiltinBodyMembers
{
    private readonly BuiltinType type;

    public BuiltinBodyMembers(int type)
    {
        this.type = (BuiltinType)type;
    }

    public BuiltinType GetBuiltinType()
    {
        return type;
    }

    public static BuiltinBodyMembers? FromValue(int type)
    {
        return Enum.IsDefined(typeof(BuiltinType), type) ? new BuiltinBodyMembers(type) : null;
    }
}