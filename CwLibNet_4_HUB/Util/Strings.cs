using System.Text.RegularExpressions;
using CwLibNet.Types.Data;

namespace CwLibNet.Util;

public static class Strings
{
    private static readonly Regex Sha1Regex = new("(h?)[a-fA-F0-9]{40}$", RegexOptions.Compiled);
    private static readonly Regex GUIDRegex = new("(g?)\\d+", RegexOptions.Compiled);
    private static readonly Regex HexGUIDRegex = new("(g?)(0x|0X)[a-fA-F0-9]+$", RegexOptions.Compiled);

    /// <summary>
    /// Left pads the input with zeros.
    /// </summary>
    public static string LeftPad(string text, int size)
    {
        return text.PadLeft(size, '0');
    }

    /// <summary>
    /// Parses the string as a long.
    /// </summary>
    public static long GetLong(string? number)
    {
        if (string.IsNullOrEmpty(number)) return -1;
        try
        {
            number = number.Replace(" ", "");
            if (number.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                return Convert.ToInt64(number[2..], 16);
            if (!number.StartsWith("g")) return long.Parse(number);
            number = number[1..];
            return number.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(number[2..], 16) : long.Parse(number);
        }
        catch (FormatException)
        {
            return -1;
        }
    }

    /// <summary>
    /// Gets a SHA1 from a string
    /// </summary>
    public static Sha1? GetSHA1(string? hash)
    {
        if (string.IsNullOrEmpty(hash)) return null;
        hash = hash.Replace(" ", "");
        if (hash.StartsWith("h"))
            hash = hash[1..];
        return IsSHA1(hash) ? new Sha1(hash) : null;
    }

    /// <summary>
    /// Gets a GUID from a string
    /// </summary>
    public static GUID? GetGUID(string? number)
    {
        var value = GetLong(number);
        return value > 0 ? new GUID(value) : null;
    }

    /// <summary>
    /// Tests if a given string is a SHA1 hash.
    /// </summary>
    public static bool IsSHA1(string? hash)
    {
        return !string.IsNullOrEmpty(hash) && Sha1Regex.IsMatch(hash);
    }

    /// <summary>
    /// Tests if a given string is a valid GUID.
    /// </summary>
    public static bool IsGUID(string? guid)
    {
        return !string.IsNullOrEmpty(guid) && (GUIDRegex.IsMatch(guid) || HexGUIDRegex.IsMatch(guid));
    }

    /// <summary>
    /// Cleans up a filepath string for consistency.
    /// </summary>
    public static string CleanupPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        path = path.Trim().Replace("\\", "/");
        path = Regex.Replace(path, "[//]+", "/");
        if (path.StartsWith("/"))
            path = path[1..];
        if (path.EndsWith("/"))
            path = path[..^1];
        return path;
    }

    /// <summary>
    /// Sets the file extension of a given path.
    /// </summary>
    public static string SetExtension(string path, string extension)
    {
        if (extension.StartsWith("."))
            extension = extension[1..];
        var index = path.LastIndexOf('.');
        if (index != -1)
            path = path[..index];
        return path + "." + extension;
    }
}