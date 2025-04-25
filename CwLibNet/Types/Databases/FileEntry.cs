using CwLibNet.Enums;
using CwLibNet.Types.Data;

namespace CwLibNet.Types.Databases;

public class FileEntry
{
    public string Path;

    /**
     * Size of the associated data buffer.
     */
    public long Size;

    /**
     * SHA1 signature of data.
     */
    public SHA1 Sha1;

    /**
     * Unique key for data
     */
    public object? Key;

    /**
     * Resource metadata
     */
    public ResourceInfo Info;

    /**
     * Map of resources assigned to this entry.
     */
    private readonly object?[] resources = new object[10];

    public FileEntry(string path, SHA1 sha1, long size)
    {
        Size = size;
        Sha1 = sha1;
        Path = path;
    }
    
    public T? GetResource<T>(int key)
    {
        object? value = resources[key];
        if (value == null) return default;
        return (T) value;
    }

    public void setDetails(FileEntry entry)
    {
        Path = entry.Path;
        Size = entry.Size;
        Sha1 = entry.Sha1;
    }
}