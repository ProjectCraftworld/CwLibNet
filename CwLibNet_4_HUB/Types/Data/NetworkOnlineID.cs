using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Types.Data;

public class NetworkOnlineId : ISerializable
{
    public const int BaseAllocationSize = 0x20;
    private byte[]? data = new byte[16];
    private byte term;
    private byte[]? dummy = new byte[3];

    public NetworkOnlineId() { }

    public NetworkOnlineId (string psid)
    {
        if (psid == null) return;
        if (psid.Length > 16) throw new ArgumentException("PSID cannot be longer than 16 characters.");

        Array.Copy(System.Text.Encoding.ASCII.GetBytes(psid), 0, data, 0, psid.Length);
    }

    public void Serialize() 
    {
        var lengthPrefixed = Serializer.GetRevision().GetVersion() < 0x234;
        if (lengthPrefixed) 
        {
            Serializer.Serialize(ref (byte)data!.Length);
        }
        data = Serializer.Serialize(ref data);

        Serializer.Serialize(ref term);
        if (lengthPrefixed) Serializer.Serialize(ref (byte)dummy.Length);
        dummy = Serializer.Serialize(ref dummy);
    }

    public int GetAllocatedSize() 
    {
        return BaseAllocationSize;
    }

    public override string ToString() 
    {
        return System.Text.Encoding.ASCII.GetString(data).TrimEnd('\0');
    }
    public override bool Equals(object? obj) 
    {
        if (obj == this) return true;
        if (obj is not NetworkOnlineId id) return false;
        return data.Equals(id.data) && term.Equals(id.term);
    }
    public override int GetHashCode() 
    {
        var result = 31 * (data[0] ^ (data[0] >> 32));
        result = 31 * result + term.GetHashCode();
        return result;
    }
}