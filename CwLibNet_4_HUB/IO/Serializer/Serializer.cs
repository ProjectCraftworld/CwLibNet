using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.Extensions;
using CwLibNet.IO.Streams;
using CwLibNet.Singleton;
using CwLibNet.Structs.Font;
using CwLibNet.Structs.Things;
using CwLibNet.Types.Data;

namespace CwLibNet.IO.Serializer;

/// <summary>
/// Reversible serializer for assets, also handles
/// references, revision, and dependencies.
/// </summary>
public class Serializer
{
    private readonly bool isWriting;
    private readonly MemoryInputStream? input;
    private readonly MemoryOutputStream? output;
    private readonly Revision revision;
    private readonly byte compressionFlags;
    private readonly Dictionary<int, object?> referenceIDs = new();
    private readonly Dictionary<object?, int?> referenceObjects = new();
    private readonly HashSet<ResourceDescriptor?> dependencies = [];
    private int nextReference = 1;
    /// <summary>
    /// Constructs a deserializer with stream and revision.
    /// </summary>
    /// <param name="stream">Input stream to use for serializer</param>
    /// <param name="revision">Revision of resource contained by stream</param>
    public Serializer(MemoryInputStream stream, Revision revision)
    {
        input = stream;
        output = null;
        this.revision = revision;
        compressionFlags = stream.GetCompressionFlags();
        isWriting = false;
    }

    /// <summary>
    /// Constructs a serializer with stream and revision.
    /// </summary>
    /// <param name="stream">Output stream to use for serializer</param>
    /// <param name="revision">Revision of resource to be serialized</param>
    public Serializer(MemoryOutputStream stream, Revision revision)
    {
        output = stream;
        input = null;
        this.revision = revision;
        compressionFlags = stream.GetCompressionFlags();
        isWriting = true;
    }

    /// <summary>
    /// Constructs a new serializer.
    /// </summary>
    /// <param name="size">Size of output stream</param>
    /// <param name="revision">Revision of resource to be serialized</param>
    /// <param name="compressionFlags">Compression flags to use during serialization</param>
    public Serializer(int size, Revision revision, byte compressionFlags)
    {
        output = new MemoryOutputStream(size, compressionFlags);
        input = null;
        this.revision = revision;
        this.compressionFlags = compressionFlags;
        isWriting = true;
    }

    /// <summary>
    /// Constructs a new deserializer.
    /// </summary>
    /// <param name="data">Buffer to use in deserializer</param>
    /// <param name="revision">Revision of resource contained by buffer</param>
    /// <param name="compressionFlags">Compression flags to use during deserialization</param>
    public Serializer(byte[]? data, Revision revision, byte compressionFlags)
    {
        input = new MemoryInputStream(data, compressionFlags);
        output = null;
        this.revision = revision;
        this.compressionFlags = compressionFlags;
        isWriting = false;
    }

    /// <summary>
    /// Pads a selected number of bytes in the stream.
    /// </summary>
    /// <param name="size">Number of bytes to pad</param>
    public void Pad(int size)
    {
        if (isWriting)
            output?.Pad(size);
        else
            input?.Bytes(size);
    }

    /// <summary>
    /// (De)serailizes a byte array to/from the stream.
    /// </summary>
    /// <param name="value">Bytes to write</param>
    /// <returns>Bytes serialized</returns>
    public byte[]? Bytearray(byte[]? value)
    {
        if (!isWriting) return input?.Bytearray();
        output?.Bytearray(value);
        return value;

    }

