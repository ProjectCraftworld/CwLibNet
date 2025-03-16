using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Inventory;

public class PhotoMetadata: ISerializable
{
    public const int BaseAllocationSize = 0x50;

    public ResourceDescriptor? Photo;
    public SlotID Level = new();
    public string? LevelName;
    public SHA1? LevelHash = new();
    public PhotoUser[] Users;
    public long Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    
    public void Serialize(Serializer serializer)
    {
        Photo = serializer.Resource(Photo, ResourceType.Texture, true);
        Level = serializer.Struct(Level);
        LevelName = serializer.Wstr(LevelName);
        LevelHash = serializer.Sha1(LevelHash);
        Timestamp = serializer.I64(Timestamp);
        Users = serializer.Array(Users);
    }

    public int GetAllocatedSize()
    {
        throw new NotImplementedException();
    }
}