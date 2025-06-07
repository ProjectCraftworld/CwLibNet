using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
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

    
    public void Serialize()
    {
        Thing = Serializer.Reference(Thing);
        if (Serializer.GetRevision().GetVersion() > 0x326)
            Serializer.Serialize(ref Port);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}