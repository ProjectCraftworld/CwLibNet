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

public class CompressorCluster: CompressorColourFit
{
    private static int _maxIterations = 8;

	private static float _twoThirds = 2.0f / 3.0f;
	private static float _oneThird = 1.0f / 3.0f;
	private static float _half = 0.5f;
	private static float _zero = 0.0f;

	private static Vec _principle;

	private static float[] _dps = new float[16];

	private static float[] _weighted = new float[16 * 3];
	private static float[] _weights = new float[16];

	private static Util.Squish.CompressionMetric _metric;

	private static int[] _indices = new int[16];
	private static int[] _bestIndices = new int[16];

	private static float[] _alpha = new float[16];
	private static float[] _beta = new float[16];

	private static int[] _unordered = new int[16];

	private static Vec _xxSum = new Vec();

	private static float _bestError;

	private static int[] _orders = new int[16 * _maxIterations];

	public CompressorCluster(ColourSet colours, Util.Squish.CompressionType type, Util.Squish.CompressionMetric metric): base(colours, type) {

		// initialise the best error
		_bestError = float.MaxValue;

		// initialise the metric
		_metric = metric;

		// get the covariance matrix
		var covariance = Matrix.ComputeWeightedCovariance(colours, CompressorColourFit.covariance);

		// compute the principle component
		_principle = Matrix.ComputePrincipleComponent(covariance);
	}

	public override void Compress3(byte[] block, int offset) {
		var count = colours.GetCount();

		var bestStart = new Vec(0.0f);
		var bestEnd = new Vec(0.0f);
		var bestError = _bestError;

		var a = new Vec();
		var b = new Vec();

		// prepare an ordering using the principle axis
		ConstructOrdering(_principle, 0);

		// loop over iterations
		var indices = _indices;
		var bestIndices = _bestIndices;

		var alpha = _alpha;
		var beta = _beta;
		var weights = _weights;

		// check all possible clusters and iterate on the total order
		var bestIteration = 0;
		for ( var iteration = 0; ; ) {
			// first cluster [0,i) is at the start
			for ( var m = 0; m < count; ++m ) {
				indices[m] = 0;
				alpha[m] = weights[m];
				beta[m] = _zero;
			}
			for ( var i = count; i >= 0; --i ) {
				// second cluster [i,j) is half along
				for ( var m = i; m < count; ++m ) {
					indices[m] = 2;
					alpha[m] = beta[m] = _half * weights[m];
				}
				for ( var j = count; j >= i; --j ) {
					// last cluster [j,k) is at the end
					if ( j < count ) {
						indices[j] = 1;
						alpha[j] = _zero;
						beta[j] = weights[j];
					}

					// solve a least squares problem to place the endpoints
					var error = SolveLeastSquares(a, b);

					// keep the solution if it wins
					if ( error < bestError ) {
						bestStart.Set(a);
						bestEnd.Set(b);
						Array.Copy(indices, 0, bestIndices, 0, 16);
						bestError = error;
						bestIteration = iteration;
					}
				}
			}

			// stop if we didn't improve in this iteration
			if ( bestIteration != iteration )
				break;

			// advance if possible
			if ( ++iteration == _maxIterations )
				break;

			// stop if a new iteration is an ordering that has already been tried
			if ( !ConstructOrdering(a.Set(bestEnd).Sub(bestStart), iteration) )
				break;
		}

		// save the block if necessary
		if ( bestError < _bestError ) {
			var orders = _orders;
			var unordered = _unordered;

			// remap the indices
			var order = 16 * bestIteration;

			for ( var i = 0; i < count; ++i )
				unordered[orders[order + i]] = bestIndices[i];
			colours.RemapIndices(unordered, bestIndices);

			// save the block
			ColourBlock.WriteColourBlock3(bestStart, bestEnd, bestIndices, block, offset);

			// save the error
			_bestError = bestError;
		}
	}

