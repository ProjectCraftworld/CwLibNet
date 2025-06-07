using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Profile 
{
    public class PinsAwarded : ISerializable
    {
        public const int BaseAllocationSize = 0x30;

        public List<PinAward> PinAwards = [];
        public List<PinProgress> PinProgress = [];
        public int[]? RecentlyAwardedPinIDs;
        public int[]? ProfileDisplayPinIDs;

        public byte PinsFlags;
        public byte WeekdaysPlayedBits;

        public int MoreOfEverythingPinProgress;
        public byte SlappedAsBits;
        public int SimonSaysProgress;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
            var revision = Serializer.GetCurrentSerializer().GetRevision();
            var version = revision.GetVersion();
            var subVersion = revision.GetSubVersion();

            if (version >= (int)Revisions.PROFILE_PINS) 
            {
                Serializer.Serialize(ref PinAwards);
                Serializer.Serialize(ref PinProgress);
                Serializer.Serialize(ref RecentlyAwardedPinIDs);
                Serializer.Serialize(ref ProfileDisplayPinIDs);
            }
            if (version >= (int)Revisions.PIN_FLAGS)
                Serializer.Serialize(ref PinsFlags);

            if (version >= (int)Revisions.WEEKDAYS_PLAYED_PIN)
                Serializer.Serialize(ref WeekdaysPlayedBits);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_MOE_PIN_PROGRESS))
                Serializer.Serialize(ref MoreOfEverythingPinProgress);

            if (subVersion >= (int)Revisions.SLAPPED_AS_PIN)
                Serializer.Serialize(ref SlappedAsBits);
            
            if (subVersion >= (int)Revisions.SIMON_SAYS_PIN)
                Serializer.Serialize(ref SimonSaysProgress);
        }

        public bool HasPin(int id)
        {
            return PinAwards.Any(t => t.pinID == id);
        }

        public int GetAllocatedSize() 
        {
            var size = BaseAllocationSize;
            if (PinAwards != null) 
            {
                size += PinAwards.Count * PinAward.BASE_ALLOCATION_SIZE;
            }
            if (PinProgress != null) 
            {
                size += PinProgress.Count * Profile.PinProgress.BASE_ALLOCATION_SIZE;
            }
            if (RecentlyAwardedPinIDs != null) 
            {
                size += RecentlyAwardedPinIDs.Length * sizeof(int);
            }
            if (ProfileDisplayPinIDs != null) 
            {
                size += ProfileDisplayPinIDs.Length * sizeof(int);
            }
            return size;
        }
    }
}