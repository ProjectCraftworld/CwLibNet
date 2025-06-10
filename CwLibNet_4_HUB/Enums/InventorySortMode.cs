using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public enum InventorySortMode
{
    // INVALID(-1)
    INVALID,
    // DATE_OLDEST(1)
    DATE_OLDEST,
    // DATE_NEWEST(2)
    DATE_NEWEST,
    // @Deprecated
    // MOST_RECENTLY_USED(3)
    MOST_RECENTLY_USED,
    // @Deprecated
    // NUM_USES(4)
    NUM_USES,
    // LOCATION(5)
    LOCATION,
    // CATEGORY(6)
    CATEGORY,
    // SIZE(7)
    SIZE,
    // COLOR(8)
    COLOR,
    // NAME(9)
    NAME,
    // CREATOR(10)
    CREATOR 
}

public sealed class InvSortBody
{
    private readonly InventorySortMode value;

    public InvSortBody(int value)
    {
        this.value = (InventorySortMode)value;
    }

    public InventorySortMode GetValue()
    {
        return value;
    }
    public static InvSortBody FromValue(int value)
    {
        return Enum.IsDefined(typeof(InventorySortMode), value) ? new InvSortBody(value) : new InvSortBody((int)InventorySortMode.INVALID);
    }
}