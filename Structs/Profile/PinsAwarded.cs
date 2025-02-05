using CwLibNet.Resources;
using CwLibNet.Singleton;
using CwLibNet.Enums;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Inventory;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serialization;

namespace Cwlib.Structs.Profile
{
    public class PinsAwarded : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x30;
        public List<PinAward> pinAwards = new List();
        public List<PinProgress> pinProgress = new List();
        public int[] recentlyAwardedPinIDs;
        public int[] profileDisplayPinIDs;
        public byte pinsFlags;
        public byte weekdaysPlayedBits;
        public int moreOfEverythingPinProgress;
        public byte slappedAsBits;
        public int simonSaysProgress;
        public override void Serialize(Serializer serializer)
        {
            Revision revision = serializer.Serialize();
            int version = revision.Serialize();
            int subVersion = revision.Serialize();
            if (version >= Revisions.PROFILE_PINS)
            {
                pinAwards = serializer.Serialize(pinAwards, typeof(PinAward));
                pinProgress = serializer.Serialize(pinProgress, typeof(PinProgress));
                recentlyAwardedPinIDs = serializer.Serialize(recentlyAwardedPinIDs);
                profileDisplayPinIDs = serializer.Serialize(profileDisplayPinIDs);
            }

            if (version >= Revisions.PIN_FLAGS)
                pinsFlags = serializer.Serialize(pinsFlags);
            if (version >= Revisions.WEEKDAYS_PLAYED_PIN)
                weekdaysPlayedBits = serializer.Serialize(weekdaysPlayedBits);
            if (revision.Has(Branch.DOUBLE11, Revisions.D1_MOE_PIN_PROGRESS))
                moreOfEverythingPinProgress = serializer.Serialize(moreOfEverythingPinProgress);
            if (subVersion >= Revisions.SLAPPED_AS_PIN)
                slappedAsBits = serializer.Serialize(slappedAsBits);
            if (subVersion >= Revisions.SIMON_SAYS_PIN)
                simonSaysProgress = serializer.Serialize(simonSaysProgress);
        }

        public virtual bool HasPin(int id)
        {
            for (int i = 0; i < pinAwards.Count; ++i)
            {
                if (pinAwards[i].pinID == id)
                    return true;
            }

            return false;
        }

        public virtual int GetAllocatedSize()
        {
            int size = PinsAwarded.BASE_ALLOCATION_SIZE;
            if (this.pinAwards != null)
                size += (this.pinAwards.Count * PinAward.BASE_ALLOCATION_SIZE);
            if (this.pinProgress != null)
                size += (this.pinProgress.Count * PinProgress.BASE_ALLOCATION_SIZE);
            if (this.recentlyAwardedPinIDs != null)
                size += (this.recentlyAwardedPinIDs.length * 4);
            if (this.profileDisplayPinIDs != null)
                size += (this.profileDisplayPinIDs.length * 4);
            return size;
        }
    }
}