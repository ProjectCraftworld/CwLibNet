using CwLibNet.Types.Data;

namespace CwLibNet.Types.Databases;

public class FileDBRow: FileEntry
{
    public long Date;
    
    public FileDBRow(String path, long date, long size, SHA1 sha1, GUID guid): base(path, sha1, size)
    {
        Path = path;
        Date = date;
        Key = guid;
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
        Date = entry.Date;
        Key = entry.GetGuid();
    }
    
    public GUID GetGuid()
    {
        return (GUID) Key;
    }

    
}