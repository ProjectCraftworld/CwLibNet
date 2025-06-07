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

public class Matrix {

	private const float FltEpsilon = 0.00001f;

	private static float[] _m = new float[6];
	private static float[] _u = new float[6];

	private float[] values = new float[6];

	public Matrix() {
	}

	public Matrix(float a) {
		for ( var i = 0; i < 6; ++i )
			values[i] = a;
	}

	private float Get(int index) {
		return values[index];
	}

	public static Matrix ComputeWeightedCovariance(ColourSet mColours, Matrix? covariance) {
		var count = mColours.GetCount();
		var points = mColours.GetPoints();
		var weights = mColours.GetWeights();

		var centroid = new Vec();
		var a = new Vec();
		var b = new Vec();

		// compute the centroid
		var total = 0.0f;
		for ( var i = 0; i < count; ++i ) {
			total += weights[i];
			centroid.Add(a.Set(points[i]).Mul(weights[i]));
		}
		centroid.Div(total);

		// accumulate the covariance matrix
		if ( covariance == null )
			covariance = new Matrix();
		else
			Array.Fill(covariance.values, 0.0f);

		var values = covariance.values;

		for ( var i = 0; i < count; ++i ) {
			a.Set(points[i]).Sub(centroid);
			b.Set(a).Mul(weights[i]);

			values[0] += a.X * b.X;
			values[1] += a.X * b.Y;
			values[2] += a.X * b.Z;
			values[3] += a.Y * b.Y;
			values[4] += a.Y * b.Z;
			values[5] += a.Z * b.Z;
		}

		// return it
		return covariance;
	}

	private static Vec GetMultiplicity1Evector(Matrix? matrix, float evalue) {
		var values = matrix.values;

		// compute M
		var m = _m;
		m[0] = values[0] - evalue;
		m[1] = values[1];
		m[2] = values[2];
		m[3] = values[3] - evalue;
		m[4] = values[4];
		m[5] = values[5] - evalue;

		// compute U
		var u = _u;
		u[0] = m[3] * m[5] - m[4] * m[4];
		u[1] = m[2] * m[4] - m[1] * m[5];
		u[2] = m[1] * m[4] - m[2] * m[3];
		u[3] = m[0] * m[5] - m[2] * m[2];
		u[4] = m[1] * m[2] - m[4] * m[0];
		u[5] = m[0] * m[3] - m[1] * m[1];

		// find the largest component
		var mc = Math.Abs(u[0]);
		var mi = 0;
		for ( var i = 1; i < 6; ++i ) {
			var c = Math.Abs(u[i]);
			if (!(c > mc)) continue;
			mc = c;
			mi = i;
		}

		// pick the column with this component
		return mi switch
		{
			0 => new Vec(u[0], u[1], u[2]),
			1 or 3 => new Vec(u[1], u[3], u[4]),
			_ => new Vec(u[2], u[4], u[5])
		};
	}

	private static Vec GetMultiplicity2Evector(Matrix? matrix, float evalue) {
		var values = matrix.values;

		// compute M
		var m = _m;
		m[0] = values[0] - evalue;
		m[1] = values[1];
		m[2] = values[2];
		m[3] = values[3] - evalue;
		m[4] = values[4];
		m[5] = values[5] - evalue;

		// find the largest component
		var mc = Math.Abs(m[0]);
		var mi = 0;
		for ( var i = 1; i < 6; ++i ) {
			var c = Math.Abs(m[i]);
			if (!(c > mc)) continue;
			mc = c;
			mi = i;
		}

		// pick the first eigenvector based on this index
		return mi switch
		{
			0 or 1 => new Vec(-m[1], m[0], 0.0f),
			2 => new Vec(m[2], 0.0f, -m[0]),
			3 or 4 => new Vec(0.0f, -m[4], m[3]),
			_ => new Vec(0.0f, -m[5], m[4])
		};
	}

	public static Vec ComputePrincipleComponent(Matrix? matrix) {
		var m = matrix.values;

		// compute the cubic coefficients
		var c0 = m[0] * m[3] * m[5]
		         + 2.0f * m[1] * m[2] * m[4]
		         - m[0] * m[4] * m[4]
		         - m[3] * m[2] * m[2]
		         - m[5] * m[1] * m[1];
		var c1 = m[0] * m[3] + m[0] * m[5] + m[3] * m[5]
		         - m[1] * m[1] - m[2] * m[2] - m[4] * m[4];
		var c2 = m[0] + m[3] + m[5];

		// compute the quadratic coefficients
		var a = c1 - 1.0f / 3.0f * c2 * c2;
		var b = -2.0f / 27.0f * c2 * c2 * c2 + 1.0f / 3.0f * c1 * c2 - c0;

		// compute the root count check
		var q = 0.25f * b * b + 1.0f / 27.0f * a * a * a;

		switch (q)
		{
			// test the multiplicity
			case > FltEpsilon:
				// only one root, which implies we have a multiple of the identity
				return new Vec(1.0f);
			case < -FltEpsilon:
			{
				// three distinct roots
				var theta = (float)Math.Atan2(Math.Sqrt(-q), -0.5f * b);
				var rho = (float)Math.Sqrt(0.25f * b * b - q);

				var rt = (float)Math.Pow(rho, 1.0f / 3.0f);
				var ct = (float)Math.Cos(theta / 3.0f);
				var st = (float)Math.Sin(theta / 3.0f);

				var l1 = 1.0f / 3.0f * c2 + 2.0f * rt * ct;
				var l2 = 1.0f / 3.0f * c2 - rt * (ct + (float)Math.Sqrt(3.0f) * st);
				var l3 = 1.0f / 3.0f * c2 - rt * (ct - (float)Math.Sqrt(3.0f) * st);

				// pick the larger
				if ( Math.Abs(l2) > Math.Abs(l1) )
					l1 = l2;
				if ( Math.Abs(l3) > Math.Abs(l1) )
					l1 = l3;

				// get the eigenvector
				return GetMultiplicity1Evector(matrix, l1);
			}
			default:
			{
				// if( -FLT_EPSILON <= Q && Q <= FLT_EPSILON )
				// two roots
				float rt;
				if ( b < 0.0f )
					rt = (float)-Math.Pow(-0.5f * b, 1.0f / 3.0f);
				else
					rt = (float)Math.Pow(0.5f * b, 1.0f / 3.0f);

				var l1 = 1.0f / 3.0f * c2 + rt;		// repeated
				var l2 = 1.0f / 3.0f * c2 - 2.0f * rt;

				// get the eigenvector
				return Math.Abs(l1) > Math.Abs(l2) ? GetMultiplicity2Evector(matrix, l1) : GetMultiplicity1Evector(matrix, l2);
			}
		}
	}

}