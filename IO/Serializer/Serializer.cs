using Cwlib.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Streams;
using CwLibNet.IO.Singleton;
using CwLibNet.IO.Singleton.ResourceSystem;
using CwLibNet.Structs.Things;
using CwLibNet.IO.Types.Data;
using CwLibNet.Util;
using System;

namespace Cwlib.Io.Serializer
{
    /// <summary>
    /// Reversible serializer for assets, also handles
    /// references, revision, and dependencies.
    /// </summary>
    public class Serializer
    {
        private readonly bool isWriting;
        private readonly MemoryInputStream input;
        private readonly MemoryOutputStream output;
        private readonly Revision revision;
        private readonly byte compressionFlags;
        private readonly HashMap<int, object> referenceIDs = new HashMap();
        private readonly HashMap<object, int> referenceObjects = new HashMap();
        private readonly HashSet<ResourceDescriptor> dependencies = new HashSet();
        private int nextReference = 1;
        /// <summary>
        /// Constructs a deserializer with stream and revision.
        /// </summary>
        /// <param name="stream">Input stream to use for serializer</param>
        /// <param name="revision">Revision of resource contained by stream</param>
        public Serializer(MemoryInputStream stream, Revision revision)
        {
            this.input = stream;
            this.output = null;
            this.revision = revision;
            this.compressionFlags = stream.GetCompressionFlags();
            this.isWriting = false;
        }

        /// <summary>
        /// Constructs a serializer with stream and revision.
        /// </summary>
        /// <param name="stream">Output stream to use for serializer</param>
        /// <param name="revision">Revision of resource to be serialized</param>
        public Serializer(MemoryOutputStream stream, Revision revision)
        {
            this.output = stream;
            this.input = null;
            this.revision = revision;
            this.compressionFlags = stream.GetCompressionFlags();
            this.isWriting = true;
        }

        /// <summary>
        /// Constructs a new serializer.
        /// </summary>
        /// <param name="size">Size of output stream</param>
        /// <param name="revision">Revision of resource to be serialized</param>
        /// <param name="compressionFlags">Compression flags to use during serialization</param>
        public Serializer(int size, Revision revision, byte compressionFlags)
        {
            this.output = new MemoryOutputStream(size, compressionFlags);
            this.input = null;
            this.revision = revision;
            this.compressionFlags = compressionFlags;
            this.isWriting = true;
        }

        /// <summary>
        /// Constructs a new deserializer.
        /// </summary>
        /// <param name="data">Buffer to use in deserializer</param>
        /// <param name="revision">Revision of resource contained by buffer</param>
        /// <param name="compressionFlags">Compression flags to use during deserialization</param>
        public Serializer(byte[] data, Revision revision, byte compressionFlags)
        {
            this.input = new MemoryInputStream(data, compressionFlags);
            this.output = null;
            this.revision = revision;
            this.compressionFlags = compressionFlags;
            this.isWriting = false;
        }

        /// <summary>
        /// Pads a selected number of bytes in the stream.
        /// </summary>
        /// <param name="size">Number of bytes to pad</param>
        public void Pad(int size)
        {
            if (this.isWriting)
                this.output.Pad(size);
            else
                this.input.Bytes(size);
        }

        /// <summary>
        /// (De)serailizes a byte array to/from the stream.
        /// </summary>
        /// <param name="value">Bytes to write</param>
        /// <returns>Bytes serialized</returns>
        public byte[] Bytearray(byte[] value)
        {
            if (this.isWriting)
            {
                this.output.Bytearray(value);
                return value;
            }

            return this.input.Bytearray();
        }

        /// <summary>
        /// (De)serializes bytes to/from the stream.
        /// </summary>
        /// <param name="value">Bytes to write</param>
        /// <param name="size">Number of bytes to read</param>
        /// <returns>Bytes serialized</returns>
        public byte[] Bytes(byte[] value, int size)
        {
            if (this.isWriting)
            {
                this.output.Bytes(value);
                return value;
            }

            return this.input.Bytes(size);
        }

        /// <summary>
        /// (De)serializes a boolean to/from the stream.
        /// </summary>
        /// <param name="value">Boolean to write</param>
        /// <returns>Boolean (de)serialized</returns>
        public bool Bool(bool value)
        {
            if (this.isWriting)
            {
                this.output.Bool(value);
                return value;
            }

            return this.input.Bool();
        }

