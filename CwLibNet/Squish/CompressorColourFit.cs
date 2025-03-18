namespace CwLibNet.Squish;

public abstract class CompressorColourFit
{
    protected static readonly Vec ONE_V = new Vec(1.0f);
    protected static readonly Vec ZERO_V = new Vec(0.0f);

    public const float GRID_X = 31.0f;
    public const float GRID_Y = 63.0f;
    public const float GRID_Z = 31.0f;

    protected const float GRID_X_RCP = 1.0f / GRID_X;
    protected const float GRID_Y_RCP = 1.0f / GRID_Y;
    protected const float GRID_Z_RCP = 1.0f / GRID_Z;

    public static readonly Matrix covariance = new Matrix();

    protected readonly ColourSet colours;
    protected readonly Util.Squish.CompressionType type;

    protected CompressorColourFit(ColourSet colours, Util.Squish.CompressionType type)
    {
        this.colours = colours;
        this.type = type;
    }

    public void Compress(byte[] block, int offset)
    {
        if (ReferenceEquals(type, Util.Squish.CompressionType.DXT1))
        {
            Compress3(block, offset);
            if (!colours.IsTransparent())
            {
                Compress4(block, offset);
            }
        }
        else
        {
            Compress4(block, offset);
        }
    }

    public abstract void Compress3(byte[] block, int offset);

    public abstract void Compress4(byte[] block, int offset);

    protected static float Clamp(float v, float grid, float gridRcp)
    {
        if (v <= 0.0f)
            return 0.0f;
        else if (v >= 1.0f)
            return 1.0f;

        return ((int)(grid * v + 0.5f)) * gridRcp;
    }
}
