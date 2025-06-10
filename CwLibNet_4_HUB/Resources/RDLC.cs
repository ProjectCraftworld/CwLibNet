using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.DLC;
using CwLibNet4Hub.Types.Data;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Resources;

public class RDLC : Resource
{
    public const int BaseAllocationSize = 0x10;

    public List<DLCGUID>? GUIDs = [];
    public List<DLCFile>? Files = [];

    public override void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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