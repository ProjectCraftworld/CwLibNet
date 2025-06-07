using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Popit;

public class PoppetMode: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public int Mode, SubMode;
    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        Serializer.Serialize(ref Mode);
        Serializer.Serialize(ref SubMode);

        switch (version)
        {
            case < 0x18b:
                Serializer.Serialize(ref 0);
                break;
            case > 0x1b7 and < 0x1ba:
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                break;
        }
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}