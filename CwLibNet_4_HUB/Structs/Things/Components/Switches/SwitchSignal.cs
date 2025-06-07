using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Switches;

public class SwitchSignal: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public float Activation;
    
    public int Ternary;
    
    public int Player = -1;

    public SwitchSignal() { }

    public SwitchSignal(float activation)
    {
        Activation = activation;
    }

    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        Serializer.Serialize(ref Activation);
        if (version >= 0x310)
            Serializer.Serialize(ref Ternary);
        if (version >= 0x2a3)
            Serializer.Serialize(ref Player);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }



}