    /// <summary>
    /// (De)serializes bytes to/from the stream.
    /// </summary>
    /// <param name="value">Bytes to write</param>
    /// <param name="size">Number of bytes to read</param>
    /// <returns>Bytes serialized</returns>
    public byte[]? Bytes(byte[]? value, int size)
    {
        if (!isWriting) return input?.Bytes(size);
        output?.Bytes(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a boolean to/from the stream.
    /// </summary>
    /// <param name="value">Boolean to write</param>
    /// <returns>Boolean (de)serialized</returns>
    public bool Bool(bool value)
    {
        if (!isWriting) return input.Boole();
        output?.Boole(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a boolean array to/from the stream.
    /// </summary>
    /// <param name="values">Boolean array to write</param>
    /// <returns>Boolean array (de)serialized</returns>
    public bool[] Boolarray(bool[] values)
    {
        if (!isWriting) return input.Boolarray();
        output.Boolarray(values);
        return values;

    }

    /// <summary>
    /// (De)serializes a padded boolean to/from the stream.
    /// </summary>
    /// <param name="value">Boolean to write</param>
    /// <returns>Boolean (de)serialized</returns>
    public bool Intbool(bool value)
    {
        if (isWriting)
        {
            output.I32(value ? 1 : 0);
            return value;
        }

        return input.I32() != 0;
    }

    /// <summary>
    /// (De)serializes a byte to/from the stream.
    /// </summary>
    /// <param name="value">Byte to write</param>
    /// <returns>Byte (de)serialized</returns>
    public byte I8(byte value)
    {
        if (isWriting)
        {
            output.I8(value);
            return value;
        }

        return input.I8();
    }

    /// <summary>
    /// (De)serializes an integer as a byte to/from the stream.
    /// </summary>
    /// <param name="value">Byte to write</param>
    /// <returns>Byte (de)serialized</returns>
    public int U8(int value)
    {
        if (isWriting)
        {
            output.U8(value);
            return value;
        }

        return input.U8();
    }

    /// <summary>
    /// (De)serializes a short to/from the stream.
    /// </summary>
    /// <param name="value">Short to write</param>
    /// <returns>Short (de)serialized</returns>
    public short I16(short value)
    {
        if (isWriting)
        {
            output.I16(value);
            return value;
        }

        return input.I16();
    }
        
    /// <summary>
    /// (De)serializes a ushort to/from the stream.
    /// </summary>
    /// <param name="value">Ushort to write</param>
    /// <returns>Ushort (de)serialized</returns>
    public ushort I16(ushort value)
    {
        if (!isWriting) return (ushort)input.I16();
        output.I16(value);
        return value;

    }

    /// <summary>
    /// (De)serializes an integer to/from the stream as a short.
    /// </summary>
    /// <param name="value">Short to write</param>
    /// <returns>Short (de)serialized</returns>
    public int U16(int value)
    {
        if (!isWriting) return input.U16();
        output.U16(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a 24-bit unsigned integer to/from the stream.
    /// </summary>
    /// <param name="value">Integer to write</param>
    /// <returns>Integer (de)serialized</returns>
    public int U24(int value)
    {
        if (!isWriting) return input.U24();
        output?.U24(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a 32-bit integer to/from the stream, compressed depending on the flags.
    /// </summary>
    /// <param name="value">Integer to write</param>
    /// <param name="force32">Whether to always write 32 bits regardless of compression flags.</param>
    /// <returns>Integer (de)serialized</returns>
    public int I32(int value, bool force32 = false)
    {
        if (!isWriting) return input.I32(force32);
        output?.I32(value, force32);
        return value;

    }

    /// <summary>
    /// (De)serializes a signed 32-bit integer to/from the stream, compressed depending on the
    /// flags.
    /// This function modifies the value written to the stream to fit an unsigned value, prefer i32
    /// for normal values that are technically unsigned, but read as signed integers.
    /// </summary>
    /// <param name="value">Integer to write</param>
    /// <returns>Integer (de)serialized</returns>
    public int S32(int value)
    {
        if (!isWriting) return input.S32();
        output.S32(value);
        return value;

    }

    /// <summary>
    /// (De)serializes an unsigned 32-bit integer to/from the stream as a long, compressed
    /// depending on the flags.
    /// </summary>
    /// <param name="value">Integer to write</param>
    /// <param name="force32">Whether to always write 32 bits regardless of compression flags.</param>
    /// <returns>Integer (de)serialized</returns>
    public long U32(long value, bool force32 = false)
    {
        if (!isWriting) return input.U32(force32);
        output.U32(value, force32);
        return value;

    }

    /// <summary>
    /// (De)serializes a "signed" long to/from the stream, compressed depending on the flags.
    /// </summary>
    /// <param name="value">Long to write</param>
    /// <param name="force64">Whether to always write 64 bits regardless of compression flags.</param>
    /// <returns>Long (de)serialized</returns>
    public long S64(long value, bool force64 = false)
    {
        if (!isWriting) return input.S64(force64);
        output.S64(value, force64);
        return value;

    }

    /// <summary>
    /// (De)serializes a long to/from the stream, compressed depending on the flags.
    /// </summary>
    /// <param name="value">Long to write</param>
    /// <param name="force64">Whether to always write 64 bits regardless of compression flags.</param>
    /// <returns>Long (de)serialized</returns>
    public long U64(long value, bool force64 = false)
    {
        if (!isWriting) return input.U64(force64);
        output.U64(value, force64);
        return value;

    }

    /// <summary>
    /// (De)serializes a variable length quantity to/from the stream.
    /// </summary>
    /// <param name="value">Long to write</param>
    /// <returns>Long (de)serialized</returns>
    public long Uleb128(long value)
    {
        if (!isWriting) return input.Uleb128();
        output.Uleb128(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a 16-bit integer array to/from the stream.
    /// </summary>
    /// <param name="values">Short array to write</param>
    /// <returns>Short array (de)serialized</returns>
    public short[] Shortarray(short[] values)
    {
        if (isWriting)
        {
            output.Shortarray(values);
            return values;
        }

        return input.Shortarray();
    }

    /// <summary>
    /// (De)serializes a 32-bit integer array to/from the stream.
    /// </summary>
    /// <param name="values">Integer array to write</param>
    /// <returns>Integer array (de)serialized</returns>
    public int[]? Intarray(int[]? values)
    {
        if (!isWriting) return input.Intarray();
        output.Intarray(values);
        return values;

    }

    /// <summary>
    /// (De)serializes a 64-bit integer array to/from the stream.
    /// </summary>
    /// <param name="values">Long array to write</param>
    /// <returns>Long array (de)serialized</returns>
    public long[]? Longarray(long[]? values)
    {
        if (isWriting)
        {
            output.Longarray(values);
            return values;
        }

        return input.Longarray();
    }

    /// <summary>
    /// (De)serializes a 16-bit floating point number to/from the stream.
    /// </summary>
    /// <param name="value">Float to write</param>
    /// <returns>Float (de)serialized</returns>
    public float F16(float value)
    {
        if (!isWriting) return input.F16();
        output.F16(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a 32-bit floating point number to/from the stream.
    /// </summary>
    /// <param name="value">Float to write</param>
    /// <returns>Float (de)serialized</returns>
    public float F32(float value)
    {
        if (isWriting)
        {
            output.F32(value);
            return value;
        }

        return input.F32();
    }

    /// <summary>
    /// (De)serializes a 32-bit floating point number array to/from the stream.
    /// </summary>
    /// <param name="values">Float array to write</param>
    /// <returns>Float array (de)serialized</returns>
    public float[] Floatarray(float[] values)
    {
        if (!isWriting) return input.Floatarray();
        output.Floatarray(values);
        return values;

    }

    /// <summary>
    /// (De)serializes a 2-dimensional floating point vector to/from the stream.
    /// </summary>
    /// <param name="value">Vector2 to write</param>
    /// <returns>Vector2f (de)serialized</returns>
    public Vector2 V2(Vector2 value)
    {
        if (!isWriting) return input.V2();
        output.V2(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a 3-dimensional floating point vector to/from the stream.
    /// </summary>
    /// <param name="value">Vector3 to write</param>
    /// <returns>Vector3f (de)serialized</returns>
    public Vector3? V3(Vector3? value)
    {
        if (!isWriting) return input.V3();
        output.V3(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a 4-dimensional floating point vector to/from the stream.
    /// </summary>
    /// <param name="value">Vector4f to write</param>
    /// <returns>Vector4f (de)serialized</returns>
    public Vector4? V4(Vector4? value)
    {
        if (!isWriting) return input.V4();
        output.V4(value);
        return value;

    }

    /// <summary>
    /// (De)serializes an array of 4-dimensional floating point vectors to/from the stream.
    /// </summary>
    /// <param name="values">Vector array to write</param>
    /// <returns>Vector array (de)serialized</returns>
    public Vector4[]? Vectorarray(Vector4[]? values)
    {
        if (!isWriting) return input.Vectorarray();
        output.Vectorarray(values);
        return values;

    }

    /// <summary>
    /// (De)serializes a Matrix4x4 to/from the stream.
    /// </summary>
    /// <param name="value">Matrix4x4 to write</param>
    /// <returns>Matrix4x4 (de)serialized</returns>
    public Matrix4x4? M44(Matrix4x4? value)
    {
        if (!isWriting) return input.M44();
        output.M44(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a fixed length string to/from the stream.
    /// </summary>
    /// <param name="value">String to write</param>
    /// <param name="size">Fixed length of string to write</param>
    /// <returns>String (de)serialized</returns>
    public string? Str(string? value, int size)
    {
        if (!isWriting) return input.Str(size);
        output.Str(value, size);
        return value;

    }

    /// <summary>
    /// (De)serializes a string to/from the stream.
    /// </summary>
    /// <param name="value">String to write</param>
    /// <returns>String (de)serialized</returns>
    public string? Str(string? value)
    {
        if (!isWriting) return input.Str();
        output.Str(value, value.Length);
        return value;

    }

    /// <summary>
    /// (De)serializes a wide string to/from the stream.
    /// </summary>
    /// <param name="value">String to write</param>
    /// <returns>String (de)serialized</returns>
    public string? Wstr(string? value)
    {
        if (!isWriting) return input.Wstr();
        output.Wstr(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a SHA1 hash to/from the stream.
    /// </summary>
    /// <param name="value">SHA1 hash to write</param>
    /// <returns>SHA1 hash (de)serialized</returns>
    public Sha1? Sha1(Sha1? value)
    {
        if (!isWriting) return input.Sha1();
        output.Sha1(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a GUID to/from the stream.
    /// </summary>
    /// <param name="value">GUID to write</param>
    /// <param name="force32">Whether to force 32 bit, regardless of compression flags.</param>
    /// <returns>GUID (de)serialized</returns>
    public GUID? Guid(GUID? value, bool force32 = false)
    {
        if (!isWriting) return input.Guid(force32);
        output.Guid(value, force32);
        return value;

    }

    /// <summary>
    /// (De)serializes a Thing reference to/from the stream.
    /// </summary>
    /// <param name="thing">Thing to write</param>
    /// <returns>Thing (de)serialized</returns>
    public Thing? Thing(Thing? thing)
    {
        return Reference(thing);
    }

    /// <summary>
    /// (De)serializes an array of Thing references to/from the stream
    /// </summary>
    /// <param name="things">Things to write</param>
    /// <returns>Things (de)serialized</returns>
    public Thing[]? Thingarray(Thing[]? things)
    {
        return Array(things, true);
    }

    /// <summary>
    /// (De)serializes a list of Thing references to/from the stream
    /// </summary>
    /// <param name="things">Things to write</param>
    /// <returns>Things (de)serialized</returns>
    public List<Thing>? Thinglist(List<Thing>? things)
    {
        return Arraylist(things, true);
    }

    public int AdventureCreatureReference(int value)
    {
        var subVersion = revision.GetSubVersion();
        if (subVersion <= 0x12a)
        {
            ResourceDescriptor? descriptor = null;
            if (IsWriting())
            {
                if (value != 0)
                    descriptor = new ResourceDescriptor(new GUID(value), ResourceType.Plan);
            }

            descriptor = Resource(descriptor, ResourceType.Plan);
            if (!IsWriting())
            {
                if (descriptor != null && descriptor.IsGUID())
                    value = (int)descriptor.GetGUID()!.Value.Value;
            }
        }

        if (subVersion >= 0xc5)
            value = S32(value);
        return value;
    }

    /// <summary>
    /// (De)serializes a resource to/from the stream.
    /// </summary>
    /// <param name="value">Resource to write</param>
    /// <param name="type">Type of resource</param>
    /// <returns>Resource (de)serialized</returns>
    public ResourceDescriptor? Resource(ResourceDescriptor? value, ResourceType type)
    {
        return Resource(value, type, false, true, false);
    }

    /// <summary>
    /// (De)serializes a resource to/from the stream.
    /// </summary>
    /// <param name="value">Resource to write</param>
    /// <param name="type">Type of resource</param>
    /// <param name="isDescriptor">Whether to skip resource flags</param>
    /// <returns>Resource (de)serialized</returns>
    public ResourceDescriptor? Resource(ResourceDescriptor? value, ResourceType type, bool isDescriptor)
    {
        return Resource(value, type, isDescriptor, true, false);
    }

    /// <summary>
    /// (De)serializes a resource to/from the stream.
    /// </summary>
    /// <param name="value">Resource to write</param>
    /// <param name="type">Type of resource</param>
    /// <param name="isDescriptor">Whether to skip resource flags</param>
    /// <param name="cp">Flag toggle</param>
    /// <param name="t">Serialize resource type</param>
    /// <returns>Resource (de)serialized</returns>
    public ResourceDescriptor? Resource(ResourceDescriptor? value, ResourceType? type, bool isDescriptor, bool cp, bool t)
    {
        const byte NONE = 0;
        byte HASH = 1, GUID = 2;

        // is it 0x191 or 0x18c?
        if (revision.GetVersion() < 0x191 && cp)
        {
            HASH = 2;
            GUID = 1;
        }

        if (!isWriting)
        {
            var flags = 0;
            if (revision.GetVersion() > 0x22e && !isDescriptor)
                flags = input!.I32();
            var guidHashFlag = input!.I8();
            if (guidHashFlag == NONE)
                return null;
            GUID? guid = null;
            Sha1? sha1 = null;
            if ((guidHashFlag & GUID) != 0)
                guid = input.Guid();
            if ((guidHashFlag & HASH) != 0)
                sha1 = input.Sha1();
            if (t)
                type = ResourceType.FromType(input.I32());
            var descriptor = new ResourceDescriptor(guid!.Value, sha1!, type!.Value); // In this part of the code,
            // I can safely assume "type"
            // is not null.
            if (!descriptor.IsValid())
                return null;
            descriptor.SetFlags(flags);
            if (descriptor.IsHash() || !(isDescriptor && type.Equals(ResourceType.Plan)))
                dependencies.Add(descriptor);
            return descriptor;
        }

        if (revision.GetVersion() > 0x22e && !isDescriptor)
            output.U32(value?.GetFlags() ?? 0);
        if (value != null && value.IsValid())
        {
            byte flags = 0;
            if (value.IsHash())
                flags |= HASH;
            if (value.IsGUID())
                flags |= GUID;
            output.I8(flags);
            if ((flags & GUID) != 0)
                output.Guid(value.GetGUID());
            if ((flags & HASH) != 0)
                output.Sha1(value.GetSHA1());
            if (flags != 0 && !(isDescriptor && type.Equals(ResourceType.Plan)))
                dependencies.Add(value);
        }
        else
            I8(NONE);
        if (t)
            output.I32(value!.GetResourceType().Value);
        return value;
    }

    /// <summary>
    /// (De)serializes a vector (uint32_t array) to/from the stream, compressed depending on the
    /// flags.
    /// </summary>
    /// <param name="value">Vector to (de)serialize</param>
    /// <returns>(De)serialized vector</returns>
    public long[]? Longvector(long[]? value)
    {
        if ((compressionFlags & CompressionFlags.USE_COMPRESSED_VECTORS) == 0)
            return Longarray(value);
        if (isWriting)
        {
            if (value == null || value.Length == 0)
            {
                output.I32(0);
                return value;
            }

            long _bytes = 0;
            foreach (var element in value)
            {
                if (element.CompareUnsigned(_bytes) > 0)
                    _bytes = element;
            }

            if (_bytes == 0)
            {
                output.I32(0);
                return value;
            }

            _bytes = (64 - _bytes.NumberOfLeadingZeros() + 7) / 8;
            output.I32(value.Length);
            output.U8((int)(_bytes & 0xFF));
            for (var i = 0; i < _bytes; ++i)
                foreach (var t in value)
                    output.U8((int)((t >>> (i * 8)) & 0xFF));

            return value;
        }

        var count = input.I32();
        if (count == 0)
            return [];
        var bytes = input.U8();
        var vector = new long[count];
        for (var i = 0; i < bytes; ++i)
        for (var j = 0; j < count; ++j)
            vector[j] |= (long)input.U8() << (i * 8);
        return vector;
    }

    /// <summary>
    /// (De)serializes a vector (uint32_t array) to/from the stream, compressed depending on the
    /// flags.
    /// </summary>
    /// <param name="value">Vector to (de)serialize</param>
    /// <returns>(De)serialized vector</returns>
    public int[]? Intvector(int[]? value)
    {
        if ((compressionFlags & CompressionFlags.USE_COMPRESSED_VECTORS) == 0)
            return Intarray(value);
        if (isWriting)
        {
            if (value == null || value.Length == 0)
            {
                output.I32(0);
                return value;
            }


            // Storing in a separate variable because we don't
            // want to mutate the original array
            var _vector = value;

            var _bytes = _vector.Select((x) => x & 0xFFFFFFFF)?.Max() ?? 0L;
            if (_bytes == 0)
            {
                output.I32(_vector.Length);
                output.I32(0);
                return value;
            }

            _bytes = (64 - _bytes.NumberOfLeadingZeros() + 7) / 8;
            output.I32(_vector.Length);
            output.U8((int)(_bytes & 0xFF));
            for (var i = 0; i < _bytes; ++i)
                foreach (var t in _vector)
                    output.U8((t >>> (i * 8)) & 0xFF);

            return value;
        }

        var count = input.I32();
        if (count == 0)
            return [];
        var bytes = input.U8();
        var vector = new int[count];
        for (var i = 0; i < bytes; ++i)
        for (var j = 0; j < count; ++j)
            vector[j] |= input.U8() << (i * 8);

        return vector;
    }

    /// <summary>
    /// (De)serializes a 8-bit enum value to/from the stream.
    /// </summary>
    /// <param name="value">Enum value</param>
    /// <returns>(De)serialized enum value</returns>
    public T? Enum8<T>(T? value) where T: Enum
    {
        if (isWriting)
        {
            output.Enum8(value);
            return value;
        }

        return input.Enum8<T>();
    }

    /// <summary>
    /// (De)serializes a 32-bit enum value to/from the stream.
    /// </summary>
    /// <param name="value">Enum value</param>
    /// <returns>(De)serialized enum value</returns>
    public T Enum32<T>(T value) where T: Enum
    {
        if (!isWriting) return input.Enum32<T>();
        output.Enum32(value);
        return value;

    }

    /// <summary>
    /// (De)serializes a 32-bit enum value to/from the stream.
    /// </summary>
    /// <param name="value">Enum value</param>
    /// <param name="signed">Whether to (de)serialize s32</param>
    /// <returns>(De)serialized enum value</returns>
    public T Enum32<T>(T value, bool signed) where T: Enum
    {
        if (!isWriting) return input.Enum32<T>(signed);
        output.Enum32(value, signed);
        return value;

    }

    /// <summary>
    /// (De)serializes a 8-bit enum value to/from the stream.
    /// </summary>
    /// <param name="values">Enum values</param>
    /// <returns>(De)serialized enum value</returns>
    public T[] Enumarray<T>(T[] values) where T: Enum
    {
        if (!isWriting) return input.Enumarray<T>();
        output.Enumarray(values);
        return values;

    }

    /// <summary>
    /// (De)serializes a structure to/from the stream as a reference.
    /// </summary>
    /// <param name="value">Structure to serialize</param>
    /// <returns>(De)serialized structure</returns>
    public T? Reference<T>(T? value) where T: ISerializable
    {
        if (isWriting)
        {
            if (value == null)
            {
                output.I32(0);
                return default;
            }

            var r = referenceObjects.GetValueOrDefault(value, -1)!.Value;
            if (r == -1)
            {
                var next = nextReference++;
                output.I32(next);
                referenceIDs.Add(next, value);
                referenceObjects.Add(value, next);
                value.Serialize(this);
                return value;
            }
            else
                output.I32(r);
            return value;
        }

        var reference = input.I32();
        if (reference == 0)
            return default;
        if (referenceIDs.TryGetValue(reference, out var d))
            return (T?)d;
        T? strutt;
        try
        {
            strutt = (T?)Activator.CreateInstance(typeof(T));
        }
        catch (Exception)
        {
            throw new SerializationException("Failed to create class instance in serializer!");
        }

        referenceIDs.Add(reference, strutt);
        referenceObjects.Add(strutt, reference);
        strutt?.Serialize(this);
        return strutt;
    }

    /// <summary>
    /// (De)serializes a structure to/from the stream.
    /// </summary>
    /// <param name="value">Structure to serialize</param>
    /// <returns>(De)serialized structure</returns>
    public T Struct<T>(T? value) where T: ISerializable
    {
        if (!isWriting || value == null)
        {
            try
            {
                value = (T)Activator.CreateInstance(typeof(T));
            }
            catch (Exception)
            {
                throw new SerializationException("Failed to create class instance in serializer!");
            }
        }

        value.Serialize(this);
        return value;
    }

    /// <summary>
    /// (De)serializes an array to/from the stream.
    /// </summary>
    /// <param name="values">Array to serialize</param>
    /// <param name="clazz">Array base-serializable type</param>
    /// <returns>(De)serialized array</returns>
    public T[]? Array<T>(T[]? values) where T : ISerializable => Array(values, false);

    /// <summary>
    /// (De)serializes an arraylist to/from the stream.
    /// </summary>
    /// <param name="values">Array to serialize</param>
    /// <param name="isReference">Whether the array base structure is a reference type</param>
    /// <returns>(De)serialized array</returns>
    public List<T>? Arraylist<T>(List<T>? values, bool isReference = false) where T : ISerializable
    {
        var clazz = typeof(T);
        if (isWriting)
        {
            if (values == null)
            {
                this.output.I32(0);
                return null;
            }

            this.output.I32(values.Count);
            foreach (var serializable in values)
            {
                if (isReference)
                    Reference(serializable);
                else
                    Struct(serializable);
            }

            return values;
        }

        var count = input.I32();
        List<T> output = new(count);
        for (var i = 0; i < count; ++i)
        {
            output.Add(isReference ? Reference<T>(default) : Struct<T>(default));
        }

        return output;
    }

    /// <summary>
    /// (De)serializes an array to/from the stream.
    /// </summary>
    /// <param name="values">Array to serialize</param>
    /// <param name="T">Array base serializable type</param>
    /// <param name="isReference">Whether the array base structure is a reference type</param>
    /// <returns>(De)serialized array</returns>
    public T[]? Array<T>(T[]? values, bool isReference) where T:ISerializable
    {
        if (isWriting)
        {
            if (values == null)
            {
                output.I32(0);
                return null;
            }

            output.I32(values.Length);
            foreach (var serializable in values)
            {
                if (isReference)
                    Reference(serializable);
                else
                    Struct(serializable);
            }

            return values;
        }

        var count = input.I32();
        var serializables = new T[count];
        try
        {
            for (var i = 0; i < count; ++i)
            {
                if (isReference)
                    serializables[i] = Reference<T>(default);
                else
                    serializables[i] = Struct<T>(default);
            }
        }
        catch (Exception ex)
        {
            throw new SerializationException("There was an error (de)serializing an array!", ex);
        }

        return serializables;
    }

    public long I64(long value, bool force64 = false) {
        if (!isWriting) return input.I64(force64);
        output?.I64(value, force64);
        return value;
    }

    /// <summary>
    /// Shrinks the buffer to current offset and returns the buffer.
    /// </summary>
    /// <returns>The shrinked buffer</returns>
    public byte[]? GetBuffer()
    {
        return !isWriting ? null : output?.Shrink().GetBuffer();
    }

    public MemoryInputStream GetInput() => input;

    public MemoryOutputStream GetOutput() => output;

    public int GetOffset()
    {
        return isWriting ? output.GetOffset() : input.GetOffset();
    }

    public int GetLength()
    {
        return isWriting ? output.GetLength() : input.GetLength();
    }

    public virtual void Log(string message)
    {
        Log(message, ResourceLogLevel.SerializerTrace);
    }

    public virtual void Log(string message, int level)
    {
        if (level > ResourceSystem.LogLevel)
            return;
        if (isWriting)
        {
            Console.WriteLine("[WRITING] @ 0x" + Util.Bytes.ToHex(Util.Bytes.ToBytesBE(GetOffset())) + " -> " + message);
        }
        else
            Console.WriteLine("[READING] @ 0x" + Util.Bytes.ToHex(Util.Bytes.ToBytesBE(GetOffset())) + " -> " + message);
    }

    /// <summary>
    /// Forcibly adds a dependency to this serializer's collection,
    /// used for RPlan's because we can't serialize thing data yet.
    /// </summary>
    /// <param name="dependency">Dependency to add</param>
    public void AddDependency(ResourceDescriptor? dependency)
    {
        dependencies.Add(dependency);
    }

    /// <summary>
    /// Remove all dependencies in collection.
    /// </summary>
    public void ClearDependencies()
    {
        dependencies.Clear();
    }

    public virtual T GetPointer<T>(int index)
    {
        return (T)referenceIDs[index];
    }

    public virtual void SetPointer(int index, object? value)
    {
        referenceIDs.Add(index, value);
        referenceObjects.Add(value, index);
    }

    public virtual int GetNextReference()
    {
        return nextReference++;
    }

    public virtual Thing[] GetThings()
    {
        List<Thing> things = [];
        foreach (var reference in referenceObjects.Keys)
        {
            if (reference is Thing thing)
                things.Add(thing);
        }

        return [.. things];
    }

    public bool IsWriting()
    {
        return isWriting;
    }

    public Revision GetRevision()
    {
        return revision;
    }

    public byte GetCompressionFlags()
    {
        return compressionFlags;
    }

    public ResourceDescriptor?[] GetDependencies()
    {
        ResourceDescriptor?[] descriptors = [.. dependencies];
        return descriptors;
    }
}