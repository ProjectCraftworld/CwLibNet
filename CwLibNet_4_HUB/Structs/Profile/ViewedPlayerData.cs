using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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

        public void Serialize()
        {
            Serializer.Serialize(ref OnlineId);
            Serializer.Serialize(ref LastReviewCount);
            Serializer.Serialize(ref LastCommentCount);
            Serializer.Serialize(ref LastPhotosByMeCount);
            Serializer.Serialize(ref LastPhotosWithMeCount);
            Serializer.Serialize(ref LastFavouriteSlotsCount);
            Serializer.Serialize(ref LastFavoriteUsersCount);
            Serializer.Serialize(ref LastStreamEventTimestamp);
            LastViewedTimestamp = Serializer.Serialize(ref LastViewedTimestamp);
        }

        public int GetAllocatedSize() 
        {
            return BaseAllocationSize;
        }
    }
}