        /// <summary>
        /// (De)serializes a boolean array to/from the stream.
        /// </summary>
        /// <param name="values">Boolean array to write</param>
        /// <returns>Boolean array (de)serialized</returns>
        public boolean[] Boolarray(boolean[] values)
        {
            if (this.isWriting)
            {
                this.output.Boolarray(values);
                return values;
            }

            return this.input.Boolarray();
        }

        /// <summary>
        /// (De)serializes a padded boolean to/from the stream.
        /// </summary>
        /// <param name="value">Boolean to write</param>
        /// <returns>Boolean (de)serialized</returns>
        public bool Intbool(bool value)
        {
            if (this.isWriting)
            {
                this.output.i32(value ? 1 : 0);
                return value;
            }

            return this.input.i32() != 0;
        }

        /// <summary>
        /// (De)serializes a byte to/from the stream.
        /// </summary>
        /// <param name="value">Byte to write</param>
        /// <returns>Byte (de)serialized</returns>
        public byte I8(byte value)
        {
            if (this.isWriting)
            {
                this.output.I8(value);
                return value;
            }

            return this.input.I8();
        }

        /// <summary>
        /// (De)serializes an integer as a byte to/from the stream.
        /// </summary>
        /// <param name="value">Byte to write</param>
        /// <returns>Byte (de)serialized</returns>
        public int U8(int value)
        {
            if (this.isWriting)
            {
                this.output.U8(value);
                return value;
            }

            return this.input.U8();
        }

        /// <summary>
        /// (De)serializes a short to/from the stream.
        /// </summary>
        /// <param name="value">Short to write</param>
        /// <returns>Short (de)serialized</returns>
        public short I16(short value)
        {
            if (this.isWriting)
            {
                this.output.I16(value);
                return value;
            }

            return this.input.I16();
        }

        /// <summary>
        /// (De)serializes an integer to/from the stream as a short.
        /// </summary>
        /// <param name="value">Short to write</param>
        /// <returns>Short (de)serialized</returns>
        public int U16(int value)
        {
            if (this.isWriting)
            {
                this.output.U16(value);
                return value;
            }

            return this.input.U16();
        }

        /// <summary>
        /// (De)serializes a 24-bit unsigned integer to/from the stream.
        /// </summary>
        /// <param name="value">Integer to write</param>
        /// <returns>Integer (de)serialized</returns>
        public int U24(int value)
        {
            if (this.isWriting)
            {
                this.output.U24(value);
                return value;
            }

            return this.input.U24();
        }

        /// <summary>
        /// (De)serializes a 32-bit integer to/from the stream, compressed depending on the flags.
        /// </summary>
        /// <param name="value">Integer to write</param>
        /// <param name="force32">Whether or not to always write 32 bits regardless of compression flags.</param>
        /// <returns>Integer (de)serialized</returns>
        public int i32(int value, bool force32)
        {
            if (this.isWriting)
            {
                this.output.i32(value, force32);
                return value;
            }

            return this.input.i32(force32);
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
            if (this.isWriting)
            {
                this.output.S32(value);
                return value;
            }

            return this.input.S32();
        }

        /// <summary>
        /// (De)serializes an unsigned 32-bit integer to/from the stream as a long, compressed
        /// depending on the flags.
        /// </summary>
        /// <param name="value">Integer to write</param>
        /// <param name="force32">Whether or not to always write 32 bits regardless of compression flags.</param>
        /// <returns>Integer (de)serialized</returns>
        public long U32(long value, bool force32)
        {
            if (this.isWriting)
            {
                this.output.U32(value, force32);
                return value;
            }

            return this.input.U32(force32);
        }

        /// <summary>
        /// (De)serializes a 32-bit integer to/from the stream.
        /// </summary>
        /// <param name="value">Integer to write</param>
        /// <returns>Integer (de)serialized</returns>
        public int i32(int value)
        {
            return this.i32(value, false);
        }

        /// <summary>
        /// (De)serializes an unsigned 32-bit integer to/from the stream.
        /// </summary>
        /// <param name="value">Integer to write</param>
        /// <returns>Integer (de)serialized</returns>
        public long U32(long value)
        {
            return this.U32(value, false);
        }

        /// <summary>
        /// (De)serializes a long to/from the stream.
        /// </summary>
        /// <param name="value">Long to write</param>
        /// <returns>Long (de)serialized</returns>
        public long U64(long value)
        {
            return this.U64(value, false);
        }

        /// <summary>
        /// (De)serializes a "signed" long to/from the stream.
        /// </summary>
        /// <param name="value">Long to write</param>
        /// <returns>Long (de)serialized</returns>
        public long S64(long value)
        {
            return this.S64(value, false);
        }

