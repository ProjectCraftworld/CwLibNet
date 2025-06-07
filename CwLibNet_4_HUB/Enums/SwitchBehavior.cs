using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public enum SwitchBehavior
{
    // OFF_ON(0)
    OFF_ON,
    // SPEED_SCALE(1)
    SPEED_SCALE,
    // DIRECTION(2)
    DIRECTION,
    // ONE_SHOT(3)
    ONE_SHOT
}