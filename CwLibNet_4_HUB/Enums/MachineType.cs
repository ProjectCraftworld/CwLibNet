using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
namespace CwLibNet.Enums;

public enum MachineType
{
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
    // V4(0x5)
    V4,
    // M44(0x6)
    M44,
    // /**
    //  * Strings used to be built-in types in the VM,
    //  * but were later replaced with a native class.
    //  * StringA/StringW
    //  */
    // @Deprecated
    // STRING(0x7)
    STRING,
    // /**
    //  * Raw pointers will generally be used
    //  * when accessing native part data from
    //  * scripts.
    //  */
    // RAW_PTR(0x8)
    RAW_PTR,
    // REF_PTR(0x9)
    REF_PTR,
    // /**
    //  * Often time, scripts that inherit from
    //  * Thing's will be of type safeptr.
    //  */
    // SAFE_PTR(0xa)
    SAFE_PTR,
    // /**
    //  * Any reference to an object in the scripting engine
    //  * that's not a safeptr, will generally be an object reference.
    //  */
    // OBJECT_REF(0xb)
    OBJECT_REF,
    // S64(0xc)
    S64,
    // F64(0xd)
    F64
}

public sealed class MachineBody
{
    private readonly MachineType value;

    public MachineBody(int value)
    {
        this.value = (MachineType)value;
    }

    public MachineType GetValue()
    {
        return value;
    }
    public static MachineBody? FromValue(int value)
    {
        return Enum.IsDefined(typeof(MachineType), value) ? new MachineBody(value) : null;
    }
}