        /// <summary>
        /// (De)serializes a "signed" long to/from the stream, compressed depending on the flags.
        /// </summary>
        /// <param name="value">Long to write</param>
        /// <param name="force64">Whether or not to always write 64 bits regardless of compression flags.</param>
        /// <returns>Long (de)serialized</returns>
        public long S64(long value, bool force64)
        {
            if (this.isWriting)
            {
                this.output.S64(value, force64);
                return value;
            }

            return this.input.S64(force64);
        }

        /// <summary>
        /// (De)serializes a long to/from the stream, compressed depending on the flags.
        /// </summary>
        /// <param name="value">Long to write</param>
        /// <param name="force64">Whether or not to always write 64 bits regardless of compression flags.</param>
        /// <returns>Long (de)serialized</returns>
        public long U64(long value, bool force64)
        {
            if (this.isWriting)
            {
                this.output.U64(value, force64);
                return value;
            }

            return this.input.U64(force64);
        }

        /// <summary>
        /// (De)serializes a variable length quantity to/from the stream.
        /// </summary>
        /// <param name="value">Long to write</param>
        /// <returns>Long (de)serialized</returns>
        public long Uleb128(long value)
        {
            if (this.isWriting)
            {
                this.output.Uleb128(value);
                return value;
            }

            return this.input.Uleb128();
        }

        /// <summary>
        /// (De)serializes a 16-bit integer array to/from the stream.
        /// </summary>
        /// <param name="values">Short array to write</param>
        /// <returns>Short array (de)serialized</returns>
        public short[] Shortarray(short[] values)
        {
            if (this.isWriting)
            {
                this.output.Shortarray(values);
                return values;
            }

            return this.input.Shortarray();
        }

        /// <summary>
        /// (De)serializes a 32-bit integer array to/from the stream.
        /// </summary>
        /// <param name="values">Integer array to write</param>
        /// <param name="signed">Whether integer values are signed</param>
        /// <returns>Integer array (de)serialized</returns>
        public int[] Intarray(int[] values, bool signed)
        {
            if (this.isWriting)
            {
                this.output.Intarray(values, signed);
                return values;
            }

            return this.input.Intarray(signed);
        }

        /// <summary>
        /// (De)serializes a 32-bit integer array to/from the stream.
        /// </summary>
        /// <param name="values">Integer array to write</param>
        /// <returns>Integer array (de)serialized</returns>
        public int[] Intarray(int[] values)
        {
            return Intarray(values, false);
        }

        /// <summary>
        /// (De)serializes a 64-bit integer array to/from the stream.
        /// </summary>
        /// <param name="values">Long array to write</param>
        /// <returns>Long array (de)serialized</returns>
        public long[] Longarray(long[] values)
        {
            if (this.isWriting)
            {
                this.output.Longarray(values);
                return values;
            }

            return this.input.Longarray();
        }

        /// <summary>
        /// (De)serializes a 16-bit floating point number to/from the stream.
        /// </summary>
        /// <param name="value">Float to write</param>
        /// <returns>Float (de)serialized</returns>
        public float F16(float value)
        {
            if (this.isWriting)
            {
                this.output.F16(value);
                return value;
            }

            return this.input.F16();
        }

        /// <summary>
        /// (De)serializes a 32-bit floating point number to/from the stream.
        /// </summary>
        /// <param name="value">Float to write</param>
        /// <returns>Float (de)serialized</returns>
        public float F32(float value)
        {
            if (this.isWriting)
            {
                this.output.F32(value);
                return value;
            }

            return this.input.F32();
        }

        /// <summary>
        /// (De)serializes a 32-bit floating point number array to/from the stream.
        /// </summary>
        /// <param name="values">Float array to write</param>
        /// <returns>Float array (de)serialized</returns>
        public float[] Floatarray(float[] values)
        {
            if (this.isWriting)
            {
                this.output.Floatarray(values);
                return values;
            }

            return this.input.Floatarray();
        }

        /// <summary>
        /// (De)serializes a 2-dimensional floating point vector to/from the stream.
        /// </summary>
        /// <param name="value">Vector2f to write</param>
        /// <returns>Vector2f (de)serialized</returns>
        public Vector2f V2(Vector2f value)
        {
            if (this.isWriting)
            {
                this.output.V2(value);
                return value;
            }

            return this.input.V2();
        }

        /// <summary>
        /// (De)serializes a 3-dimensional floating point vector to/from the stream.
        /// </summary>
        /// <param name="value">Vector3f to write</param>
        /// <returns>Vector3f (de)serialized</returns>
        public Vector3f V3(Vector3f value)
        {
            if (this.isWriting)
            {
                this.output.V3(value);
                return value;
            }

            return this.input.V3();
        }

