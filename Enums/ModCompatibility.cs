namespace CwLibNet.Enums
{
    public enum ModCompatibility
    {
        // LBP1(1)
        LBP1,
        // LBP2(2)
        LBP2,
        // LBP3(7 | 11)
        LBP3,
        // LBP3_PS3(7)
        LBP3_PS3,
        // LBP3_PS4(11)
        LBP3_PS4,
        // LBPV(16)
        LBPV,
        // PS3(7)
        PS3,
        // PS4(8)
        PS4,
        // ALL(31)
        ALL 

        // --------------------
        // TODO enum body members
        // private final int value;
        // ModCompatibility(int value) {
        //     this.value = value;
        // }
        // public int getValue() {
        //     return this.value;
        // }
        // public static ModCompatibility fromValue(int value) {
        //     for (ModCompatibility type : ModCompatibility.values()) {
        //         if (type.value == value)
        //             return type;
        //     }
        //     return ModCompatibility.ALL;
        // }
        // --------------------
    }
}