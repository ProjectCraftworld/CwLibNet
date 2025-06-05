namespace CwLibNet.Enums;

public enum ModCompatibility
{
    // LBP1(1)
    LBP1,
    // LBP2(2)
    LBP2,
    // LBP3(7 | 11)
    LBP3,
    // LBP3_PS3(7)
    LBP3_PS3,
    // LBP3_PS4(11)
    LBP3_PS4,
    // LBPV(16)
    LBPV,
    // PS3(7)
    PS3,
    // PS4(8)
    PS4,
    // ALL(31)
    ALL 
}

public sealed class ModCompBody
{
    private readonly ModCompatibility value;

    public ModCompBody(int value)
    {
        this.value = (ModCompatibility)value;
    }

    public ModCompatibility GetValue()
    {
        return value;
    }
    public static ModCompBody FromValue(int value)
    {
        return Enum.IsDefined(typeof(ModCompatibility), value) ? new ModCompBody(value) : new ModCompBody((int)ModCompatibility.ALL);
    }
}