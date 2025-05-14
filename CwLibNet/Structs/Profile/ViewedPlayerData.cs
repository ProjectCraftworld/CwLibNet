using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class ViewedPlayerData : ISerializable
    {
        public const int BaseAllocationSize = 0x50;

        public NetworkOnlineId? OnlineId;
        public int LastReviewCount;
        public int LastCommentCount;
        public int LastPhotosByMeCount;
        public int LastPhotosWithMeCount;
        public int LastFavouriteSlotsCount;
        public int LastFavoriteUsersCount;
        public int LastStreamEventTimestamp;
        public long LastViewedTimestamp;

        public void Serialize(Serializer serializer)
        {
            OnlineId = serializer.Struct(OnlineId);
            LastReviewCount = serializer.S32(LastReviewCount);
            LastCommentCount = serializer.S32(LastCommentCount);
            LastPhotosByMeCount = serializer.S32(LastPhotosByMeCount);
            LastPhotosWithMeCount = serializer.S32(LastPhotosWithMeCount);
            LastFavouriteSlotsCount = serializer.S32(LastFavouriteSlotsCount);
            LastFavoriteUsersCount = serializer.S32(LastFavoriteUsersCount);
            LastStreamEventTimestamp = serializer.S32(LastStreamEventTimestamp);
            LastViewedTimestamp = serializer.S64(LastViewedTimestamp);
        }

        public int GetAllocatedSize() 
        {
            return BaseAllocationSize;
        }
    }
}