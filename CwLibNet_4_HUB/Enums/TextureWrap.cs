using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public enum TextureWrap
{
    // WRAP(1)
    WRAP = 1,
    // MIRROR(2)
    MIRROR,
    // CLAMP_TO_EDGE(3)
    CLAMP_TO_EDGE,
    // BORDER(4)
    BORDER,
    // CLAMP(5)
    CLAMP,
    // MIRROR_ONCE_CLAMP_TO_EDGE(6)
    MIRROR_ONCE_CLAMP_TO_EDGE,
    // MIRROR_ONCE_BORDER(7)
    MIRROR_ONCE_BORDER,
    // MIRROR_ONCE_CLAMP(8)
    MIRROR_ONCE_CLAMP 
}