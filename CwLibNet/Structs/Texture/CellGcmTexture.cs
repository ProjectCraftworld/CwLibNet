using CwLibNet.Enums.ValueEnum;
using CwLibNet.Enums;
using CwLibNet.External;
using CwLibNet.IO.Streams;

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

        public bool IsBumpTexture()
        {
            return (this.flags & 0x1) != 0;
        }
        
        public CellGcmTexture(byte[]? ddsData, bool isNoSrgb)
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
            stream.Bytes([(byte)format, mipmap, dimension, cubemap]);
            stream.I32(remap, true);
            stream.I16(width);
            stream.I16(height);
            stream.I16(depth);
            stream.Bytes([location, flags]);
            stream.I32(pitch, true);
            stream.I32(offset, true);
        }

        /**
         * Deserializes TextureInfo from stream.
         *
         * @param stream to read texture info from
         * @param method Texture type
         */
        public CellGcmTexture(MemoryInputStream stream, SerializationType method)
        {
            this.method = method;

            // I don't want to grab the full structure for this right now,
            // and it's not really necessary for anything either, so I guess I'll finish
            // it at some other point

            if (method == SerializationType.GXT_SWIZZLED)
                stream.Seek(0x14);

            this.format = (CellGcmEnumForGtf)(stream.U8());
            this.mipmap = stream.I8();
            this.dimension = stream.I8();
            // if dimension > 2, MARK TEXTURE AS VOL(UME)TEX
            this.cubemap = stream.I8();
            this.remap = stream.I32(true);
            this.width = stream.I16();
            this.height = stream.I16();
            this.depth = stream.I16();
            this.location = stream.I8();

            // If (padding & 0x1) != 0, MARK TEXTURE AS BUMPTEX | NOSRGB_TEX
            // if (padding & 0x2) != 0, MARK TEXTURE as 0x20000000
            // if (padding & 0x4) != 0, MARK TEXTURE AS 0x40000000
            this.flags = stream.I8(); // padding

            this.pitch = stream.I32(true);
            this.offset = stream.I32(true);
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
