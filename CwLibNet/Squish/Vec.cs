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