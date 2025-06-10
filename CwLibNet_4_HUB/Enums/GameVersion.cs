using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Enums;

public class GameVersion
{
    public static readonly int LBP1 = 1 << 0;
    public static readonly int LBP2 = 1 << 1;
    public static readonly int LBP3 = 1 << 2;
    public static int GetFlag(Revision revision)
    {
        if (revision.GetVersion() <= 0x332)
            return LBP1;
        return revision.IsLbp3() ? LBP3 : LBP2;
    }
}