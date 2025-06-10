using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components.Switches;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Thing = Serializer.SerializeReference(Thing);
        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() > 0x326)
            Serializer.Serialize(ref Port);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}