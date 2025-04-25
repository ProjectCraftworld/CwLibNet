using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Streaming;

public class QuestTracker: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public Vector3? Position;
    public StreamingID? QuestId;
    public StreamingID? ObjectiveId;
    public int QuestKey;
    public int ObjectiveKey;

    
    public void Serialize(Serializer serializer)
    {
        int subVersion = serializer.GetRevision().GetSubVersion();

        if (subVersion > 0xeb)
        {
            Position = serializer.V3(Position);
            QuestId = serializer.Struct(QuestId);
        }

        if (subVersion > 0xf3)
            ObjectiveId = serializer.Struct(QuestId);

        if (subVersion <= 0x140) return;
        QuestKey = serializer.I32(QuestKey);
        ObjectiveKey = serializer.I32(ObjectiveKey);

        // jenkins(?) hash of questID and objectiveID
        // for keys if revision too early
    }

    
    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (QuestId != null)
            size += QuestId.GetAllocatedSize();
        if (ObjectiveId != null)
            size += ObjectiveId.GetAllocatedSize();
        return size;
    }


}