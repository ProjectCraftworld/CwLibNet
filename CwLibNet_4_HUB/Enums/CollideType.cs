using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public sealed class CollideType
{
    public static readonly int NONE = 0x0;
    public static readonly int NORMAL = 0x1;
    public static readonly int COLLECTABLE = 0x2;
    public static readonly int PLAYER = 0x4;
    public static readonly int MAGIC_WALL = 0x8;
    public static readonly int PROJECTILE = 0x10;
    public static readonly int TEMPORARY = 0x20;
}