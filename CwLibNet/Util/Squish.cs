using CwLibNet.Squish;

namespace CwLibNet.Util;

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

            public static readonly CompressionType DXT1 = new CompressionType(8);
            public static readonly CompressionType DXT3 = new CompressionType(16);
            public static readonly CompressionType DXT5 = new CompressionType(16);
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

            public static readonly CompressionMetric Perceptual = new CompressionMetric(0.2126f, 0.7152f, 0.0722f);
            public static readonly CompressionMetric Uniform = new CompressionMetric(1.0f, 1.0f, 1.0f);

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
        private static readonly ColourSet colours = new ColourSet();

        // Calculate storage requirements for a given image width, height, and compression type.
        public static int GetStorageRequirements(int width, int height, CompressionType type)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException($"Invalid image dimensions specified: {width} x {height}");

            int blockCount = ((width + 3) / 4) * ((height + 3) / 4);
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
            byte[] sourceRGBA = new byte[16 * 4]; // 4x4 block of pixels (each pixel has 4 bytes)

            int targetBlock = 0;
            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    int targetPixel = 0;
                    int mask = 0;
                    for (int py = 0; py < 4; ++py)
                    {
                        int sy = y + py;
                        for (int px = 0; px < 4; ++px)
                        {
                            int sx = x + px;
                            if (sx < width && sy < height)
                            {
                                int sourcePixel = 4 * (width * sy + sx);
                                for (int i = 0; i < 4; ++i)
                                    sourceRGBA[targetPixel++] = rgba[sourcePixel++];
                                mask |= (1 << (4 * py + px));
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
            int storageSize = GetStorageRequirements(width, height, type);

            if (rgba == null || rgba.Length < (width * height * 4))
                throw new ArgumentException("Invalid source image data specified.");

            if (blocks == null || blocks.Length < storageSize)
                blocks = new byte[storageSize];

            return blocks;
        }

        private static void Compress(byte[] rgba, int mask, byte[] block, int offset,
                                     CompressionType type, CompressionMethod method, CompressionMetric metric, bool weightAlpha)
        {
            // Determine block positions
            int colourBlock = offset + type.BlockOffset;
            int alphaBlock = offset;

            // Initialize the minimal point set
            colours.Init(rgba, mask, type, weightAlpha);

            // Choose the compressor based on the number of colours.
            CompressorColourFit fit;
            if (colours.GetCount() == 1)
            {
                fit = new CompressorSingleColour(colours, type);
            }
            else
            {
                fit = method.GetCompressor(colours, type, metric);
            }
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
            byte[] targetRGBA = new byte[16 * 4];

            int sourceBlock = 0;
            for (int y = 0; y < height; y += 4)
            {
                for (int x = 0; x < width; x += 4)
                {
                    Decompress(targetRGBA, blocks, sourceBlock, type);

                    int sourcePixel = 0;
                    for (int py = 0; py < 4; ++py)
                    {
                        for (int px = 0; px < 4; ++px)
                        {
                            int sx = x + px;
                            int sy = y + py;
                            if (sx < width && sy < height)
                            {
                                int targetPixel = 4 * (width * sy + sx);
                                for (int i = 0; i < 4; ++i)
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
            int storageSize = GetStorageRequirements(width, height, type);

            if (blocks == null || blocks.Length < storageSize)
                throw new ArgumentException("Invalid source image data specified.");

            if (rgba == null || rgba.Length < (width * height * 4))
                rgba = new byte[width * height * 4];

            return rgba;
        }

        private static void Decompress(byte[] rgba, byte[] block, int offset, CompressionType type)
        {
            int colourBlock = offset + type.BlockOffset;
            int alphaBlock = offset;

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