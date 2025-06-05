using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Types.Data;

public class NetworkPlayerID: ISerializable
{
    public const int BaseAllocationSize = NetworkOnlineId.BaseAllocationSize + 0x10;

    private NetworkOnlineId handle = new();
    private byte[]? opt = new byte[8];
    private byte[]? reserved = new byte[8];

    public NetworkPlayerID() { }

    public NetworkPlayerID(string psid)
    {
        handle = new NetworkOnlineId(psid);
    }

    
    public void Serialize(Serializer serializer)
    {
        handle = serializer.Struct(handle);

        var lengthPrefixed = serializer.GetRevision().GetVersion() < 0x234;

        if (lengthPrefixed) serializer.I32(8);
        opt = serializer.Bytes(opt, 8);

        if (lengthPrefixed) serializer.I32(8);
        reserved = serializer.Bytes(reserved, 8);
    }

    public NetworkOnlineId GetHandle()
    {
        return handle;
    }


    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }

    
    public NetworkPlayerID Clone()
    {
        var id = new NetworkPlayerID();
        id.handle = new NetworkOnlineId(id.handle.ToString());
        if (opt != null)
            id.opt = (byte[])opt.Clone();
        if (reserved != null)
            id.reserved = (byte[])reserved.Clone();
        return id;
    }

    public override bool Equals(object? other)
    {
        if (other == this) return true;
        if (!(other is NetworkPlayerID otherId)) return false;
        return otherId.handle.Equals(handle);
    }

    public override int GetHashCode()
    {
        return handle != null ? handle.GetHashCode() : 0;
    }

    public override string ToString()
    {
        return handle.ToString();
    }


}