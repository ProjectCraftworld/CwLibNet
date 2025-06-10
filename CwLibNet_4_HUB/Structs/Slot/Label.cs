using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Slot;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Slot;

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

    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Key);
        Serializer.Serialize(ref Order);
    }

    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }

}