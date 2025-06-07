using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Enums;

public class CacheFlags
{
    public static readonly int NONE = 0;
    public static readonly int DIFFUSE = 1 << 0;
    public static readonly int SPECULAR = 1 << 1;
    public static readonly int BUMP = 1 << 2;
    public static readonly int GLOW = 1 << 3;
    public static readonly int REFLECTION = 1 << 4;
    public static readonly int ALPHA_CLIP = 1 << 5;
    public static readonly int DIRT = 1 << 9;
    [Obsolete]
    public static readonly int PROCEDURAL = 1 << 6;
    public static readonly int DIFFUSE_COLOR = DIFFUSE | (1 << 7);
    public static readonly int SPECULAR_COLOR = SPECULAR | (1 << 8);
}