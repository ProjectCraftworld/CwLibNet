using CwLibNet.Enums;
using CwLibNet.IO.Streams;
using CwLibNet.Types.Data;
using CwLibNet.Types;
using System.Numerics;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Util
{
    public class Bytes
    {
        private static readonly char[] HEX_ARRAY = ("0123456789ABCDEF".ToCharArray());

        /**
         * Converts a byte array into a hex string.
         * Implementation sourced from
         * <a href="https://stackoverflow.com/questions/9655181/how-to-convert-a-byte-array-to-a-hex-string-in-java">here</a>.
         *
         * @param bytes Byte array to hexify
         * @return Hex representation of byte array
         */
        public static string ToHex(byte[] bytes)
        {
            if (bytes == null)
                throw new NullReferenceException("Can't convert null byte array to hexadecimal string!");
            char[] hex = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; ++i)
            {
                int b = bytes[i] & 0xFF;
                hex[i * 2] = HEX_ARRAY[b >>> 4];
                hex[(i * 2) + 1] = HEX_ARRAY[b & 0xF];
            }
            return string.Join("", hex);
        }

        /**
         * Converts an integer into a hex string.
         *
         * @param value Integer to hexify
         * @return Hex representation of integer
         */
        public static String ToHex(int value)
        {
            return ToHex(ToBytesBE(value));
        }

        /**
         * Converts a hex string into a byte array.
         *
         * @param v Hex string
         * @return Byte array from hex string
         */
        public static byte[] FromHex(String v)
        {
            int length = v.Length;
            byte[] b = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
                b[i / 2] = (byte)(
                    (Convert.ToInt32(v[i].ToString(), 16) << 4) + Convert.ToInt32(v[i + 1].ToString(), 16)
                );
            return b;
        }

        /**
         * Converts a big-order byte array to a short primitive.
         *
         * @param b 2-byte array containing big-order short
         * @return the short from the byte array
         */
        public static short ToShortBE(byte[] b)
        {
            if (b == null)
                throw new NullReferenceException("Can't read data type from null byte array!");
            return (short)((b[0] & 0xFF) << 8 | (b[1] & 0xFF));
        }

        /**
         * Converts a little-order byte array to a short primitive.
         *
         * @param b 2-byte array containing litte-order short
         * @return the short from the byte array
         */
        public static short ToShortLE(byte[] b)
        {
            if (b == null)
                throw new NullReferenceException("Can't read data type from null byte array!");
            return (short)((b[0] & 0xFF) | (b[1] & 0xFF) << 8);
        }

        /**
         * Converts a big-order byte array to an integer primitive.
         *
         * @param b 4-byte array containing big-order integer
         * @return the integer from the byte array
         */
        public static int ToIntegerBE(byte[] b)
        {
            if (b == null)
                throw new NullReferenceException("Can't read data type from null byte array!");
            return (b[0] & 0xFF) << 24 |
                   (b[1] & 0xFF) << 16 |
                   (b[2] & 0xFF) << 8 |
                   (b[3] & 0xFF);
        }

        /**
         * Converts a little-order byte array to an integer primitive.
         *
         * @param b 4-byte array containing little-order integer
         * @return the integer from the byte array
         */
        public static int ToIntegerLE(byte[] b)
        {
            if (b == null)
                throw new NullReferenceException("Can't read data type from null byte array!");
            return (b[0] & 0xFF) |
                   (b[1] & 0xFF) << 8 |
                   (b[2] & 0xFF) << 16 |
                   (b[3] & 0xFF) << 24;
        }

        /**
         * Converts a short into a big-order byte array.
         *
         * @param v short primitive
         * @return the big-order byte array containing the short
         */
        public static byte[] ToBytesBE(short v)
        {
            return [
            (byte) (v >>> 8),
            (byte) (v & 0xFF)
        ];
        }

        /**
         * Converts a short into a little-order byte array.
         *
         * @param v short primitive
         * @return the little-order byte array containing the short
         */
        public static byte[] ToBytesLE(short v)
        {
            return [
            (byte) (v & 0xFF),
            (byte) (v >>> 8)
        ];
        }

        /**
         * Converts a integer into a big-order byte array.
         *
         * @param v integer primitive
         * @return the big-order byte array containing the integer
         */
        public static byte[] ToBytesBE(int v)
        {
            return [
            (byte) (v >>> 24),
            (byte) (v >>> 16),
            (byte) (v >>> 8),
            (byte) (v & 0xFF)
        ];
        }

        /**
         * Converts a integer into a little-order byte array.
         *
         * @param v integer primitive
         * @return the little-order byte array containing the integer
         */
        public static byte[] ToBytesLE(int v)
        {
            return [
            (byte) (v & 0xFF),
            (byte) (v >>> 8),
            (byte) (v >>> 16),
            (byte) (v >>> 24),
        ];
        }

        public static byte[] GetIntegerBuffer(long value, byte compressionFlags)
        {
            MemoryOutputStream output = new MemoryOutputStream(0x8, compressionFlags);
            output.U32(value);
            output.Shrink();
            return output.GetBuffer();
        }

        public static byte[] GetResourceReference(ResourceDescriptor? res, Revision revision,
                                                  byte compressionFlags)
        {
            Serializer serializer = new Serializer(0x1c + 0x4, revision, compressionFlags);
            serializer.Resource(res, ResourceType.Invalid, true);
            return serializer.GetBuffer();
        }

        /**
         * Converts an array of 32 bit big endian integers to a byte array.
         *
         * @param data Integer array
         * @return Converted byte array
         */
        public static byte[] FromIntArrayBE(int[] data)
        {
            if (data == null)
                throw new NullReferenceException("Integer stream cannot be null!");
            byte[] output = new byte[data.Length * 4];
            for (int i = 0; i < data.Length; ++i)
            {
                int v = data[i];
                int dest = (i * 4);
                output[dest] = (byte)(v >>> 24);
                output[dest + 1] = (byte)(v >>> 16);
                output[dest + 2] = (byte)(v >>> 8);
                output[dest + 3] = (byte)(v & 0xFF);
            }
            return output;
        }

        /**
         * Converts a big endian byte array to an array of 32 bit integers.
         *
         * @param data Byte array
         * @return Converted integer array
         */
        public static int[] ToIntArrayBE(byte[] data)
        {
            if (data == null)
                throw new NullReferenceException("Byte stream cannot be null!");
            if (data.Length % 4 != 0)
                throw new ArgumentException("Byte stream length must be divisible by 4!");
            int[] output = new int[data.Length / 4];
            for (int i = 0; i < output.Length; ++i)
            {
                int src = (i * 4);
                output[i] = (data[src] & 0xFF) << 24 |
                            (data[src + 1] & 0xFF) << 16 |
                            (data[src + 2] & 0xFF) << 8 |
                            (data[src + 3] & 0xFF);
            }
            return output;
        }

        /**
         * Splits a byte array into a series of chunks.
         * Implementation sourced from
         * <a href="https://stackoverflow.com/questions/3405195/divide-array-into-smaller-parts/26695737">here</a>.
         *
         * @param data Byte array to split
         * @param size Size of each byte chunk
         * @return Chunked byte arrays
         */
        public static byte[][] Split(byte[] data, int size)
        {
            byte[][] _out = new byte[(int)Math.Ceiling(data.Length / (double)size)][];
            int start = 0;
            for (int i = 0; i < _out.Length; ++i)
        {
                int end = Math.Min(data.Length, start + size);
                Array.Copy(data, _out[i], end + start);
                _out = _out.Skip(start).ToArray();
                start += size;
            }
            return _out;
        }

        /**
         * Combines a series of byte arrays into one.
         * Implementation sourced from
         * <a href="https://stackoverflow.com/questions/5513152/easy-way-to-concatenate-two-byte-arrays">here</a>.
         *
         * @param arrays Byte arrays to combine
         * @return Combined byte arrays.
         */
        public static byte[] Combine(params byte[][] arrays)
        {
            int totalLength = 0;
            foreach (byte[] bytes in arrays) totalLength += bytes.Length;
            byte[] result = new byte[totalLength];
            int currentIndex = 0;
            foreach (byte[] array in arrays)
            {
                Array.Copy(array, 0, result, currentIndex, array.Length);
                currentIndex += array.Length;
            }
            return result;
        }

        /**
         * Replaces all instances of pattern inside bytearray with another.
         * Does in-place modification if the lengths are the same, otherwise
         * a new array is created.
         *
         * @param source      Buffer to replace patterns in
         * @param original    Original pattern to replace
         * @param replacement Data to replace original pattern with
         * @return Bytearray with replaced patterns
         */
        public static byte[] Replace(byte[] source, byte[] original, byte[] replacement)
        {
            if (Equals(original, replacement)) return source;

            int[] offsets = Matcher.indicesOf(source, original);

            // If the original/replacement buffer are the same length,
            // we can save time and memory by just copying into
            // those regions.
            if (original.Length == replacement.Length)
            {
                foreach (int offset in offsets)
                    Array.Copy(replacement, 0, source, offset, replacement.Length);
                return source;
            }

            int diff = replacement.Length - original.Length;
            byte[] buffer =
                new byte[source.Length - (original.Length * offsets.Length) + (replacement.Length * offsets.Length)];

            int sourceOffset = 0;
            int destOffset = 0;
            for (int i = 0; i < offsets.Length; ++i)
            {
                int offset = offsets[i];
                int dest = offset + (diff * i);

                Array.Copy(source, sourceOffset, buffer, sourceOffset + (diff * i),
                    offset - sourceOffset);
                Array.Copy(replacement, 0, buffer, dest, replacement.Length);

                sourceOffset = offset + original.Length;
                destOffset = dest + replacement.Length;
            }

            int remaining = source.Length - sourceOffset;
            if (remaining != 0)
                Array.Copy(source, sourceOffset, buffer, destOffset, remaining);

            return buffer;
        }

        /**
         * Gets a byte array containing a ULEB-128 encoded value.
         *
         * @param value Value to encode
         * @return Encoded byte array
         */
        public static byte[] PackULEB128(long value)
        {
            MemoryOutputStream stream = new MemoryOutputStream(0x10,
                CompressionFlags.USE_ALL_COMPRESSION);
            return stream.U32(value).Shrink().GetBuffer();
        }

        /**
         * Unpacks a 11/11/10 normal value.
         *
         * @param value Value to unpack
         * @return Unpacked vector
         */
        public static Vector3 UnpackNormal32(long value)
        {
            // There's probably a much better way to handle
            // the fact that this is probably just signed data
            // but this works, so maybe I'll come back to it
            // at some point, who knows.

            Vector3 output = new Vector3(0, 0, 0);

            float x = (float)(value & 0x3ffL);
            bool x_sign = ((value >>> 10) & 1L) > 0L;

            float y = (float)((value >>> 11) & 0x3ffL);
            bool y_sign = ((value >>> 21) & 1L) > 0L;

            float z = (float)((value >>> 22) & 0x1ffL);
            bool z_sign = ((value >>> 31 & 1L)) > 0L;

            if (x_sign) output.X = -((1023f - x) / 1023f);
            else output.X = ((x / 1023f));

            if (y_sign) output.Z = -((1023f - y) / 1023f);
            else output.Y = (y / 1023f);

            if (z_sign) output.Z = -((511f - z) / 511f);
            else output.Z = (z / 511f);

            return output;
        }

        /**
         * Unpacks a 12/11/1 normal value.
         *
         * @param normal Value to unpack
         * @return Unpacked vector
         */
        public static Vector3 UnpackNormal24(int normal)
        {
            float x = (float)(normal & 0x7ff);
            x = ((normal & 0x800) != 0) ? (-(0x800 - x) / 0x7ff) : (x / 0x7ff);

            float y = (float)((normal >> 12) & 0x3ff);
            y = (((normal >> 12) & 0x400) != 0) ? (-(0x400 - y) / 0x3ff) : (y / 0x3ff);

            float z = (float)(Math.Pow(-1, (normal >> 23)) *
                               (Math.Sqrt((1 - (Math.Pow(x, 2) + Math.Pow(y, 2))))));

            return new Vector3(x, y, z);
        }

        /**
         * Packs a 11/11/10 normal value.
         *
         * @param normal Vertex normal to pack
         * @return Packed normal
         */
        public static int PackNormal32(Vector3 normal)
        {
            if (normal == null)
                throw new NullReferenceException("Can't pack null vertex normal!");

            int x = (int)Math.Round(normal.X * 0x3ff) & 0x7ff;
            int y = (int)Math.Round(normal.Y * 0x3ff) & 0x7ff;
            int z = (int)Math.Round(normal.Z * 0x1ff) & 0x3ff;

            return (x | (y << 11) | (z << 22));
        }

        /**
         * Packs a 12/11/1 normal value.
         *
         * @param normal Vertex normal to pack
         * @return Packed normal
         */
        public static int PackNormal24(Vector3 normal)
        {
            if (normal == null)
                throw new NullReferenceException("Can't pack null vertex normal!");

            int x = (int)Math.Round(normal.X * 0x7ff) & 0xfff;
            int y = (int)Math.Round(normal.Y * 0x3ff) & 0x7ff;
            int z = (normal.Z < 0) ? 1 : 0;

            return (x | (y << 12) | (z << 23));
        }
    }
}
