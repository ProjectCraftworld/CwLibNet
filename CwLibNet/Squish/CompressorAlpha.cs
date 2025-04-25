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

public class CompressorAlpha
{
    private static int[] _swapped = new int[16];

	private static int[] _codes5 = new int[8];
	private static int[] _codes7 = new int[8];

	private static int[] _indices5 = new int[16];
	private static int[] _indices7 = new int[16];

	private static int[] _codes = new int[8];
	private static int[] _indices = new int[16];

	private CompressorAlpha() {}

	public static void CompressAlphaDxt3(byte[] rgba, int mask, byte[] block, int offset) {
		// quantise and pack the alpha values pairwise
		for ( var i = 0; i < 8; ++i ) {
			// quantise down to 4 bits
			var alpha1 = (rgba[8 * i + 3] & 0xFF) * (15.0f / 255.0f);
			var alpha2 = (rgba[8 * i + 7] & 0xFF) * (15.0f / 255.0f);
			var quant1 = (int)Math.Round(alpha1);
			var quant2 = (int)Math.Round(alpha2);

			// set alpha to zero where masked
			var bit1 = 1 << (2 * i);
			var bit2 = 1 << (2 * i + 1);
			if ( (mask & bit1) == 0 )
				quant1 = 0;
			if ( (mask & bit2) == 0 )
				quant2 = 0;

			// pack into the byte
			block[offset + i] = (byte)(quant1 | (quant2 << 4));
		}
	}

	public static void DecompressAlphaDxt3(byte[] rgba, byte[] block, int offset) {
		// unpack the alpha values pairwise
		for ( var i = 0; i < 8; ++i ) {
			// quantise down to 4 bits
			var quant = (block[offset + i] & 0xFF);

			// unpack the values
			var lo = quant & 0x0f;
			var hi = quant & 0xf0;

			// convert back up to bytes
			rgba[8 * i + 3] = (byte)(lo | (lo << 4));
			rgba[8 * i + 7] = (byte)(hi | (hi >> 4));
		}
	}

	private static int FitCodes(byte[] rgba, int mask, int[] codes, int[] indices) {
		// fit each alpha value to the codebook
		var err = 0;
		for ( var i = 0; i < 16; ++i ) {
			// check this pixel is valid
			var bit = 1 << i;
			if ( (mask & bit) == 0 ) {
				// use the first code
				indices[i] = 0;
				continue;
			}

			// find the least error and corresponding index
			var value = (rgba[4 * i + 3] & 0xFF);
			var least = int.MaxValue;
			var index = 0;
			for ( var j = 0; j < 8; ++j ) {
				// get the squared error from this code
				var dist = value - codes[j];
				dist *= dist;

				// compare with the best so far
				if ( dist < least ) {
					least = dist;
					index = j;
				}
			}

			// save this index and accumulate the error
			indices[i] = index;
			err += least;
		}

		// return the total error
		return err;
	}

	private static void WriteAlphaBlock(int alpha0, int alpha1, int[] indices, byte[] block, int offset) {
		// write the first two bytes
		block[offset + 0] = (byte)alpha0;
		block[offset + 1] = (byte)alpha1;

		// pack the indices with 3 bits each
		var src = 0;
		var dest = 2;
		for ( var i = 0; i < 2; ++i ) {
			// pack 8 3-bit values
			var value = 0;
			for ( var j = 0; j < 8; ++j ) {
				var index = indices[src++];
				value |= (index << 3 * j);
			}

			// store in 3 bytes
			for ( var j = 0; j < 3; ++j )
				block[offset + dest++] = (byte)((value >> 8 * j) & 0xff);
		}
	}

	private static void WriteAlphaBlock5(int alpha0, int alpha1, int[] indices, byte[] block, int offset) {
		// check the relative values of the endpoints
		var swapped = _swapped;

		if ( alpha0 > alpha1 ) {
			// swap the indices
			for ( var i = 0; i < 16; ++i ) {
				var index = indices[i];
				if ( index == 0 )
					swapped[i] = 1;
				else if ( index == 1 )
					swapped[i] = 0;
				else if ( index <= 5 )
					swapped[i] = 7 - index;
				else
					swapped[i] = index;
			}

			// write the block
			WriteAlphaBlock(alpha1, alpha0, swapped, block, offset);
		} else {
			// write the block
			WriteAlphaBlock(alpha0, alpha1, indices, block, offset);
		}
	}

