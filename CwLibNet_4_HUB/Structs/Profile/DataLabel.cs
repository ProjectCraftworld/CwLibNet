using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Types.Data;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Profile 
{
    public class DataLabel : ISerializable
    {
        public const int BaseAllocationSize = 0x8;

        public int LabelIndex;
        public string? Name;

        public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref LabelIndex);
            Serializer.Serialize(ref Name);
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