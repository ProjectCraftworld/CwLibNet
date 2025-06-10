using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Slot;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components;

public class Value: ISerializable
{
    public const int BaseAllocationSize = 0x100;

    public int Fluff;
    public ResourceDescriptor? Icon;
    public int Type, SubType;
    public GlobalThingDescriptor? LinkTo;
    public SlotID? LinkSlotId;
    public int LinkType;
    public int LastUsed, NumUses;
    public string? Name;
    public string? Location;
    public string? Category;
    public byte Color, Size;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Fluff);
        Serializer.Serialize(ref Icon, ResourceType.Texture, true, true, false);
        Serializer.Serialize(ref Type);
        Serializer.Serialize(ref SubType);
        Serializer.Serialize(ref LinkTo);
        Serializer.Serialize(ref LinkSlotId);
        Serializer.Serialize(ref LinkType);
        Serializer.Serialize(ref LastUsed);
        Serializer.Serialize(ref NumUses);
        Serializer.Serialize(ref Name);
        Serializer.Serialize(ref Location);
        Serializer.Serialize(ref Category);
        Serializer.Serialize(ref Color);
        Serializer.Serialize(ref Size);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null) size += Name.Length;
        if (Location != null) size += Location.Length;
        if (Category != null) size += Category.Length;
        return size;
    }
}