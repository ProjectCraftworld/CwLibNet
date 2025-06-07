using System.Numerics;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        if (subVersion > 0x92)
            Serializer.Serialize(ref LevelSlot);

        if (subVersion is >= 0x93 and < 0xa1)
            Serializer.Serialize(ref temp_int);

        if (subVersion > 0x9a)
            Serializer.Serialize(ref Type);

        if (subVersion >= 0x93)
            Serializer.Serialize(ref ChunkFileList);

        if (subVersion > 0x92)
            Serializer.Serialize(ref Offset);
        if (subVersion > 0xa0)
        {
            Serializer.Serialize(ref Min);
            Serializer.Serialize(ref Max);
        }

        if (subVersion > 0xab)
            Serializer.Serialize(ref Parent);

        if (subVersion < 0x93 || subVersion > 0xa0)
        {
            if (subVersion >= 0x1ac)
                Serializer.Serialize(ref FarcGuid);
        }
        else serializer.Reference<LevelData>(null);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}