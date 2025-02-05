using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum GameplayPartType
    {
        // UNDEFINED(0)
        UNDEFINED,
        // LEVEL_KEY(1)
        LEVEL_KEY,
        // PRIZE_BUBBLE(2)
        PRIZE_BUBBLE,
        // SCORE_BUBBLE(3)
        SCORE_BUBBLE,
        // COLLECTABUBBLE(4)
        COLLECTABUBBLE,
        // POCKET_ITEM_BUBBLE(5)
        POCKET_ITEM_BUBBLE,
        // TREASURE_ITEM(6)
        TREASURE_ITEM,
        // OBJECT_SAVER(7)
        OBJECT_SAVER,
        // RUMBLER(8)
        RUMBLER 

        // --------------------
        // TODO enum body members
        // private final int value;
        // GameplayPartType(int value) {
        //     this.value = value;
        // }
        // public Integer getValue() {
        //     return this.value;
        // }
        // public static GameplayPartType fromValue(int value) {
        //     for (GameplayPartType part : GameplayPartType.values()) {
        //         if (part.value == value)
        //             return part;
        //     }
        //     return null;
        // }
        // --------------------
    }
}