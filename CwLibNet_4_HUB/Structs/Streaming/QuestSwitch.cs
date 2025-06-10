using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Streaming;

public class QuestSwitch: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public StreamingID? QuestId;
    public int QuestAction;
    public int QuestKey;
    public int ObjectiveId;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        if (subVersion > 0x118)
        {
            Serializer.Serialize(ref QuestId);
            Serializer.Serialize(ref QuestAction);
        }

        if (subVersion > 0x140)
            Serializer.Serialize(ref QuestKey);

        if (subVersion > 0x17a)
            Serializer.Serialize(ref ObjectiveId);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (QuestId != null)
            size += QuestId.GetAllocatedSize();
        return size;
    }


}