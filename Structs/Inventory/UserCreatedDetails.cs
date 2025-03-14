using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Inventory;

public class UserCreatedDetails: ISerializable
{
    public String Name = "";
    public String Description = "";

    public UserCreatedDetails() { }

    public UserCreatedDetails(String name, String description)
    {
        this.Name = name;
        this.Description = description;
    }
    
    public void Serialize(Serializer serializer)
    {
        throw new NotImplementedException();
    }

    public int GetAllocatedSize()
    {
        throw new NotImplementedException();
    }
}