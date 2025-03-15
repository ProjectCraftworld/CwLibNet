using Cwlib.Enums;
using CwLibNet.Types.Data;
using CwLibNet.Util;
using System.Numerics;
using System.Text;

namespace CwLibNet.IO.Streams
{

    /**
     * Big-endian binary output stream.
     */
    public class MemoryOutputStream
    {
        private byte[] buffer;

        private int offset = 0;
        private readonly int length;
        private byte compressionFlags;

        private bool isLittleEndian = false;

        /**
         * Creates a memory output stream with specified size.
         *
         * @param size Size of stream
         */
        public MemoryOutputStream(int size)
        {
            length = size;
            buffer = new byte[size];
        }

        /**
         * Creates a memory output stream with specified size and compression flags.
         *
         * @param size             Size of stream
         * @param compressionFlags Flags for compression methods used
         */
        public MemoryOutputStream(int size, byte compressionFlags) : this(size)
        {
            this.compressionFlags = compressionFlags;
        }

        /**
         * Writes an arbitrary number of bytes to the stream.
         *
         * @param value Bytes to write
         * @return This output stream
         */
        public MemoryOutputStream Bytes(byte[] value)
        {
            Array.Copy(value, 0, buffer, offset, value.Length);
            offset += value.Length;
            return this;
        }

        /**
         * Writes a byte array to the stream.
         *
         * @param value Bytes to write
         * @return This output stream
         */
        public MemoryOutputStream Bytearray(byte[]? value)
        {
            if (value == null) return I32(0);
            I32(value.Length);
            return Bytes(value);
        }

        /**
         * Writes a bool to the stream.
         *
         * @param value bool to write
         * @return This output stream
         */
        public MemoryOutputStream Boole(bool value)
        {
            return U8(value ? 1 : 0);
        }

        /**
         * Writes an array of bools to the stream.
         *
         * @param values bool array to write
         * @return This output stream
         */
        public MemoryOutputStream Boolarray(bool[]? values)
        {
            if (values == null) return I32(0);
            I32(values.Length);
            foreach (bool value in values)
                Boole(value);
            return this;
        }

        /**
         * Writes a byte to the stream.
         *
         * @param value Byte to write
         * @return This output stream
         */
        public MemoryOutputStream I8(byte value)
        {
            buffer[offset++] = value;
            return this;
        }

        /**
         * Writes an integer to the stream as a byte.
         *
         * @param value Byte to write
         * @return This output stream
         */
        public MemoryOutputStream U8(int value)
        {
            buffer[offset++] = (byte)(value & 0xFF);
            return this;
        }

        /**
         * Writes a short to the stream.
         *
         * @param value Short to write
         * @return This output stream
         */
        public MemoryOutputStream I16(short value)
        {
            if (isLittleEndian)
                return this.Bytes(Util.Bytes.ToBytesLE(value));
            return this.Bytes(Util.Bytes.ToBytesBE(value));
        }

        /**
         * Writes an integer to the stream as an unsigned short.
         *
         * @param value Short to write
         * @return This output stream
         */
        public MemoryOutputStream U16(int value)
        {
            return I16((short)(value & 0xFFFF));
        }

        /**
         * Writes a 24-bit unsigned integer to the stream.
         *
         * @param value Integer to write
         * @return This output stream
         */
        public MemoryOutputStream U24(int value)
        {
            value &= 0xFFFFFF;
            byte[] b;
            if (isLittleEndian)
            {
                b = new byte[] {
                (byte) (value & 0xFF),
                (byte) (value >>> 8),
                (byte) (value >>> 16),
            };
            }
            else
            {
                b = new byte[] {
                (byte) (value >>> 16),
                (byte) (value >>> 8),
                (byte) (value & 0xFF)
            };
            }
            return Bytes(b);
        }

