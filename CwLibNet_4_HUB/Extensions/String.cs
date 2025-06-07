using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Extensions;

public static class Strings
{
    public static string FirstCase(this string s)
    {
        var copy = s.ToLower();
        copy = copy[1..];
        copy = s[0].ToString().ToUpper() + copy;
        return copy;
    }
}