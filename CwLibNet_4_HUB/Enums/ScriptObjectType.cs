using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Enums;

public enum ScriptObjectType
{
    // NULL(0)
    NULL,
    // ARRAY_BOOL(1)
    ARRAY_BOOL,
    // ARRAY_CHAR(2)
    ARRAY_CHAR,
    // ARRAY_S32(3)
    ARRAY_S32,
    // ARRAY_F32(4)
    ARRAY_F32,
    // ARRAY_VECTOR4(5)
    ARRAY_VECTOR4,
    // ARRAY_M44(6)
    ARRAY_M44,
    // ARRAY_STRING(7)
    ARRAY_STRING,
    // ARRAY_RAW_PTR(8)
    ARRAY_RAW_PTR,
    // ARRAY_REF_PTR(9)
    ARRAY_REF_PTR,
    // ARRAY_SAFE_PTR(10)
    ARRAY_SAFE_PTR,
    // ARRAY_OBJECT_REF(11)
    ARRAY_OBJECT_REF,
    // RESOURCE(12)
    RESOURCE,
    // INSTANCE(13)
    INSTANCE,
    // STRINGW(14)
    STRINGW,
    // AUDIOHANDLE(15)
    AUDIOHANDLE,
    // STRINGA(16)
    STRINGA,
    // POPPET(17)
    POPPET,
    // EXPOSED_COLLECTBUBBLE(18)
    EXPOSED_COLLECTBUBBLE,
    // ARRAY_S64(19)
    ARRAY_S64,
    // ARRAY_F64(20)
    ARRAY_F64
}