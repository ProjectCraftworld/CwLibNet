using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.Switches;

public class SwitchOutput: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public SwitchSignal Activation = new();
    public SwitchTarget[]? TargetList;

    
    public string? UserDefinedName;

    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();

        Activation = serializer.Struct(Activation);
        TargetList = serializer.Array(TargetList);
        if (version >= 0x34d)
            UserDefinedName = serializer.Wstr(UserDefinedName);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (UserDefinedName != null) size += UserDefinedName.Length * 2;
        return size;
    }


}