using XSHA1 = System.Security.Cryptography.SHA1;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Types.Data;

public sealed class Sha1
{
    public static readonly Sha1 Empty = new();

    private readonly byte[]? _hashBytes;
    private readonly string _hashString;

    /// <summary>
    /// Creates an empty SHA1 hash.
    /// </summary>
    public Sha1()
    {
        _hashBytes = new byte[20];
        _hashString = BitConverter.ToString(_hashBytes).Replace("-", "").ToLower();
    }

    /// <summary>
    /// Constructs a SHA1 hash from a 40-character string.
    /// </summary>
    /// <param name="hash">SHA1 hash string</param>
    public Sha1(string? hash)
    {
        if (hash == null)
            throw new ArgumentNullException(nameof(hash), "SHA1 hash string cannot be null!");
        if (hash.Length != 40)
            throw new ArgumentException("SHA1 hash string must be 40 characters in length!");

        _hashString = hash.ToLower();
        _hashBytes = Enumerable.Range(0, hash.Length / 2)
            .Select(i => Convert.ToByte(hash.Substring(i * 2, 2), 16))
            .ToArray();
    }

    /// <summary>
    /// Creates a SHA1 instance from a 20-byte buffer.
    /// </summary>
    /// <param name="hash">SHA1 source buffer</param>
    public Sha1(byte[]? hash)
    {
        if (hash == null)
            throw new ArgumentNullException(nameof(hash), "SHA1 hash cannot be null!");
        if (hash.Length != 20)
            throw new ArgumentException("SHA1 hash must be 20 bytes in length!");

        _hashBytes = hash;
        _hashString = BitConverter.ToString(_hashBytes).Replace("-", "").ToLower();
    }

    /// <summary>
    /// Computes a SHA1 hash from a buffer.
    /// </summary>
    /// <param name="buffer">Source buffer to be hashed</param>
    /// <returns>A SHA1 hash instance</returns>
    public static Sha1 FromBuffer(byte[]? buffer)
    {
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer), "Data buffer provided to SHA1 hasher cannot be null!");

        using var sha1 = XSHA1.Create();
        var hash = sha1.ComputeHash(buffer);
        return new Sha1(hash);
    }

    public byte[]? GetHash()
    {
        return _hashBytes;
    }

    public override bool Equals(object? obj)
    {
        if (obj == this) return true;
        return obj is Sha1 otherSha1 && otherSha1.ToString().Equals(ToString(), StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return _hashString.GetHashCode();
    }

    public override string ToString()
    {
        return _hashString;
    }
}