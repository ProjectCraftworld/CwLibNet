using CwLibNet.Types.Data;

namespace CwLibNet.Types.Archives;

public class Fat
{
    private Fart _archive;

    /**
     * SHA1 signature of data.
     */
    private readonly Sha1 _sha1;

    /**
     * Offset of the data in the archive.
     */
    private readonly long _offset;

    /**
     * Size of the associated data buffer.
     */
    private readonly int _size;

    public Fat(Fart archive, Sha1 sha1, long offset, int size)
    {
        this._archive = archive ?? throw new NullReferenceException("File archive provided to Fat cannot be null!");
        this._sha1 = sha1 ?? throw new NullReferenceException("File SHA1 provided to Fat cannot be null!");
        this._offset = offset;
        this._size = size;
    }

    public Sha1 GetSha1()
    {
        return _sha1;
    }

    public long GetOffset()
    {
        return _offset;
    }

    public int GetSize()
    {
        return _size;
    }

    public Fart GetFileArchive()
    {
        return _archive;
    }

    /**
     * Extracts this entry from the associated archive.
     *
     * @return Extracted resource.
     */
    public byte[]? Extract()
    {
        return _archive.Extract(this);
    }

    public override string ToString()
    {
        return string.Format("Fat (%h, offset=%d, size=%d)",
            _sha1, _offset, _size);
    }
}