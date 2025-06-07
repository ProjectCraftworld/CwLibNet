using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Switches;

public class SwitchOutput: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public SwitchSignal Activation = new();
    public SwitchTarget[]? TargetList;

    
    public string? UserDefinedName;

    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        Serializer.Serialize(ref Activation);
        TargetList = Serializer.Serialize(ref TargetList);
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