using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

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