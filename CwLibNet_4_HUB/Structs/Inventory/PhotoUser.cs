using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Inventory;

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


    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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