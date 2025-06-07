using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.Structs.Things;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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

    
    public void Serialize()
    {
        var subVersion = Serializer.GetRevision().GetSubVersion();

        EditingThingsList = subVersion switch
        {
            < 0xa1 => throw new SerializationException("Streaming manager below 0xa1 not supported!"),
            >= 0x46 => Serializer.Serialize(ref EditingThingsList),
            // _ => EditingThingsList
        };

        switch (subVersion)
        {
            case < 0x4d:
                // 00c57988 - islands
                break;
            case >= 0x4d and < 0x73:
                Serializer.Serialize(ref 0); // numChunks
                Serializer.Serialize(ref 0); // numChunks
                break;
            case >= 0x73 and < 0x93:
                // Serializer.Serialize(ref null, ChunkFile.class, true);
                break;
            case >= 0x93 and < 0xa1 when Serializer.IsWriting():
            {
                LevelData data = null;
                if (LevelData != null && LevelData.Length != 0)
                    data = LevelData[0];
                Serializer.Serialize(ref data);
                break;
            }
            case >= 0x93 and < 0xa1:
                LevelData = new LevelData[1]; Serializer.Serialize(ref LevelData[0]);
                break;
            case > 0xa0:
                LevelData = Serializer.Serialize(ref LevelData, true);
                break;
        }

        if (subVersion > 0x76)
            Serializer.Serialize(ref NumIslands);
        switch (subVersion)
        {
            case > 0x1ff:
                Serializer.Serialize(ref NumPendingIslands);
                break;
            case >= 0x4e and <= 0x7e:
                Serializer.Serialize(ref null);
                break;
        }

        if (subVersion >= 0x89)
            FartDesc = Serializer.Serialize(ref FartDesc, ResourceType.FileOfBytes);

        if (subVersion >= 0x8f)
            StartingPointPosition = Serializer.Serialize(ref StartingPointPosition);

        if (subVersion < 0x160) return;
        Serializer.Serialize(ref StreamingZoneShape);
        Serializer.Serialize(ref StreamingZoneSize);

        Serializer.Serialize(ref NumInUsePlans);
        if (subVersion >= 0x1a9)
            Serializer.Serialize(ref MaintainIslandsCount);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}