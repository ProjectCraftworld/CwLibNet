using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Enums
{
    public class GameVersion
    {
        public static readonly int LBP1 = (1 << 0);
        public static readonly int LBP2 = (1 << 1);
        public static readonly int LBP3 = (1 << 2);
        public static int GetFlag(Revision revision)
        {
            if (revision.GetVersion() <= 0x332)
                return GameVersion.LBP1;
            if (revision.IsLbp3())
                return GameVersion.LBP3;
            return GameVersion.LBP2;
        }

        internal static int GetFlag(Revision revision)
        {
            throw new NotImplementedException();
        }
    }
}