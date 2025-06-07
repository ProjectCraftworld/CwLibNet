using CwLibNet.Enums;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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

    public void Serialize() 
    {
        Serializer.Serialize(ref ContentsType);
        Serializer.Serialize(ref Mesh, Mesh, ResourceType.Mesh, true);
        Serializer.Serialize(ref Slot);
        Serializer.Serialize(ref ContentId);
        TimeStamp = Serializer.Serialize(ref TimeStamp);
        if (Serializer.GetRevision().IsVita()) Serializer.Serialize(ref CrossBuyCompatible); 
    }
}