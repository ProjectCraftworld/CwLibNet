using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Inventory;

public class PhotoUser: ISerializable
{
    public const int BASE_ALLOCATION_SIZE = 0x30;

    public string? PSID;
    public string? User;
    public Vector4? Bounds = Vector4.Zero;

    public PhotoUser() {}
    
    public PhotoUser(string? psid) {
        if (psid == null) return;
        if (psid.Length > 0x14)
            psid = psid[..0x14];
        this.PSID = psid;
        this.User = psid;
    }


    public void Serialize(Serializer serializer)
    {
        PSID = serializer.Str(PSID, 0x14);
        User  = serializer.Wstr(User);
        Bounds = serializer.V4(Bounds);
    }

    public int GetAllocatedSize()
    {
        int size = BASE_ALLOCATION_SIZE;
        if (this.User != null)
            size += (User.Length * 2);
        return size;
    }
}