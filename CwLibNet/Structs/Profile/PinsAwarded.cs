using System;
using System.Collections.Generic;

using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types;
using CwLibNet.Types.Data;

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

        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int version = revision.GetVersion();
            int subVersion = revision.GetSubVersion();

            if (version >= (int)Revisions.PROFILE_PINS) 
            {
                PinAwards = serializer.Arraylist(PinAwards);
                PinProgress = serializer.Arraylist(PinProgress);
                RecentlyAwardedPinIDs = serializer.Intvector(RecentlyAwardedPinIDs);
                ProfileDisplayPinIDs = serializer.Intvector(ProfileDisplayPinIDs);
            }
            if (version >= (int)Revisions.PIN_FLAGS)
                PinsFlags = serializer.I8(PinsFlags);

            if (version >= (int)Revisions.WEEKDAYS_PLAYED_PIN)
                WeekdaysPlayedBits = serializer.I8(WeekdaysPlayedBits);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_MOE_PIN_PROGRESS))
                MoreOfEverythingPinProgress = serializer.I32(MoreOfEverythingPinProgress);

            if (subVersion >= (int)Revisions.SLAPPED_AS_PIN)
                SlappedAsBits = serializer.I8(SlappedAsBits);
            
            if (subVersion >= (int)Revisions.SIMON_SAYS_PIN)
                SimonSaysProgress = serializer.I32(SimonSaysProgress);
        }

        public bool HasPin(int id)
        {
            return PinAwards.Any(t => t.pinID == id);
        }

        public int GetAllocatedSize() 
        {
            int size = BaseAllocationSize;
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