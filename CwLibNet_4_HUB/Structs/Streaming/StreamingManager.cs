using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.EX;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Things;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Streaming;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        if (subVersion < 0xa1) 
            throw new SerializationException("Streaming manager below 0xa1 not supported!");
            
        if (subVersion >= 0x46)
            EditingThingsList = Serializer.SerializeArray(EditingThingsList);
        
        if (subVersion >= 0x46)
        {
            Serializer.Serialize(ref EditingThingsList);
        }

        switch (subVersion)
        {
            case < 0x4d:
                // 00c57988 - islands
                break;
            case >= 0x4d and < 0x73:
                int numChunks1 = 0;
                int numChunks2 = 0;
                Serializer.Serialize(ref numChunks1); // numChunks
                Serializer.Serialize(ref numChunks2); // numChunks
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
                LevelData = new LevelData[1]; 
                var levelDataItem = LevelData[0];
                Serializer.Serialize(ref levelDataItem);
                LevelData[0] = levelDataItem;
                break;
            case > 0xa0:
                LevelData = Serializer.GetCurrentSerializer().Array(LevelData);
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
                {
                    int tempNullInt = 0;
                    Serializer.Serialize(ref tempNullInt);
                }
                break;
        }

        if (subVersion >= 0x89)
            Serializer.Serialize(ref FartDesc, ResourceType.FileOfBytes);

        if (subVersion >= 0x8f)
            Serializer.Serialize(ref StartingPointPosition);

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