using CwLibNet4Hub.Squish;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Util;

public static class Squish
    {
        // Conversion of Java's enum CompressionType into a sealed class.
        public sealed class CompressionType
        {
            public int BlockSize { get; }
            public int BlockOffset { get; }

            private CompressionType(int blockSize)
            {
                BlockSize = blockSize;
                BlockOffset = blockSize - 8;
            }

            public static readonly CompressionType DXT1 = new(8);
            public static readonly CompressionType DXT3 = new(16);
            public static readonly CompressionType DXT5 = new(16);
        }

        // Conversion of Java's enum CompressionMetric into a sealed class.
        public sealed class CompressionMetric
        {
            public float R { get; }
            public float G { get; }
            public float B { get; }

            private CompressionMetric(float r, float g, float b)
            {
                R = r;
                G = g;
                B = b;
            }

            public static readonly CompressionMetric Perceptual = new(0.2126f, 0.7152f, 0.0722f);
            public static readonly CompressionMetric Uniform = new(1.0f, 1.0f, 1.0f);

            public float Dot(float x, float y, float z)
            {
                return R * x + G * y + B * z;
            }
        }

        // Conversion of Java's enum CompressionMethod into an abstract class with static instances.
        public abstract class CompressionMethod
        {
            public abstract CompressorColourFit GetCompressor(ColourSet colours, CompressionType type, CompressionMetric metric);

            public static readonly CompressionMethod ClusterFit = new ClusterFitMethod();
            public static readonly CompressionMethod RangeFit = new RangeFitMethod();

            private sealed class ClusterFitMethod : CompressionMethod
            {
                public override CompressorColourFit GetCompressor(ColourSet colours, CompressionType type, CompressionMetric metric)
                {
                    return new CompressorCluster(colours, type, metric);
                }
            }

            private sealed class RangeFitMethod : CompressionMethod
            {
                public override CompressorColourFit GetCompressor(ColourSet colours, CompressionType type, CompressionMetric metric)
                {
                    return new CompressorRange(colours, type, metric);
                }
            }
        }

        // A shared static instance of ColourSet (assuming this is defined elsewhere)
        private static readonly ColourSet colours = new();

        // Calculate storage requirements for a given image width, height, and compression type.
        public static int GetStorageRequirements(int width, int height, CompressionType type)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException($"Invalid image dimensions specified: {width} x {height}");

            var blockCount = (width + 3) / 4 * ((height + 3) / 4);
            return blockCount * type.BlockSize;
        }

        // Overload 1: Use default method and metric.
        public static byte[] CompressImage(byte[] rgba, int width, int height, byte[] blocks, CompressionType type)
        {
            return CompressImage(rgba, width, height, blocks, type, CompressionMethod.ClusterFit, CompressionMetric.Perceptual, false);
        }

        // Overload 2: Use default metric and weightAlpha.
        public static byte[] CompressImage(byte[] rgba, int width, int height, byte[] blocks, CompressionType type, CompressionMethod method)
        {
            return CompressImage(rgba, width, height, blocks, type, method, CompressionMetric.Perceptual, false);
        }

        // Main compressImage method.
        public static byte[] CompressImage(byte[] rgba, int width, int height, byte[] blocks,
                                           CompressionType type, CompressionMethod method, CompressionMetric metric, bool weightAlpha)
        {
            blocks = CheckCompressInput(rgba, width, height, blocks, type);
            var sourceRGBA = new byte[16 * 4]; // 4x4 block of pixels (each pixel has 4 bytes)

            var targetBlock = 0;
            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 4)
                {
                    var targetPixel = 0;
                    var mask = 0;
                    for (var py = 0; py < 4; ++py)
                    {
                        var sy = y + py;
                        for (var px = 0; px < 4; ++px)
                        {
                            var sx = x + px;
                            if (sx < width && sy < height)
                            {
                                var sourcePixel = 4 * (width * sy + sx);
                                for (var i = 0; i < 4; ++i)
                                    sourceRGBA[targetPixel++] = rgba[sourcePixel++];
                                mask |= 1 << (4 * py + px);
                            }
                            else
                            {
                                targetPixel += 4;
                            }
                        }
                    }
                    Compress(sourceRGBA, mask, blocks, targetBlock, type, method, metric, weightAlpha);
                    targetBlock += type.BlockSize;
                }
            }

            return blocks;
        }

        private static byte[] CheckCompressInput(byte[] rgba, int width, int height, byte[] blocks, CompressionType type)
        {
            var storageSize = GetStorageRequirements(width, height, type);

            if (rgba == null || rgba.Length < width * height * 4)
                throw new ArgumentException("Invalid source image data specified.");

            if (blocks == null || blocks.Length < storageSize)
                blocks = new byte[storageSize];

            return blocks;
        }

        private static void Compress(byte[] rgba, int mask, byte[] block, int offset,
                                     CompressionType type, CompressionMethod method, CompressionMetric metric, bool weightAlpha)
        {
            // Determine block positions
            var colourBlock = offset + type.BlockOffset;
            var alphaBlock = offset;

            // Initialize the minimal point set
            colours.Init(rgba, mask, type, weightAlpha);

            // Choose the compressor based on the number of colours.
            CompressorColourFit fit;
            fit = colours.GetCount() == 1 ? new CompressorSingleColour(colours, type) : method.GetCompressor(colours, type, metric);
            fit.Compress(block, colourBlock);

            // Compress alpha separately if necessary.
            if (ReferenceEquals(type, CompressionType.DXT3))
            {
                CompressorAlpha.CompressAlphaDxt3(rgba, mask, block, alphaBlock);
            }
            else if (ReferenceEquals(type, CompressionType.DXT5))
            {
                CompressorAlpha.CompressAlphaDxt5(rgba, mask, block, alphaBlock);
            }
        }

        public static byte[] DecompressImage(byte[] rgba, int width, int height, byte[] blocks, CompressionType type)
        {
            rgba = CheckDecompressInput(rgba, width, height, blocks, type);
            var targetRGBA = new byte[16 * 4];

            var sourceBlock = 0;
            for (var y = 0; y < height; y += 4)
            {
                for (var x = 0; x < width; x += 4)
                {
                    Decompress(targetRGBA, blocks, sourceBlock, type);

                    var sourcePixel = 0;
                    for (var py = 0; py < 4; ++py)
                    {
                        for (var px = 0; px < 4; ++px)
                        {
                            var sx = x + px;
                            var sy = y + py;
                            if (sx < width && sy < height)
                            {
                                var targetPixel = 4 * (width * sy + sx);
                                for (var i = 0; i < 4; ++i)
                                    rgba[targetPixel++] = targetRGBA[sourcePixel++];
                            }
                            else
                            {
                                sourcePixel += 4;
                            }
                        }
                    }
                    sourceBlock += type.BlockSize;
                }
            }

            return rgba;
        }

        private static byte[] CheckDecompressInput(byte[] rgba, int width, int height, byte[] blocks, CompressionType type)
        {
            var storageSize = GetStorageRequirements(width, height, type);

            if (blocks == null || blocks.Length < storageSize)
                throw new ArgumentException("Invalid source image data specified.");

            if (rgba == null || rgba.Length < width * height * 4)
                rgba = new byte[width * height * 4];

            return rgba;
        }

        private static void Decompress(byte[] rgba, byte[] block, int offset, CompressionType type)
        {
            var colourBlock = offset + type.BlockOffset;
            var alphaBlock = offset;

            // The fourth parameter indicates whether this is DXT1.
            ColourBlock.DecompressColour(rgba, block, colourBlock, ReferenceEquals(type, CompressionType.DXT1));

            // Decompress alpha separately if necessary.
            if (ReferenceEquals(type, CompressionType.DXT3))
            {
                CompressorAlpha.DecompressAlphaDxt3(rgba, block, alphaBlock);
            }
            else if (ReferenceEquals(type, CompressionType.DXT5))
            {
                CompressorAlpha.DecompressAlphaDxt5(rgba, block, alphaBlock);
            }
        }
    }