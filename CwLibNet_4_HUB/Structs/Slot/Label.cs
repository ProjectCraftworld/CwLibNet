using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Slot;

public class Label : ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x8;

    public int Key;
    public int Order;

    public Label() { }

    public Label(int key, int order)
    {
        Key = key;
        Order = order;
    }

    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Key);
        Serializer.Serialize(ref Order);
    }

    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }

}