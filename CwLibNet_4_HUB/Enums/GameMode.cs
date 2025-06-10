using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public enum GameMode
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

public sealed class GameModeBody(int value)
{
    private readonly GameMode value = (GameMode)value;

    public GameMode GetValue()
    {
        return value;
    }

    /// <summary>
    /// Attempts to get a GameMode from value.
    /// </summary>
    /// <param name="value">Game mode value</param>
    /// <returns>GameModeBody</returns>
    public static GameModeBody FromValue(int value)
    {
        return Enum.IsDefined(typeof(GameMode), value) ? new GameModeBody(value) : new GameModeBody((int)GameMode.NORMAL);
    }
}