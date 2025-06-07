using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Level;

public class AdventureItem: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public int Puid;
    public ResourceDescriptor? Descriptor;
    public int Flags;
    public int IconPuid;

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        Serializer.Serialize(ref Puid);
        Serializer.Serialize(ref Descriptor, ResourceType.Plan, true, false, false);

        switch (subVersion)
        {
            case < 0xae:
                int temp1 = 0, temp2 = 0, temp3 = 0, temp4 = 0;
                Serializer.Serialize(ref temp1);
                Serializer.Serialize(ref temp2);
                Serializer.Serialize(ref temp3);
                Serializer.Serialize(ref temp4);
                break;
            case > 0xbe:
                Serializer.Serialize(ref Flags);
                break;
        }

        if (subVersion > 0xe0)
            Serializer.Serialize(ref IconPuid);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}