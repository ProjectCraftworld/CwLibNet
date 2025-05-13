using System;
using System.Collections.Generic;

using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class ViewedPlayerData : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x50;

        public NetworkOnlineID onlineID;
        public int lastReviewCount;
        public int lastCommentCount;
        public int lastPhotosByMeCount;
        public int lastPhotosWithMeCount;
        public int lastFavouriteSlotsCount;
        public int lastFavoriteUsersCount;
        public int lastStreamEventTimestamp;
        public long lastViewedTimestamp;

        public void Serialize(Serializer serializer)
        {
            onlineID = serializer.Struct<NetworkOnlineID>(onlineID);
            lastReviewCount = serializer.S32(lastReviewCount);
            lastCommentCount = serializer.S32(lastCommentCount);
            lastPhotosByMeCount = serializer.S32(lastPhotosByMeCount);
            lastPhotosWithMeCount = serializer.S32(lastPhotosWithMeCount);
            lastFavouriteSlotsCount = serializer.S32(lastFavouriteSlotsCount);
            lastFavoriteUsersCount = serializer.S32(lastFavoriteUsersCount);
            lastStreamEventTimestamp = serializer.S32(lastStreamEventTimestamp);
            lastViewedTimestamp = serializer.S64(lastViewedTimestamp);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}