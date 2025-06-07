using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Enums;

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