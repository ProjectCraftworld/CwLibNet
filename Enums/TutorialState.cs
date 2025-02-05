using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum TutorialState
    {
        // UNPLAYED(0x0)
        UNPLAYED,
        // SKIPPED(0x1)
        SKIPPED,
        // PLAYED(0x2)
        PLAYED,
        // COMPLETED(0x3)
        COMPLETED 

        // --------------------
        // TODO enum body members
        // private final int value;
        // TutorialState(int value) {
        //     this.value = value;
        // }
        // public Integer getValue() {
        //     return this.value;
        // }
        // public static TutorialState fromValue(byte value) {
        //     for (TutorialState state : TutorialState.values()) {
        //         if (state.value == value)
        //             return state;
        //     }
        //     return TutorialState.UNPLAYED;
        // }
        // --------------------
    }
}