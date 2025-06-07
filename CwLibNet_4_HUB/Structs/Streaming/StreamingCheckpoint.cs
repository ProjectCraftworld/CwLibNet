using System.Numerics;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Streaming;

public class StreamingCheckpoint : ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public int Type;
    public Vector3? Position;
    public StreamingID? StartPointName;
    public SlotID SlotId;

    
    public void Serialize()
    {
        var subVersion = Serializer.GetRevision().GetSubVersion();

        if (subVersion > 0x72)
        {
            Serializer.Serialize(ref Type);
            Position = Serializer.Serialize(ref Position);
        }

        if (subVersion > 0xdc)
            Serializer.Serialize(ref StartPointName);

        switch (subVersion)
        {
            case >= 0x13b:
                Serializer.Serialize(ref SlotId);
                break;
            case > 0x115 and <= 0x128:
                Serializer.Serialize(ref 0);
                break;
        }
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (StartPointName != null)
            size += StartPointName.GetAllocatedSize();
        return size;
    }


}