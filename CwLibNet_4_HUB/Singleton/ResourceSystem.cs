using CwLibNet.Types.Archives;
using CwLibNet.Types.Data;
using CwLibNet.Types.Databases;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Singleton;

public static class ResourceLogLevel
{
    public const int None = 0;
    public const int Info = 1;
    public const int Error = 2;
    public const int Warning = 3;
    public const int Debug = 4;
    public const int SerializerTrace = 5;
}

public static class ResourceSystem
{
    private static List<FileDb> _fileDBs = [];
    private static List<FileArchive> _fileArchives = [];

    public static bool IsInitialized => _fileDBs.Count > 0 && _fileArchives.Count > 0;

    public static void LoadGameRoot(string path)
    {
        _fileDBs = [];
        _fileArchives = [];
        var dbFiles = Directory.GetFiles(path, "*.map", SearchOption.AllDirectories);
        foreach (var dbFile in dbFiles)
        {
            var db = new FileDb(dbFile);
            _fileDBs.Add(db);
        }
        var archiveFiles = Directory.GetFiles(path, "*.farc", SearchOption.AllDirectories);
        foreach (var archiveFile in archiveFiles)
        {
            var archive = new FileArchive(archiveFile);
            _fileArchives.Add(archive);
        }
    }
    
    public static FileEntry? Get(ResourceDescriptor resource)
    {
        return resource.IsGUID() ? Get(resource.GetGUID()!.Value) : Get(resource.GetSHA1()!);
    }

    private static FileEntry? Get(Sha1 hash)
    {
        return _fileDBs.Select(file => file.Get(hash)).OfType<FileDbRow>().FirstOrDefault();
    }

    private static FileEntry? Get(GUID guid)
    {
        return _fileDBs.Select(file => file.Get(guid)).OfType<FileDbRow>().FirstOrDefault();
    }

    public static byte[]? Extract(ResourceDescriptor resource)
    {
        return resource.IsGUID() ? Extract(resource.GetGUID()!.Value) : Extract(resource.GetSHA1()!);
    }

    private static byte[]? Extract(Sha1 hash)
    {
        return _fileArchives.Select(farc => farc.Extract(hash)).OfType<byte[]>().FirstOrDefault();
    }

    private static byte[]? Extract(GUID guid)
    {
        var sha1 = Get(guid)?.Sha1;
        return sha1 != null ? Extract(sha1) : null;
    }
    
    public static int LogLevel = ResourceLogLevel.SerializerTrace;
}