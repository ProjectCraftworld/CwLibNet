using CwLibNet.IO.Streams;
using CwLibNet.EX;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Util;

public static class Compressor
{
    public static byte[] DeflateData(byte[] data)
    {
        Ionic.Zlib.ZlibStream deflater = new(new MemoryStream(data), Ionic.Zlib.CompressionMode.Compress);
        MemoryStream stream = new();
        deflater.CopyTo(stream);
        return stream.ToArray();
    }

    public static byte[] InflateData(byte[]? data, int size)
    {
        Ionic.Zlib.ZlibStream inflater = new(new MemoryStream(data), Ionic.Zlib.CompressionMode.Decompress);
        MemoryStream stream = new();
        inflater.CopyTo(stream);
        return stream.ToArray();
    }

    public static byte[]? DecompressData(MemoryInputStream stream, int endOffset)
    {
        var flag = stream.I16(); // Some flag? Always 0x0001
        var chunks = stream.I16();

        if (chunks == 0)
        {
            return stream.Bytes(endOffset - stream.GetOffset());
        }

        var compressed = new int[chunks];
        var decompressed = new int[chunks];
        var decompressedSize = 0;
        for (var i = 0; i < chunks; ++i)
        {
            compressed[i] = stream.U16();
            decompressed[i] = stream.U16();
            decompressedSize += decompressed[i];
        }

        var inflateStream = new MemoryOutputStream(decompressedSize);
        for (var i = 0; i < chunks; ++i)
        {
            var deflatedData = stream.Bytes(compressed[i]);
            if (compressed[i] == decompressed[i])
            {
                inflateStream.Bytes(deflatedData);
                continue;
            }
            var inflatedData = InflateData(deflatedData, decompressed[i]);
            if (inflatedData == null)
            {
                throw new SerializationException("An error occurred while inflating data!");
            }
            inflateStream.Bytes(inflatedData);
        }

        return inflateStream.GetBuffer();
    }

    public static byte[] GetCompressedStream(byte[]? data, bool isCompressed)
    {
        if (data == null) return [];
        if (!isCompressed)
        {
            return Bytes.Combine([0x00, 0x00, 0x00, 0x00], data);
        }

        var chunks = Bytes.Split(data, 0x8000);

        var compressedSize = new short[chunks.Length];
        var uncompressedSize = new short[chunks.Length];

        byte[]?[] zlibStreams = new byte[chunks.Length][];

        for (var i = 0; i < chunks.Length; ++i)
        {
            var compressed = DeflateData(chunks[i]);
            zlibStreams[i] = compressed;
            compressedSize[i] = (short)compressed.Length;
            uncompressedSize[i] = (short)chunks[i].Length;
        }

        var output = new MemoryOutputStream(4 + chunks.Length * 4);
        output.U16(1); // Some flag? Always 0x0001
        output.U16(zlibStreams.Length);

        for (var i = 0; i < zlibStreams.Length; ++i)
        {
            output.I16(compressedSize[i]);
            output.I16(uncompressedSize[i]);
        }

        var combinedZlibStreams = Bytes.Combine(zlibStreams);
        return Bytes.Combine(output.GetBuffer(), combinedZlibStreams);
    }
}