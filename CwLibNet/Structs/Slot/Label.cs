using CwLibNet.IO;
using CwLibNet.IO.Serializer;

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

    public void Serialize(Serializer serializer)
    {
        Key = serializer.I32(Key);
        Order = serializer.I32(Order);
    }

    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }

}