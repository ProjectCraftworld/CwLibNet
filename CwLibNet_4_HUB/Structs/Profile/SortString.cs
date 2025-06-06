using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Profile
{
    public class SortString : ISerializable
    {
        public const int BaseAllocationSize = 0x10;

        public int LamsKeyId;
        public string? MyString;
        public int Index;

        public SortString() { }

        public SortString(int key, string? myString, int index)
        {
            LamsKeyId = key;
            this.MyString = myString;
            this.Index = index;
        }

        public void Serialize()
        {
            Serializer.Serialize(ref LamsKeyId);
            Serializer.Serialize(ref MyString);
            Serializer.Serialize(ref Index);
        }

        public int GetAllocatedSize()
        {
            var size = BaseAllocationSize;
            if (MyString != null)
            {
                size += MyString.Length * 2;
            }
            return size;
        }

        public override bool Equals(object? other)
        {
            var myString = ((SortString?) other)?.MyString;
            return myString != null && (other == this || myString.Equals(MyString));
        }

        public override int GetHashCode()
        {
            return MyString != null ? MyString.GetHashCode() : 0;
        }
    }
}