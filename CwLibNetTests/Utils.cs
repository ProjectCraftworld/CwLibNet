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
    }
}