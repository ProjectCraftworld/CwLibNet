using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
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

public class ColourBlock {

	private static int[] _remapped = new int[16];

	private static int[] _indices = new int[16];

	private static int[] _codes = new int[16];

	private ColourBlock() {}

	private static int GammaColour(float colour, float scale) {
		//return round(scale * (float)Math.pow(colour, 1.0 / 2.2));
		return (int)Math.Round(scale * colour);
	}

	private static int FloatTo565(Vec colour) {
		// get the components in the correct range
		var r = (int)Math.Round(CompressorColourFit.GRID_X * colour.X);
		var g = (int)Math.Round(CompressorColourFit.GRID_Y * colour.Y);
		var b = (int)Math.Round(CompressorColourFit.GRID_Z * colour.Z);

		// pack into a single value
		return (r << 11) | (g << 5) | b;
	}

	private static void WriteColourBlock(int a, int b, int[] indices, byte[] block, int offset) {
		// write the endpoints
		block[offset + 0] = (byte)(a & 0xff);
		block[offset + 1] = (byte)(a >> 8);
		block[offset + 2] = (byte)(b & 0xff);
		block[offset + 3] = (byte)(b >> 8);

		// write the indices
		for ( var i = 0; i < 4; ++i ) {
			var index = 4 * i;
			block[offset + 4 + i] = (byte)(indices[index + 0] | (indices[index + 1] << 2) | (indices[index + 2] << 4) | (indices[index + 3] << 6));
		}
	}

	public static void WriteColourBlock3(Vec start, Vec end, int[] indices, byte[] block, int offset) {
		// get the packed values
		var a = FloatTo565(start);
		var b = FloatTo565(end);

		// remap the indices
		if ( a <= b ) {
			// use the indices directly
			Array.Copy(indices, 0, _remapped, 0, 16);
		} else {
			// swap a and b
			(a, b) = (b, a);
			for ( var i = 0; i < 16; ++i )
			{
				switch (indices[i])
				{
					case 0:
						_remapped[i] = 1;
						break;
					case 1:
						_remapped[i] = 0;
						break;
					default:
						_remapped[i] = indices[i];
						break;
				}
			}
		}

		// write the block
		WriteColourBlock(a, b, _remapped, block, offset);
	}

	public static void WriteColourBlock4(Vec start, Vec end, int[] indices, byte[] block, int offset) {
		// get the packed values
		var a = FloatTo565(start);
		var b = FloatTo565(end);

		// remap the indices

		if ( a < b ) {
			// swap a and b
			(a, b) = (b, a);
			for ( var i = 0; i < 16; ++i )
				_remapped[i] = (indices[i] ^ 0x1) & 0x3;
		} else if ( a == b ) {
			// use index 0
			Array.Fill(_remapped, 0);
		} else {
			// use the indices directly
			Array.Copy(indices, 0, _remapped, 0, 16);
		}

		// write the block
		WriteColourBlock(a, b, _remapped, block, offset);
	}

	public static void DecompressColour(byte[] rgba, byte[] block, int offset, bool isDxt1) {
		// unpack the endpoints
		var codes = _codes;

		var a = Unpack565(block, offset, codes, 0);
		var b = Unpack565(block, offset + 2, codes, 4);

		// generate the midpoints
		for ( var i = 0; i < 3; ++i ) {
			var c = codes[i];
			var d = codes[4 + i];

			if ( isDxt1 && a <= b ) {
				codes[8 + i] = (c + d) / 2;
				codes[12 + i] = 0;
			} else {
				codes[8 + i] = (2 * c + d) / 3;
				codes[12 + i] = (c + 2 * d) / 3;
			}
		}

		// fill in alpha for the intermediate values
		codes[8 + 3] = 255;
		codes[12 + 3] = isDxt1 && a <= b ? 0 : 255;

		// unpack the indices
		var indices = _indices;

		for ( var i = 0; i < 4; ++i ) {
			var index = 4 * i;
			var packed = block[offset + 4 + i] & 0xFF;

			indices[index + 0] = packed & 0x3;
			indices[index + 1] = (packed >> 2) & 0x3;
			indices[index + 2] = (packed >> 4) & 0x3;
			indices[index + 3] = (packed >> 6) & 0x3;
		}

		// store out the colours
		for ( var i = 0; i < 16; ++i ) {
			var index = 4 * indices[i];
			for ( var j = 0; j < 4; ++j )
				rgba[4 * i + j] = (byte)codes[index + j];
		}
	}

	private static int Unpack565(byte[] packed, int pOffset, int[] colour, int cOffset) {
		// build the packed value
		var value = (packed[pOffset + 0] & 0xff) | ((packed[pOffset + 1] & 0xff) << 8);

		// get the components in the stored range
		var red = (value >> 11) & 0x1f;
		var green = (value >> 5) & 0x3f;
		var blue = value & 0x1f;

		// scale up to 8 bits
		colour[cOffset + 0] = (red << 3) | (red >> 2);
		colour[cOffset + 1] = (green << 2) | (green >> 4);
		colour[cOffset + 2] = (blue << 3) | (blue >> 2);
		colour[cOffset + 3] = 255;

		// return the value
		return value;
	}

}