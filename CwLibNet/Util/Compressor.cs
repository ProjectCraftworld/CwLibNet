using System;
using System.IO;
using System.IO.Compression;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.EX;
namespace CwLibNet.Util
{
    public static class Compressor
    {
        public static byte[]? DeflateData(byte[] data)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (ZLibStream deflateStream = new ZLibStream(outputStream, CompressionLevel.Optimal))
                {
                    deflateStream.Write(data, 0, data.Length);
                }
                return outputStream.ToArray();
            }
        }

        public static byte[]? InflateData(byte[]? data, int size)
        {
            try
            {
                using (MemoryStream inputStream = new MemoryStream(data))
                using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                using (MemoryStream outputStream = new MemoryStream(size))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = deflateStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                    }
                    return outputStream.ToArray();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static byte[]? DecompressData(MemoryInputStream stream, int endOffset)
        {
            short flag = stream.I16(); // Some flag? Always 0x0001
            short chunks = stream.I16();

            if (chunks == 0)
            {
                return stream.Bytes(endOffset - stream.GetOffset());
            }

            int[] compressed = new int[chunks];
            int[] decompressed = new int[chunks];
            int decompressedSize = 0;
            for (int i = 0; i < chunks; ++i)
            {
                compressed[i] = stream.U16();
                decompressed[i] = stream.U16();
                decompressedSize += decompressed[i];
            }

            MemoryOutputStream inflateStream = new MemoryOutputStream(decompressedSize);
            for (int i = 0; i < chunks; ++i)
            {
                byte[]? deflatedData = stream.Bytes(compressed[i]);
                if (compressed[i] == decompressed[i])
                {
                    inflateStream.Bytes(deflatedData);
                    continue;
                }
                byte[]? inflatedData = InflateData(deflatedData, decompressed[i]);
                if (inflatedData == null)
                {
                    throw new SerializationException("An error occurred while inflating data!");
                }
                inflateStream.Bytes(inflatedData);
            }

            return inflateStream.GetBuffer();
        }

        public static byte[]? GetCompressedStream(byte[]? data, bool isCompressed)
        {
            if (data == null) return Array.Empty<byte>();
            if (!isCompressed)
            {
                return Bytes.Combine(new byte[] { 0x00, 0x00, 0x00, 0x00 }, data);
            }

            byte[][] chunks = Bytes.Split(data, 0x8000);

            short[] compressedSize = new short[chunks.Length];
            short[] uncompressedSize = new short[chunks.Length];

            byte[]?[] zlibStreams = new byte[chunks.Length][];

            for (int i = 0; i < chunks.Length; ++i)
            {
                byte[]? compressed = DeflateData(chunks[i]);
                zlibStreams[i] = compressed;
                compressedSize[i] = (short)compressed.Length;
                uncompressedSize[i] = (short)chunks[i].Length;
            }

            MemoryOutputStream output = new MemoryOutputStream(4 + (chunks.Length * 4));
            output.U16(1); // Some flag? Always 0x0001
            output.U16(zlibStreams.Length);

            for (int i = 0; i < zlibStreams.Length; ++i)
            {
                output.I16(compressedSize[i]);
                output.I16(uncompressedSize[i]);
            }

            byte[]? combinedZlibStreams = Bytes.Combine(zlibStreams);
            return Bytes.Combine(output.GetBuffer(), combinedZlibStreams);
        }
    }
}