	public override void Compress4(byte[] block, int offset) {
		var count = colours.GetCount();

		var bestStart = new Vec(0.0f);
		var bestEnd = new Vec(0.0f);
		var bestError = _bestError;

		var start = new Vec();
		var end = new Vec();

		// prepare an ordering using the principle axis
		ConstructOrdering(_principle, 0);

		// check all possible clusters and iterate on the total order
		var indices = _indices;
		var bestIndices = _bestIndices;

		var alpha = _alpha;
		var beta = _beta;
		var weights = _weights;

		var bestIteration = 0;

		// loop over iterations
		for ( var iteration = 0; ; ) {
			// first cluster [0,i) is at the start
			for ( var m = 0; m < count; ++m ) {
				indices[m] = 0;
				alpha[m] = weights[m];
				beta[m] = _zero;
			}
			for ( var i = count; i >= 0; --i ) {
				// second cluster [i,j) is one third along
				for ( var m = i; m < count; ++m ) {
					indices[m] = 2;
					alpha[m] = _twoThirds * weights[m];
					beta[m] = _oneThird * weights[m];
				}
				for ( var j = count; j >= i; --j ) {
					// third cluster [j,k) is two thirds along
					for ( var m = j; m < count; ++m ) {
						indices[m] = 3;
						alpha[m] = _oneThird * weights[m];
						beta[m] = _twoThirds * weights[m];
					}
					for ( var k = count; k >= j; --k ) {
						// last cluster [k,n) is at the end
						if ( k < count ) {
							indices[k] = 1;
							alpha[k] = _zero;
							beta[k] = weights[k];
						}

						// solve a least squares problem to place the endpoints
						var error = SolveLeastSquares(start, end);

						// keep the solution if it wins
						if ( error < bestError ) {
							bestStart.Set(start);
							bestEnd.Set(end);
							Array.Copy(indices, 0, bestIndices, 0, 16);
							bestError = error;
							bestIteration = iteration;
						}
					}
				}
			}

			// stop if we didn't improve in this iteration
			if ( bestIteration != iteration )
				break;

			// advance if possible
			++iteration;
			if ( iteration == _maxIterations )
				break;

			// stop if a new iteration is an ordering that has already been tried
			if ( !ConstructOrdering(start.Set(bestEnd).Sub(bestStart), iteration) )
				break;
		}

		// save the block if necessary
		if ( bestError < _bestError ) {
			var orders = _orders;
			var unordered = _unordered;

			// remap the indices
			var order = 16 * bestIteration;
			for ( var i = 0; i < count; ++i )
				unordered[orders[order + i]] = bestIndices[i];
			colours.RemapIndices(unordered, bestIndices);

			// save the block
			ColourBlock.WriteColourBlock4(bestStart, bestEnd, bestIndices, block, offset);

			// save the error
			_bestError = bestError;
		}
	}

	private bool ConstructOrdering(Vec axis, int iteration) {
		// cache some values
		var count = colours.GetCount();
		Vec[] values = colours.GetPoints();

		var orders = _orders;

		// build the list of dot products
		var dps = _dps;
		var order = 16 * iteration;
		for ( var i = 0; i < count; ++i ) {
			dps[i] = values[i].Dot(axis);
			orders[order + i] = i;
		}

		// stable sort using them
		for ( var i = 0; i < count; ++i ) {
			for ( var j = i; j > 0 && dps[j] < dps[j - 1]; --j ) {
				(dps[j], dps[j - 1]) = (dps[j - 1], dps[j]);

				(orders[order + j], orders[order + j - 1]) = (orders[order + j - 1], orders[order + j]);
			}
		}

		// check this ordering is unique
		for ( var it = 0; it < iteration; ++it ) {
			var prev = 16 * it;
			var same = true;
			for ( var i = 0; i < count; ++i ) {
				if ( orders[order + i] != orders[prev + i] ) {
					same = false;
					break;
				}
			}
			if ( same )
				return false;
		}

		// copy the ordering and weight all the points
		Vec[] points = colours.GetPoints();
		var cWeights = colours.GetWeights();
		_xxSum.Set(0.0f);

		var weighted = _weighted;

		for ( int i = 0, j = 0; i < count; ++i, j += 3 ) {
			var p = orders[order + i];

			var weight = cWeights[p];
			var point = points[p];

			_weights[i] = weight;

			var wX = weight * point.X;
			var wY = weight * point.Y;
			var wZ = weight * point.Z;

			_xxSum.Add(wX * wX, wY * wY, wZ * wZ);

			weighted[j + 0] = wX;
			weighted[j + 1] = wY;
			weighted[j + 2] = wZ;
		}
		return true;
	}

