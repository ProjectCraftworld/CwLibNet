using static net.torutheredfox.craftworld.serialization.Serializer;
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