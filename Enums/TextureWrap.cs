using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum TextureWrap
    {
        // WRAP(1)
        WRAP,
        // MIRROR(2)
        MIRROR,
        // CLAMP_TO_EDGE(3)
        CLAMP_TO_EDGE,
        // BORDER(4)
        BORDER,
        // CLAMP(5)
        CLAMP,
        // MIRROR_ONCE_CLAMP_TO_EDGE(6)
        MIRROR_ONCE_CLAMP_TO_EDGE,
        // MIRROR_ONCE_BORDER(7)
        MIRROR_ONCE_BORDER,
        // MIRROR_ONCE_CLAMP(8)
        MIRROR_ONCE_CLAMP 

        // --------------------
        // TODO enum body members
        // private final byte value;
        // TextureWrap(int value) {
        //     this.value = (byte) (value & 0xFF);
        // }
        // public Byte getValue() {
        //     return this.value;
        // }
        // public static TextureWrap fromValue(byte value) {
        //     for (TextureWrap wrap : TextureWrap.values()) {
        //         if (wrap.value == value)
        //             return wrap;
        //     }
        //     return null;
        // }
        // --------------------
    }
}