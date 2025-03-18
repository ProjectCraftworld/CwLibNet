namespace CwLibNet.Squish;

public class ColourSet {

	private int count;

	private Vec[] points = new Vec[16];
	private float[] weights = new float[16];
	private int[] remap = new int[16];

	private bool transparent;

	public ColourSet() {
		for ( int i = 0; i < points.Length; i++ )
			points[i] = new Vec();
	}

	public void Init(byte[] rgba, int mask, Util.Squish.CompressionType type, bool weightAlpha) {
		// check the compression mode for dxt1
		bool isDxt1 = type == Util.Squish.CompressionType.DXT1;

		count = 0;
		transparent = false;

		// create the minimal set
		for ( int i = 0; i < 16; ++i ) {
			// check this pixel is enabled
			int bit = 1 << i;
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
			for ( int j = 0; ; ++j ) {
				// allocate a new point
				if ( j == i ) {
					// normalise coordinates to [0,1]
					float r = (rgba[4 * i] & 0xFF) / 255.0f;
					float g = (rgba[4 * i + 1] & 0xFF) / 255.0f;
					float b = (rgba[4 * i + 2] & 0xFF) / 255.0f;

					// add the point
					points[count].Set(r, g, b);
					// ensure there is always non-zero weight even for zero alpha
					weights[count] = (weightAlpha ? ((rgba[4 * i + 3] & 0xFF) + 1) / 256.0f : 1.0f);
					remap[i] = count++; // advance
					break;
				}

				// check for a match
				int oldbit = 1 << j;
				bool match = ((mask & oldbit) != 0)
									  && (rgba[4 * i] == rgba[4 * j])
									  && (rgba[4 * i + 1] == rgba[4 * j + 1])
									  && (rgba[4 * i + 2] == rgba[4 * j + 2])
									  && (rgba[4 * j + 3] >= 128 || !isDxt1);

				if ( match ) {
					// get the index of the match
					int index = remap[j];

					// ensure there is always non-zero weight even for zero alpha
					// map to this point and increase the weight
					weights[index] += (weightAlpha ? ((rgba[4 * i + 3] & 0xFF) + 1) / 256.0f : 1.0f);
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
		for ( int i = 0; i < 16; ++i ) {
			int j = remap[i];
			if ( j == -1 )
				target[i] = 3;
			else
				target[i] = source[j];
		}
	}

}