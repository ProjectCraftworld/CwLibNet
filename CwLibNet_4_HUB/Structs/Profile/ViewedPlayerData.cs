using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Slot;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Profile
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

        public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref OnlineId);
            Serializer.Serialize(ref LastReviewCount);
            Serializer.Serialize(ref LastCommentCount);
            Serializer.Serialize(ref LastPhotosByMeCount);
            Serializer.Serialize(ref LastPhotosWithMeCount);
            Serializer.Serialize(ref LastFavouriteSlotsCount);
            Serializer.Serialize(ref LastFavoriteUsersCount);
            Serializer.Serialize(ref LastStreamEventTimestamp);
            Serializer.Serialize(ref LastViewedTimestamp);
        }

        public int GetAllocatedSize() 
        {
            return BaseAllocationSize;
        }
    }
}