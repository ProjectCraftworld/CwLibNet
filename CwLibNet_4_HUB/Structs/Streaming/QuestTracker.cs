using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Streaming;

public class QuestTracker: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public Vector3? Position;
    public StreamingID? QuestId;
    public StreamingID? ObjectiveId;
    public int QuestKey;
    public int ObjectiveKey;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        if (subVersion > 0xeb)
        {
            Serializer.Serialize(ref Position);
            Serializer.Serialize(ref QuestId);
        }

        if (subVersion > 0xf3)
            Serializer.Serialize(ref QuestId);

        if (subVersion <= 0x140) return;
        Serializer.Serialize(ref QuestKey);
        Serializer.Serialize(ref ObjectiveKey);

        // jenkins(?) hash of questID and objectiveID
        // for keys if revision too early
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (QuestId != null)
            size += QuestId.GetAllocatedSize();
        if (ObjectiveId != null)
            size += ObjectiveId.GetAllocatedSize();
        return size;
    }


}