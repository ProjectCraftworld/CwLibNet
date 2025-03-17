using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Things;

namespace CwLibNet.Structs.Things.Components.Popit;

public class ObjectState: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public Thing? Thing;
    public int BackZ;
    public int FrontZ;
    public int Flags;
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        Thing = serializer.Thing(Thing);
        BackZ = serializer.S32(BackZ);
        FrontZ = serializer.S32(FrontZ);

        if (version < 0x2bd) serializer.Bool(false);
        if (version is > 0x147 and < 0x2be) serializer.Bool(false);

        if (version > 0x2bc)
            Flags = serializer.I32(Flags);
        else if (version > 0x25e) serializer.Bool(false);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}