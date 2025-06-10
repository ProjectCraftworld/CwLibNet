using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public class ShapeFlags
{
    public static readonly int COLLIDABLE_GAME = 0x1;
    public static readonly int COLLIDABLE_POPPET = 0x2;
    public static readonly int COLLIDABLE_WITH_PARENT = 0x4;
    public static readonly int DEFAULT_FLAGS = 0x7;
}