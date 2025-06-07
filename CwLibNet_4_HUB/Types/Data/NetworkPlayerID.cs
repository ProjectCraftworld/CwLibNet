using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
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

    
    public void Serialize()
    {
        Serializer.Serialize(ref handle);

        var lengthPrefixed = Serializer.GetRevision().GetVersion() < 0x234;

        if (lengthPrefixed) Serializer.Serialize(ref 8);
        opt = Serializer.Serialize(ref opt);

        if (lengthPrefixed) Serializer.Serialize(ref 8);
        reserved = Serializer.Serialize(ref reserved);
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