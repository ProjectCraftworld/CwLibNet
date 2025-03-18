using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Slot;

public class SlotID: ISerializable
{
    public const int BaseAllocationSize = 0x10;
    public SlotType SlotType = SlotType.DEVELOPER;
    public long SlotNumber;
    public void Serialize(Serializer serializer)
    {
        SlotType = serializer.Enum32(SlotType);
        SlotNumber = serializer.U32(SlotNumber);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }

    public SlotID()
    {
    }

    public SlotID(SlotType type, long id)
    {
        this.SlotType = type;
        this.SlotNumber = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj == this) return true;
        if (obj is not SlotID id) return false;
        return SlotType.Equals(id.SlotType) && SlotNumber.Equals(id.SlotNumber);
    }

    public override int GetHashCode()
    {
        int result = (int) (SlotNumber ^ (SlotNumber >>> 32));
        result = 31 * result + SlotType.GetHashCode();
        return result; 
    }

    public override string ToString()
    {
        return String.Format("SlotID: SlotType={0}, SlotNumber={1}", SlotType, SlotNumber);
    }
}