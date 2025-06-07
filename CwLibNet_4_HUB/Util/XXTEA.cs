using static CwLibNet.IO.Serializer.Serializer;
/**********************************************************\
|                                                          |
| XXTEA.cs                                                 |
|                                                          |
| XXTEA encryption algorithm library for .NET.             |
|                                                          |
| Encryption Algorithm Authors:                            |
|      David J. Wheeler                                    |
|      Roger M. Needham                                    |
|                                                          |
| Code Author:  Ma Bingyao <mabingyao@gmail.com>           |
| LastModified: Mar 10, 2015                               |
|                                                          |
\**********************************************************/

namespace Xxtea;

using System;
using System.Text;
using CwLibNet.IO.Serializer;

public static class XXTEA {
    private static readonly UTF8Encoding utf8 = new();

    private const uint delta = 0x9E3779B9;

    private static uint MX(uint sum, uint y, uint z, int p, uint e, uint[] k) {
        return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
    }

    public static byte[]? Encrypt(byte[]? data, byte[]? key)
    {
        return data is { Length: 0 } ? data : ToByteArray(Encrypt(ToUInt32Array(data, true), ToUInt32Array(FixKey(key), false)), false);
    }

    public static byte[]? Encrypt(string data, byte[]? key) {
        return Encrypt(utf8.GetBytes(data), key);
    }

    public static byte[]? Encrypt(byte[]? data, string key) {
        return Encrypt(data, utf8.GetBytes(key));
    }

    public static byte[]? Encrypt(string data, string key) {
        return Encrypt(utf8.GetBytes(data), utf8.GetBytes(key));
    }

    public static string EncryptToBase64String(byte[]? data, byte[]? key) {
        return Convert.ToBase64String(Encrypt(data, key));
    }

    public static string EncryptToBase64String(string data, byte[]? key) {
        return Convert.ToBase64String(Encrypt(data, key));
    }

    public static string EncryptToBase64String(byte[]? data, string key) {
        return Convert.ToBase64String(Encrypt(data, key));
    }

    public static string EncryptToBase64String(string data, string key) {
        return Convert.ToBase64String(Encrypt(data, key));
    }

    public static byte[]? Decrypt(byte[]? data, byte[]? key) {
        if (data.Length == 0) {
            return data;
        }
        return ToByteArray(Decrypt(ToUInt32Array(data, false), ToUInt32Array(FixKey(key), false)), true);
    }

    public static byte[]? Decrypt(byte[]? data, string key) {
        return Decrypt(data, utf8.GetBytes(key));
    }

    public static byte[]? DecryptBase64String(string data, byte[]? key) {
        return Decrypt(Convert.FromBase64String(data), key);
    }

    public static byte[]? DecryptBase64String(string data, string key) {
        return Decrypt(Convert.FromBase64String(data), key);
    }

    public static string DecryptToString(byte[]? data, byte[]? key) {
        return utf8.GetString(Decrypt(data, key));
    }

    public static string DecryptToString(byte[]? data, string key) {
        return utf8.GetString(Decrypt(data, key));
    }

    public static string DecryptBase64StringToString(string data, byte[]? key) {
        return utf8.GetString(DecryptBase64String(data, key));
    }

    public static string DecryptBase64StringToString(string data, string key) {
        return utf8.GetString(DecryptBase64String(data, key));
    }

    private static uint[] Encrypt(uint[] v, uint[] k) {
        var n = v.Length - 1;
        if (n < 1) {
            return v;
        }

        var z = v[n];
        uint sum = 0;
        var q = 6 + 52 / (n + 1);
        unchecked {
            while (0 < q--) {
                sum += delta;
                var e = sum >> 2 & 3;
                uint y;
                int p;
                for (p = 0; p < n; p++) {
                    y = v[p + 1];
                    z = v[p] += MX(sum, y, z, p, e, k);
                }
                y = v[0];
                z = v[n] += MX(sum, y, z, p, e, k);
            }
        }
        return v;
    }

    private static uint[] Decrypt(uint[] v, uint[] k) {
        var n = v.Length - 1;
        if (n < 1) {
            return v;
        }

        var y = v[0];
        var q = 6 + 52 / (n + 1);
        unchecked
        {
            var sum = (uint)(q * delta);
            while (sum != 0) {
                var e = sum >> 2 & 3;
                uint z;
                int p;
                for (p = n; p > 0; p--) {
                    z = v[p - 1];
                    y = v[p] -= MX(sum, y, z, p, e, k);
                }
                z = v[n];
                y = v[0] -= MX(sum, y, z, p, e, k);
                sum -= delta;
            }
        }
        return v;
    }

    private static byte[] FixKey(byte[]? key) {
        if (key is { Length: 16 }) return key;
        var fixedkey = new byte[16];
        if (key is { Length: < 16 }) {
            key.CopyTo(fixedkey, 0);
        }
        else {
            Array.Copy(key ?? throw new ArgumentNullException(nameof(key)), 0, fixedkey, 0, 16);
        }
        return fixedkey;
    }

    private static uint[] ToUInt32Array(byte[]? data, bool includeLength) {
        var length = data.Length;
        var n = (length & 3) == 0 ? length >> 2 : (length >> 2) + 1;
        uint[] result;
        if (includeLength) {
            result = new uint[n + 1];
            result[n] = (uint)length;
        }
        else {
            result = new uint[n];
        }
        for (var i = 0; i < length; i++) {
            result[i >> 2] |= (uint)data[i] << ((i & 3) << 3);
        }
        return result;
    }

    private static byte[]? ToByteArray(uint[] data, bool includeLength) {
        var n = data.Length << 2;
        if (includeLength) {
            var m = (int)data[^1];
            n -= 4;
            if (m < n - 3 || m > n) {
                return null;
            }
            n = m;
        }
        var result = new byte[n];
        for (var i = 0; i < n; i++) {
            result[i] = (byte)(data[i >> 2] >> ((i & 3) << 3));
        }
        return result;
    }

    /// <summary>
    /// Stores the TEA_KEY from Toolkit
    /// </summary>
    /// <returns></returns>
    public static byte[] TEA_KEY()
    {
        int[] integers =[0x1B70CBD, 0x149607D6, 0x7F94DD5, 0x10DB8CA0];
        var result = new byte[integers.Length * sizeof(int)];
        Buffer.BlockCopy(integers, 0, result, 0, integers.Length);
        return result;
    }
}