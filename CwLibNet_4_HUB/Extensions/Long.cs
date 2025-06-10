using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Extensions;

public static class Longs
{
    public static int NumberOfLeadingZeros(this long value)
    {
        if (value == 0)
            return 64;

        var count = 0;
        var unsignedValue = (ulong)value;

        while ((unsignedValue & 0x8000000000000000) == 0)
        {
            count++;
            unsignedValue <<= 1;
        }

        return count;
    }
    
    public static int CompareUnsigned(this long x, long y)
    {
        var ux = (ulong)x;
        var uy = (ulong)y;
        return ux.CompareTo(uy);
    }
}