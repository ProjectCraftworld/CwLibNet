using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum GameMode : int
    {
        // NORMAL(0)
        NORMAL,
        // COMPETITIVE(1)
        COMPETITIVE,
        // CUT_SCENE(2)
        CUT_SCENE,
        // SINGLE_PLAYER(3)
        SINGLE_PLAYER,
        // SOCIAL(4)
        SOCIAL,
        // TEAMS(5)
        TEAMS
    }

    public sealed class GameModeBody
    {
        private readonly GameMode value;

        public GameModeBody(int value)
        {
            this.value = (GameMode)value;
        }

        public GameMode getValue()
        {
            return value;
        }

        /// <summary>
        /// Attempts to get a GameMode from value.
        /// </summary>
        /// <param name="value">Game mode value</param>
        /// <returns>GameModeBody</returns>
        public static GameModeBody fromValue(int value)
        {
            if (Enum.IsDefined(typeof(GameMode), value))
        {
            return new GameModeBody(value);
        }
            return new GameModeBody((int)GameMode.NORMAL);
        }
    }
}