using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components.Switches;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

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