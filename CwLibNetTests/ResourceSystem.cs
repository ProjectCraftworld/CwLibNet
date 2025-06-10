using System.IO;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.Types.Data;

namespace CwLibNetTests
{

public class ResourceSystem
{
    [Fact]
    public void TestLoad()
    {
        // Arrange
        string path = "Y:\\Giochi PS3\\GAMES\\LBP_KNOWIN\\PS3_GAME\\USRDIR";

        // Skip test if path doesn't exist (e.g., on non-Windows systems)
        if (!Directory.Exists(path))
        {
            Assert.True(true, "Skipping test - game path not found on this system");
            return;
        }

        // Act
        CwLibNet4Hub.Singleton.ResourceSystem.LoadGameRoot(path);

        byte[]? bytes = CwLibNet4Hub.Singleton.ResourceSystem.Extract(new ResourceDescriptor(new GUID(1756), ResourceType.Invalid));

        // Assert
        Assert.True(CwLibNet4Hub.Singleton.ResourceSystem.IsInitialized);
        Assert.NotNull(bytes);
    }
}

}
