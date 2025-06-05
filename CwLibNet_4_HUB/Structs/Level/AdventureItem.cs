using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Level;

public class AdventureItem: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public int Puid;
    public ResourceDescriptor? Descriptor;
    public int Flags;
    public int IconPuid;

    
    public void Serialize(Serializer serializer)
    {
        var subVersion = serializer.GetRevision().GetSubVersion();

        Puid = serializer.I32(Puid);
        Descriptor = serializer.Resource(Descriptor, ResourceType.Plan, true);

        switch (subVersion)
        {
            case < 0xae:
                serializer.U16(0);
                serializer.U8(0);
                serializer.F32(0);
                serializer.I32(0);
                break;
            case > 0xbe:
                Flags = serializer.I32(Flags);
                break;
        }

        if (subVersion > 0xe0)
            IconPuid = serializer.I32(IconPuid);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}