        /// <summary>
        /// (De)serializes a 4-dimensional floating point vector to/from the stream.
        /// </summary>
        /// <param name="value">Vector4f to write</param>
        /// <returns>Vector4f (de)serialized</returns>
        public Vector4f V4(Vector4f value)
        {
            if (this.isWriting)
            {
                this.output.V4(value);
                return value;
            }

            return this.input.V4();
        }

        /// <summary>
        /// (De)serializes an array of 4-dimensional floating point vectors to/from the stream.
        /// </summary>
        /// <param name="values">Vector array to write</param>
        /// <returns>Vector array (de)serialized</returns>
        public Vector4f[] Vectorarray(Vector4f[] values)
        {
            if (this.isWriting)
            {
                this.output.Vectorarray(values);
                return values;
            }

            return this.input.Vectorarray();
        }

        /// <summary>
        /// (De)serializes a Matrix4x4 to/from the stream.
        /// </summary>
        /// <param name="value">Matrix4x4 to write</param>
        /// <returns>Matrix4x4 (de)serialized</returns>
        public Matrix4f M44(Matrix4f value)
        {
            if (this.isWriting)
            {
                this.output.M44(value);
                return value;
            }

            return this.input.M44();
        }

        /// <summary>
        /// (De)serializes a fixed length string to/from the stream.
        /// </summary>
        /// <param name="value">String to write</param>
        /// <param name="size">Fixed length of string to write</param>
        /// <returns>String (de)serialized</returns>
        public string Str(string value, int size)
        {
            if (this.isWriting)
            {
                this.output.Str(value, size);
                return value;
            }

            return this.input.Str(size);
        }

        /// <summary>
        /// (De)serializes a string to/from the stream.
        /// </summary>
        /// <param name="value">String to write</param>
        /// <returns>String (de)serialized</returns>
        public string Str(string value)
        {
            if (this.isWriting)
            {
                this.output.Str(value);
                return value;
            }

            return this.input.Str();
        }

        /// <summary>
        /// (De)serializes a wide string to/from the stream.
        /// </summary>
        /// <param name="value">String to write</param>
        /// <returns>String (de)serialized</returns>
        public string Wstr(string value)
        {
            if (this.isWriting)
            {
                this.output.Wstr(value);
                return value;
            }

            return this.input.Wstr();
        }

        /// <summary>
        /// (De)serializes a SHA1 hash to/from the stream.
        /// </summary>
        /// <param name="value">SHA1 hash to write</param>
        /// <returns>SHA1 hash (de)serialized</returns>
        public SHA1 Sha1(SHA1 value)
        {
            if (this.isWriting)
            {
                this.output.Sha1(value);
                return value;
            }

            return this.input.Sha1();
        }

        /// <summary>
        /// (De)serializes a GUID to/from the stream.
        /// </summary>
        /// <param name="value">GUID to write</param>
        /// <param name="force32">Whether or not to force 32 bit, regardless of compression flags.</param>
        /// <returns>GUID (de)serialized</returns>
        public GUID Guid(GUID value, bool force32)
        {
            if (this.isWriting)
            {
                this.output.Guid(value, force32);
                return value;
            }

            return this.input.Guid(force32);
        }

        /// <summary>
        /// (De)serializes a GUID to/from the stream.
        /// </summary>
        /// <param name="value">GUID to write</param>
        /// <returns>GUID (de)serialized</returns>
        public GUID Guid(GUID value)
        {
            return this.Guid(value, false);
        }

        /// <summary>
        /// (De)serializes a Thing reference to/from the stream.
        /// </summary>
        /// <param name="thing">Thing to write</param>
        /// <returns>Thing (de)serialized</returns>
        public Thing Thing(Thing thing)
        {
            return this.Reference(thing, typeof(Thing));
        }

        /// <summary>
        /// (De)serializes an array of Thing references to/from the stream
        /// </summary>
        /// <param name="things">Things to write</param>
        /// <returns>Things (de)serialized</returns>
        public Thing[] Thingarray(Thing[] things)
        {
            return this.Array(things, typeof(Thing), true);
        }

        /// <summary>
        /// (De)serializes a list of Thing references to/from the stream
        /// </summary>
        /// <param name="things">Things to write</param>
        /// <returns>Things (de)serialized</returns>
        public List<Thing> Thinglist(List<Thing> things)
        {
            return this.Arraylist(things, typeof(Thing), true);
        }

