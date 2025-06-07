using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Popit;

public class ObjectState: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public Thing? Thing;
    public int BackZ;
    public int FrontZ;
    public int Flags;
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        Thing = Serializer.Reference(Thing);
        Serializer.Serialize(ref BackZ);
        Serializer.Serialize(ref FrontZ);

        if (version < 0x2bd) Serializer.Serialize(ref false);
        if (version is > 0x147 and < 0x2be) Serializer.Serialize(ref false);

        switch (version)
        {
            case > 0x2bc:
                Serializer.Serialize(ref Flags);
                break;
            case > 0x25e:
                Serializer.Serialize(ref false);
                break;
        }
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}