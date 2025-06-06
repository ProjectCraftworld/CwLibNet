using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Extensions;

public static class Reader
{
    public static byte[] ReadAllBytes(this BinaryReader reader)
    {
        const int bufferSize = 4096;
        using var ms = new MemoryStream();
        var buffer = new byte[bufferSize];
        int count;
        while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
            ms.Write(buffer, 0, count);
        return ms.ToArray();
    }
}