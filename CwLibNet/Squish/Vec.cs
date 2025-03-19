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
public class Vec {

	public float X;
	public float Y;
	public float Z;

	public Vec() {
	}

	public Vec(float a):
		this(a, a, a) {
	}

	public Vec(Vec v): 
		this(v.X, v.Y, v.Z){
	}

	public Vec(float a, float b, float c) {
		X = a;
		Y = b;
		Z = c;
	}


	public Vec Set(float a) {
		this.X = a;
		this.Y = a;
		this.Z = a;

		return this;
	}

	public Vec Set(float x, float y, float z) {
		this.X = x;
		this.Y = y;
		this.Z = z;

		return this;
	}

	public Vec Set(Vec v) {
		this.X = v.X;
		this.Y = v.Y;
		this.Z = v.Z;

		return this;
	}

	public Vec Add(Vec v) {
		X += v.X;
		Y += v.Y;
		Z += v.Z;

		return this;
	}

	public Vec Add(float x, float y, float z) {
		this.X += x;
		this.Y += y;
		this.Z += z;

		return this;
	}

	public Vec Sub(Vec v) {
		X -= v.X;
		Y -= v.Y;
		Z -= v.Z;

		return this;
	}

	public Vec Mul(float s) {
		X *= s;
		Y *= s;
		Z *= s;

		return this;
	}

	public Vec Mul(Vec v) {
		X *= v.X;
		Y *= v.Y;
		Z *= v.Z;

		return this;
	}

	public Vec Div(float s) {
		float t = 1.0f / s;

		X *= t;
		Y *= t;
		Z *= t;

		return this;
	}

	public float LengthSq() {
		return Dot(this);
	}

	public float Dot(Vec v) {
		return X * v.X + Y * v.Y + Z * v.Z;
	}

}