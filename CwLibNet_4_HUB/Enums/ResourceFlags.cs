namespace CwLibNet.Enums;

public sealed class ResourceFlags
{
    public static readonly int NONE = 0x0;
    public static readonly int REF_COUNT_DIRTY = 0x2;
    public static readonly int CONTAINS_EYETOY = 0x80;
    public static readonly int DONT_SWIZZLE = 0x100;
    public static readonly int VOLTEX = 0x200;
    public static readonly int NOSRGB_TEX = 0x400;
    public static readonly int BUMP_TEX = 0x800;
    public static readonly int TEMPORARY = 0x2000;
    public static readonly int UNSHARED = 0x4000;
    public static readonly int MAX_MIPS_128 = 0x10000;
}