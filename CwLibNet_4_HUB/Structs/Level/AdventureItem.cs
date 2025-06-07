using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Level;

public class AdventureItem: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public int Puid;
    public ResourceDescriptor? Descriptor;
    public int Flags;
    public int IconPuid;

    
    public void Serialize()
    {
        var subVersion = Serializer.GetRevision().GetSubVersion();

        Serializer.Serialize(ref Puid);
        Serializer.Serialize(ref Descriptor, Descriptor, ResourceType.Plan, true);

        switch (subVersion)
        {
            case < 0xae:
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref 0);
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