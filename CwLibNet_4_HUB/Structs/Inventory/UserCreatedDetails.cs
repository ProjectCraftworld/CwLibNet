using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Inventory;

public class UserCreatedDetails: ISerializable
{
    public const int BaseAllocationSize = 0x8;
    
    public string? Name = "";
    public string? Description = "";

    public UserCreatedDetails() { }

    public UserCreatedDetails(string? name, string? description)
    {
        Name = name;
        Description = description;
    }
    
    public void Serialize()
    {
        Serializer.Serialize(ref Name);
        Serializer.Serialize(ref Description);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null)
            size += Name.Length * 2;
        if (Description != null)
            size += Description.Length * 2;
        return size;
    }


}