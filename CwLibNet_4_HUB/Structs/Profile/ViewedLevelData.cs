using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class ViewedLevelData : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x30;

        public SlotID slotID;
        public int lastReviewCount;
        public int lastCommentCount;
        public int lastPhotoCount;
        public int lastAuthorPhotoCount;
        public long lastStreamEventTimestamp;
        public long lastViewedTimestamp;

        public void Serialize() 
        {
            slotID = Serializer.Serialize(ref slotID);
            Serializer.Serialize(ref lastReviewCount);
            Serializer.Serialize(ref lastCommentCount);
            Serializer.Serialize(ref lastPhotoCount);
            Serializer.Serialize(ref lastAuthorPhotoCount);
            lastStreamEventTimestamp = Serializer.Serialize(ref lastStreamEventTimestamp);
            lastViewedTimestamp = Serializer.Serialize(ref lastViewedTimestamp);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}