using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public enum GameProgressionStatus
{
    // NEW_GAME(0)
    NEW_GAME,
    // POD_ARRIVAL(1)
    POD_ARRIVAL,
    // ENTERED_STORY_LEVEL(2)
    ENTERED_STORY_LEVEL,
    // FIRST_LEVEL_COMPLETED(3)
    FIRST_LEVEL_COMPLETED,
    // GAME_PROGRESSION_COMPLETED(4)
    GAME_PROGRESSION_COMPLETED,
    // FIRST_GROUP_COMPLETED(4)
    FIRST_GROUP_COMPLETED
}
public sealed class GameProgressionBody(int value)
{
    private readonly GameProgressionStatus value = (GameProgressionStatus)value;

    public GameProgressionStatus GetValue()
    {
        return value;
    }
    /// <summary>
    /// Attempts to get GameProgressionStatus from value.
    /// </summary>
    /// <param name="value">Game progression status value</param>
    /// <returns>f-GameProgressionStatus</returns>
    public static GameProgressionBody? FromValue(int value)
    {
        return Enum.IsDefined(typeof(GameProgressionStatus), value) ? new GameProgressionBody(value) : null;
    }
}