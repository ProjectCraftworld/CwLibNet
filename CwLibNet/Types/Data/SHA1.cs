using XSHA1 = System.Security.Cryptography.SHA1;

public sealed class SHA1
{
    public static readonly SHA1 EMPTY = new();

    private readonly byte[]? hashBytes;
    private readonly string hashString;

    /// <summary>
    /// Creates an empty SHA1 hash.
    /// </summary>
    public SHA1()
    {
        hashBytes = new byte[20];
        hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    /// <summary>
    /// Constructs a SHA1 hash from a 40-character string.
    /// </summary>
    /// <param name="hash">SHA1 hash string</param>
    public SHA1(string? hash)
    {
        if (hash == null)
            throw new ArgumentNullException(nameof(hash), "SHA1 hash string cannot be null!");
        if (hash.Length != 40)
            throw new ArgumentException("SHA1 hash string must be 40 characters in length!");

        hashString = hash.ToLower();
        hashBytes = Enumerable.Range(0, hash.Length / 2)
                                   .Select(i => Convert.ToByte(hash.Substring(i * 2, 2), 16))
                                   .ToArray();
    }

    /// <summary>
    /// Creates a SHA1 instance from a 20-byte buffer.
    /// </summary>
    /// <param name="hash">SHA1 source buffer</param>
    public SHA1(byte[]? hash)
    {
        if (hash == null)
            throw new ArgumentNullException(nameof(hash), "SHA1 hash cannot be null!");
        if (hash.Length != 20)
            throw new ArgumentException("SHA1 hash must be 20 bytes in length!");

        hashBytes = hash;
        hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    /// <summary>
    /// Computes a SHA1 hash from a buffer.
    /// </summary>
    /// <param name="buffer">Source buffer to be hashed</param>
    /// <returns>A SHA1 hash instance</returns>
    public static SHA1 FromBuffer(byte[]? buffer)
    {
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer), "Data buffer provided to SHA1 hasher cannot be null!");

        using var sha1 = XSHA1.Create();
        var hash = sha1.ComputeHash(buffer);
        return new SHA1(hash);
    }

    public byte[]? GetHash()
    {
        return hashBytes;
    }

    public override bool Equals(object? obj)
    {
        if (obj == this) return true;
        return obj is SHA1 otherSHA1 && otherSHA1.ToString().Equals(ToString(), StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return hashString.GetHashCode();
    }

    public override string ToString()
    {
        return hashString;
    }
}
