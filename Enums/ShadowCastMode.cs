using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum ShadowCastMode
    {
        // OFF(0x0)
        OFF,
        // ON(0x1)
        ON,
        // ALPHA(0x2)
        ALPHA 

        // --------------------
        // TODO enum body members
        // private final byte value;
        // ShadowCastMode(int value) {
        //     this.value = (byte) (value & 0xFF);
        // }
        // public Byte getValue() {
        //     return this.value;
        // }
        // public static ShadowCastMode fromValue(byte value) {
        //     for (ShadowCastMode mode : ShadowCastMode.values()) {
        //         if (mode.value == value)
        //             return mode;
        //     }
        //     return null;
        // }
        // --------------------
    }
}