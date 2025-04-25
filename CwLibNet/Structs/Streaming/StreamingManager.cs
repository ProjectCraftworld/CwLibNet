using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Types.Things;

namespace CwLibNet.Structs.Streaming;

public class StreamingManager: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public Thing[]? EditingThingsList;
    public LevelData[] LevelData;
    public int NumIslands;
    public int NumPendingIslands;
    public ResourceDescriptor? FartDesc;
    public Vector3? StartingPointPosition;
    public int StreamingZoneShape;
    public int StreamingZoneSize;
    public int NumInUsePlans;
    public int MaintainIslandsCount;
    public Thing[][] MaintainIslandList;

    
    public void Serialize(Serializer serializer)
    {
        int subVersion = serializer.GetRevision().GetSubVersion();

        EditingThingsList = subVersion switch
        {
            < 0xa1 => throw new SerializationException("Streaming manager below 0xa1 not supported!"),
            >= 0x46 => serializer.Thingarray(EditingThingsList),
            // _ => EditingThingsList
        };

        switch (subVersion)
        {
            case < 0x4d:
                // 00c57988 - islands
                break;
            case >= 0x4d and < 0x73:
                serializer.I32(0); // numChunks
                serializer.I32(0); // numChunks
                break;
            case >= 0x73 and < 0x93:
                // serializer.Array(null, ChunkFile.class, true);
                break;
            case >= 0x93 and < 0xa1 when serializer.IsWriting():
            {
                LevelData data = null;
                if (LevelData != null && LevelData.Length != 0)
                    data = LevelData[0];
                serializer.Struct(data);
                break;
            }
            case >= 0x93 and < 0xa1:
                LevelData = [serializer.Struct<LevelData>(null)];
                break;
            case > 0xa0:
                LevelData = serializer.Array(LevelData, true);
                break;
        }

        if (subVersion > 0x76)
            NumIslands = serializer.I32(NumIslands);
        if (subVersion > 0x1ff)
            NumPendingIslands = serializer.I32(NumPendingIslands);

        if (subVersion is >= 0x4e and <= 0x7e)
            serializer.V3(null);

        if (subVersion >= 0x89)
            FartDesc = serializer.Resource(FartDesc, ResourceType.FileOfBytes,
                true);

        if (subVersion >= 0x8f)
            StartingPointPosition = serializer.V3(StartingPointPosition);

        if (subVersion < 0x160) return;
        StreamingZoneShape = serializer.I32(StreamingZoneShape);
        StreamingZoneSize = serializer.I32(StreamingZoneSize);

        NumInUsePlans = serializer.I32(NumInUsePlans);
        if (subVersion >= 0x1a9)
            MaintainIslandsCount = serializer.I32(MaintainIslandsCount);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}