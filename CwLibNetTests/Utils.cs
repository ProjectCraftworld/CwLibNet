using CwLibNet.Enums;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using CwLibNet.Util;

namespace CwLibNetTests;

public class Utils
{
    public class BytesTests
    {
        [Fact]
        public void ToHex()
        {
            string hexifiedNumber = Bytes.ToHex(2025); // 2025 is a random number I chose
            Assert.Equal("000007E9", hexifiedNumber); // the result is the same obtained with Toolkit using JShell
        }

        [Fact]
        public void FromHex()
        {
            byte[] hexifiedNumber = Bytes.FromHex("000007E9"); // The hex from above
            byte[] originalBytes = [0, 0, 7, 233]; // Result from toolkit
            Assert.Equal(originalBytes, hexifiedNumber);
        }

        [Fact]
        public void ToShortBE()
        {
            byte[]? testBytes = [24, 32]; // Random chosen number
            short testShort = Bytes.ToShortBE(testBytes);
            Assert.Equal(6176, testShort);
        }

        [Fact]
        public void ToShortLE()
        {
            byte[]? testBytes = [24, 32];
            short testShort = Bytes.ToShortLE(testBytes);
            Assert.Equal(8216, testShort);
        }

        [Fact]
        public void ToIntegerBE()
        {
            byte[]? testBytes = [24, 32, 41, 71];
            int testInt = Bytes.ToIntegerBE(testBytes);
            Assert.Equal(404760903, testInt);
        }

        [Fact]
        public void ToIntegerLE()
        {
            byte[]? testBytes = [24, 32, 41, 71];
            int testInt = Bytes.ToIntegerLE(testBytes);
            Assert.Equal(1193877528, testInt);
        }

        [Fact]
        public void ToBytesBE()
        {
            byte[]? originalBytes = [24, 32];
            byte[]? testBytes = Bytes.ToBytesBE((short)6176);
            Assert.Equal(originalBytes, testBytes);
        }

        [Fact]
        public void ToBytesLE()
        {
            byte[]? originalBytes = [24, 32];
            byte[]? testBytes = Bytes.ToBytesLE((short)8216);
            Assert.Equal(originalBytes, testBytes);
        }

        [Fact]
        public void ToBytesBEInt()
        {
            byte[]? originalBytes = [24, 32, 41, 71];
            byte[]? testBytes = Bytes.ToBytesBE(404760903);
            Assert.Equal(originalBytes, testBytes);
        }

        [Fact]
        public void ToBytesLEInt()
        {
            byte[]? originalBytes = [24, 32, 41, 71];
            byte[]? testBytes = Bytes.ToBytesLE(1193877528);
            Assert.Equal(originalBytes, testBytes);
        }

        [Fact]
        public void GetIntegerBufferAllCompressed()
        {
            byte[]? originalBytes = [223, 219, 22];
            byte[]? testBytes = Bytes.GetIntegerBuffer(372191, 7);
            Assert.Equal(originalBytes, testBytes);
        }

        [Fact]
        public void GetIntegerBufferAllUncompressed()
        {
            byte[]? originalBytes = [0, 5, 173, 223];
            byte[]? testBytes = Bytes.GetIntegerBuffer(372191, 0);
            Assert.Equal(originalBytes, testBytes);
        }

        [Fact]
        public void GetResourceReferenceAllCompressed()
        {
            byte[]? originalBytes = [2, 255, 255, 3];
            byte[]? testBytes = Bytes.GetResourceReference(new ResourceDescriptor(65535, ResourceType.Texture),
                new Revision((int)Revisions.LBP1_MAX), 7);
            Assert.Equal(originalBytes, testBytes);
        }

        [Fact]
        public void GetResourceReferenceAllUncompressed()
        {
            byte[]? originalBytes = [2, 0, 0, 255, 255];
            byte[]? testBytes =  Bytes.GetResourceReference(new ResourceDescriptor(65535, ResourceType.Texture),
                new Revision((int)Revisions.LBP1_MAX), 0);
            Assert.Equal(originalBytes, testBytes);
        }
    }
}