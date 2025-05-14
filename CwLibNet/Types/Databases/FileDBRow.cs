using CwLibNet.Types.Data;

namespace CwLibNet.Types.Databases;

public class FileDbRow: FileEntry
{
    public long Date;
    
    public FileDbRow(string path, long date, long size, Sha1 sha1, GUID guid): base(path, sha1, size)
    {
        Path = path;
        Date = date;
        Key = guid;
    }

    public void UpdateDate()
    {
        Date = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
    }
    
    public void SetDetails(FileDbRow entry)
    {
        if (entry == null)
            throw new NullReferenceException("Entry cannot be null!");
        base.SetDetails(entry);
        Date = entry.Date;
        Key = entry.GetGuid();
    }
    
    public GUID GetGuid()
    {
        return (GUID) Key;
    }

    
}