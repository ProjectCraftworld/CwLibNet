using Cwlib.Enums;
using CwLibNet.Types.Data;
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
        public MemoryOutputStream bytes(byte[] value)
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
        public MemoryOutputStream bytearray(byte[] value)
        {
            if (value == null) return i32(0);
            i32(value.Length);
            return bytes(value);
        }

        /**
         * Writes a bool to the stream.
         *
         * @param value bool to write
         * @return This output stream
         */
        public MemoryOutputStream boole(bool value)
        {
            return u8(value ? 1 : 0);
        }

        /**
         * Writes an array of bools to the stream.
         *
         * @param values bool array to write
         * @return This output stream
         */
        public MemoryOutputStream boolarray(bool[] values)
        {
            if (values == null) return i32(0);
            i32(values.Length);
            foreach (bool value in values)
                boole(value);
            return this;
        }

        /**
         * Writes a byte to the stream.
         *
         * @param value Byte to write
         * @return This output stream
         */
        public MemoryOutputStream i8(byte value)
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
        public MemoryOutputStream u8(int value)
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
        public MemoryOutputStream i16(short value)
        {
            if (isLittleEndian)
                return this.bytes(Bytes.toBytesLE(value));
            return this.bytes(Bytes.toBytesBE(value));
        }

        /**
         * Writes an integer to the stream as an unsigned short.
         *
         * @param value Short to write
         * @return This output stream
         */
        public MemoryOutputStream u16(int value)
        {
            return i16((short)(value & 0xFFFF));
        }

        /**
         * Writes a 24-bit unsigned integer to the stream.
         *
         * @param value Integer to write
         * @return This output stream
         */
        public MemoryOutputStream u24(int value)
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
            return bytes(b);
        }

        /**
         * Writes a 32-bit integer to the stream, compressed depending on flags.
         *
         * @param value   Integer to write
         * @param force32 Whether or not to write as a 32-bit integer, regardless of compression
         *                flags.
         * @return This output stream
         */
        public MemoryOutputStream i32(int value, bool force32)
        {
            if (!force32 && ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0))
                return uleb128(value & 0xFFFFFFFFL);
            if (isLittleEndian))
            return this.bytes(Bytes.toBytesLE(value));
            return this.bytes(Bytes.toBytesBE(value));
        }

        /**
         * Writes a 32-bit signed integer to the stream, compressed depending on flags.
         * This function modifies the value written to the stream to fit an unsigned value, prefer i32
         *
         * @param value Signed integer to write
         * @return This output stream
         */
        public MemoryOutputStream s32(int value)
        {
            if ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0)
            {
                return uleb128((long)((value & 0x7fffffff)) << 1 ^ ((value >> 0x1f)));
            }
            else
            {
                return i32(value, true);
            }
        }

        /**
         * Writes a long as a 32-bit integer to the stream, compressed depending on flags.
         *
         * @param value   Integer to write
         * @param force32 Whether or not to write as a 32-bit integer, regardless of compression
         *                flags.
         * @return This output stream
         */
        public MemoryOutputStream u32(long value, bool force32)
        {
            if (!force32 && ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0))
                return uleb128(value & 0xFFFFFFFFL);
            if (isLittleEndian)
                return this.bytes(Bytes.toBytesLE((int)(value & 0xFFFFFFFF)));
            return this.bytes(Bytes.toBytesBE((int)(value & 0xFFFFFFFF)));
        }

        /**
         * Writes a long to the stream, compressed depending on flags.
         *
         * @param value   Long to write
         * @param force64 Whether or not to write as a 64-bit integer, regardless of compression
         *                flags.
         * @return This output stream
         */
        public MemoryOutputStream u64(long value, bool force64)
        {
            if (!force64 && ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0))
                return uleb128(value);
            if (isLittleEndian)
            {
                return bytes([
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
            return bytes([
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
        public MemoryOutputStream s64(long value, bool force64)
        {
            return !force64 && ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0)
                ? uleb128(value << 1 ^ (value >> 0x3f))
                : u64(value, true);
        }

        /**
         * Writes an integer to the stream.
         *
         * @param value Integer to write
         * @return This output stream
         */
        public MemoryOutputStream i32(int value)
        {
            return i32(value, false);
        }

        /**
         * Writes a long as an unsigned integer to the stream.
         *
         * @param value Integer to write
         * @return This output stream
         */
        public MemoryOutputStream u32(long value)
        {
            return u32(value, false);
        }

        /**
         * Writes a long to the stream.
         *
         * @param value Long to write
         * @return This output stream
         */
        public MemoryOutputStream u64(long value)
        {
            return u64(value, false);
        }

        /**
         * Writes a "signed" long to the stream.
         *
         * @param value Long to write
         * @return This output stream
         */
        public MemoryOutputStream s64(long value)
        {
            return s64(value, false);
        }

        /**
         * Writes a variable length quantity to the stream.
         *
         * @param value Long to write
         * @return This output stream
         */
        public MemoryOutputStream uleb128(long value)
        {
            while (true)
            {
                byte b = (byte)(value & 0x7f);
                value >>>= 7;
                if (value > 0L) b |= 128;
                i8(b);
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
        public MemoryOutputStream shortarray(short[] values)
        {
            if (values == null) return i32(0);
            i32(values.Length);
            foreach (short value in values)
                i16(value);
            return this;
        }

        /**
         * Writes a 32-bit integer array to the stream.
         *
         * @param values Integer array to write
         * @return This output stream
         */
        public MemoryOutputStream intarray(int[] values)
        {
            if (values == null) return i32(0);
            i32(values.Length);
            foreach (int value in values)
                i32(value);
            return this;
        }

        /**
         * Writes a 64-bit integer array to the stream.
         *
         * @param values Long array to write
         * @return This output stream
         */
        public MemoryOutputStream longarray(long[] values)
        {
            if (values == null) return i32(0);
            this.i32(values.Length);
            foreach (long value in values)
                u64(value);
            return this;
        }

        /**
         * Writes a 16 bit floating point number to the stream.
         * https://stackoverflow.com/questions/6162651/half-precision-floating-point-in-java
         *
         * @param value Float to write
         * @return This output stream
         */
        public MemoryOutputStream f16(float value)
        {
            int fbits = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            int sign = fbits >>> 16 & 0x8000;
            int val = (fbits & 0x7fffffff) + 0x1000;

            if (val >= 0x47800000)
            {
                if ((fbits & 0x7fffffff) >= 0x47800000)
                {
                    if (val < 0x7f800000)
                        return u16(sign | 0x7c00);
                    return u16(sign | 0x7c00 | (fbits & 0x007fffff) >>> 13);
                }
                return u16(sign | 0x7bff);
            }

            if (val >= 0x38800000)
                return u16(sign | val - 0x38000000 >>> 13);
            if (val < 0x33000000)
                return u16(sign);
            val = (fbits & 0x7fffffff) >>> 23;
            return u16(sign | ((fbits & 0x7fffff | 0x800000) + (0x800000 >>> val - 102) >>> 126 - val));
        }

        /**
         * Writes a 32 bit floating point number to the stream.
         *
         * @param value Float to write
         * @return This output stream
         */
        public MemoryOutputStream f32(float value)
        {
            return i32(BitConverter.ToInt32(BitConverter.GetBytes(value), 0), true);
        }

        /**
         * Writes a 32-bit floating point number array to the stream.
         *
         * @param values Float array to write
         * @return This output stream
         */
        public MemoryOutputStream floatarray(float[] values)
        {
            if (values == null) return i32(0);
            i32(values.Length);
            foreach (float value in values)
                f32(value);
            return this;
        }

        /**
         * Writes a 2-dimensional floating point vector to the stream.
         *
         * @param value Vector2f to write
         * @return This output stream
         */
        public MemoryOutputStream v2(Vector2f value)
        {
            if (value == null) value = new Vector2f().zero();
            this.f32(value.x);
            this.f32(value.y);
            return this;
        }

        /**
         * Writes a 3-dimensional floating point vector to the stream.
         *
         * @param value Vector3f to write
         * @return This output stream
         */
        public MemoryOutputStream v3(Vector3f value)
        {
            if (value == null) value = new Vector3f().zero();
            this.f32(value.x);
            this.f32(value.y);
            this.f32(value.z);
            return this;
        }

        /**
         * Writes a 3-dimensional floating point vector to the stream.
         *
         * @param value Vector3f to write
         * @return This output stream
         */
        public MemoryOutputStream v3(float x, float y, float z)
        {
            f32(x);
            f32(y);
            f32(z);
            return this;
        }

        /**
         * Writes a 3-dimensional floating point vector to the stream.
         *
         * @param value Vector3f to write
         * @return This output stream
         */
        public MemoryOutputStream v3(Vector4f value)
        {
            if (value == null) value = new Vector4f().zero();
            this.f32(value.x);
            this.f32(value.y);
            this.f32(value.z);
            return this;
        }

        /**
         * Writes a 4-dimensional floating point vector to the stream.
         *
         * @param value Vector4f to write
         * @return This output stream
         */
        public MemoryOutputStream v4(Vector4f value)
        {
            if (value == null) value = new Vector4f().zero();
            this.f32(value.x);
            this.f32(value.y);
            this.f32(value.z);
            this.f32(value.w);
            return this;
        }

        /**
         * Writes an array of 4-dimensional 32-bit floating point vectors to the stream.
         *
         * @param values Vector array to write
         * @return This output stream
         */
        public MemoryOutputStream vectorarray(Vector4f[] values)
        {
            if (values == null) return i32(0);
            this.i32(values.Length);
            foreach (Vector4f value in values)
                v4(value);
            return this;
        }

        /**
         * Writes a Matrix4x4 to the stream, compressed depending on flags.
         *
         * @param value Matrix4x4 to write
         * @return This output stream
         */
        public MemoryOutputStream m44(Matrix4f value)
        {
            if (value == null) value = new Matrix4f().identity();

            float[] identity = new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };

            float[] values = new float[16];
            value.get(values);

            int flags = 0xFFFF;
            if ((compressionFlags & CompressionFlags.USE_COMPRESSED_MATRICES) != 0)
            {
                flags = 0;
                for (int i = 0; i < 16; ++i)
                    if (values[i] != identity[i])
                        flags |= (1 << i);
                i16((short)flags);
            }

            for (int i = 0; i < 16; ++i)
                if ((flags & (1 << i)) != 0)
                    f32(values[i]);

            return this;
        }

        /**
         * Writes a string of fixed size to the stream.
         *
         * @param value String to write
         * @param size  Fixed size of string
         * @return This output stream
         */
        public MemoryOutputStream str(String value, int size)
        {
            if (value == null) return bytes(new byte[size]);
            byte[] data = Encoding.ASCII.GetBytes(value);
            if (data.Length > size)
                data = data.Take(size).ToArray();
            bytes(data);
            this.pad(size - data.Length);
            return this;
        }

        /**
         * Writes a wide string of fixed size to the stream.
         *
         * @param value String to write
         * @param size  Fixed size of string
         * @return This output stream
         */
        public MemoryOutputStream wstr(String value, int size)
        {
            size *= 2;
            if (value == null) return bytes(new byte[size]);
            byte[] str = Encoding.BigEndianUnicode.GetBytes(value);
            if (str.Length > size)
                str = str.Take(size).ToArray();
            bytes(str);
            pad(size - str.Length);
            return this;
        }

        /**
         * Writes a length-prefixed string to the stream.
         *
         * @param value String to write
         * @return This output stream
         */
        public MemoryOutputStream str(String value)
        {
            if (value == null) return i32(0);
            byte[] string = value.getBytes(StandardCharsets.US_ASCII);
            this.s32(string.length);
            return this.bytes(string);
        }

        /**
         * Writes a length-prefixed wide string to the stream.
         *
         * @param value String to write
         * @return This output stream
         */
        public MemoryOutputStream wstr(String value)
        {
            if (value == null) return i32(0);
            byte[] string = value.getBytes(StandardCharsets.UTF_16BE);
            this.s32(string.length / 2);
            return this.bytes(string);
        }

        /**
         * Writes a SHA1 hash to the stream.
         *
         * @param value SHA1 hash to write
         * @return This output stream
         */
        public MemoryOutputStream sha1(SHA1 value)
        {
            if (value == null) return pad(0x14);
            return this.bytes(value.GetHash());
        }

        /**
         * Writes a GUID (uint32_t) to the stream.
         *
         * @param value   GUID to write
         * @param force32 Whether or not to read as a 32 bit integer, regardless of compression flags.
         * @return This output stream
         */
        public MemoryOutputStream guid(GUID value, bool force32)
        {
            if (value == null) return u32(0, force32);
            return this.u32(value.Value, force32);
        }

        /**
         * Writes a GUID (uint32_t) to the stream.
         *
         * @param value GUID to write
         * @return This output stream
         */
        public MemoryOutputStream guid(GUID value)
        {
            return guid(value, false);
        }


        /**
         * Writes an 8-bit enum value to the stream.
         *
         * @param <T>   Type of enum
         * @param value Enum value
         * @return This output stream
         */
        public MemoryOutputStream enum8(Enum value)
        {
            if (value == null) return u8(0);
            return i8(Convert.ToByte(value));
        }

        /**
         * Writes an 32-bit enum value to the stream.
         *
         * @param <T>   Type of enum
         * @param value Enum value
         * @return This output stream
         */
        public MemoryOutputStream enum32(Enum value)
        {
            if (value == null) return i32(0);
            return i32(Convert.ToInt32(value));
        }

        /**
         * Writes an 32-bit enum value to the stream.
         *
         * @param <T>    Type of enum
         * @param value  Enum value
         * @param signed Whether or not to write an s32
         * @return This output stream
         */
        public MemoryOutputStream enum32(Enum value, bool signed)
        {
            if (value == null) return i32(0);
            int v = Convert.ToInt32(value);
            if (signed) return s32(v);
            return i32(v);
        }

        /**
         * Writes an 32-bit enum value to the stream.
         *
         * @param <T>   Type of enum
         * @param value Enum value
         * @return This output stream
         */
        public MemoryOutputStream enumarray(Enum[] values)
        {
            if (values == null) return i32(0);
            this.i32(values.Length);
            foreach (Enum value in values)
                this.enum8(value);
            return this;
        }

        /**
         * Writes a series of null characters to the stream.
         *
         * @param size Number of bytes to write
         * @return This output stream
         */
        public MemoryOutputStream pad(int size)
        {
            offset += size;
            return this;
        }

        /**
         * Shrinks the size of the buffer to the current offset.
         *
         * @return This output stream
         */
        public MemoryOutputStream shrink()
        {
            Buffer = Arrays.copyOfRange(Buffer, 0, Offset);
            return this;
        }

        /**
         * Seeks to position relative to seek mode.
         *
         * @param offset Offset relative to seek position
         * @param mode   Seek origin
         */
        public void seek(int off, SeekMode mode)
        {
            if (mode == null)
                throw new NullReferenceException("SeekMode cannot be null!");
            if (offset < 0) throw new ArgumentException("Can't seek to negative offsets.");
            switch (mode)
            {
                case Begin:
                    {
                        if (offset > length)
                            throw new ArgumentException("Can't seek past stream length.");
                        offset = off;
                        break;
                    }
                case Relative:
                    {
                        int newOffset = offset + off;
                        if (newOffset > length || newOffset < 0)
                            throw new ArgumentException("Can't seek outside bounds of " +
                                                               "stream.");
                        offset = newOffset;
                        break;
                    }
                case End:
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
        public void seek(int offset)
        {
            this.seek(offset, SeekMode.Relative);
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
