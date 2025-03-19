using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Inventory;

public class UserCreatedDetails: ISerializable
{
    public const int BaseAllocationSize = 0x8;
    
    public string? Name = "";
    public string? Description = "";

    public UserCreatedDetails() { }

    public UserCreatedDetails(string? name, string? description)
    {
        this.Name = name;
        this.Description = description;
    }
    
    public void Serialize(Serializer serializer)
    {
        Name = serializer.Wstr(Name);
        Description = serializer.Wstr(Description);
    }

    
    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (Name != null)
            size += (Name.Length * 2);
        if (Description != null)
            size += (Description.Length * 2);
        return size;
    }


}