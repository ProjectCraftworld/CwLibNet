/* -----------------------------------------------------------------------------

	Copyright (c) 2006 Simon Brown                          si@sjbrown.co.uk

	Permission is hereby granted, free of charge, to any person obtaining
	a copy of this software and associated documentation files (the
	"Software"), to	deal in the Software without restriction, including
	without limitation the rights to use, copy, modify, merge, publish,
	distribute, sublicense, and/or sell copies of the Software, and to
	permit persons to whom the Software is furnished to do so, subject to
	the following conditions:

	The above copyright notice and this permission notice shall be included
	in all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
	OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
	MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
	IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
	CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
	TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
	SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
	
	Translated from Java to C# by Alessandro Mascolo <manoplayinc@gmail.com>

   -------------------------------------------------------------------------- */

namespace CwLibNet.Squish;

public abstract class CompressorColourFit
{
    protected static readonly Vec ONE_V = new(1.0f);
    protected static readonly Vec ZERO_V = new(0.0f);

    public const float GRID_X = 31.0f;
    public const float GRID_Y = 63.0f;
    public const float GRID_Z = 31.0f;

    protected const float GRID_X_RCP = 1.0f / GRID_X;
    protected const float GRID_Y_RCP = 1.0f / GRID_Y;
    protected const float GRID_Z_RCP = 1.0f / GRID_Z;

    public static readonly Matrix? covariance = new();

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
	    switch (v)
	    {
		    case <= 0.0f:
			    return 0.0f;
		    case >= 1.0f:
			    return 1.0f;
		    default:
			    return (int)(grid * v + 0.5f) * gridRcp;
	    }
    }
}