        /**
         * Writes a 32-bit integer to the stream, compressed depending on flags.
         *
         * @param value   Integer to write
         * @param force32 Whether or not to write as a 32-bit integer, regardless of compression
         *                flags.
         * @return This output stream
         */
        public MemoryOutputStream I32(int value, bool force32)
        {
            if (!force32 && ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0))
                return Uleb128(value & 0xFFFFFFFFL);
            return this.Bytes(isLittleEndian ? Util.Bytes.ToBytesLE(value) : Util.Bytes.ToBytesBE(value));
        }

        /**
         * Writes a 32-bit signed integer to the stream, compressed depending on flags.
         * This function modifies the value written to the stream to fit an unsigned value, prefer i32
         *
         * @param value Signed integer to write
         * @return This output stream
         */
        public MemoryOutputStream S32(int value)
        {
            return (compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0 ? Uleb128((long)((value & 0x7fffffff)) << 1 ^ ((value >> 0x1f))) : I32(value, true);
        }

        /**
         * Writes a long as a 32-bit integer to the stream, compressed depending on flags.
         *
         * @param value   Integer to write
         * @param force32 Whether to write as a 32-bit integer, regardless of compression
         *                flags.
         * @return This output stream
         */
        public MemoryOutputStream U32(long? value, bool force32)
        {
            if (!force32 && ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0))
                return Uleb128(value & 0xFFFFFFFFL);
            if (isLittleEndian)
                return this.Bytes(Util.Bytes.ToBytesLE((int)(value & 0xFFFFFFFF)));
            return this.Bytes(Util.Bytes.ToBytesBE((int)(value & 0xFFFFFFFF)));
        }

