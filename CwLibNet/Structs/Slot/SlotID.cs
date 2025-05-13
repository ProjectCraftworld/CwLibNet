using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Slot;

public class SlotID: ISerializable
{
    public const int BASE_ALLOCATION_SIZE = 0x10;
    public SlotType SlotType = SlotType.DEVELOPER;
    public long SlotNumber;
    public void Serialize(Serializer serializer)
    {
        SlotType = serializer.Enum32(SlotType);
        SlotNumber = serializer.U32(SlotNumber);
    }

    public SlotID()
    {
    }

    public SlotID(SlotType type, long id)
    {
        SlotType = type;
        SlotNumber = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj == this) return true;
        if (obj is not SlotID id) return false;
        return SlotType.Equals(id.SlotType) && SlotNumber.Equals(id.SlotNumber);
    }

    public override int GetHashCode()
    {
        var result = (int) (SlotNumber ^ (SlotNumber >>> 32));
        result = 31 * result + SlotType.GetHashCode();
        return result; 
    }

    public override string ToString()
    {
        return $"SlotID: SlotType={SlotType}, SlotNumber={SlotNumber}";
    }

    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }
}