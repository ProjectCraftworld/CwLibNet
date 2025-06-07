using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.DLC;
using CwLibNet.Types.Data;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Resources;

public class RDLC : Resource
{
    public const int BaseAllocationSize = 0x10;

    public List<DLCGUID>? GUIDs = [];
    public List<DLCFile>? Files = [];

    public override void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
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
        Serializer.SetCurrentSerializer(serializer);
        Serialize(serializer);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.DownloadableContent,
            SerializationType.BINARY,
            serializer.GetDependencies()
        );
    }
}