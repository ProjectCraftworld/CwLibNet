using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

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
        this.Name = details.Name;
        this.Modifiers = new HashSet<ModifierType>(details.Modifiers);
        this.MachineType = details.MachineType;
        this.FishType = details.FishType;
        this.DimensionCount = details.DimensionCount;
        this.ArrayBaseMachineType = details.ArrayBaseMachineType;
        this.InstanceOffset = details.InstanceOffset;
    }

    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        Name = serializer.Str(Name);

        if (serializer.IsWriting())
        {
            short flags = ModifierBody.GetFlags(Modifiers);
            if (version >= 0x3d9) serializer.GetOutput().I16(flags);
            else serializer.GetOutput().I32(flags);
        }
        else
        {
            int flags = (version >= 0x3d9) ? serializer.GetInput().I16() :
                serializer.GetInput().I32();
            Modifiers = ModifierBody.fromValue(flags);
        }

        MachineType = serializer.Enum32(MachineType);
        if (version >= 0x145)
            FishType = serializer.Enum32(FishType);

        DimensionCount = serializer.I8(DimensionCount);
        ArrayBaseMachineType = serializer.Enum32(ArrayBaseMachineType);

        InstanceOffset = serializer.I32(InstanceOffset);
    }

    
    public int GetAllocatedSize()
    {
        int size = FieldLayoutDetails.BaseAllocationSize;
        if (this.Name != null) size += (this.Name.Length);
        return size;
    }


}