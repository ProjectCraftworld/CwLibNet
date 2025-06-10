using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public sealed class CompressionFlags
{
    public static readonly byte USE_NO_COMPRESSION = 0;
    // no compression
    public static readonly byte USE_COMPRESSED_INTEGERS = 1;
    // 32/64bit integer data compression
    public static readonly byte USE_COMPRESSED_VECTORS = 2;
    // arrays are serialized
    public static readonly byte USE_COMPRESSED_MATRICES = 4;
    // 4x4 matrices will have a short flag for src
    public static readonly byte USE_ALL_COMPRESSION = 7;
    // all of the above will take effect
    public static string ToString(int flags)
    {
        var components = new List<string>(3);
        if ((flags & USE_COMPRESSED_INTEGERS) != 0)
            components.Add("COMPRESSED_INTEGERS");
        if ((flags & USE_COMPRESSED_MATRICES) != 0)
            components.Add("COMPRESSED_MATRICES");
        if ((flags & USE_COMPRESSED_VECTORS) != 0)
            components.Add("COMPRESSED_VECTORS");
        return components.ToString();
    }
}