using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum GameplayPartType : int
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
    }

    public sealed class GameplayBody
    {
        private readonly GameplayPartType value;

        public GameplayBody
        (int value)
        {
            this.value = (GameplayPartType)value;
        }

        public GameplayPartType getType()
        {
            return this.value;
        }

        public static GameplayBody
         fromValue(int value)
        {
            if (Enum.IsDefined(typeof(GameplayPartType), value))
        {
            return new GameplayBody
            (value);
        }
            return default(GameplayBody);
        }
    }
}