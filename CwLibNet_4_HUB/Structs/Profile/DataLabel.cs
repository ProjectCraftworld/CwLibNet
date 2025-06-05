using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class DataLabel : ISerializable
    {
        public const int BaseAllocationSize = 0x8;

        public int LabelIndex;
        public string? Name;

        public void Serialize(Serializer serializer)
        {
            LabelIndex = serializer.I32(LabelIndex);
            Name = serializer.Wstr(Name);
        }

        public int GetAllocatedSize() 
        {
            var size = BaseAllocationSize;
            if (Name != null)
            {
                size += (Name.Length * 2);
            }
            return size;
        }
    }
}