using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Streaming;

public class QuestSwitch: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public StreamingID? QuestId;
    public int QuestAction;
    public int QuestKey;
    public int ObjectiveId;

    
    public void Serialize(Serializer serializer)
    {
        int subVersion = serializer.GetRevision().GetSubVersion();

        if (subVersion > 0x118)
        {
            QuestId = serializer.Struct(QuestId);
            QuestAction = serializer.I32(QuestAction);
        }

        if (subVersion > 0x140)
            QuestKey = serializer.I32(QuestKey);

        if (subVersion > 0x17a)
            ObjectiveId = serializer.I32(ObjectiveId);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (QuestId != null)
            size += QuestId.GetAllocatedSize();
        return size;
    }


}