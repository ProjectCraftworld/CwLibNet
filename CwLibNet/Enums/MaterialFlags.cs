namespace CwLibNet.Enums
{
    public class MaterialFlags
    {
        public static readonly int HAS_DIFFUSE = (1 << 0);
        public static readonly int HAS_SPECULAR = (1 << 1);
        public static readonly int HAS_BUMP = (1 << 2);
        public static readonly int HAS_GLOW = (1 << 3);
        public static readonly int HAS_REFLECTION = (1 << 4);
        public static readonly int IS_SOLID_COLOR = (1 << 5);
    }
}