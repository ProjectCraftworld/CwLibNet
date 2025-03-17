using CwLibNet.Resources;
using CwLibNet.Types;

namespace CwLibNetTests;

public class RTranslationTableTests
{
    private static readonly byte[] lamsKeyExample =
    [
        0, 0, 0, 2, 73, 150, 2, 210, 0, 0, 0, 0, 73, 150, 2, 211,
        0, 0, 0, 18, 254, 255, 0, 84, 0, 101, 0, 115, 0, 116, 0, 32,
        0, 107, 0, 101, 0, 121, 254, 255, 0, 84, 0, 101, 0, 115, 0, 116,
        0, 32, 0, 107, 0, 101, 0, 121, 0, 32, 0, 50
    ];
    
    [Fact]
    public void CheckLAMSKey()
    {
        long key = RTranslationTable.MakeLamsKeyID("LITTLEBIGPLANET");
        Assert.Equal(2788102254, key);
    }

    [Fact]
    public void CheckTranslationTable()
    {
        RTranslationTable table = new RTranslationTable(lamsKeyExample);
        string? translation = table.Translate(1234567890)!.Normalize();
        Assert.Equal("Test key", translation);
    }

    [Fact]
    public void BuildTranslationTable()
    {
        RTranslationTable table = new RTranslationTable(new Dictionary<long, string?>
        {
            [1234567890] = "Test key",
            [1234567891] = "Test key 2"
        });
        byte[]? bytes = table.Build().Buffer;
        Assert.Equal(bytes, lamsKeyExample);
    }
}