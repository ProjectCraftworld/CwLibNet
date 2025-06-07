using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public class TextureType
{
    public static readonly int DIFFUSE = 0;
    public static readonly int SPECULAR = 1;
    public static readonly int BUMP = 2;
    public static readonly int GLOW = 3;
    public static readonly int REFLECTION = 4;
    public static readonly int OCCLUSION = 5;
    public static readonly int ANISO = 6;
    public static readonly int FUZZ = 7;
    public static readonly int MASK = 8;
    public static readonly int DIRT = 9;
    public static readonly int MAX = 10;
}