        public int AdventureCreatureReference(int value)
        {
            int subVersion = revision.GetSubVersion();
            if (subVersion <= 0x12a)
            {
                ResourceDescriptor descriptor = null;
                if (IsWriting())
                {
                    if (value != 0)
                        descriptor = new ResourceDescriptor(value, ResourceType.PLAN);
                }

                descriptor = Resource(descriptor, ResourceType.PLAN);
                if (!IsWriting())
                {
                    if (descriptor != null && descriptor.IsGUID())
                        value = (int)descriptor.GetGUID().GetValue();
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
        public ResourceDescriptor Resource(ResourceDescriptor value, ResourceType type)
        {
            return this.Resource(value, type, false, true, false);
        }

        /// <summary>
        /// (De)serializes a resource to/from the stream.
        /// </summary>
        /// <param name="value">Resource to write</param>
        /// <param name="type">Type of resource</param>
        /// <param name="isDescriptor">Whether or not to skip resource flags</param>
        /// <returns>Resource (de)serialized</returns>
        public ResourceDescriptor Resource(ResourceDescriptor value, ResourceType type, bool isDescriptor)
        {
            return this.Resource(value, type, isDescriptor, true, false);
        }

        /// <summary>
        /// (De)serializes a resource to/from the stream.
        /// </summary>
        /// <param name="value">Resource to write</param>
        /// <param name="type">Type of resource</param>
        /// <param name="isDescriptor">Whether or not to skip resource flags</param>
        /// <param name="cp">Flag toggle</param>
        /// <param name="t">Serialize resource type</param>
        /// <returns>Resource (de)serialized</returns>
        public ResourceDescriptor Resource(ResourceDescriptor value, ResourceType type, bool isDescriptor, bool cp, bool t)
        {
            byte NONE = 0, HASH = 1, GUID = 2;

            // is it 0x191 or 0x18c
            if (this.revision.GetVersion() < 0x191 && cp)
            {
                HASH = 2;
                GUID = 1;
            }

            if (!this.isWriting)
            {
                int flags = 0;
                if (this.revision.GetVersion() > 0x22e && !isDescriptor)
                    flags = this.input.i32();
                byte guidHashFlag = this.input.I8();
                ResourceDescriptor descriptor = null;
                if (guidHashFlag == NONE)
                    return null;
                GUID guid = null;
                SHA1 sha1 = null;
                if ((guidHashFlag & GUID) != 0)
                    guid = this.input.Guid();
                if ((guidHashFlag & HASH) != 0)
                    sha1 = this.input.Sha1();
                if (t)
                    type = ResourceType.FromType(this.input.i32());
                descriptor = new ResourceDescriptor(guid, sha1, type);
                if (!descriptor.IsValid())
                    return null;
                descriptor.SetFlags(flags);
                if (descriptor.IsHash() || (!(isDescriptor && type == ResourceType.PLAN)))
                    this.dependencies.Add(descriptor);
                return descriptor;
            }

            if (this.revision.GetVersion() > 0x22e && !isDescriptor)
                this.output.U32(value != null ? value.GetFlags() : 0);
            if (value != null && value.IsValid())
            {
                byte flags = 0;
                if (value.IsHash())
                    flags |= HASH;
                if (value.IsGUID())
                    flags |= GUID;
                this.output.I8(flags);
                if ((flags & GUID) != 0)
                    this.output.Guid(value.GetGUID());
                if ((flags & HASH) != 0)
                    this.output.Sha1(value.GetSHA1());
                if (flags != 0 && !(isDescriptor && type == ResourceType.PLAN))
                    this.dependencies.Add(value);
            }
            else
                this.I8(NONE);
            if (t)
                this.output.i32(value != null ? value.GetType().GetValue() : 0);
            return value;
        }

        /// <summary>
        /// (De)serializes a vector (uint32_t array) to/from the stream, compressed depending on the
        /// flags.
        /// </summary>
        /// <param name="value">Vector to (de)serialize</param>
        /// <returns>(De)serialized vector</returns>
        public long[] Longvector(long[] value)
        {
            if ((this.compressionFlags & CompressionFlags.USE_COMPRESSED_VECTORS) == 0)
                return this.Longarray(value);
            if (this.isWriting)
            {
                if (value == null || value.length == 0)
                {
                    this.output.i32(0);
                    return value;
                }

                long bytes = 0;
                foreach (long element in value)
                {
                    if (Long.CompareUnsigned(element, bytes) > 0)
                        bytes = element;
                }

                if (bytes == 0)
                {
                    this.output.i32(0);
                    return value;
                }

                bytes = ((64 - Long.NumberOfLeadingZeros(bytes)) + 7) / 8;
                this.output.i32(value.length);
                this.output.U8((int)(bytes & 0xFF));
                for (int i = 0; i < bytes; ++i)
                    for (int j = 0; j < value.length; ++j)
                        this.output.U8((int)(((value[j] >>> (i * 8)) & 0xFF)));
                return value;
            }

            int count = this.input.i32();
            if (count == 0)
                return new long[0];
            int bytes = this.input.U8();
            long[] vector = new long[count];
            for (int i = 0; i < bytes; ++i)
                for (int j = 0; j < count; ++j)
                    vector[j] |= ((long)this.input.U8()) << (i * 8);
            return vector;
        }

        /// <summary>
        /// (De)serializes a vector (uint32_t array) to/from the stream, compressed depending on the
        /// flags
        /// </summary>
        /// <param name="value">Vector to (de)serialize</param>
        /// <returns>(De)serialized vector</returns>
        public int[] Intvector(int[] value)
        {
            return Intvector(value, false);
        }

        /// <summary>
        /// (De)serializes a vector (uint32_t array) to/from the stream, compressed depending on the
        /// flags.
        /// </summary>
        /// <param name="value">Vector to (de)serialize</param>
        /// <param name="signed">Whether integer values are signed</param>
        /// <returns>(De)serialized vector</returns>
        public int[] Intvector(int[] value, bool signed)
        {
            if ((this.compressionFlags & CompressionFlags.USE_COMPRESSED_VECTORS) == 0)
                return this.Intarray(value, signed);
            if (this.isWriting)
            {
                if (value == null || value.length == 0)
                {
                    this.output.i32(0);
                    return value;
                }


                // Storing in a separate variable because we don't
                // want to mutate the original array
                int[] vector = value;
                if (signed)
                {
                    vector = new int[value.length];
                    for (int i = 0; i < vector.length; ++i)
                        vector[i] = ((value[i] & 0x7fffffff)) << 1 ^ ((value[i] >> 0x1f));
                }

                long bytes = Arrays.Stream(vector).MapToLong((x) => x & 0xFFFFFFFF).Max().OrElse(0);
                if (bytes == 0)
                {
                    this.output.i32(vector.length);
                    this.output.i32(0);
                    return value;
                }

                bytes = ((64 - Long.NumberOfLeadingZeros(bytes)) + 7) / 8;
                this.output.i32(vector.length);
                this.output.U8((int)(bytes & 0xFF));
                for (int i = 0; i < bytes; ++i)
                    for (int j = 0; j < vector.length; ++j)
                        this.output.U8((vector[j] >>> (i * 8)) & 0xFF);
                return value;
            }

            int count = this.input.i32();
            if (count == 0)
                return new int[0];
            int bytes = this.input.U8();
            int[] vector = new int[count];
            for (int i = 0; i < bytes; ++i)
                for (int j = 0; j < count; ++j)
                    vector[j] |= (this.input.U8() << (i * 8));
            if (signed)
            {
                for (int i = 0; i < vector.length; ++i)
                    vector[i] = (vector[i] >> 1 ^ -(vector[i] & 1));
            }

            return vector;
        }

        /// <summary>
        /// (De)serializes a 8-bit enum value to/from the stream.
        /// </summary>
        /// <param name="<T>">Enum class</param>
        /// <param name="value">Enum value</param>
        /// <returns>(De)serialized enum value</returns>
        public T Enum8<T extends Enum<T> & ValueEnum<Byte>>(T value)
        {
            if (this.isWriting)
            {
                this.output.Enum8(value);
                return value;
            }

            return this.input.Enum8((Class<T>)value.GetType());
        }

        /// <summary>
        /// (De)serializes a 32-bit enum value to/from the stream.
        /// </summary>
        /// <param name="<T>">Enum class</param>
        /// <param name="value">Enum value</param>
        /// <returns>(De)serialized enum value</returns>
        public T Enum32<T extends Enum<T> & ValueEnum<Integer>>(T value)
        {
            if (this.isWriting)
            {
                this.output.Enum32(value);
                return value;
            }

            return this.input.Enum32((Class<T>)value.GetType());
        }

        /// <summary>
        /// (De)serializes a 32-bit enum value to/from the stream.
        /// </summary>
        /// <param name="<T>">Enum class</param>
        /// <param name="value">Enum value</param>
        /// <param name="signed">Whether or not to (de)serialize s32</param>
        /// <returns>(De)serialized enum value</returns>
        public T Enum32<T extends Enum<T> & ValueEnum<Integer>>(T value, bool signed)
        {
            if (this.isWriting)
            {
                this.output.Enum32(value, signed);
                return value;
            }

            return this.input.Enum32((Class<T>)value.GetType(), signed);
        }

        /// <summary>
        /// (De)serializes a 8-bit enum value to/from the stream.
        /// </summary>
        /// <param name="<T>">Enum class</param>
        /// <param name="values">Enum values</param>
        /// <param name=""></param>
        /// <returns>(De)serialized enum value</returns>
        public T[] Enumarray<T extends Enum<T> & ValueEnum<Byte>>(T[] values, Class<T> enumeration)
        {
            if (this.isWriting)
            {
                this.output.Enumarray(values);
                return values;
            }

            return this.input.Enumarray(enumeration);
        }

        /// <summary>
        /// (De)serializes a structure to/from the stream as a reference.
        /// </summary>
        /// <param name="<T>">Generic serializable structure</param>
        /// <param name="value">Structure to serialize</param>
        /// <param name="clazz">Serializable class type</param>
        /// <returns>(De)serialized structure</returns>
        public T Reference<T extends Serializable>(T value, Class<T> clazz)
        {
            if (this.isWriting)
            {
                if (value == null)
                {
                    this.output.i32(0);
                    return null;
                }

                int reference = this.referenceObjects.GetOrDefault(value, -1);
                if (reference == -1)
                {
                    int next = this.nextReference++;
                    this.output.i32(next);
                    this.referenceIDs.Put(next, value);
                    this.referenceObjects.Put(value, next);
                    value.Serialize(this);
                    return value;
                }
                else
                    this.output.i32(reference);
                return value;
            }

            int reference = this.input.i32();
            if (reference == 0)
                return null;
            if (this.referenceIDs.ContainsKey(reference))
                return (T)this.referenceIDs[reference];
            T struct = null;
            try
            {
                struct = clazz.GetDeclaredConstructor().NewInstance();
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to create class instance in " + "serializer!");
            }

            this.referenceIDs.Put(reference, struct);
            this.referenceObjects.Put(struct, reference);
            struct.Serialize(this);
            return struct;
        }

        /// <summary>
        /// (De)serializes a structure to/from the stream.
        /// </summary>
        /// <param name="<T>">Generic serializable structure</param>
        /// <param name="value">Structure to serialize</param>
        /// <param name="clazz">Serializable class type</param>
        /// <returns>(De)serialized structure</returns>
        public T Struct<T extends Serializable>(T value, Class<T> clazz)
        {
            if (!this.isWriting || value == null)
            {
                try
                {
                    value = clazz.GetDeclaredConstructor().NewInstance();
                }
                catch (Exception ex)
                {
                    ex.PrintStackTrace();
                    throw new SerializationException("Failed to create class instance in " + "serializer!");
                }
            }

            value.Serialize(this);
            return value;
        }

        /// <summary>
        /// (De)serializes an array to/from the stream.
        /// </summary>
        /// <param name="<T>">Generic serializable structure</param>
        /// <param name="values">Array to serialize</param>
        /// <param name="clazz">Array base serializable type</param>
        /// <returns>(De)serialized array</returns>
        public T[] Array<T extends Serializable>(T[] values, Class<T> clazz)
        {
            return this.Array(values, clazz, false);
        }

        /// <summary>
        /// (De)serializes an array to/from the stream.
        /// </summary>
        /// <param name="<T>">Generic serializable structure</param>
        /// <param name="values">Array to serialize</param>
        /// <param name="clazz">Array base serializable type</param>
        /// <returns>(De)serialized array</returns>
        public List<T> Arraylist<T extends Serializable>(List<T> values, Class<T> clazz)
        {
            return this.Arraylist(values, clazz, false);
        }

        /// <summary>
        /// (De)serializes an arraylist to/from the stream.
        /// </summary>
        /// <param name="<T>">Generic serializable structure</param>
        /// <param name="values">Array to serialize</param>
        /// <param name="clazz">Array base serializable type</param>
        /// <param name="isReference">Whether the array base structure is a reference type</param>
        /// <returns>(De)serialized array</returns>
        public List<T> Arraylist<T extends Serializable>(List<T> values, Class<T> clazz, bool isReference)
        {
            if (this.isWriting)
            {
                if (values == null)
                {
                    this.output.i32(0);
                    return null;
                }

                this.output.i32(values.Count);
                foreach (T serializable in values)
                {
                    if (isReference)
                        this.Reference(serializable, clazz);
                    else
                        this.Struct(serializable, clazz);
                }

                return values;
            }

            int count = this.input.i32();
            List<T> output = new List<T>(count);
            for (int i = 0; i < count; ++i)
            {
                if (isReference)
                    output.Add(clazz.Cast(this.Reference(null, clazz)));
                else
                    output.Add(this.Struct(null, clazz));
            }

            return output;
        }

        /// <summary>
        /// (De)serializes an array to/from the stream.
        /// </summary>
        /// <param name="<T>">Generic serializable structure</param>
        /// <param name="values">Array to serialize</param>
        /// <param name="clazz">Array base serializable type</param>
        /// <param name="isReference">Whether the array base structure is a reference type</param>
        /// <returns>(De)serialized array</returns>
        public T[] Array<T extends Serializable>(T[] values, Class<T> clazz, bool isReference)
        {
            if (this.isWriting)
            {
                if (values == null)
                {
                    this.output.i32(0);
                    return null;
                }

                this.output.i32(values.length);
                foreach (T serializable in values)
                {
                    if (isReference)
                        this.Reference(serializable, clazz);
                    else
                        this.Struct(serializable, clazz);
                }

                return values;
            }

            int count = this.input.i32();
            T[] output = (T[])Array.NewInstance(clazz, count);
            try
            {
                for (int i = 0; i < count; ++i)
                {
                    if (isReference)
                        output[i] = this.Reference(null, clazz);
                    else
                        output[i] = this.Struct(null, clazz);
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("There was an error (de)serializing an array!");
            }

            return output;
        }

        /// <summary>
        /// Shrinks the buffer to current offset and returns the buffer.
        /// </summary>
        /// <returns>The shrinked buffer</returns>
        public byte[] GetBuffer()
        {
            if (!this.isWriting)
                return null;
            return this.output.Shrink().GetBuffer();
        }

        public MemoryInputStream GetInput()
        {
            return this.input;
        }

        public MemoryOutputStream GetOutput()
        {
            return this.output;
        }

        public int GetOffset()
        {
            if (this.isWriting)
                return this.output.GetOffset();
            return this.input.GetOffset();
        }

        public int GetLength()
        {
            if (this.isWriting)
                return this.output.GetLength();
            return this.input.GetLength();
        }

        public virtual void Log(string message)
        {
            this.Log(message, ResourceLogLevel.SERIALIZER_TRACE);
        }

        public virtual void Log(string message, int level)
        {
            if (level > ResourceSystem.LOG_LEVEL)
                return;
            if (this.isWriting)
            {
                System.@out.Println("[WRITING] @ 0x" + Bytes.ToHex(Bytes.ToBytesBE(this.GetOffset())) + " -> " + message);
            }
            else
                System.@out.Println("[READING] @ 0x" + Bytes.ToHex(Bytes.ToBytesBE(this.GetOffset())) + " -> " + message);
        }

        /// <summary>
        /// Forcibly adds a dependency to this serializer's collection,
        /// used for RPlan's because we can't serialize thing data yet.
        /// </summary>
        /// <param name="dependency">Dependency to add</param>
        public void AddDependency(ResourceDescriptor dependency)
        {
            this.dependencies.Add(dependency);
        }

        /// <summary>
        /// Remove all dependencies in collection.
        /// </summary>
        public void ClearDependencies()
        {
            this.dependencies.Clear();
        }

        public virtual T GetPointer<T>(int index)
        {
            return (T)this.referenceIDs[index];
        }

        public virtual void SetPointer(int index, object value)
        {
            this.referenceIDs.Put(index, value);
            this.referenceObjects.Put(value, index);
        }

        public virtual int GetNextReference()
        {
            return this.nextReference++;
        }

        public virtual Thing[] GetThings()
        {
            List<Thing> things = new List();
            foreach (object reference in this.referenceObjects.KeySet())
            {
                if (reference is Thing)
                    things.Add((Thing)reference);
            }

            return things.ToArray(Thing[].New());
        }

        public bool IsWriting()
        {
            return this.isWriting;
        }

        public Revision GetRevision()
        {
            return this.revision;
        }

        public byte GetCompressionFlags()
        {
            return this.compressionFlags;
        }

        public ResourceDescriptor[] GetDependencies()
        {
            ResourceDescriptor[] descriptors = new ResourceDescriptor[this.dependencies.Count];
            descriptors = this.dependencies.ToArray(descriptors);
            return descriptors;
        }
    }
}