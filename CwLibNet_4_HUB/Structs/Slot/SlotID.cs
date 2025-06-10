using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Slot;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Slot;

public class SlotID: ISerializable
{
    public const int BaseAllocationSize = 0x10;
    public SlotType SlotType = SlotType.DEVELOPER;
    public long SlotNumber;
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.SerializeEnum32(ref SlotType);
        Serializer.Serialize(ref SlotNumber);
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
        return BaseAllocationSize;
    }
}