using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Components;

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

    
    public void Serialize()
    {
        Serializer.Serialize(ref Fluff);
        Serializer.Serialize(ref Icon, Icon, ResourceType.Texture);
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