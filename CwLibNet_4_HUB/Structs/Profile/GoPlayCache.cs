using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class GoPlayCache : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x20;

        public SlotID levelID;
        public SlotID planetID;
        public byte numBubbleCollected;
        public byte awardLevelComplete;
        public byte awardCollectAll;
        public byte awardAcedLevel;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref levelID);
            Serializer.Serialize(ref planetID);
            Serializer.Serialize(ref numBubbleCollected);
            Serializer.Serialize(ref awardLevelComplete);
            Serializer.Serialize(ref awardCollectAll);
            Serializer.Serialize(ref awardAcedLevel);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}