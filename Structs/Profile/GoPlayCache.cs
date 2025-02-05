using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class GoPlayCache : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;
        public SlotID levelID;
        public SlotID planetID;
        public byte numBubbleCollected;
        public byte awardLevelComplete;
        public byte awardCollectAll;
        public byte awardAcedLevel;
        public override void Serialize(Serializer serializer)
        {
            levelID = serializer.Serialize(levelID, typeof(SlotID));
            planetID = serializer.Serialize(planetID, typeof(SlotID));
            numBubbleCollected = serializer.Serialize(numBubbleCollected);
            awardLevelComplete = serializer.Serialize(awardLevelComplete);
            awardCollectAll = serializer.Serialize(awardCollectAll);
            awardAcedLevel = serializer.Serialize(awardAcedLevel);
        }

        public virtual int GetAllocatedSize()
        {
            return GoPlayCache.BASE_ALLOCATION_SIZE;
        }
    }
}