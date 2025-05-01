using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Streaming;

public class LevelData : ISerializable
{
    public const int BaseAllocationSize = 0x100;

    public SlotID? LevelSlot;
    public int Type;
    public List<ChunkFile> ChunkFileList = [];
    public Vector3? Offset;
    public Vector3? Min;
    public Vector3? Max;
    public LevelData? Parent;
    public GUID? FarcGuid;

    
    public void Serialize(Serializer serializer)
    {
        var subVersion = serializer.GetRevision().GetSubVersion();

        if (subVersion > 0x92)
            LevelSlot = serializer.Struct(LevelSlot);

        if (subVersion is >= 0x93 and < 0xa1)
            serializer.I32(0);

        if (subVersion > 0x9a)
            Type = serializer.I32(Type);

        if (subVersion >= 0x93)
            ChunkFileList = serializer.Arraylist(ChunkFileList, true);

        if (subVersion > 0x92)
            Offset = serializer.V3(Offset);
        if (subVersion > 0xa0)
        {
            Min = serializer.V3(Min);
            Max = serializer.V3(Max);
        }

        if (subVersion > 0xab)
            Parent = serializer.Reference(Parent);

        if (subVersion < 0x93 || subVersion > 0xa0)
        {
            if (subVersion >= 0x1ac)
                FarcGuid = serializer.Guid(FarcGuid);
        }
        else serializer.Reference<LevelData>(null);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}