using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Inventory;

public class PhotoUser: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public string? PSID;
    public string? User;
    public Vector4? Bounds = Vector4.Zero;

    public PhotoUser() {}
    
    public PhotoUser(string? psid) {
        if (psid == null) return;
        if (psid.Length > 0x14)
            psid = psid[..0x14];
        PSID = psid;
        User = psid;
    }


    public void Serialize()
    {
        Serializer.Serialize(ref PSID);
        Serializer.Serialize(ref User);
        Serializer.Serialize(ref Bounds);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (User != null)
            size += User.Length * 2;
        return size;
    }
}