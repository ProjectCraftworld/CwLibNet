using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Slot;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Profile
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

        public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer) 
        {
            Serializer.Serialize(ref slotID);
            Serializer.Serialize(ref lastReviewCount);
            Serializer.Serialize(ref lastCommentCount);
            Serializer.Serialize(ref lastPhotoCount);
            Serializer.Serialize(ref lastAuthorPhotoCount);
            Serializer.Serialize(ref lastStreamEventTimestamp);
            Serializer.Serialize(ref lastViewedTimestamp);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}