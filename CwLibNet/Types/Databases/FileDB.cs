using System.Collections;
using CwLibNet.Enums;
using CwLibNet.Extensions;
using CwLibNet.IO.Streams;
using CwLibNet.Types.Data;

namespace CwLibNet.Types.Databases;

public class FileDb: IEnumerable<FileDbRow>
{
    private const int DefaultCapacity = 10;
    public bool HasChanges;

    /**
     * Minimum set of GUIDs not used by any of the LittleBigPlanet games.
     */
    private const long MinSafeGuid = 0x00180000;

    private int revision;
    protected List<FileDbRow> Entries;
    public readonly DatabaseType Type;
    private string? file;


    protected Dictionary<long, FileDbRow> Lookup;
    
    protected FileDb(string? file, DatabaseType type, int revision)
    {
        this.file = file;
        Type = type;
        this.revision = revision;
        Entries = new List<FileDbRow>(DefaultCapacity);
        Lookup = new Dictionary<long, FileDbRow>(DefaultCapacity);
    }
    
    protected FileDb(string? file, DatabaseType type)
    {
        this.file = file;
        Type = type;
        Entries = new List<FileDbRow>(DefaultCapacity);
        Lookup = new Dictionary<long, FileDbRow>(DefaultCapacity);
    }

    
    public FileDb(int revision, int capacity = DefaultCapacity): this(null, DatabaseType.FILE_DATABASE, revision)
    {
        this.revision = revision;
        if (capacity < 0)
            throw new ArgumentException("Cannot allocate entry array with negative " +
                                        "count!");
        Entries = new List<FileDbRow>(capacity);
        Lookup = new Dictionary<long, FileDbRow>(capacity);
    }

    public FileDb(string file): this(file, DatabaseType.FILE_DATABASE)
    {
        Process(new MemoryInputStream(file));
    }
    
    
    
    protected void Process(MemoryInputStream stream)
    {
        revision = stream.I32();
        var isLbp3 = revision >> 0x10 >= 0x148;
        var count = stream.I32();
        Entries = new List<FileDbRow>(count);
        Lookup = new Dictionary<long, FileDbRow>(count);

        for (var i = 0; i < count; ++i)
        {
            var path = stream.Str(isLbp3 ? stream.I16() : stream.I32());
            var timestamp = isLbp3 ? stream.U32() : stream.S64();
            var size = stream.U32();
            var sha1 = stream.Sha1();
            var guid = stream.Guid()!.Value;

            // If a GUID is duplicated, skip it
            if (Lookup.ContainsKey(guid.Value))
                continue;

            /* 	In LittleBigPlanet Vita, some versions of the databases don't store any filenames,
                only the extensions, so we'll use the hash of the resource in place of a name. */
            if (path.StartsWith("."))
            {
                path = string.Format("data/{0}{1}{2}",
                    GetFolderFromExtension(path), sha1, path);
            }

            var entry = new FileDbRow(path, timestamp, size, sha1, guid);

            Entries.Add(entry);
            Lookup.Add(guid.Value, entry);
        }
    }
    
    public bool Exists(long guid)
    {
        return Lookup.ContainsKey(guid);
    }
    
    public bool Exists(GUID guid)
    {
        return Exists(guid.Value);
    }

    public FileDbRow? Get(SHA1 sha1)
    {
        return Entries.FirstOrDefault(row => row.Sha1.Equals(sha1));
    }
    
    public FileDbRow? Get(long guid)
    {
        return Lookup.GetValueOrDefault(guid);
    }
    
    public FileDbRow? Get(GUID guid)
    {
        return Get(guid.Value);
    }
    
    public FileDbRow? Get(string path)
    {
        if (path == null)
            throw new NullReferenceException("Can't find null path!");
        path = path.ToLower(); // Ignore cases
        return Entries.FirstOrDefault(entry => entry.Path.ToLower().Contains(path));
    }
    
