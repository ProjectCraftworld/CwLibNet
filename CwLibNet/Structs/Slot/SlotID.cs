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
        throw new NotImplementedException();
    }

    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
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
        int result = (int) (this.SlotNumber ^ (this.SlotNumber >>> 32));
        result = 31 * result + (this.SlotType.GetHashCode());
        return result; 
    }

    public override string ToString()
    {
        return String.Format("SlotID: SlotType={0}, SlotNumber={1}", SlotType, SlotNumber);
    }
}