	private static void WriteAlphaBlock7(int alpha0, int alpha1, int[] indices, byte[] block, int offset) {
		// check the relative values of the endpoints
		var swapped = _swapped;

		if ( alpha0 < alpha1 ) {
			// swap the indices
			for ( var i = 0; i < 16; ++i ) {
				var index = indices[i];
				if ( index == 0 )
					swapped[i] = 1;
				else if ( index == 1 )
					swapped[i] = 0;
				else
					swapped[i] = 9 - index;
			}

			// write the block
			WriteAlphaBlock(alpha1, alpha0, swapped, block, offset);
		} else {
			// write the block
			WriteAlphaBlock(alpha0, alpha1, indices, block, offset);
		}
	}

	public static void CompressAlphaDxt5(byte[] rgba, int mask, byte[] block, int offset) {
		// get the range for 5-alpha and 7-alpha interpolation
		var min5 = 255;
		var max5 = 0;
		var min7 = 255;
		var max7 = 0;
		for ( var i = 0; i < 16; ++i ) {
			// check this pixel is valid
			var bit = 1 << i;
			if ( (mask & bit) == 0 )
				continue;

			// incorporate into the min/max
			var value = (rgba[4 * i + 3] & 0xFF);
			if ( value < min7 )
				min7 = value;
			if ( value > max7 )
				max7 = value;
			if ( value != 0 && value < min5 )
				min5 = value;
			if ( value != 255 && value > max5 )
				max5 = value;
		}

		// handle the case that no valid range was found
		if ( min5 > max5 )
			min5 = max5;
		if ( min7 > max7 )
			min7 = max7;

		// fix the range to be the minimum in each case
		if ( max5 - min5 < 5 )
			max5 = Math.Min(min5 + 5, 255);
		if ( max5 - min5 < 5 )
			min5 = Math.Max(0, max5 - 5);

		if ( max7 - min7 < 7 )
			max7 = Math.Min(min7 + 7, 255);
		if ( max7 - min7 < 7 )
			min7 = Math.Max(0, max7 - 7);

		// set up the 5-alpha code book
		var codes5 = _codes5;

		codes5[0] = min5;
		codes5[1] = max5;
		for ( var i = 1; i < 5; ++i )
			codes5[1 + i] = ((5 - i) * min5 + i * max5) / 5;
		codes5[6] = 0;
		codes5[7] = 255;

		// set up the 7-alpha code book
		var codes7 = _codes7;

		codes7[0] = min7;
		codes7[1] = max7;
		for ( var i = 1; i < 7; ++i )
			codes7[1 + i] = ((7 - i) * min7 + i * max7) / 7;

		// fit the data to both code books
		var err5 = FitCodes(rgba, mask, codes5, _indices5);
		var err7 = FitCodes(rgba, mask, codes7, _indices7);

		// save the block with least error
		if ( err5 <= err7 )
			WriteAlphaBlock5(min5, max5, _indices5, block, offset);
		else
			WriteAlphaBlock7(min7, max7, _indices7, block, offset);
	}

	public static void DecompressAlphaDxt5(byte[] rgba, byte[] block, int offset) {
		// get the two alpha values
		var alpha0 = (block[offset + 0] & 0xFF);
		var alpha1 = (block[offset + 1] & 0xFF);

		// compare the values to build the codebook
		var codes = _codes;

		codes[0] = alpha0;
		codes[1] = alpha1;
		if ( alpha0 <= alpha1 ) {
			// use 5-alpha codebook
			for ( var i = 1; i < 5; ++i )
				codes[1 + i] = ((5 - i) * alpha0 + i * alpha1) / 5;
			codes[6] = 0;
			codes[7] = 255;
		} else {
			// use 7-alpha codebook
			for ( var i = 1; i < 7; ++i )
				codes[1 + i] = ((7 - i) * alpha0 + i * alpha1) / 7;
		}

		// decode the indices
		var indices = _indices;

		var src = 2;
		var dest = 0;
		for ( var i = 0; i < 2; ++i ) {
			// grab 3 bytes
			var value = 0;
			for ( var j = 0; j < 3; ++j ) {
				var b = (block[offset + src++] & 0xFF);
				value |= (b << 8 * j);
			}

			// unpack 8 3-bit values from it
			for ( var j = 0; j < 8; ++j ) {
				var index = (value >> 3 * j) & 0x7;
				indices[dest++] = index;
			}
		}

		// write out the indexed codebook values
		for ( var i = 0; i < 16; ++i )
			rgba[4 * i + 3] = (byte)codes[indices[i]];
	}
}