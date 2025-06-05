using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.Switches;

public class SwitchTarget: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public Thing? Thing;

    
    public int Port;

    public SwitchTarget() { }

    public SwitchTarget(Thing? thing)
    {
        Thing = thing;
    }

    
    public void Serialize(Serializer serializer)
    {
        Thing = serializer.Thing(Thing);
        if (serializer.GetRevision().GetVersion() > 0x326)
            Port = serializer.I32(Port);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}