using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Inventory;

public class PhotoMetadata: ISerializable
{
    public const int BaseAllocationSize = 0x50;

    public ResourceDescriptor? Photo;
    public SlotID Level = new();
    public string? LevelName;
    public Sha1? LevelHash = new();
    public PhotoUser[]? Users;
    public long Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Photo, ResourceType.Texture, true, false, false);
        Serializer.Serialize(ref Level);
        Serializer.Serialize(ref LevelName);
        Serializer.Serialize(ref LevelHash);
        Serializer.Serialize(ref Timestamp);
        Serializer.Serialize(ref Users);
    }

    public int GetAllocatedSize()
    {
        throw new NotImplementedException();
    }
}