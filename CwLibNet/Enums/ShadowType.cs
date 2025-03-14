using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum ShadowType
    {
        // ALWAYS(0)
        ALWAYS,
        // NEVER(1)
        NEVER,
        // IF_ON_SCREEN(2)
        IF_ON_SCREEN 

        // --------------------
        // TODO enum body members
        // private final byte value;
        // ShadowType(int value) {
        //     this.value = (byte) (value & 0xFF);
        // }
        // public Byte getValue() {
        //     return this.value;
        // }
        // public static ShadowType fromValue(byte value) {
        //     for (ShadowType mode : ShadowType.values()) {
        //         if (mode.value == value)
        //             return mode;
        //     }
        //     return null;
        // }
        // --------------------
    }
}