	private float SolveLeastSquares(Vec start, Vec end) {
		var count = colours.GetCount();

		var alpha2Sum = 0.0f;
		var beta2Sum = 0.0f;
		var alphabetaSum = 0.0f;

		var alphaxSumX = 0f;
		var alphaxSumY = 0f;
		var alphaxSumZ = 0f;

		var betaxSumX = 0f;
		var betaxSumY = 0f;
		var betaxSumZ = 0f;

		var alpha = _alpha;
		var beta = _beta;
		var weighted = _weighted;

		// accumulate all the quantities we need
		for ( int i = 0, j = 0; i < count; ++i, j += 3 ) {
			var a = alpha[i];
			var b = beta[i];

			alpha2Sum += a * a;
			beta2Sum += b * b;
			alphabetaSum += a * b;

			alphaxSumX += weighted[j + 0] * a;
			alphaxSumY += weighted[j + 1] * a;
			alphaxSumZ += weighted[j + 2] * a;

			betaxSumX += weighted[j + 0] * b;
			betaxSumY += weighted[j + 1] * b;
			betaxSumZ += weighted[j + 2] * b;
		}

		float aX, aY, aZ;
		float bX, bY, bZ;

		// zero where non-determinate
		if ( beta2Sum == 0.0f ) {
			var rcp = 1.0f / alpha2Sum;

			aX = alphaxSumX * rcp;
			aY = alphaxSumY * rcp;
			aZ = alphaxSumZ * rcp;
			bX = bY = bZ = 0.0f;
		} else if ( alpha2Sum == 0.0f ) {
			var rcp = 1.0f / beta2Sum;

			aX = aY = aZ = 0.0f;
			bX = betaxSumX * rcp;
			bY = betaxSumY * rcp;
			bZ = betaxSumZ * rcp;
		} else {
			var rcp = 1.0f / (alpha2Sum * beta2Sum - alphabetaSum * alphabetaSum);
			if ( float.IsPositiveInfinity(rcp) ) // Detect Infinity
				return float.MaxValue;

			aX = (alphaxSumX * beta2Sum - betaxSumX * alphabetaSum) * rcp;
			aY = (alphaxSumY * beta2Sum - betaxSumY * alphabetaSum) * rcp;
			aZ = (alphaxSumZ * beta2Sum - betaxSumZ * alphabetaSum) * rcp;

			bX = (betaxSumX * alpha2Sum - alphaxSumX * alphabetaSum) * rcp;
			bY = (betaxSumY * alpha2Sum - alphaxSumY * alphabetaSum) * rcp;
			bZ = (betaxSumZ * alpha2Sum - alphaxSumZ * alphabetaSum) * rcp;
		}

		// clamp the output to [0, 1]
		// clamp to the grid
		aX = Clamp(aX, GRID_X, GRID_X_RCP);
		aY = Clamp(aY, GRID_Y, GRID_Y_RCP);
		aZ = Clamp(aZ, GRID_Z, GRID_Z_RCP);

		start.Set(aX, aY, aZ);

		bX = Clamp(bX, GRID_X, GRID_X_RCP);
		bY = Clamp(bY, GRID_Y, GRID_Y_RCP);
		bZ = Clamp(bZ, GRID_Z, GRID_Z_RCP);

		end.Set(bX, bY, bZ);

		// compute the error
		var eX = aX * aX * alpha2Sum + bX * bX * beta2Sum + _xxSum.X + 2.0f * (aX * bX * alphabetaSum - aX * alphaxSumX - bX * betaxSumX);
		var eY = aY * aY * alpha2Sum + bY * bY * beta2Sum + _xxSum.Y + 2.0f * (aY * bY * alphabetaSum - aY * alphaxSumY - bY * betaxSumY);
		var eZ = aZ * aZ * alpha2Sum + bZ * bZ * beta2Sum + _xxSum.Z + 2.0f * (aZ * bZ * alphabetaSum - aZ * alphaxSumZ - bZ * betaxSumZ);

		// apply the metric to the error term
		return _metric.Dot(eX, eY, eZ);
	}
}