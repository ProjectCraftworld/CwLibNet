using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum TriggerType
    {
        // RADIUS(0)
        RADIUS,
        // RECT(1)
        RECT,
        // SWITCH(2)
        SWITCH,
        // TOUCH(3)
        TOUCH,
        // RADIUS_3D(4)
        RADIUS_3D 

        // --------------------
        // TODO enum body members
        // private final byte value;
        // TriggerType(int value) {
        //     this.value = (byte) value;
        // }
        // public Byte getValue() {
        //     return this.value;
        // }
        // public static TriggerType fromValue(int value) {
        //     for (TriggerType type : TriggerType.values()) {
        //         if (type.value == value)
        //             return type;
        //     }
        //     return null;
        // }
        // --------------------
    }
}