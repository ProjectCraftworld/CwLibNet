using CwLibNet4Hub.Enums;
using CwLibNet4Hub.Types.Data;
using System.Numerics;
using System.Text;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.IO.Streams;

public enum SeekMode
{
    Begin,
    Relative,
    End
}

public class MemoryInputStream
{
    private readonly byte[]? buffer;

    private int offset;
    private readonly int length;
    private byte compressionFlags;

    private bool isLittleEndian;

    /**
     * Creates a memory input stream from a byte array.
     *
     * @param buffer Byte array to use as source
     */
    public MemoryInputStream(byte[]? buffer)
    {
        this.buffer = buffer ?? throw new NullReferenceException("Buffer supplied to MemoryInputStream cannot be " +
                                                                 "null!");
        length = buffer.Length;
    }

    /**
     * Creates a memory input stream from byte array with compression flags.
     *
     * @param buffer           Byte array to use as source
     * @param compressionFlags Flags for compression methods used
     */
    public MemoryInputStream(byte[]? buffer, byte compressionFlags): this(buffer)
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
        var data = File.ReadAllBytes(path) ?? throw new ArgumentException("File provided could not be read!");
        buffer = data;
        length = data.Length;
    }

    /**
     * Creates a memory input stream from file at path with compression flags.
     *
     * @param path             Location to read data from
     * @param compressionFlags Flags for compression methods used
     */
    public MemoryInputStream(string path, byte compressionFlags): this(path)
    {
        this.compressionFlags = compressionFlags;
    }

    /**
     * Reads an arbitrary number of bytes from the stream.
     *
     * @param size Number of bytes to read from the stream
     * @return Bytes read from the stream
     */
    public byte[]? Bytes(int size)
    {
        if (size < 0)
            throw new ArgumentOutOfRangeException(nameof(size), $"Size cannot be negative (size={size}, offset={offset})");
        if (offset + size > buffer.Length)
            throw new ArgumentOutOfRangeException(nameof(size), $"Requested size exceeds buffer bounds (size={size}, offset={offset}, buffer.Length={buffer.Length})");
        
        offset += size;
        var secondBuffer = buffer[(offset - size)..offset]; // buffer.Skip(offset - size).ToArray();
//            Array.Resize(ref secondBuffer, size);
//             Array.Copy(this.buffer, 0, this.buffer, this.offset - size, this.offset);
        return secondBuffer;
    }

    /**
     * Reads a byte array from the stream.
     *
     * @return Bytes read from the stream
     */
    public byte[]? Bytearray()
    {
        var offsetBefore = offset;
        var size = I32();
        Console.WriteLine($"🔍 Bytearray() - Size read: {size}, Offset before I32: {offsetBefore}, Offset after I32: {offset}, Total length: {length}");
        if (size == 0) {
            Console.WriteLine($"🚨 Zero size detected! Dumping next 20 bytes from current position:");
            var remainingBytes = Math.Min(20, length - offset);
            for (int i = 0; i < remainingBytes; i++) {
                Console.Write($"{buffer[offset + i]:X2} ");
            }
            Console.WriteLine();
        }
        return Bytes(size);
    }

    /**
     * Reads a bool from the stream.
     *
     * @return bool read from the stream
     */
    public bool Boole()
    {
        return I8() != 0;
    }

    /**
     * Reads an array of bools from the stream.
     *
     * @return bool array read from the stream
     */
    public bool[] Boolarray()
    {
        var count = I32();
        var elements = new bool[count];
        for (var i = 0; i < count; ++i)
            elements[i] = Boole();
        return elements;
    }

    /**
     * Reads a byte from the stream.
     *
     * @return Byte read from the stream
     */
    public byte I8()
    {
        return buffer[offset++];
    }

    /**
     * Reads an unsigned byte from the stream as an integer.
     *
     * @return Byte read from the stream
     */
    public int U8()
    {
        return buffer[offset++] & 0xFF;
    }

    /**
     * Reads a short from the stream.
     *
     * @return Short read from the stream
     */
    public short I16()
    {
        var bytes = Bytes(2);
        if (isLittleEndian) return Util.Bytes.ToShortLE(bytes);
        return Util.Bytes.ToShortBE(bytes);
    }

    /**
     * Reads an unsigned short from the stream as an integer.
     *
     * @return Short read from the stream
     */
    public int U16()
    {
        return I16() & 0xFFFF;
    }

    /**
     * Reads an unsigned 24-bit integer from the stream.
     *
     * @return Integer read from the stream
     */
    public int U24()
    {
        var b = Bytes(3);
        if (isLittleEndian)
            return (b[2] & 0xFF) << 16 | (b[1] & 0xFF) << 8 | b[0] & 0xFF;
        return (b[0] & 0xFF) << 16 | (b[1] & 0xFF) << 8 | b[2] & 0xFF;
    }

    /**
     * Reads a 32-bit integer from the stream, compressed depending on flags.
     *
     * @param force32 Whether to read as a 32-bit integer, regardless of compression flags.
     * @return Integer read from the stream
     */
    public int I32(bool force32 = false)
    {
        if (!force32 && (compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0)
            return (int)(Uleb128() & 0xFFFFFFFF);
        var bytes = Bytes(4);
        return isLittleEndian ? Util.Bytes.ToIntegerLE(bytes) : Util.Bytes.ToIntegerBE(bytes);
    }
        
    public long I64(bool force64) {
        if (!force64 && (compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0)
            return Uleb128();
        var b = Bytes(8);
        if (isLittleEndian) {
            return	(b[7] & 0xFFL) << 56 |
                    (b[6] & 0xFFL) << 48 |
                    (b[5] & 0xFFL) << 40 |
                    (b[4] & 0xFFL) << 32 |
                    (b[3] & 0xFFL) << 24 |
                    (b[2] & 0xFFL) << 16 |
                    (b[1] & 0xFFL) << 8 |
                    (b[0] & 0xFFL) << 0;
        }
        return	(b[0] & 0xFFL) << 56 |
                (b[1] & 0xFFL) << 48 |
                (b[2] & 0xFFL) << 40 |
                (b[3] & 0xFFL) << 32 |
                (b[4] & 0xFFL) << 24 |
                (b[5] & 0xFFL) << 16 |
                (b[6] & 0xFFL) << 8 |
                (b[7] & 0xFFL) << 0;
    }

    /**
     * Reads a signed 32-bit integer from the stream, compressed depending on flags.
     * This function modifies the value written to the stream to fit an unsigned value, prefer i32
     *
     * @return Signed integer read from the stream
     */
    public int S32()
    {
        if ((compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) == 0)
            return I32(true);
        var v = (int)Uleb128();
        return v >> 1 ^ -(v & 1);
    }

    /**
     * Reads a long as an unsigned integer from the stream, compressed depending on flags.
     *
     * @param force64 Whether or not to read as a 32-bit integer, regardless of compression flags.
     * @return Unsigned integer read from the stream
     */
    public long U32(bool force32 = false)
    {
        if (force32 || (compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) == 0)
            return I32(true) & 0xFFFFFFFFL;
        return Uleb128();
    }

    /**
     * Reads a long from the stream, compressed depending on flags.
     *
     * @param force64 Whether to read as a 64-bit long, regardless of compression flags.
     * @return Long read from the stream
     */
    public long U64(bool force64 = false)
    {
        if (!force64 && (compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) != 0) return Uleb128();
        var b = Bytes(8);
        if (isLittleEndian)
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

    /**
     * Reads a "signed" long from the stream, compressed depending on flags.
     *
     * @param force64 Whether to read as a 64-bit long, regardless of compression flags.
     * @return Long read from the stream
     */
    public long S64(bool force64 = false)
    {
        if (force64 || (compressionFlags & CompressionFlags.USE_COMPRESSED_INTEGERS) == 0)
            return U64(true);
        var v = Uleb128();
        return v >> 1 ^ -(v & 1L);
    }

    /**
     * Reads a variable length quantity from the stream.
     *
     * @return Long value read from the stream
     */
    public long Uleb128()
    {
        long result = 0;
        var i = 0;
        while (true)
        {
            var b = U8() & 0xFFL;
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
        var count = I32();
        var elements = new short[count];
        for (var i = 0; i < count; ++i)
            elements[i] = I16();
        return elements;
    }

    /**
     * Reads a 32-bit integer array from the stream.
     *
     * @return Integer array read from the stream
     */
    public int[] Intarray()
    {
        var count = I32();
        var elements = new int[count];
        for (var i = 0; i < count; ++i)
            elements[i] = I32();
        return elements;
    }

    /**
     * Reads a 64-bit integer array from the stream.
     *
     * @return Long array read from the stream
     */
    public long[] Longarray()
    {
        var count = I32();
        var elements = new long[count];
        for (var i = 0; i < count; ++i)
            elements[i] = U64();
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
        var half = U16();
        var mant = half & 0x03ff;
        var exp = half & 0x7c00;
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
        return BitConverter.Int32BitsToSingle(I32(true));
    }

    /**
     * Reads a 32-bit floating point number array from the stream.
     *
     * @return Float array read from the stream
     */
    public float[] Floatarray()
    {
        var count = I32();
        var elements = new float[count];
        for (var i = 0; i < count; ++i)
            elements[i] = F32();
        return elements;
    }

    /**
     * Reads a 2-dimensional floating point vector from the stream.
     *
     * @return Vector2f read from the stream
     */
    public Vector2 V2()
    {
        return new Vector2(F32(), F32());
    }

    /**
     * Reads a 3-dimensional floating point vector from the stream.
     *
     * @return Vector3f read from the stream
     */
    public Vector3 V3()
    {
        return new Vector3(F32(), F32(), F32());
    }

    /**
     * Reads a 4-dimensional floating point vector from the stream.
     *
     * @return Vector4f read from the stream
     */
    public Vector4 V4()
    {
        return new Vector4(F32(), F32(), F32(), F32());
    }

    /**
     * Reads an array of 4-dimensional floating point vectors from the stream.
     *
     * @return Vector array read from the stream
     */
    public Vector4[] Vectorarray()
    {
        var count = I32();
        var elements = new Vector4[count];
        for (var i = 0; i < count; ++i)
            elements[i] = V4();
        return elements;
    }

    /**
     * Reads a Matrix4x4 from the stream, compressed depending on flags.
     *
     * @return Matrix4x4 read from the stream
     */
    public Matrix4x4 M44()
    {
        float[] matrix = [1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1];

        var flags = 0xFFFF;
        if ((compressionFlags & CompressionFlags.USE_COMPRESSED_MATRICES) != 0)
            flags = I16();

        for (var i = 0; i < 16; ++i)
            if ((flags & (1 << i)) != 0)
                matrix[i] = F32();

        var mat = new Matrix4x4(matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5], matrix[6], matrix[7], matrix[8], matrix[9], matrix[10], matrix[11], matrix[12], matrix[13], matrix[14], matrix[15]);
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
        return size == 0 ? "" : Encoding.ASCII.GetString(Bytes(size)).Replace("\0", "");
    }

    /**
     * Reads a wide string of specified size from the stream.
     *
     * @param size Size of string to read
     * @return String value read from the stream
     */
    public string Wstr(int size)
    {
        return size == 0 ? "" : Encoding.BigEndianUnicode.GetString(Bytes(size * 2)).Replace("\0", "");
    }

    /**
     * Reads a length-prefixed string from the stream.
     *
     * @return String value read from the stream
     */
    public string? Str()
    {
        return Str(S32());
    }

    /**
     * Reads a length-prefixed wide string from the stream.
     *
     * @return String value read from the stream
     */
    public string? Wstr()
    {
        return Wstr(S32());
    }

    /**
     * Reads a SHA1 hash from the stream.
     *
     * @return SHA1 hash read from the stream
     */
    public Sha1 Sha1()
    {
        return new Sha1(Bytes(0x14));
    }

    /**
     * Reads a GUID (uint32_t) from the stream.
     *
     * @param force32 Whether or not to read as a 32 bit integer, regardless of compression flags.
     * @return GUID read from the stream
     */
    public GUID? Guid(bool force32 = false)
    {
        var number = U32(force32);
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
        var number = I8();
        foreach (var constant in Enum.GetValues(typeof(T)))
            if ((int)constant == number)
                return (T)constant;
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
        return Enum32<T>(false);
    }

    /**
     * Reads an 32-bit integer from the stream and resolves the enum value.
     *
     * @param <T>         Type of enum
     * @param enumeration Enum class
     * @param signed      Whether to read a signed value
     * @return Resolved enum constant
     */
    public T Enum32<T>(bool signed)
    {
        var number = signed ? S32() : I32();
        var constants = Enum.GetValues(typeof(T));
        foreach (var constant in constants)
            if ((int)constant == number)
                return (T)constant;
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
        var count = I32();
        var elements = new T[count];
        for (var i = 0; i < count; ++i)
            elements[i] = Enum8<T>();
        return elements;
    }

    /**
     * Seeks to position relative to seek mode.
     *
     * @param offset Offset relative to seek position
     * @param mode   Seek origin
     */
    public void Seek(int offset, SeekMode mode = SeekMode.Relative)
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
                this.offset = offset;
                break;
            }
            case SeekMode.Relative:
            {
                var newOffset = this.offset + offset;
                if (newOffset > length || newOffset < 0)
                    throw new ArgumentException("Can't seek outside bounds of " +
                                                "stream.");
                this.offset = newOffset;
                break;
            }
            case SeekMode.End:
            {
                if (offset < 0 || length - offset < 0)
                    throw new ArgumentException("Can't seek outside bounds of " +
                                                "stream.");
                this.offset = length - offset;
                break;
            }
        }
    }

    public bool IsLittleEndian()
    {
        return isLittleEndian;
    }

    public byte[]? GetBuffer()
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

    public void SetLittleEndian(bool value)
    {
        isLittleEndian = value;
    }
}