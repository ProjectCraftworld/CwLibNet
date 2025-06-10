using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public class CellGcm
{
    public static readonly int LN = 0x20;
    public static readonly int UN = 0x40;
    public static readonly int REMAP_FROM_A = 0;
    public static readonly int REMAP_FROM_R = 1;
    public static readonly int REMAP_FROM_G = 2;
    public static readonly int REMAP_FROM_B = 3;
}