using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum MappingMode
    {
        // HIDDEN(0)
        HIDDEN,
        // /**
        //  * Standard XYZ UV Mapping.
        //  */
        // DAVE(1)
        DAVE,
        // /**
        //  * Wraps vertically; separate faces.
        //  */
        // CYLINDER_01(2)
        CYLINDER_01,
        // /**
        //  * Wraps vertical; all one face.
        //  */
        // CYLINDER(3)
        CYLINDER,
        // /**
        //  * Wraps vertically; seperate faces, stretches on Y axis.
        //  */
        // PLANARXZ_01(4)
        PLANARXZ_01,
        // /**
        //  * Wraps vertically; all one face, stretches on Y axis.
        //  */
        // PLANARXZ(5)
        PLANARXZ 

        // --------------------
        // TODO enum body members
        // private final byte value;
        // MappingMode(int value) {
        //     this.value = (byte) value;
        // }
        // public Byte getValue() {
        //     return this.value;
        // }
        // public static MappingMode fromValue(int value) {
        //     for (MappingMode mode : MappingMode.values()) {
        //         if (mode.value == value)
        //             return mode;
        //     }
        //     return null;
        // }
        // --------------------
    }
}