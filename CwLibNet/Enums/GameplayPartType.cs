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
        private readonly GameplayPartType type;

        public GameplayBody
        (int type)
        {
            this.type = (GameplayPartType)type;
        }

        public GameplayPartType getType()
        {
            return this.type;
        }

        public static GameplayBody
         fromtype(int type)
        {
            if (Enum.IsDefined(typeof(GameplayPartType), type))
        {
            return new GameplayBody
            (type);
        }
            return default(GameplayBody);
        }
    }
}