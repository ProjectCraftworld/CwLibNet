using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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
    
    public void Serialize()
    {
        Serializer.Serialize(ref Photo, Photo, ResourceType.Texture, true);
        Serializer.Serialize(ref Level);
        Serializer.Serialize(ref LevelName);
        LevelHash = Serializer.Serialize(ref LevelHash);
        Serializer.Serialize(ref Timestamp);
        Users = Serializer.Serialize(ref Users);
    }

    public int GetAllocatedSize()
    {
        throw new NotImplementedException();
    }
}