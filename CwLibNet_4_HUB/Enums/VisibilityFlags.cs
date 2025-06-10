using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

[Flags]
public enum VisibilityFlags : byte
{
    NONE = 0x0,
    PLAY_MODE = 0x1,
    EDIT_MODE = 0x2
}