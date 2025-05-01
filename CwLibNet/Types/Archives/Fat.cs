namespace CwLibNet.Types.Archives;

public class Fat
{
    private Fart archive;

    /**
     * SHA1 signature of data.
     */
    private readonly SHA1 sha1;

    /**
     * Offset of the data in the archive.
     */
    private readonly long offset;

    /**
     * Size of the associated data buffer.
     */
    private readonly int size;

    public Fat(Fart archive, SHA1 sha1, long offset, int size)
    {
        if (archive == null)
            throw new NullReferenceException("File archive provided to Fat cannot be null!");

        this.archive = archive;
        this.sha1 = sha1 ?? throw new NullReferenceException("File SHA1 provided to Fat cannot be null!");
        this.offset = offset;
        this.size = size;
    }

    public SHA1 GetSha1()
    {
        return sha1;
    }

    public long GetOffset()
    {
        return offset;
    }

    public int GetSize()
    {
        return size;
    }

    public Fart GetFileArchive()
    {
        return archive;
    }

    /**
     * Extracts this entry from the associated archive.
     *
     * @return Extracted resource.
     */
    public byte[]? Extract()
    {
        return archive.Extract(this);
    }

    public override string ToString()
    {
        return string.Format("Fat (%h, offset=%d, size=%d)",
            sha1, offset, size);
    }
}