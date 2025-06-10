using CwLibNet4Hub.Enums;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Slot;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Slot;

public class Pack
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x40 + Slot.BaseAllocationSize;
    public ContentsType ContentsType = ContentsType.LEVEL;
    public ResourceDescriptor? Mesh;
    public Slot Slot = new();
    public string? ContentId;
    public long TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // seconds since epoch
    public bool CrossBuyCompatible;

    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer) 
    {
        Serializer.SerializeEnum32(ref ContentsType);
        Serializer.Serialize(ref Mesh, ResourceType.Mesh, true, false, false);
        Serializer.Serialize(ref Slot);
        Serializer.Serialize(ref ContentId);
        Serializer.Serialize(ref TimeStamp);
        if (Serializer.GetCurrentSerializer().GetRevision().IsVita()) Serializer.Serialize(ref CrossBuyCompatible); 
    }
}