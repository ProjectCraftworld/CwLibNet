using CwLibNet.Enums;
using CwLibNet.Types.Data;

namespace CwLibNetTests;

public class ResourceSystem
{
    [Fact]
    public void TestLoad()
    {
        // Arrange
        string path = "Y:\\Giochi PS3\\GAMES\\LBP_KNOWIN\\PS3_GAME\\USRDIR";

        // Act
        CwLibNet.Singleton.ResourceSystem.LoadGameRoot(path);

        byte[] bytes = CwLibNet.Singleton.ResourceSystem.Extract(new ResourceDescriptor(new GUID(1756), ResourceType.Invalid));

        // Assert
        Assert.True(CwLibNet.Singleton.ResourceSystem.IsInitialized);
        Assert.NotNull(bytes);
    }
}
