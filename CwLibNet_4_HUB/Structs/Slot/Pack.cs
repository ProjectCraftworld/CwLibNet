using CwLibNet.Enums;
using CwLibNet.Types.Data;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Slot;

public class Pack
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x40 + Slot.BaseAllocationSize;
    public ContentsType ContentsType = ContentsType.LEVEL;
    public ResourceDescriptor? Mesh;
    public Slot Slot = new();
    public string? ContentId;
    public long TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // seconds since epoch
    public bool CrossBuyCompatible;

    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer) 
    {
        Serializer.SerializeEnum32(ref ContentsType);
        Serializer.Serialize(ref Mesh, ResourceType.Mesh, true, false, false);
        Serializer.Serialize(ref Slot);
        Serializer.Serialize(ref ContentId);
        Serializer.Serialize(ref TimeStamp);
        if (Serializer.GetCurrentSerializer().GetRevision().IsVita()) Serializer.Serialize(ref CrossBuyCompatible); 
    }
}