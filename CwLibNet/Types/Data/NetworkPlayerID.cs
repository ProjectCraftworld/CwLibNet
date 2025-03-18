using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Types.Data;

public class NetworkPlayerID: ISerializable
{
    public const int BaseAllocationSize = NetworkOnlineID.BASE_ALLOCATION_SIZE + 0x10;

    private NetworkOnlineID handle = new NetworkOnlineID();
    private byte[] opt = new byte[8];
    private byte[] reserved = new byte[8];

    public NetworkPlayerID() { }

    public NetworkPlayerID(String psid)
    {
        handle = new NetworkOnlineID(psid);
    }

    
    public void Serialize(Serializer serializer)
    {
        handle = serializer.Struct(handle);

        bool lengthPrefixed = serializer.GetRevision().GetVersion() < 0x234;

        if (lengthPrefixed) serializer.I32(8);
        opt = serializer.Bytes(opt, 8);

        if (lengthPrefixed) serializer.I32(8);
        reserved = serializer.Bytes(reserved, 8);
    }

    public NetworkOnlineID GetHandle()
    {
        return handle;
    }


    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }

    
    public NetworkPlayerID Clone()
    {
        NetworkPlayerID id = new NetworkPlayerID();
        id.handle = new NetworkOnlineID(id.handle.ToString());
        id.opt = (byte[])opt.Clone();
        id.reserved = (byte[])reserved.Clone();
        return id;
    }

    
    public override bool Equals(Object? other)
    {
        if (other == this) return true;
        if (!(other is NetworkPlayerID otherId)) return false;
        return otherId.handle.Equals(handle);
    }

    
    public override String ToString()
    {
        return handle.ToString();
    }


}