using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.DLC;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Resources;

public class RDLC : Resource
{
    public const int BaseAllocationSize = 0x10;

    public List<DLCGUID>? GUIDs = [];
    public List<DLCFile>? Files = [];

    public override void Serialize()
    {
        Serializer.Serialize(ref GUIDs);
        Serializer.Serialize(ref Files);
    }

    public override int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (GUIDs != null) size += GUIDs.Count * DLCGUID.BaseAllocationSize;
        if (Files == null) return size;
        size += Files.Sum(file => file.GetAllocatedSize());
        return size;
    }

    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        var serializer = new Serializer(GetAllocatedSize(), revision, compressionFlags);
        Serializer.Serialize(ref this);
        return new SerializationData(
            Serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.DownloadableContent,
            SerializationType.BINARY,
            Serializer.GetDependencies()
        );
    }
}