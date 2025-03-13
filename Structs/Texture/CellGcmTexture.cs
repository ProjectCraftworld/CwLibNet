using CwLibNet.Enums.ValueEnum;
using CwLibNet.Enums;
using CwLibNet.External;

namespace CwLibNet.Structs.Texture
{
    public class CellGcmTexture
    {
        private CellGcmEnumForGtf format;
        private byte mipmap;
        private byte dimension;
        private byte cubemap;
        private int remap;
        private short width;
        private short height;
        private short depth;
        private byte location;
        private byte flags;
        private int pitch;
        private int offset;
        private SerializationType method;

        public CellGcmTexture(CellGcmEnumForGtf format, short width, short height, byte mipmapCount, bool isNoSrgb)
        {
            this.format = format;
            this.width = width;
            this.height = height;
            mipmap = (byte)(mipmapCount & 0xFF);
            // Assuming other fields are initialized to default values
            cubemap = 0;
            depth = 1;
            remap = 0xaae4;
            dimension = 2; // Default for non-cubemap, non-volume textures
            location = 0;
            flags = isNoSrgb ? (byte)1 : (byte)0; // Assuming noSRGB affects the flag
            pitch = 0;
            offset = 0;
        }

        public CellGcmTexture(byte[] ddsData, bool isNoSrgb)
        {
            // Implement DDS parsing logic here
            // For example:
            var type = DDSReader.GetType(ddsData);
            format = type switch
            {
                0xFF => CellGcmEnumForGtf.B8,
                1146639409 => CellGcmEnumForGtf.DXT1,
                1146639411 => CellGcmEnumForGtf.DXT3,
                1146639413 => CellGcmEnumForGtf.DXT5,
                65538 => CellGcmEnumForGtf.A1R5G5B5,
                196610 => CellGcmEnumForGtf.A4R4G4B4,
                327682 => CellGcmEnumForGtf.R5G5B5,
                196612 => CellGcmEnumForGtf.A8R8G8B8,
                _ => throw new ArgumentException("Invalid format!")
            };
            mipmap = DDSReader.GetMipmap(ddsData);
            width = DDSReader.GetWidth(ddsData);
            height = DDSReader.GetHeight(ddsData);
            depth = 1;
            cubemap = 0;
            remap = 0xaae4;
            dimension = 2;
            location = 0;
            flags = isNoSrgb ? (byte)1 : (byte)0;
            pitch = 0;
            offset = 0; // Calculate based on data
        }

        public void Write(MemoryOutputStream stream)
        {
            stream.WriteByte((byte)format);
            stream.WriteByte(mipmap);
            stream.WriteByte(dimension);
            stream.WriteByte(cubemap);
            stream.WriteInt32(remap, true); // Assuming true means little-endian
            stream.WriteInt16(width);
            stream.WriteInt16(height);
            stream.WriteInt16(depth);
            stream.WriteByte(location);
            stream.WriteByte(flags);
            stream.WriteInt32(pitch, true);
            stream.WriteInt32(offset, true);
        }

        /**
         * Deserializes TextureInfo from stream.
         *
         * @param stream Stream to read texture info from
         * @param method Texture type
         */
        public CellGcmTexture(MemoryInputStream stream, SerializationType method)
        {
            this.method = method;

            // I don't want to grab the full structure for this right now,
            // and it's not really necessary for anything either, so I guess I'll finish
            // it at some other point

            if (method == SerializationType.GXT_SWIZZLED)
                stream.seek(0x14);

            this.format = (CellGcmEnumForGtf)(stream.u8());
            this.mipmap = stream.i8();
            this.dimension = stream.i8();
            // if dimension > 2, MARK TEXTURE AS VOL(UME)TEX
            this.cubemap = stream.i8();
            this.remap = stream.i32(true);
            this.width = stream.i16();
            this.height = stream.i16();
            this.depth = stream.i16();
            this.location = stream.i8();

            // If (padding & 0x1) != 0, MARK TEXTURE AS BUMPTEX | NOSRGB_TEX
            // if (padding & 0x2) != 0, MARK TEXTURE as 0x20000000
            // if (padding & 0x4) != 0, MARK TEXTURE AS 0x40000000
            this.flags = stream.i8(); // padding

            this.pitch = stream.i32(true);
            this.offset = stream.i32(true);
        }

        public SerializationType GetMethod() => method;

        public CellGcmEnumForGtf GetFormat()
        {
            return format;
        }

        public int GetMipCount()
        {
            return mipmap & 0xFF;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public int GetDepth()
        {
            return depth;
        }

        public bool IsCubemap()
        {
            return (cubemap != 0);
        }

        public void FixupFlags()
        {
            flags = (byte)(flags & 0x1);
        }
    }
}
