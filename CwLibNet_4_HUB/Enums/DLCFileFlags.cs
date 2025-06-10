using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public class DLCFileFlags
{
    public static readonly int NONE = 0x0;
    public static readonly int OWNED = 0x1;
    public static readonly int AUTO_ADD_DISABLED = 0x2;
    public static readonly int IS_GOTY_GIVEAWAY = 0x4;
    public static readonly int IS_BETA = 0x8;
}