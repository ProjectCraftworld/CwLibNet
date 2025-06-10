using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Extensions;

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