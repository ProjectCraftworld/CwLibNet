using Cwlib.Enums;
using CwLibNet.Types.Data;
using CwLibNet.Util;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CwLibNet.IO.Streams
{

    public enum SeekMode
    {
        Begin,
        Relative,
        End
    }

    public class MemoryInputStream
    {
        private readonly byte[] buffer;

        private int offset = 0;
        private readonly int length;
        private byte compressionFlags;

        private bool isLittleEndian = false;

        /**
         * Creates a memory input stream from byte array.
         *
         * @param buffer Byte array to use as source
         */
        public MemoryInputStream(byte[] buffer)
        {
            if (buffer == null)
                throw new NullReferenceException("Buffer supplied to MemoryInputStream cannot be " +
                                               "null!");
            this.buffer = buffer;
            this.length = buffer.Length;
        }

        /**
         * Creates a memory input stream from byte array with compression flags.
         *
         * @param buffer           Byte array to use as source
         * @param compressionFlags Flags for compression methods used
         */
        public MemoryInputStream(byte[] buffer, byte compressionFlags): this(buffer)
        {
            this.compressionFlags = compressionFlags;
        }

        /**
         * Creates a memory input stream from file at path.
         *
         * @param path Location to read data from
         */
        public MemoryInputStream(string path)
        {
            if (path == null)
                throw new NullReferenceException("Path supplied to MemoryInputStream cannot be " +
                                               "null!");
            byte[] data = File.ReadAllBytes(path) ?? throw new ArgumentException("File provided could not be read!");
            this.buffer = data;
            this.length = data.Length;
        }

        /**
         * Creates a memory input stream from file at path with compression flags.
         *
         * @param path             Location to read data from
         * @param compressionFlags Flags for compression methods used
         */
        public MemoryInputStream(String path, byte compressionFlags): this(path)
        {
            this.compressionFlags = compressionFlags;
        }

        /**
         * Reads an arbitrary number of bytes from the stream.
         *
         * @param size Number of bytes to read from the stream
         * @return Bytes read from the stream
         */
        public byte[] Bytes(int size)
        {
            this.offset += size;
            Array.Copy(this.buffer, 0, this.buffer, this.offset - size, this.offset);
            return this.buffer;
        }

        /**
         * Reads a byte array from the stream.
         *
         * @return Bytes read from the stream
         */
        public byte[] Bytearray()
        {
            int size = this.I32();
            return this.Bytes(size);
        }

        /**
         * Reads a bool from the stream.
         *
         * @return bool read from the stream
         */
        public bool Boole()
        {
        return this.I8() != 0;
        }

        /**
         * Reads an array of bools from the stream.
         *
         * @return bool array read from the stream
         */
        public bool[] Boolarray()
        {
            int count = this.I32();
            bool[] elements = new bool[count];
            for (int i = 0; i < count; ++i)
                elements[i] = this.Boole();
            return elements;
        }

        /**
         * Reads a byte from the stream.
         *
         * @return Byte read from the stream
         */
        public byte I8()
        {
            return this.buffer[this.offset++];
        }

        /**
         * Reads an unsigned byte from the stream as an integer.
         *
         * @return Byte read from the stream
         */
        public int U8()
        {
            return this.buffer[this.offset++] & 0xFF;
        }

        /**
         * Reads a short from the stream.
         *
         * @return Short read from the stream
         */
        public short I16()
        {
            byte[] bytes = this.Bytes(2);
            if (isLittleEndian) return CwLibNet.Util.Bytes.ToShortLE(bytes);
            return CwLibNet.Util.Bytes.ToShortBE(bytes);
        }

        /**
         * Reads an unsigned short from the stream as an integer.
         *
         * @return Short read from the stream
         */
        public int U16()
        {
            return (this.I16() & 0xFFFF);
        }

        /**
         * Reads an unsigned 24-bit integer from the stream.
         *
         * @return Integer read from the stream
         */
        public int U24()
        {
            byte[] b = this.Bytes(3);
            if (this.isLittleEndian)
                return (b[2] & 0xFF) << 16 | (b[1] & 0xFF) << 8 | b[0] & 0xFF;
            return (b[0] & 0xFF) << 16 | (b[1] & 0xFF) << 8 | b[2] & 0xFF;
        }

        /**
         * Reads a 32-bit integer from the stream, compressed depending on flags.
         *
         * @param force32 Whether or not to read as a 32-bit integer, regardless of compression flags.
         * @return Integer read from the stream
         */
        public int I32(bool force32)
        {
            if (force32 || (this.compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) == 0)
            {
                byte[] bytes = this.Bytes(4);
                if (this.isLittleEndian) return CwLibNet.Util.Bytes.ToIntegerLE(bytes);
                return CwLibNet.Util.Bytes.ToIntegerBE(bytes);
            }
            return (int)(this.Uleb128() & 0xFFFFFFFF);
        }

        /**
         * Reads a signed 32-bit integer from the stream, compressed depending on flags.
         * This function modifies the value written to the stream to fit an unsigned value, prefer i32
         *
         * @return Signed integer read from the stream
         */
        public int S32()
        {
            if (((this.compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) == 0))
                return this.I32(true);
            int v = (int)this.Uleb128();
            return (v >> 1 ^ -(v & 1));
        }

        /**
         * Reads a long as an unsigned integer from the stream, compressed depending on flags.
         *
         * @param force64 Whether or not to read as a 32-bit integer, regardless of compression flags.
         * @return Unsigned integer read from the stream
         */
        public long U32(bool force32)
        {
            if (force32 || (this.compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) == 0)
                return this.I32(true) & 0xFFFFFFFFL;
            return this.Uleb128();
        }

        /**
         * Reads a long from the stream, compressed depending on flags.
         *
         * @param force64 Whether or not to read as a 64-bit long, regardless of compression flags.
         * @return Long read from the stream
         */
        public long U64(bool force64)
        {
            if (force64 || (this.compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) == 0)
            {
                byte[] b = this.Bytes(8);
                if (this.isLittleEndian)
                {
                    return (b[7] & 0xFF) << 56 |
                           (b[6] & 0xFF) << 48 |
                           (b[5] & 0xFF) << 40 |
                           (b[4] & 0xFF) << 32 |
                           (b[3] & 0xFF) << 24 |
                           (b[2] & 0xFF) << 16 |
                           (b[1] & 0xFF) << 8 |
                           (b[0] & 0xFF) << 0;
                }
                return (b[0] & 0xFF) << 56 |
                       (b[1] & 0xFF) << 48 |
                       (b[2] & 0xFF) << 40 |
                       (b[3] & 0xFF) << 32 |
                       (b[4] & 0xFF) << 24 |
                       (b[5] & 0xFF) << 16 |
                       (b[6] & 0xFF) << 8 |
                       (b[7] & 0xFF) << 0;
            }
            return this.Uleb128();
        }

        /**
         * Reads a "signed" long from the stream, compressed depending on flags.
         *
         * @param force64 Whether or not to read as a 64-bit long, regardless of compression flags.
         * @return Long read from the stream
         */
        public long S64(bool force64)
        {
            if (force64 || ((this.compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) == 0))
                return this.U64(true);
            long v = this.Uleb128();
            return v >> 1 ^ -(v & 1L);
        }

        /**
         * Reads an integer from the stream.
         *
         * @return Integer read from the stream
         */
        public int I32()
        {
            return this.I32(false);
        }

        /**
         * Reads a long as an unsigned integer from the stream.
         *
         * @return Unsigned integer read from the stream
         */
        public long U32()
        {
            return this.U32(false);
        }

        /**
         * Reads a long from the stream.
         *
         * @return Long read from the stream
         */
        public long U64()
        {
            return this.U64(false);
        }

        /**
         * Reads a "signed" long from the stream.
         *
         * @return Long read from the stream
         */
        public long S64()
        {
            return this.S64(false);
        }

        /**
         * Reads a variable length quantity from the stream.
         *
         * @return Long value read from the stream
         */
        public long Uleb128()
        {
            long result = 0;
            int i = 0;
            while (true)
            {
                long b = this.U8() & 0xFFL;
                result |= (b & 0x7f) << 7 * i;
                if ((b & 0x80L) == 0L)
                    break;
                ++i;
            }
            return result >>> 0;
        }

        /**
         * Reads a 16-bit integer array from the stream.
         *
         * @return Short array read from the stream
         */
        public short[] Shortarray()
        {
            int count = this.I32();
            short[] elements = new short[count];
            for (int i = 0; i < count; ++i)
                elements[i] = this.I16();
            return elements;
        }

        /**
         * Reads a 32-bit integer array from the stream.
         *
         * @return Integer array read from the stream
         */
        public int[] Intarray()
        {
            int count = this.I32();
            int[] elements = new int[count];
            for (int i = 0; i < count; ++i)
                elements[i] = this.I32();
            return elements;
        }

        /**
         * Reads a 64-bit integer array from the stream.
         *
         * @return Long array read from the stream
         */
        public long[] Longarray()
        {
            int count = this.I32();
            long[] elements = new long[count];
            for (int i = 0; i < count; ++i)
                elements[i] = this.U64();
            return elements;
        }

        /**
         * Reads a 16 bit floating point number from the stream.
         * https://stackoverflow.com/questions/6162651/half-precision-floating-point-in-java
         *
         * @return Float read from the stream
         */
        public float F16()
        {
            int half = this.U16();
            int mant = half & 0x03ff;
            int exp = half & 0x7c00;
            if (exp == 0x7c00) exp = 0x3fc00;
            else if (exp != 0)
            {
                exp += 0x1c000;
                if (mant == 0 && exp > 0x1c400)
                    return BitConverter.Int32BitsToSingle((half & 0x8000) << 16 | exp << 13 | 0x3ff);
            }
            else if (mant != 0)
            {
                exp = 0x1c400;
                do
                {
                    mant <<= 1;
                    exp -= 0x400;
                } while ((mant & 0x400) != 0);
                mant &= 0x3ff;
            }
            return BitConverter.Int32BitsToSingle((half & 0x8000) << 16 | (exp | mant) << 13);
        }

        /**
         * Reads a 32 bit floating point number from the stream.
         *
         * @return Float read from the stream
         */
        public float F32()
        {
            return BitConverter.Int32BitsToSingle(this.I32(true));
        }

        /**
         * Reads a 32-bit floating point number array from the stream.
         *
         * @return Float array read from the stream
         */
        public float[] Floatarray()
        {
            int count = this.I32();
            float[] elements = new float[count];
            for (int i = 0; i < count; ++i)
                elements[i] = this.F32();
            return elements;
        }

        /**
         * Reads a 2-dimensional floating point vector from the stream.
         *
         * @return Vector2f read from the stream
         */
        public Vector2 V2()
        {
            return new Vector2(this.F32(), this.F32());
        }

        /**
         * Reads a 3-dimensional floating point vector from the stream.
         *
         * @return Vector3f read from the stream
         */
        public Vector3 V3()
        {
            return new Vector3(this.F32(), this.F32(), this.F32());
        }

        /**
         * Reads a 4-dimensional floating point vector from the stream.
         *
         * @return Vector4f read from the stream
         */
        public Vector4 V4()
        {
            return new Vector4(this.F32(), this.F32(), this.F32(), this.F32());
        }

        /**
         * Reads an array of 4-dimensional floating point vectors from the stream.
         *
         * @return Vector array read from the stream
         */
        public Vector4[] Vectorarray()
        {
            int count = this.I32();
            Vector4[] elements = new Vector4[count];
            for (int i = 0; i < count; ++i)
                elements[i] = this.V4();
            return elements;
        }

        /**
         * Reads a Matrix4x4 from the stream, compressed depending on flags.
         *
         * @return Matrix4x4 read from the stream
         */
        public Matrix4x4 M44()
        {
            float[] matrix = new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };

            int flags = 0xFFFF;
            if ((this.compressionFlags & CompressionFlags.USE_COMPRESSED_MATRICES) != 0)
                flags = this.I16();

            for (int i = 0; i < 16; ++i)
                if ((flags & (1 << i)) != 0)
                    matrix[i] = this.F32();

            Matrix4x4 mat = new Matrix4x4(matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5], matrix[6], matrix[7], matrix[8], matrix[9], matrix[10], matrix[11], matrix[12], matrix[13], matrix[14], matrix[15]);
            return mat;
        }

        /**
         * Reads a string of specified size from the stream.
         *
         * @param size Size of string to read
         * @return String value read from the stream
         */
        public string Str(int size)
        {
            if (size == 0) return "";
            return Encoding.ASCII.GetString(this.Bytes(size)).Replace("\0", "");
        }

        /**
         * Reads a wide string of specified size from the stream.
         *
         * @param size Size of string to read
         * @return String value read from the stream
         */
        public string Wstr(int size)
        {
            if (size == 0) return "";
            return Encoding.BigEndianUnicode.GetString(this.Bytes(size * 2)).Replace("\0", "");
        }

        /**
         * Reads a length-prefixed string from the stream.
         *
         * @return String value read from the stream
         */
        public String Str()
        {
            return this.Str(this.S32());
        }

        /**
         * Reads a length-prefixed wide string from the stream.
         *
         * @return String value read from the stream
         */
        public String Wstr()
        {
            return this.Wstr(this.S32());
        }

        /**
         * Reads a SHA1 hash from the stream.
         *
         * @return SHA1 hash read from the stream
         */
        public SHA1 Sha1()
        {
            return new SHA1(this.Bytes(0x14));
        }

        /**
         * Reads a GUID (uint32_t) from the stream.
         *
         * @return GUID read from the stream
         */
        public GUID? Guid()
        {
            return this.Guid(false);
        }

        /**
         * Reads a GUID (uint32_t) from the stream.
         *
         * @param force32 Whether or not to read as a 32 bit integer, regardless of compression flags.
         * @return GUID read from the stream
         */
        public GUID? Guid(bool force32)
        {
            long number = this.U32(force32);
            if (number == 0) return null;
            return new GUID(number);
        }

        /**
         * Reads an 8-bit integer from the stream and resolves the enum value.
         *
         * @param <T>         Type of enum
         * @param enumeration Enum class
         * @return Resolved enum constant
         */
        public T? Enum8<T>() where T : Enum
        {
            byte number = this.I8();
            foreach (T constant in Enum.GetValues(typeof(T)))
                if (constant.Equals(number))
                    return constant;
            return default;
        }

        /**
         * Reads an 32-bit integer from the stream and resolves the enum value.
         *
         * @param <T>         Type of enum
         * @param enumeration Enum class
         * @return Resolved enum constant
         */
        public T Enum32<T>() where T : Enum
        {
            return this.Enum32<T>(false);
        }

        /**
         * Reads an 32-bit integer from the stream and resolves the enum value.
         *
         * @param <T>         Type of enum
         * @param enumeration Enum class
         * @param signed      Whether or not to read a signed value
         * @return Resolved enum constant
         */
        public T Enum32<T>(bool signed)
        {
            int number = (signed) ? this.S32() : this.I32();
            T[] constants = (T[])Enum.GetValues(typeof(T));
            foreach (T constant in constants)
                if (constant.Equals(number))
                    return constant;
            return default;
        }

        /**
         * Reads a series of 8-bit integers from the stream and resolves them
         * as an enum array.
         *
         * @param <T>         Type of enum
         * @param enumeration Enum class
         * @return Resolved enum constant
         */
        public T[] Enumarray<T>() where T: Enum
        {
            int count = this.I32();
            T[] elements = new T[count];
            for (int i = 0; i < count; ++i)
                elements[i] = this.Enum8<T>();
            return elements;
        }

        /**
         * Seeks to position relative to seek mode.
         *
         * @param offset Offset relative to seek position
         * @param mode   Seek origin
         */
        public void Seek(int offset, SeekMode mode)
        {
            if (mode == null)
                throw new NullReferenceException("SeekMode cannot be null!");
            if (offset < 0) throw new ArgumentException("Can't seek to negative offsets.");
            switch (mode)
            {
                case SeekMode.Begin:
                    {
                        if (offset > this.length)
                            throw new ArgumentException("Can't seek past stream length.");
                        this.offset = offset;
                        break;
                    }
                case SeekMode.Relative:
                    {
                        int newOffset = this.offset + offset;
                        if (newOffset > this.length || newOffset < 0)
                            throw new ArgumentException("Can't seek outside bounds of " +
                                                               "stream.");
                        this.offset = newOffset;
                        break;
                    }
                case SeekMode.End:
                    {
                        if (offset < 0 || this.length - offset < 0)
                            throw new ArgumentException("Can't seek outside bounds of " +
                                                               "stream.");
                        this.offset = this.length - offset;
                        break;
                    }
            }
        }

        /**
         * Seeks ahead in stream relative to offset.
         *
         * @param offset Offset to go to
         */
        public void Seek(int offset)
        {
            this.Seek(offset, SeekMode.Relative);
        }

        public bool IsLittleEndian()
        {
            return this.isLittleEndian;
        }

        public byte[] GetBuffer()
        {
            return this.buffer;
        }

        public int GetOffset()
        {
            return this.offset;
        }

        public int GetLength()
        {
            return this.length;
        }

        public byte GetCompressionFlags()
        {
            return this.compressionFlags;
        }

        public void SetLittleEndian(bool value)
        {
            this.isLittleEndian = value;
        }
    }
}
