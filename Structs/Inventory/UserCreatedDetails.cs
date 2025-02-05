using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Inventory
{
    public class UserCreatedDetails : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x8;
        public string name = "";
        public string description = "";
        public UserCreatedDetails()
        {
        }

        public UserCreatedDetails(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public override void Serialize(Serializer serializer)
        {
            name = serializer.Serialize(name);
            description = serializer.Serialize(description);
        }

        public virtual int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (this.name != null)
                size += (this.name.Length() * 2);
            if (this.description != null)
                size += (this.description.Length() * 2);
            return size;
        }
    }
}