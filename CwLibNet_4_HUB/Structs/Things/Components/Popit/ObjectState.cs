using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Things.Components.Popit;

public class ObjectState: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public Thing? Thing;
    public int BackZ;
    public int FrontZ;
    public int Flags;
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Thing = Serializer.SerializeReference(Thing);
        Serializer.Serialize(ref BackZ);
        Serializer.Serialize(ref FrontZ);

        if (version < 0x2bd) Serializer.Serialize(ref temp_bool_false);
        if (version is > 0x147 and < 0x2be) Serializer.Serialize(ref temp_bool_false);

        switch (version)
        {
            case > 0x2bc:
                Serializer.Serialize(ref Flags);
                break;
            case > 0x25e:
                Serializer.Serialize(ref temp_bool_false);
                break;
        }
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}