    public FileDbRow NewFileDbRow(string path, GUID guid)
    {
        if (Lookup.ContainsKey(guid.Value))
            throw new ArgumentException("GUID already exists in database!");
        var entry = new FileDbRow( path, 0, 0, new SHA1(), guid);
        entry.UpdateDate();
        Entries.Add(entry);
        Lookup.Add(guid.Value, entry);
        return entry;
    }
    
    public FileDbRow NewFileDbRow(FileDbRow entry)
    {
        var newEntry = NewFileDbRow(entry.Path, entry.GetGuid());
        newEntry.SetDetails(entry);
        newEntry.Date = entry.Date;
        newEntry.Key = entry.Key;
        return newEntry;
    }
    
    public static string GetFolderFromExtension(string extension)
    {
        switch (extension.ToLower())
        {
            case ".slt":
                return "slots/";
            case ".tex":
                return "textures/";
            case ".bpr":
            case ".ipr":
                return "profiles/";
            case ".mol":
            case ".msh":
                return "models/";
            case ".gmat":
            case ".gmt":
                return "gfx/";
            case ".mat":
                return "materials/";
            case ".ff":
            case ".fsh":
                return "scripts/";
            case ".plan":
            case ".pln":
                return "plans/";
            case ".pal":
                return "palettes/";
            case ".oft":
                return "outfits/";
            case ".sph":
                return "skeletons/";
            case ".bin":
            case ".lvl":
                return "levels/";
            case ".vpo":
                return "shaders/vertex/";
            case ".fpo":
                return "shaders/fragment/";
            case ".anim":
            case ".anm":
                return "animations/";
            case ".bev":
                return "bevels/";
            case ".smh":
                return "static_meshes/";
            case ".mus":
                return "audio/settings/";
            case ".fsb":
                return "audio/music/";
            case ".txt":
                return "text/";
            default:
                return "unknown/";
        }
    }
    
    public void Remove(FileEntry entry)
    {
        Entries.Remove((FileDbRow)entry);
        Lookup.Remove(((GUID)entry.Key).Value);
    }
    
    public void Patch(FileDb patch)
    {
        foreach (var entry in patch)
        {
            if (Exists(entry.GetGuid()))
                Get(entry.GetGuid())!.SetDetails(entry);
            else
                NewFileDbRow(entry);
        }
    }
    
    public GUID GetNextGuid()
    {
        var lastGuid = MinSafeGuid;
        while (Lookup.ContainsKey(lastGuid)) lastGuid++;
        return new GUID(lastGuid);
    }
    
    public int GetEntryCount()
    {
        return Entries.Count;
    }

    public byte[] Build()
    {
        // Just figure the GUIDs should be in ascending order.
        Entries.Sort((l, r) => l.GetGuid().Value.CompareUnsigned(r.GetGuid().Value));

        var pathSize = Entries
            .Select(e => e.Path.Length)
            .Aggregate(0, (total, element) => total + element);

        var isLbp3 = revision >> 0x10 >= 0x148;
        var baseEntrySize = isLbp3 ? 0x22 : 0x28;
        var stream =
            new MemoryOutputStream(0x8 + baseEntrySize * Entries.Count + pathSize);
        stream.I32(revision);
        stream.I32(Entries.Count);
        foreach (var entry in Entries)
        {
            var length = entry.Path.Length;
            if (isLbp3)
                stream.I16((short) length);
            else
                stream.I32(length);

            stream.Str(entry.Path, length);

            if (isLbp3) stream.U32(entry.Date);
            else stream.S64(entry.Date);

            stream.U32(entry.Size);
            stream.Sha1(entry.Sha1);
            stream.Guid(entry.GetGuid());
        }
        return stream.GetBuffer();
    }

    public void Save(string? file = null)
    {
        file ??= this.file;
        File.WriteAllBytes(file!, Build());
        HasChanges = false;
    }
    
    public IEnumerator<FileDbRow> GetEnumerator()
    {
        return Entries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}