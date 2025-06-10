using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public enum TutorialState
{
    // UNPLAYED(0x0)
    UNPLAYED,
    // SKIPPED(0x1)
    SKIPPED,
    // PLAYED(0x2)
    PLAYED,
    // COMPLETED(0x3)
    COMPLETED
}