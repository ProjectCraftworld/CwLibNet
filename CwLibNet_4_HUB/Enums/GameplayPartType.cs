using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Enums;

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
}

public sealed class GameplayBody
{
    private readonly GameplayPartType type;

    public GameplayBody
        (int type)
    {
        this.type = (GameplayPartType)type;
    }

    public GameplayPartType GetPartType()
    {
        return type;
    }

    public static GameplayBody? FromType(int type)
    {
        if (Enum.IsDefined(typeof(GameplayPartType), type))
        {
            return new GameplayBody
                (type);
        }
        return null;
    }
}