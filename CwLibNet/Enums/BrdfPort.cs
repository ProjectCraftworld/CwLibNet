namespace CwLibNet.Enums;

public class BrdfPort
{
    public static readonly int DIFFUSE = 0;
    public static readonly int ALPHA_CLIP = 1;
    public static readonly int SPECULAR = 2;
    public static readonly int BUMP = 3;
    public static readonly int GLOW = 4;
    public static readonly int REFLECTION = 6;
    public static readonly int UNKNOWN = 7;

    //169
    public static readonly int ANISO = 170;
    public static readonly int TRANS = 171;
    public static readonly int COLOR_CORRECTION = 172;
    public static readonly int FUZZ = 173;
    public static readonly int BRDF_REFLECTANCE = 174;
    public static readonly int TOON_RAMP = 175;
}