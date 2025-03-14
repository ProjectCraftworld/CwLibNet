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

public class ResourceSystem
{
    public static int LOG_LEVEL = ResourceLogLevel.INFO;
}