using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

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