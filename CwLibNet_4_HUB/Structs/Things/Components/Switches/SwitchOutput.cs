using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Things.Components.Switches;

public class SwitchOutput: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public SwitchSignal Activation = new();
    public SwitchTarget[]? TargetList;

    
    public string? UserDefinedName;

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref Activation);
        Serializer.Serialize(ref TargetList);
        if (version >= 0x34d)
            Serializer.Serialize(ref UserDefinedName);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (UserDefinedName != null) size += UserDefinedName.Length * 2;
        return size;
    }


}