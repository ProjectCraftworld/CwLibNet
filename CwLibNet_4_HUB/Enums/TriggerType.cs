using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public enum TriggerType
{
    // RADIUS(0)
    RADIUS,
    // RECT(1)
    RECT,
    // SWITCH(2)
    SWITCH,
    // TOUCH(3)
    TOUCH,
    // RADIUS_3D(4)
    RADIUS_3D
}