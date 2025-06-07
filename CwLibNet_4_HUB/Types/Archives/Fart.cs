using System.Collections;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Streams;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Types.Archives;

public abstract class Fart: IEnumerable<Fat>
{
    protected string File;

    /**
     * The last time this archive was modified,
     * used for checking if we need to refresh
     * the FAT table.
     */
    protected long LastModified;

    /**
     * Offset of the FAT table.
     */
    protected long FatOffset;

    /**
     * Type of FileArchive
     */
    private ArchiveType type;

    /**
     * Queue mapping for entries to be added on save.
     */
    protected Dictionary<Sha1, byte[]> Queue = new(32);

    protected Fat[] Entries;

    /**
     * Structure to map SHA1s to their respective entries in the archive,
     * so access is constant time.
     */
    protected Dictionary<Sha1, Fat> Lookup = new();

    protected Fart(string file, ArchiveType type)
    {
        // Only save archives can have null paths
        if (file == null && type != ArchiveType.SAVE)
            throw new NullReferenceException("Archive path cannot be null!");
        if (type == null)
            throw new NullReferenceException("Archive type cannot be null!");
        this.type = type;
        File = file;
        if (File != null)
            LastModified = new DateTimeOffset(System.IO.File.GetLastWriteTime(File)).ToUnixTimeSeconds();
    }

    /**
     * Extracts a resource from the archive via SHA1.
     *
     * @param sha1 SHA1 signature of resource to extract
     * @return Extracted resource
     */
    public byte[]? Extract(Sha1 sha1)
    {
        if (sha1 == null)
            throw new NullReferenceException("Can't search for null hash in archive!");

        // Grab the resource from the queue if it exists
        if (Queue.TryGetValue(sha1, out var extract1))
            return extract1;

        return Lookup.ContainsKey(sha1) ? Extract(Lookup[sha1]) : null;
    }

    /**
     * Extracts a resource via a FAT entry.
     *
     * @param fat FAT row to extract
     * @return Extracted resource
     */
    public byte[]? Extract(Fat fat)
    {
        if (fat == null)
            throw new NullReferenceException("Can't search for null entry in archive!");
        if (fat.GetFileArchive() != this)
            throw new ArgumentException("This entry does not belong to this archive!");
        try
        {
            using var fileStream = new FileStream(File, FileMode.Open, FileAccess.Read);
            var buffer = new byte[fat.GetSize()];
            fileStream.Seek(fat.GetOffset(), SeekOrigin.Begin);
            fileStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        catch (IOException) { return null; }
    }

    /**
     * Checks if a hash exists in the archive.
     *
     * @param sha1 Hash to query
     * @return Whether the hash exists
     */
    public bool Exists(Sha1 sha1)
    {
        if (sha1 == null)
            throw new NullReferenceException("Can't search for null hash in archive!");
        return Lookup.ContainsKey(sha1) || Queue.ContainsKey(sha1);
    }

    /**
     * Adds a buffer to the archive.
     *
     * @param data Data to add
     * @return SHA1 hash of data added
     */
    public Sha1 Add(byte[] data)
    {
        if (data == null)
            throw new NullReferenceException("Can't add null buffer to archive!");
        var sha1 = Sha1.FromBuffer(data);

        // Already exists, no point adding it to the queue.
        if (Exists(sha1)) return sha1;

        Queue.Add(sha1, data);

        return sha1;
    }

    /**
     * Adds the contents of another archive to this one.
     *
     * @param fart Archive containing data to add
     * @return Hashes added
     */
    public Sha1[] Add(Fart fart)
    {
        List<Sha1> hashes = new(fart.Entries.Length);
        foreach (var fat in fart.Entries)
        {
            var sha1 = fat.GetSha1();
            if (Exists(sha1))
                continue;
            // Don't use the add method since it hashes the data again.
            Queue.Add(sha1, fart.Extract(fat));
            hashes.Add(sha1);
        }
        return hashes.ToArray();
    }

    /**
     * Flushes changes to the archive.
     *
     * @return Whether the operation was successful.
     */
    public abstract bool Save();

    public ArchiveType GetArchiveType()
    {
        return type;
    }

    public string GetFile()
    {
        return File;
    }

    /**
     * Gets the accumulated size of the data in the queue.
     *
     * @return Queue size
     */
    public long GetQueueSize()
    {
        return Queue.Values
            .Select(data => data.Length)
            .Aggregate(0, (p, c) => p + c);
    }

    /**
     * Gets a list of all hashes queued to be saved.
     *
     * @return All hashes currently in queue
     */
    public List<Sha1> GetQueueHashes()
    {
        return [..Queue.Keys];
    }

    /**
     * Generates a FAT buffer.
     *
     * @param fat Fat array
     * @return Generated buffer
     */
    protected static byte[] GenerateFat(Fat[] fat)
    {
        var stream = new MemoryOutputStream(fat.Length * 0x1c);
        foreach (var entry in fat)
        {
            stream.Sha1(entry.GetSha1());
            stream.U32(entry.GetOffset());
            stream.I32(entry.GetSize());
        }
        return stream.GetBuffer();
    }

    /**
     * Deserializes a resource extracted from this archive.
     *
     * @param <T>   Resource type that implements Serializable
     * @param hash  Hash of resource to extract
     * @param clazz Resource class reference that implements Serializable
     * @return Deserialized resource
     */
    public T? LoadResource<T>(Sha1 hash) where T: ISerializable
    {
        var data = Extract(hash);
        if (data == null) return default;
        var resource = new SerializedResource(data);
        return resource.LoadResource<T>();
    }

    /**
     * Checks if the archive has been modified since
     * being loaded.
     *
     * @return Whether or not the archive has been modified
     */
    public bool WasModified()
    {
        if (File == null) return true;
        if (!System.IO.File.Exists(File)) return true;
        return new DateTimeOffset(System.IO.File.GetLastAccessTime(File)).ToUnixTimeSeconds() != LastModified;
    }

    /**
     * Validates that all SHA1s match their corresponding buffers in FAT
     *
     * @return Number of entries that failed validation
     */
    public int Validate()
    {
        List<Fat> entries = new(Entries.Length);
        foreach (var fat in Entries)
        {
            var sha1 = Sha1.FromBuffer(fat.Extract());
            if (sha1.Equals(fat.GetSha1()))
                entries.Add(fat);
        }
        var missing = Entries.Length - entries.Count;
        Entries = entries.ToArray();
        return missing;
    }

    /**
     * Checks if the archive contains data to save.
     *
     * @return Whether the archive should save.
     */
    public bool ShouldSave()
    {
        return Queue.Count != 0;
    }

    /**
     * Gets number of entries in FAT.
     *
     * @return Number of entries.
     */
    public int GetEntryCount()
    {
        return Entries.Length;
    }

    public IEnumerator GetEnumerator()
    {
        return Entries.GetEnumerator();
    }

    IEnumerator<Fat> IEnumerable<Fat>.GetEnumerator()
    {
        return (IEnumerator<Fat>)Entries.GetEnumerator();
    }
}