using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Profile 
{
    public class DataLabel : ISerializable
    {
        public const int BaseAllocationSize = 0x8;

        public int LabelIndex;
        public string? Name;

        public void Serialize()
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