namespace CwLibNet.Extensions;

public static class Long
{
    public static int NumberOfLeadingZeros(this long value)
    {
        if (value == 0)
            return 64;

        int count = 0;
        ulong unsignedValue = (ulong)value;

        while ((unsignedValue & 0x8000000000000000) == 0)
        {
            count++;
            unsignedValue <<= 1;
        }

        return count;
    }
    
    public static int CompareUnsigned(this long x, long y)
    {
        ulong ux = (ulong)x;
        ulong uy = (ulong)y;
        return ux.CompareTo(uy);
    }
}