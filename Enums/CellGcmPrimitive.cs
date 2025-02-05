using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum CellGcmPrimitive
    {
        // POINTS(1)
        POINTS,
        // LINES(2)
        LINES,
        // LINE_LOOP(3)
        LINE_LOOP,
        // LINE_STRIP(4)
        LINE_STRIP,
        // TRIANGLES(5)
        TRIANGLES,
        // TRIANGLE_STRIP(6)
        TRIANGLE_STRIP,
        // TRIANGLE_FAN(7)
        TRIANGLE_FAN,
        // QUADS(8)
        QUADS,
        // QUAD_STRIP(9)
        QUAD_STRIP,
        // POLYGON(10)
        POLYGON 

        // --------------------
        // TODO enum body members
        // private final byte value;
        // CellGcmPrimitive(int value) {
        //     this.value = (byte) (value & 0xFF);
        // }
        // public Byte getValue() {
        //     return this.value;
        // }
        // public static CellGcmPrimitive fromValue(int value) {
        //     for (CellGcmPrimitive type : CellGcmPrimitive.values()) {
        //         if (type.value == value)
        //             return type;
        //     }
        //     return null;
        // }
        // --------------------
    }
}