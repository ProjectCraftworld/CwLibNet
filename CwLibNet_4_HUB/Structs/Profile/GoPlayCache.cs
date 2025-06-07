using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using static net.torutheredfox.craftworld.serialization.Serializer;

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

        public void Serialize()
        {
            levelID = Serializer.Serialize(ref levelID);
            planetID = Serializer.Serialize(ref planetID);
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