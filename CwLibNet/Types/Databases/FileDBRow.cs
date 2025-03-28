using CwLibNet.Types.Data;

namespace CwLibNet.Types.Databases;

public class FileDBRow: FileEntry
{
    public long Date;
    
    public FileDBRow(String path, long date, long size, SHA1 sha1, GUID guid): base(path, sha1, size)
    {
        this.Path = path;
        this.Date = date;
        this.Key = guid;
    }

    public void updateDate()
    {
        Date = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
    }
    
    public void setDetails(FileDBRow entry)
    {
        if (entry == null)
            throw new NullReferenceException("Entry cannot be null!");
        base.setDetails(entry);
        this.Date = entry.Date;
        this.Key = entry.GetGuid();
    }
    
    public GUID GetGuid()
    {
        return (GUID) Key;
    }

    
}