        /**
         * Writes a long to the stream, compressed depending on flags.
         *
         * @param value   Long to write
         * @param force64 Whether or not to write as a 64-bit integer, regardless of compression
         *                flags.
         * @return This output stream
         */
        public MemoryOutputStream U64(long value, bool force64)
        {
            if (!force64 && ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0))
                return Uleb128(value);
            if (isLittleEndian)
            {
                return Bytes([
                (byte) (value),
                (byte) (value >>> 8),
                (byte) (value >>> 16),
                (byte) (value >>> 24),
                (byte) (value >>> 32),
                (byte) (value >>> 40),
                (byte) (value >>> 48),
                (byte) (value >>> 56),
            ]);
            }
            return Bytes([
            (byte) (value >>> 56),
            (byte) (value >>> 48),
            (byte) (value >>> 40),
            (byte) (value >>> 32),
            (byte) (value >>> 24),
            (byte) (value >>> 16),
            (byte) (value >>> 8),
            (byte) (value)
        ]);
        }

        /**
         * Writes a 64-bit signed integer to the stream, compressed depending on flags.
         *
         * @param value   Long to write
         * @param force64 Whether or not to write as a 64-bit integer, regardless of compression
         *                flags.
         * @return This output stream
         */
        public MemoryOutputStream S64(long value, bool force64)
        {
            return !force64 && ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0)
                ? Uleb128(value << 1 ^ (value >> 0x3f))
                : U64(value, true);
        }

        /**
         * Writes an integer to the stream.
         *
         * @param value Integer to write
         * @return This output stream
         */
        public MemoryOutputStream I32(int value)
        {
            return I32(value, false);
        }

        /**
         * Writes a long as an unsigned integer to the stream.
         *
         * @param value Integer to write
         * @return This output stream
         */
        public MemoryOutputStream U32(long value)
        {
            return U32(value, false);
        }

        /**
         * Writes a long to the stream.
         *
         * @param value Long to write
         * @return This output stream
         */
        public MemoryOutputStream U64(long value)
        {
            return U64(value, false);
        }

        /**
         * Writes a "signed" long to the stream.
         *
         * @param value Long to write
         * @return This output stream
         */
        public MemoryOutputStream S64(long value)
        {
            return S64(value, false);
        }

        /**
         * Writes a variable length quantity to the stream.
         *
         * @param value Long to write
         * @return This output stream
         */
        public MemoryOutputStream Uleb128(long? value)
        {
            while (true)
            {
                byte b = (byte)(value & 0x7f)!;
                value >>>= 7;
                if (value > 0L) b |= 128;
                I8(b);
                if (value == 0) break;
            }
            return this;
        }

        /**
         * Writes a 16-bit integer array to the stream.
         *
         * @param values Short array to write
         * @return This output stream
         */
        public MemoryOutputStream Shortarray(short[]? values)
        {
            if (values == null) return I32(0);
            I32(values.Length);
            foreach (short value in values)
                I16(value);
            return this;
        }

        /**
         * Writes a 32-bit integer array to the stream.
         *
         * @param values Integer array to write
         * @return This output stream
         */
        public MemoryOutputStream Intarray(int[]? values)
        {
            if (values == null) return I32(0);
            I32(values.Length);
            foreach (int value in values)
                I32(value);
            return this;
        }

        /**
         * Writes a 64-bit integer array to the stream.
         *
         * @param values Long array to write
         * @return This output stream
         */
        public MemoryOutputStream Longarray(long[]? values)
        {
            if (values == null) return I32(0);
            this.I32(values.Length);
            foreach (long value in values)
                U64(value);
            return this;
        }

        /**
         * Writes a 16 bit floating point number to the stream.
         * https://stackoverflow.com/questions/6162651/half-precision-floating-point-in-java
         *
         * @param value Float to write
         * @return This output stream
         */
        public MemoryOutputStream F16(float value)
        {
            int fbits = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            int sign = fbits >>> 16 & 0x8000;
            int val = (fbits & 0x7fffffff) + 0x1000;

            if (val >= 0x47800000)
            {
                if ((fbits & 0x7fffffff) >= 0x47800000)
                {
                    if (val < 0x7f800000)
                        return U16(sign | 0x7c00);
                    return U16(sign | 0x7c00 | (fbits & 0x007fffff) >>> 13);
                }
                return U16(sign | 0x7bff);
            }

            if (val >= 0x38800000)
                return U16(sign | val - 0x38000000 >>> 13);
            if (val < 0x33000000)
                return U16(sign);
            val = (fbits & 0x7fffffff) >>> 23;
            return U16(sign | ((fbits & 0x7fffff | 0x800000) + (0x800000 >>> val - 102) >>> 126 - val));
        }

        /**
         * Writes a 32 bit floating point number to the stream.
         *
         * @param value Float to write
         * @return This output stream
         */
        public MemoryOutputStream F32(float? value)
        {
            return I32(BitConverter.ToInt32(BitConverter.GetBytes(value ?? 0), 0), true);
        }

        /**
         * Writes a 32-bit floating point number array to the stream.
         *
         * @param values Float array to write
         * @return This output stream
         */
        public MemoryOutputStream Floatarray(float[] values)
        {
            if (values == null) return I32(0);
            I32(values.Length);
            foreach (float value in values)
                F32(value);
            return this;
        }

        /**
         * Writes a 2-dimensional floating point vector to the stream.
         *
         * @param value Vector2f to write
         * @return This output stream
         */
        public MemoryOutputStream V2(Vector2 value)
        {
            if (value == null) value = Vector2.Zero;
            this.F32(value.X);
            this.F32(value.Y);
            return this;
        }

        /**
         * Writes a 3-dimensional floating point vector to the stream.
         *
         * @param value Vector3 to write
         * @return This output stream
         */
        public MemoryOutputStream V3(Vector3? value)
        {
            value ??= Vector3.Zero;
            this.F32(value?.X);
            this.F32(value?.Y);
            this.F32(value?.Z);
            return this;
        }

        /**
         * Writes a 3-dimensional floating point vector to the stream.
         *
         * @param value Vector3 to write
         * @return This output stream
         */
        public MemoryOutputStream V3(float x, float y, float z)
        {
            F32(x);
            F32(y);
            F32(z);
            return this;
        }

        /**
         * Writes a 3-dimensional floating point vector to the stream.
         *
         * @param value Vector3f to write
         * @return This output stream
         */
        public MemoryOutputStream V3(Vector4 value)
        {
            if (value == null) value = Vector4.Zero;
            this.F32(value.X);
            this.F32(value.Y);
            this.F32(value.Z);
            return this;
        }

        /**
         * Writes a 4-dimensional floating point vector to the stream.
         *
         * @param value Vector4f to write
         * @return This output stream
         */
        public MemoryOutputStream V4(Vector4? value)
        {
            if (value == null) value = Vector4.Zero;
            this.F32(value?.X);
            this.F32(value?.Y);
            this.F32(value?.Z);
            this.F32(value?.W);
            return this;
        }

        /**
         * Writes an array of 4-dimensional 32-bit floating point vectors to the stream.
         *
         * @param values Vector array to write
         * @return This output stream
         */
        public MemoryOutputStream Vectorarray(Vector4[] values)
        {
            if (values == null) return I32(0);
            this.I32(values.Length);
            foreach (Vector4 value in values)
                V4(value);
            return this;
        }

        /**
         * Writes a Matrix4x4 to the stream, compressed depending on flags.
         *
         * @param value Matrix4x4 to write
         * @return This output stream
         */
        public MemoryOutputStream M44(Matrix4x4? value)
        {
            value ??= Matrix4x4.Identity;

            float[] identity = new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };

            float[] values = new float[16];
            // value.get(values);
            values =
            [
                value.Value.M11, value.Value.M12, value.Value.M13, value.Value.M14,
                value.Value.M21, value.Value.M22, value.Value.M23, value.Value.M24,
                value.Value.M31, value.Value.M32, value.Value.M33, value.Value.M34,
                value.Value.M41, value.Value.M42, value.Value.M43, value.Value.M44
            ];

            int flags = 0xFFFF;
            if ((compressionFlags & CompressionFlags.USE_COMPRESSED_MATRICES) != 0)
            {
                flags = 0;
                for (int i = 0; i < 16; ++i)
                    if (values[i] != identity[i])
                        flags |= (1 << i);
                I16((short)flags);
            }

            for (int i = 0; i < 16; ++i)
                if ((flags & (1 << i)) != 0)
                    F32(values[i]);

            return this;
        }

        /**
         * Writes a string of fixed size to the stream.
         *
         * @param value String to write
         * @param size  Fixed size of string
         * @return This output stream
         */
        public MemoryOutputStream Str(string? value, int size)
        {
            if (value == null) return Bytes(new byte[size]);
            byte[] data = Encoding.ASCII.GetBytes(value);
            if (data.Length > size)
                data = data.Take(size).ToArray();
            Bytes(data);
            this.Pad(size - data.Length);
            return this;
        }

        /**
         * Writes a wide string of fixed size to the stream.
         *
         * @param value String to write
         * @param size  Fixed size of string
         * @return This output stream
         */
        public MemoryOutputStream Wstr(String value, int size)
        {
            size *= 2;
            if (value == null) return Bytes(new byte[size]);
            byte[] str = Encoding.BigEndianUnicode.GetBytes(value);
            if (str.Length > size)
                str = str.Take(size).ToArray();
            Bytes(str);
            Pad(size - str.Length);
            return this;
        }

        /**
         * Writes a length-prefixed string to the stream.
         *
         * @param value String to write
         * @return This output stream
         */
        public MemoryOutputStream str(string value)
        {
            if (value == null) return I32(0);
            byte[] _string = Encoding.ASCII.GetBytes(value);
            this.S32(_string.Length);
            return this.Bytes(_string);
        }

        /**
         * Writes a length-prefixed wide string to the stream.
         *
         * @param value String to write
         * @return This output stream
         */
        public MemoryOutputStream Wstr(string? value)
        {
            if (value == null) return I32(0);
            byte[] _string = Encoding.BigEndianUnicode.GetBytes(value);
            this.S32(_string.Length / 2);
            return this.Bytes(_string);
        }

        /**
         * Writes a SHA1 hash to the stream.
         *
         * @param value SHA1 hash to write
         * @return This output stream
         */
        public MemoryOutputStream Sha1(SHA1? value)
        {
            return value == null ? Pad(0x14) : this.Bytes(value.GetHash());
        }

        /**
         * Writes a GUID (uint32_t) to the stream.
         *
         * @param value   GUID to write
         * @param force32 Whether to read as a 32-bit integer, regardless of compression flags.
         * @return This output stream
         */
        public MemoryOutputStream Guid(GUID? value, bool force32)
        {
            return value == null ? U32(0, force32) : this.U32(value?.Value, force32);
        }

        /**
         * Writes a GUID (uint32_t) to the stream.
         *
         * @param value GUID to write
         * @return This output stream
         */
        public MemoryOutputStream Guid(GUID? value)
        {
            return Guid(value, false);
        }


        /**
         * Writes an 8-bit enum value to the stream.
         *
         * @param <T>   Type of enum
         * @param value Enum value
         * @return This output stream
         */
        public MemoryOutputStream Enum8(Enum? value)
        {
            return value == null ? U8(0) : I8(Convert.ToByte(value));
        }

        /**
         * Writes an 32-bit enum value to the stream.
         *
         * @param <T>   Type of enum
         * @param value Enum value
         * @return This output stream
         */
        public MemoryOutputStream Enum32(Enum? value)
        {
            return I32(value == null ? 0 : Convert.ToInt32(value));
        }

        /**
         * Writes an 32-bit enum value to the stream.
         *
         * @param <T>    Type of enum
         * @param value  Enum value
         * @param signed Whether to write a s32
         * @return This output stream
         */
        public MemoryOutputStream Enum32(Enum? value, bool signed)
        {
            if (value == null) return I32(0);
            int v = Convert.ToInt32(value);
            return signed ? S32(v) : I32(v);
        }

        /**
         * Writes an 32-bit enum value to the stream.
         *
         * @param <T>   Type of enum
         * @param value Enum value
         * @return This output stream
         */
        public MemoryOutputStream Enumarray<T>(T[]? values) where T : Enum
        {
            if (values == null) return I32(0);
            this.I32(values.Length);
            foreach (T? value in values)
                this.Enum8(value);
            return this;
        }

        /**
         * Writes a series of null characters to the stream.
         *
         * @param size Number of bytes to write
         * @return This output stream
         */
        public MemoryOutputStream Pad(int size)
        {
            offset += size;
            return this;
        }

        /**
         * Shrinks the size of the buffer to the current offset.
         *
         * @return This output stream
         */
        public MemoryOutputStream Shrink()
        {
            Array.Resize(ref buffer, offset);
            return this;
        }

        /**
         * Seeks to position relative to seek mode.
         *
         * @param offset Offset relative to seek position
         * @param mode   Seek origin
         */
        public void Seek(int off, SeekMode? mode)
        {
            if (mode == null)
                throw new NullReferenceException("SeekMode cannot be null!");
            if (offset < 0) throw new ArgumentException("Can't seek to negative offsets.");
            switch (mode)
            {
                case SeekMode.Begin:
                    {
                        if (offset > length)
                            throw new ArgumentException("Can't seek past stream length.");
                        offset = off;
                        break;
                    }
                case SeekMode.Relative:
                    {
                        int newOffset = offset + off;
                        if (newOffset > length || newOffset < 0)
                            throw new ArgumentException("Can't seek outside bounds of " +
                                                               "stream.");
                        offset = newOffset;
                        break;
                    }
                case SeekMode.End:
                    {
                        if (off < 0 || length - off < 0)
                            throw new ArgumentException("Can't seek outside bounds of " +
                                                               "stream.");
                        offset = length - off;
                        break;
                    }
            }
        }

        /**
         * Seeks ahead in stream relative to offset.
         *
         * @param offset Offset to go to
         */
        public void Seek(int off)
        {
            this.Seek(off, SeekMode.Relative);
        }

        public byte[] GetBuffer()
        {
            return buffer;
        }

        public int GetOffset()
        {
            return offset;
        }

        public int GetLength()
        {
            return length;
        }

        public byte GetCompressionFlags()
        {
            return compressionFlags;
        }

        public bool IsLittleEndian()
        {
            return isLittleEndian;
        }

        public void SetLittleEndian(bool value)
        {
            isLittleEndian = value;
        }
    }
}
