using CwLibNet.IO;
using CwLibNet.IO.Serializer;

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

    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();

        Activation = serializer.F32(Activation);
        if (version >= 0x310)
            Ternary = serializer.S32(Ternary);
        if (version >= 0x2a3)
            Player = serializer.I32(Player);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }



}