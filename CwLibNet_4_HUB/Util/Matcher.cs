namespace CwLibNet.Util;

public class Matcher
{
    /**
 * Finds the first occurence of a pattern in a byte array starting from the origin.
 *
 * @param data    Data to search
 * @param pattern Pattern to search for
 * @return Index in byte array where pattern occurs, -1 if not found
 */
    public static int IndexOf(byte[]? data, byte[]? pattern) => IndexOf(data, pattern, 0);

    /**
     * Finds the first occurence of a pattern in a byte array starting from an offset.
     *
     * @param data    Data to search
     * @param pattern Pattern to search for
     * @param offset  Offset to start from
     * @return Index in byte array where pattern occurs, -1 if not found
     */
    public static int IndexOf(byte[]? data, byte[]? pattern, int offset)
    {
        var failure = ComputeFailure(pattern);
        var j = 0;
        if (data.Length == 0)
            return -1;
        for (var i = offset; i < data.Length; i++)
        {
            while (j > 0 && pattern[j] != data[i])
                j = failure[j - 1];
            if (pattern[j] == data[i])
                j++;
            if (j == pattern.Length)
                return i - pattern.Length + 1;
        }
        return -1;
    }

    /**
     * Finds tall occurences of a pattern in a byte array starting from the origin.
     *
     * @param data    Data to search
     * @param pattern Pattern to search for
     * @return Indices in byte array where pattern occurs
     */
    public static int[] IndicesOf(byte[]? data, byte[]? pattern)
    {
        List<int> indices = new ();
        var offset = IndexOf(data, pattern, 0);
        while (offset != -1)
        {
            indices.Add(offset);
            offset = IndexOf(data, pattern, offset + pattern.Length);
        }
        return [.. indices];
    }

    /**
     * Computes the failure function using a boot-strapping process,
     * where the pattern is matched against itself.
     *
     * @param pattern Pattern to search for
     * @return Partial match table
     */
    private static int[] ComputeFailure(byte[]? pattern)
    {
        var failure = new int[pattern.Length];
        var j = 0;
        for (var i = 1; i < pattern.Length; i++)
        {
            while (j > 0 && pattern[j] != pattern[i])
                j = failure[j - 1];
            if (pattern[j] == pattern[i])
                j++;
            failure[i] = j;
        }
        return failure;
    }
}