using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
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

namespace CwLibNet4Hub.Squish;

public class ColourSet {

	private int count;

	private Vec[] points = new Vec[16];
	private float[] weights = new float[16];
	private int[] remap = new int[16];

	private bool transparent;

	public ColourSet() {
		for ( var i = 0; i < points.Length; i++ )
			points[i] = new Vec();
	}

	public void Init(byte[] rgba, int mask, Util.Squish.CompressionType type, bool weightAlpha) {
		// check the compression mode for dxt1
		var isDxt1 = type == Util.Squish.CompressionType.DXT1;

		count = 0;
		transparent = false;

		// create the minimal set
		for ( var i = 0; i < 16; ++i ) {
			// check this pixel is enabled
			var bit = 1 << i;
			if ( (mask & bit) == 0 ) {
				remap[i] = -1;
				continue;
			}

			// check for transparent pixels when using dxt1
			if ( isDxt1 && (rgba[4 * i + 3] & 0xFF) < 128 ) {
				remap[i] = -1;
				transparent = true;
				continue;
			}

			// loop over previous points for a match
			for ( var j = 0; ; ++j ) {
				// allocate a new point
				if ( j == i ) {
					// normalise coordinates to [0,1]
					var r = (rgba[4 * i] & 0xFF) / 255.0f;
					var g = (rgba[4 * i + 1] & 0xFF) / 255.0f;
					var b = (rgba[4 * i + 2] & 0xFF) / 255.0f;

					// add the point
					points[count].Set(r, g, b);
					// ensure there is always non-zero weight even for zero alpha
					weights[count] = weightAlpha ? ((rgba[4 * i + 3] & 0xFF) + 1) / 256.0f : 1.0f;
					remap[i] = count++; // advance
					break;
				}

				// check for a match
				var oldbit = 1 << j;
				var match = (mask & oldbit) != 0
				            && rgba[4 * i] == rgba[4 * j]
				            && rgba[4 * i + 1] == rgba[4 * j + 1]
				            && rgba[4 * i + 2] == rgba[4 * j + 2]
				            && (rgba[4 * j + 3] >= 128 || !isDxt1);

				if ( match ) {
					// get the index of the match
					var index = remap[j];

					// ensure there is always non-zero weight even for zero alpha
					// map to this point and increase the weight
					weights[index] += weightAlpha ? ((rgba[4 * i + 3] & 0xFF) + 1) / 256.0f : 1.0f;
					remap[i] = index;
					break;
				}
			}
		}
	}

	public int GetCount() { return count; }

	public Vec[] GetPoints() { return points; }

	public float[] GetWeights() { return weights; }

	public bool IsTransparent() { return transparent; }

	public void RemapIndices(int[] source, int[] target) {
		for ( var i = 0; i < 16; ++i ) {
			var j = remap[i];
			if ( j == -1 )
				target[i] = 3;
			else
				target[i] = source[j];
		}
	}

}