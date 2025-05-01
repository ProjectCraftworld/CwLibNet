using CwLibNet.Types.Data;

namespace CwLibNet.Types.Databases;

public class FileEntry(string path, SHA1 sha1, long size)
{
    public string Path = path;

    /**
     * Size of the associated data buffer.
     */
    public long Size = size;

    /**
     * SHA1 signature of data.
     */
    public SHA1 Sha1 = sha1;

    /**
     * Unique key for data
     */
    public object? Key;

    /**
     * Resource metadata
     */
    public ResourceInfo? Info;

    /**
     * Map of resources assigned to this entry.
     */
    private readonly object?[] resources = new object[10];

    public T? GetResource<T>(int key)
    {
        var value = resources[key];
        if (value == null) return default;
        return (T) value;
    }

    public void SetDetails(FileEntry entry)
    {
        Path = entry.Path;
        Size = entry.Size;
        Sha1 = entry.Sha1;
    }
}