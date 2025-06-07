using System.Numerics;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Streaming;

public class StreamingCheckpoint : ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public int Type;
    public Vector3? Position;
    public StreamingID? StartPointName;
    public SlotID SlotId;

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        if (subVersion > 0x72)
        {
            Serializer.Serialize(ref Type);
            Serializer.Serialize(ref Position);
        }

        if (subVersion > 0xdc)
            Serializer.Serialize(ref StartPointName);

        switch (subVersion)
        {
            case >= 0x13b:
                Serializer.Serialize(ref SlotId);
                break;
            case > 0x115 and <= 0x128:
                Serializer.Serialize(ref temp_int);
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