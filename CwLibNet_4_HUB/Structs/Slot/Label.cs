using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
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

    public void Serialize()
    {
        Serializer.Serialize(ref Key);
        Serializer.Serialize(ref Order);
    }

    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }

}