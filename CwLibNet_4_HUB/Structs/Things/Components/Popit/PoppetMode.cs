using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Things.Components.Popit;

public class PoppetMode: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public int Mode, SubMode;
    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref Mode);
        Serializer.Serialize(ref SubMode);

        switch (version)
        {
            case < 0x18b:
                Serializer.Serialize(ref temp_int);
                break;
            case > 0x1b7 and < 0x1ba:
                Serializer.Serialize(ref temp_int);
                Serializer.Serialize(ref temp_int);
                Serializer.Serialize(ref temp_int);
                Serializer.Serialize(ref temp_int);
                break;
        }
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}