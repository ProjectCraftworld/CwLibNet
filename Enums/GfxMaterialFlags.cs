namespace CwLibNet.Enums
{
    public sealed class GfxMaterialFlags
    {
        public static readonly int TWO_SIDED = 0x1;
        public static readonly int FURRY = 0x2;
        public static readonly int WIRE = 0x4;
        public static readonly int MAX_PRIORITY = 0x8;
        public static readonly int SQUISHY = 0x10;
        public static readonly int NO_INSTANCE_TEXTURE = 0x20;
        public static readonly int RECEIVE_SHADOWS = 0x100;
        public static readonly int RECEIVE_SUN = 0x200;
        public static readonly int RECEIVE_SPRITELIGHTS = 0x400;
        public static readonly int ALPHA_CLIP = 0x800;
        public static readonly int DEFAULT = MAX_PRIORITY | RECEIVE_SHADOWS | RECEIVE_SUN | RECEIVE_SPRITELIGHTS;
    }
}