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

namespace Xxtea {
    using System;
    using System.Text;

    public sealed class XXTEA {
        private static readonly UTF8Encoding utf8 = new UTF8Encoding();

        private const UInt32 delta = 0x9E3779B9;

        private static UInt32 MX(UInt32 sum, UInt32 y, UInt32 z, Int32 p, UInt32 e, UInt32[] k) {
            return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
        }

        private XXTEA() {
        }

        public static byte[]? Encrypt(byte[]? data, byte[]? key) {
            if (data.Length == 0) {
                return data;
            }
            return ToByteArray(Encrypt(ToUInt32Array(data, true), ToUInt32Array(FixKey(key), false)), false);
        }

        public static byte[]? Encrypt(String data, byte[]? key) {
            return Encrypt(utf8.GetBytes(data), key);
        }

        public static byte[]? Encrypt(byte[]? data, String key) {
            return Encrypt(data, utf8.GetBytes(key));
        }

        public static byte[]? Encrypt(String data, String key) {
            return Encrypt(utf8.GetBytes(data), utf8.GetBytes(key));
        }

        public static String EncryptToBase64String(byte[]? data, byte[]? key) {
            return Convert.ToBase64String(Encrypt(data, key));
        }

        public static String EncryptToBase64String(String data, byte[]? key) {
            return Convert.ToBase64String(Encrypt(data, key));
        }

        public static String EncryptToBase64String(byte[]? data, String key) {
            return Convert.ToBase64String(Encrypt(data, key));
        }

        public static String EncryptToBase64String(String data, String key) {
            return Convert.ToBase64String(Encrypt(data, key));
        }

        public static byte[]? Decrypt(byte[]? data, byte[]? key) {
            if (data.Length == 0) {
                return data;
            }
            return ToByteArray(Decrypt(ToUInt32Array(data, false), ToUInt32Array(FixKey(key), false)), true);
        }

        public static byte[]? Decrypt(byte[]? data, String key) {
            return Decrypt(data, utf8.GetBytes(key));
        }

        public static byte[]? DecryptBase64String(String data, byte[]? key) {
            return Decrypt(Convert.FromBase64String(data), key);
        }

        public static byte[]? DecryptBase64String(String data, String key) {
            return Decrypt(Convert.FromBase64String(data), key);
        }

        public static String DecryptToString(byte[]? data, byte[]? key) {
            return utf8.GetString(Decrypt(data, key));
        }

        public static String DecryptToString(byte[]? data, String key) {
            return utf8.GetString(Decrypt(data, key));
        }

        public static String DecryptBase64StringToString(String data, byte[]? key) {
            return utf8.GetString(DecryptBase64String(data, key));
        }

        public static String DecryptBase64StringToString(String data, String key) {
            return utf8.GetString(DecryptBase64String(data, key));
        }

        private static UInt32[] Encrypt(UInt32[] v, UInt32[] k) {
            Int32 n = v.Length - 1;
            if (n < 1) {
                return v;
            }
            UInt32 z = v[n], y, sum = 0, e;
            Int32 p, q = 6 + 52 / (n + 1);
            unchecked {
                while (0 < q--) {
                    sum += delta;
                    e = sum >> 2 & 3;
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

        private static UInt32[] Decrypt(UInt32[] v, UInt32[] k) {
            Int32 n = v.Length - 1;
            if (n < 1) {
                return v;
            }
            UInt32 z, y = v[0], sum, e;
            Int32 p, q = 6 + 52 / (n + 1);
            unchecked {
                sum = (UInt32)(q * delta);
                while (sum != 0) {
                    e = sum >> 2 & 3;
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

        private static byte[]? FixKey(byte[]? key) {
            if (key.Length == 16) return key;
            byte[]? fixedkey = new Byte[16];
            if (key.Length < 16) {
                key.CopyTo(fixedkey, 0);
            }
            else {
                Array.Copy(key, 0, fixedkey, 0, 16);
            }
            return fixedkey;
        }

        private static UInt32[] ToUInt32Array(byte[]? data, Boolean includeLength) {
            Int32 length = data.Length;
            Int32 n = (((length & 3) == 0) ? (length >> 2) : ((length >> 2) + 1));
            UInt32[] result;
            if (includeLength) {
                result = new UInt32[n + 1];
                result[n] = (UInt32)length;
            }
            else {
                result = new UInt32[n];
            }
            for (Int32 i = 0; i < length; i++) {
                result[i >> 2] |= (UInt32)data[i] << ((i & 3) << 3);
            }
            return result;
        }

        private static byte[]? ToByteArray(UInt32[] data, Boolean includeLength) {
            Int32 n = data.Length << 2;
            if (includeLength) {
                Int32 m = (Int32)data[data.Length - 1];
                n -= 4;
                if ((m < n - 3) || (m > n)) {
                    return null;
                }
                n = m;
            }
            byte[]? result = new Byte[n];
            for (Int32 i = 0; i < n; i++) {
                result[i] = (Byte)(data[i >> 2] >> ((i & 3) << 3));
            }
            return result;
        }

        /// <summary>
        /// Stores the TEA_KEY from Toolkit
        /// </summary>
        /// <returns></returns>
        public static byte[]? TEA_KEY()
        {
            int[] integers =[0x1B70CBD, 0x149607D6, 0x7F94DD5, 0x10DB8CA0];
            byte[]? result = new byte[integers.Length * sizeof(int)];
            Buffer.BlockCopy(integers, 0, result, 0, integers.Length);
            return result;
        }
    }
}