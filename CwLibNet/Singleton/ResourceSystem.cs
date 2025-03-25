using CwLibNet.Types.Data;

namespace CwLibNet.Singleton;

public static class ResourceLogLevel
{
    public const int NONE = 0;
    public const int INFO = 1;
    public const int ERROR = 2;
    public const int WARNING = 3;
    public const int DEBUG = 4;
    public const int SERIALIZER_TRACE = 5;
}

public struct FileEntry
{
    public string Path;
}

public class ResourceSystem
{
    public static FileEntry? Get(ResourceDescriptor resource)
    {
        throw new NotImplementedException("Missing resource system");
    }

    public static byte[]? Extract(ResourceDescriptor resource)
    {
        throw new NotImplementedException("Missing resource system");
    }
    public static int LOG_LEVEL = ResourceLogLevel.SERIALIZER_TRACE;
}