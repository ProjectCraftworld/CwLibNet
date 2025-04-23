using CwLibNet.Types.Archives;
using CwLibNet.Types.Data;
using CwLibNet.Types.Databases;

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

public static class ResourceSystem
{
    private static List<FileDb> fileDBs = [];
    private static List<FileArchive> fileArchives = [];

    public static bool IsInitialized => fileDBs.Count > 0 && fileArchives.Count > 0;

    public static void LoadGameRoot(string path)
    {
        fileDBs = new List<FileDb>();
        fileArchives = new List<FileArchive>();
        var dbFiles = Directory.GetFiles(path, "*.map", SearchOption.AllDirectories);
        foreach (var dbFile in dbFiles)
        {
            var db = new FileDb(dbFile);
            fileDBs.Add(db);
        }
        var archiveFiles = Directory.GetFiles(path, "*.farc", SearchOption.AllDirectories);
        foreach (var archiveFile in archiveFiles)
        {
            var archive = new FileArchive(archiveFile);
            fileArchives.Add(archive);
        }
    }
    
    public static FileEntry? Get(ResourceDescriptor resource)
    {
        return resource.IsGUID() ? Get(resource.GetGUID()!.Value) : Get(resource.GetSHA1()!);
    }

    private static FileEntry? Get(SHA1 hash)
    {
        return fileDBs.Select(file => file.Get(hash)).OfType<FileDBRow>().FirstOrDefault();
    }

    private static FileEntry? Get(GUID guid)
    {
        return fileDBs.Select(file => file.Get(guid)).OfType<FileDBRow>().FirstOrDefault();
    }

    public static byte[]? Extract(ResourceDescriptor resource)
    {
        return resource.IsGUID() ? Extract(resource.GetGUID()!.Value) : Extract(resource.GetSHA1()!);
    }

    private static byte[]? Extract(SHA1 hash)
    {
        return fileArchives.Select(farc => farc.Extract(hash)).OfType<byte[]>().FirstOrDefault();
    }

    private static byte[]? Extract(GUID guid)
    {
        var sha1 = Get(guid)?.Sha1;
        return sha1 != null ? Extract(sha1) : null;
    }
    
    public static int LOG_LEVEL = ResourceLogLevel.SERIALIZER_TRACE;
}