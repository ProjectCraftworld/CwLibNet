using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

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

    
    public void Serialize(Serializer serializer)
    {
        Fluff = serializer.I32(Fluff);
        Icon = serializer.Resource(Icon, ResourceType.Texture);
        Type = serializer.I32(Type);
        SubType = serializer.I32(SubType);
        LinkTo = serializer.Struct(LinkTo);
        LinkSlotId = serializer.Struct(LinkSlotId);
        LinkType = serializer.I32(LinkType);
        LastUsed = serializer.I32(LastUsed);
        NumUses = serializer.I32(NumUses);
        Name = serializer.Str(Name);
        Location = serializer.Str(Location);
        Category = serializer.Str(Category);
        Color = serializer.I8(Color);
        Size = serializer.I8(Size);
    }

    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (Name != null) size += Name.Length;
        if (Location != null) size += Location.Length;
        if (Category != null) size += Category.Length;
        return size;
    }
}