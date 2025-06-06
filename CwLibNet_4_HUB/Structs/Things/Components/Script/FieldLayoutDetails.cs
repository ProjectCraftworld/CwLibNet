using CwLibNet.Enums;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Script;

public class FieldLayoutDetails: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public string? Name;
    public HashSet<ModifierType>? Modifiers = [];
    public MachineType MachineType = MachineType.VOID;

    
    public BuiltinType FishType = BuiltinType.VOID;

    public byte DimensionCount;
    public MachineType ArrayBaseMachineType = MachineType.VOID;
    public int InstanceOffset;

    public object? Value;

    public FieldLayoutDetails() { }

    public FieldLayoutDetails(FieldLayoutDetails details)
    {
        Name = details.Name;
        Modifiers = new HashSet<ModifierType>(details.Modifiers);
        MachineType = details.MachineType;
        FishType = details.FishType;
        DimensionCount = details.DimensionCount;
        ArrayBaseMachineType = details.ArrayBaseMachineType;
        InstanceOffset = details.InstanceOffset;
    }

    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        Serializer.Serialize(ref Name);

        if (Serializer.IsWriting())
        {
            var flags = ModifierBody.GetFlags(Modifiers);
            if (version >= 0x3d9) Serializer.GetOutput().I16(flags);
            else Serializer.GetOutput().I32(flags);
        }
        else
        {
            var flags = version >= 0x3d9 ? Serializer.GetInput().I16() :
                Serializer.GetInput().I32();
            Modifiers = ModifierBody.fromValue(flags);
        }

        Serializer.Serialize(ref MachineType);
        if (version >= 0x145)
            Serializer.Serialize(ref FishType);

        Serializer.Serialize(ref DimensionCount);
        Serializer.Serialize(ref ArrayBaseMachineType);

        Serializer.Serialize(ref InstanceOffset);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null) size += Name.Length;
        return size;
    }


}