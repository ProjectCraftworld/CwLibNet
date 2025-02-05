using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum ParameterSubType
    {
        // NONE(0x0)
        NONE,
        // XY(0x1)
        XY,
        // ZW(0x2)
        ZW,
        // Z(0x3)
        Z 

        // --------------------
        // TODO enum body members
        // private final byte value;
        // ParameterSubType(int value) {
        //     this.value = (byte) value;
        // }
        // public Byte getValue() {
        //     return this.value;
        // }
        // public static ParameterSubType fromValue(byte value) {
        //     for (ParameterSubType type : ParameterSubType.values()) {
        //         if (type.value == value)
        //             return type;
        //     }
        //     return null;
        // }
        // --------------------
    }
}