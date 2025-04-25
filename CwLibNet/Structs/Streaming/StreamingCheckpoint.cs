using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;

namespace CwLibNet.Structs.Streaming;

public class StreamingCheckpoint : ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public int Type;
    public Vector3? Position;
    public StreamingID? StartPointName;
    public SlotID SlotId;

    
    public void Serialize(Serializer serializer)
    {
        int subVersion = serializer.GetRevision().GetSubVersion();

        if (subVersion > 0x72)
        {
            Type = serializer.I32(Type);
            Position = serializer.V3(Position);
        }

        if (subVersion > 0xdc)
            StartPointName = serializer.Struct(StartPointName);

        switch (subVersion)
        {
            case >= 0x13b:
                SlotId = serializer.Struct(SlotId);
                break;
            case > 0x115 and <= 0x128:
                serializer.S32(0);
                break;
        }
    }

    
    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (StartPointName != null)
            size += StartPointName.GetAllocatedSize();
        return size;
    }


}