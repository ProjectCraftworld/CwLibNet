using CwLibNet.Enums;

namespace CwLibNet.Types.Data;

public class GatherData
{
    private readonly string path;
    private readonly Sha1 sha1;
    private readonly byte[]? data;
    private readonly GUID guid;

    public GatherData(string path, ResourceDescriptor descriptor, byte[]? data)
    {
        this.path = path;
        sha1 = Sha1.FromBuffer(data);

        this.data = data;
        guid = (GUID)descriptor.GetGUID();
    }

    public GatherData(string path, Sha1 sha1, byte[]? data)
        : this(path, new ResourceDescriptor(sha1, ResourceType.Invalid), data) { }
        
    public GatherData(string path, GUID guid, byte[]? data)
        : this(path, new ResourceDescriptor(guid, ResourceType.Invalid), data) { }

    public GatherData(string path, GUID guid, Sha1 sha1, byte[]? data)
    {
        this.path = path;
        this.guid = guid;
        this.sha1 = sha1;
        this.data = data;
    }

    public GatherData()
    {
        path = string.Empty;
        sha1 = new Sha1(); // Assuming SHA1 has a default constructor
        data = [];
        guid = new GUID(); // Assuming GUID has a default constructor
    }

    public string GetPath() => path;
    public Sha1 GetSHA1() => sha1;
    public byte[]? GetData() => data;
    public GUID GetGUID() => guid;
}