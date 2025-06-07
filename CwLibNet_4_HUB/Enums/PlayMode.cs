using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Enums;

public enum PlayMode : byte
{
    // TRIGGER_BY_FALLOFF(0x0)
    TRIGGER_BY_FALLOFF,
    // TRIGGER_BY_IMPACT(0x1)
    TRIGGER_BY_IMPACT,
    // TRIGGER_BY_DESTROY(0x2)
    TRIGGER_BY_DESTROY,
    // TRIGGER_BY_SWITCH(0x3)
    TRIGGER_BY_SWITCH 
}

public sealed class PlayBody
{
    private readonly PlayMode value;

    public PlayBody(int value)
    {
        this.value = (PlayMode)value;
    }

    public PlayMode getValue()
    {
        return value;
    }

    public static PlayBody fromValue(int value)
    {   
        if (Enum.IsDefined(typeof(PlayMode), value))
            return new PlayBody(value);
        return default(PlayBody);
    }
}