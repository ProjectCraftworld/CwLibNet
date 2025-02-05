using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum CurveType
    {
        // CONSTANT(0)
        CONSTANT,
        // LINEAR(1)
        LINEAR,
        // QUADRATIC(2)
        QUADRATIC,
        // CUBIC(3)
        CUBIC,
        // WAVE(4)
        WAVE,
        // BOX(5)
        BOX,
        // SAW(6)
        SAW,
        // MAX(7)
        MAX 

        // --------------------
        // TODO enum body members
        // private final int value;
        // CurveType(int value) {
        //     this.value = value;
        // }
        // public Integer getValue() {
        //     return this.value;
        // }
        // /**
        //  * Attempts to get a f-curve type from value.
        //  *
        //  * @param value curve type value
        //  * @return f-curve type
        //  */
        // public static CurveType fromValue(int value) {
        //     for (CurveType type : CurveType.values()) {
        //         if (type.value == value)
        //             return type;
        //     }
        //     return null;
        // }
        // --------------------
    }
}