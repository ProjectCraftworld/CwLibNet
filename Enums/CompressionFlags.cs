using System;

namespace Cwlib.Enums
{
    public sealed class CompressionFlags
    {
        public static readonly byte USE_NO_COMPRESSION = 0;
        public static readonly byte USE_COMPRESSED_INTEGERS = 1;
        public static readonly byte USE_COMPRESSED_VECTORS = 2;
        public static readonly byte USE_COMPRESSED_MATRICES = 4;
        public static readonly byte USE_ALL_COMPRESSION = 7;
        public static string ToString(int flags)
        {
            List<string> components = new List<string>(3);
            if ((flags & USE_COMPRESSED_INTEGERS) != 0)
                components.Add("COMPRESSED_INTEGERS");
            if ((flags & USE_COMPRESSED_MATRICES) != 0)
                components.Add("COMPRESSED_MATRICES");
            if ((flags & USE_COMPRESSED_VECTORS) != 0)
                components.Add("COMPRESSED_VECTORS");
            return components.ToString();
        }
    }
}