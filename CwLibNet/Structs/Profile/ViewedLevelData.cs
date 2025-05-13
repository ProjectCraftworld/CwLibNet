using System;
using System.Collections.Generic;

using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;

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

        public void Serialize(Serializer serializer) 
        {
            slotID = serializer.Struct<SlotID>(slotID);
            lastReviewCount = serializer.S32(lastReviewCount);
            lastCommentCount = serializer.S32(lastCommentCount);
            lastPhotoCount = serializer.S32(lastPhotoCount);
            lastAuthorPhotoCount = serializer.S32(lastAuthorPhotoCount);
            lastStreamEventTimestamp = serializer.S64(lastStreamEventTimestamp);
            lastViewedTimestamp = serializer.S64(lastViewedTimestamp);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}