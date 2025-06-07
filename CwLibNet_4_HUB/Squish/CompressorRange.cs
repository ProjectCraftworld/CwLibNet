using static net.torutheredfox.craftworld.serialization.Serializer;
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

public class CompressorRange : CompressorColourFit
{
    // Static arrays for temporary storage.
    private static readonly int[] closest = new int[16];
    private static readonly int[] indices = new int[16];
    private static readonly Vec[] codes = new Vec[4];

    // Static constructor to initialize the codes array.
    static CompressorRange()
    {
        for (var i = 0; i < codes.Length; i++)
        {
            codes[i] = new Vec();
        }
    }

    private readonly Util.Squish.CompressionMetric metric;
    private readonly Vec start = new();
    private readonly Vec end = new();

    private float bestError;

    public CompressorRange(ColourSet colours, Util.Squish.CompressionType type, Util.Squish.CompressionMetric metric)
        : base(colours, type)
    {
        this.metric = metric;
        bestError = float.MaxValue;

        var count = colours.GetCount();
        var points = colours.GetPoints();

        // Compute the covariance matrix using a shared matrix from the base class.
        var cov = Matrix.ComputeWeightedCovariance(colours, covariance);
        // Compute the principal component from the covariance.
        var principle = Matrix.ComputePrincipleComponent(cov);

        if (count <= 0) return;
        float bX, bY, bZ;
        float max;

        // Initialize with the first point.
        var aX = bX = points[0].X;
        var aY = bY = points[0].Y;
        var aZ = bZ = points[0].Z;
        var min = max = points[0].Dot(principle);

        // Find the points that have the minimum and maximum dot product with the principle.
        for (var i = 1; i < count; ++i)
        {
            var p = points[i];
            var val = p.Dot(principle);
            if (val < min)
            {
                aX = p.X;
                aY = p.Y;
                aZ = p.Z;
                min = val;
            }
            else if (val > max)
            {
                bX = p.X;
                bY = p.Y;
                bZ = p.Z;
                max = val;
            }
        }

        // Clamp the endpoints to the grid and [0,1] range.
        aX = Clamp(aX, GRID_X, GRID_X_RCP);
        aY = Clamp(aY, GRID_Y, GRID_Y_RCP);
        aZ = Clamp(aZ, GRID_Z, GRID_Z_RCP);
        start.Set(aX, aY, aZ);

        bX = Clamp(bX, GRID_X, GRID_X_RCP);
        bY = Clamp(bY, GRID_Y, GRID_Y_RCP);
        bZ = Clamp(bZ, GRID_Z, GRID_Z_RCP);
        end.Set(bX, bY, bZ);
    }

    // Implements the 3–code compression method.
    public override void Compress3(byte[] block, int offset)
    {
        var count = colours.GetCount();
        var points = colours.GetPoints();
        var v = new Vec();

        // Create a codebook:
        //   codes[0] = start
        //   codes[1] = end
        //   codes[2] = (start + end) * 0.5f
        codes[0].Set(start);
        codes[1].Set(end);
        codes[2].Set(start).Add(end).Mul(0.5f);

        var error = 0.0f;
        for (var i = 0; i < count; ++i)
        {
            var p = points[i];

            var dist = float.MaxValue;
            var index = 0;
            // Find the closest code among the three.
            for (var j = 0; j < 3; ++j)
            {
                var c = codes[j];
                v.Set(
                    (p.X - c.X) * metric.R,
                    (p.Y - c.Y) * metric.G,
                    (p.Z - c.Z) * metric.G
                );
                var d = v.LengthSq();
                if (d < dist)
                {
                    dist = d;
                    index = j;
                }
            }

            closest[i] = index;
            error += dist;
        }

        // If this scheme improves the error, save the indices and write the block.
        if (error < bestError)
        {
            colours.RemapIndices(closest, indices);
            ColourBlock.WriteColourBlock3(start, end, indices, block, offset);
            bestError = error;
        }
    }

    // Implements the 4–code compression method.
    public override void Compress4(byte[] block, int offset)
    {
        var count = colours.GetCount();
        var points = colours.GetPoints();
        var v = new Vec();

        // Create a codebook:
        //   codes[0] = start
        //   codes[1] = end
        //   codes[2] = (start * (2/3)) + (end * (1/3))
        //   codes[3] = (start * (1/3)) + (end * (2/3))
        codes[0].Set(start);
        codes[1].Set(end);
        codes[2].Set(2.0f / 3.0f).Mul(start).Add(v.Set(1.0f / 3.0f).Mul(end));
        codes[3].Set(1.0f / 3.0f).Mul(start).Add(v.Set(2.0f / 3.0f).Mul(end));

        var error = 0.0f;
        for (var i = 0; i < count; ++i)
        {
            var p = points[i];

            var dist = float.MaxValue;
            var index = 0;
            // Find the closest code among the four.
            for (var j = 0; j < 4; ++j)
            {
                var c = codes[j];
                v.Set(
                    (p.X - c.X) * metric.R,
                    (p.Y - c.Y) * metric.G,
                    (p.Z - c.Z) * metric.B
                );
                var d = v.LengthSq();
                if (d < dist)
                {
                    dist = d;
                    index = j;
                }
            }

            closest[i] = index;
            error += dist;
        }

        // Save this scheme if it results in a lower error.
        if (error < bestError)
        {
            colours.RemapIndices(closest, indices);
            ColourBlock.WriteColourBlock4(start, end, indices, block, offset);
            bestError = error;
        }
    }
}