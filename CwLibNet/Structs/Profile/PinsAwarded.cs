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
        public const int BASE_ALLOCATION_SIZE = 0x30;

        public List<PinAward> pinAwards = new List<PinAward>();
        public List<PinProgress> pinProgress = new List<PinProgress>();
        public int[] recentlyAwardedPinIDs;
        public int[] profileDisplayPinIDs;

        public byte pinsFlags;
        public byte weekdaysPlayedBits;

        public int moreOfEverythingPinProgress;
        public byte slappedAsBits;
        public int simonSaysProgress;

        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int version = revision.GetVersion();
            int subVersion = revision.GetSubVersion();

            if (version >= (int)Revisions.ProfilePins) 
            {
                pinAwards = serializer.Arraylist<PinAward>(pinAwards);
                pinProgress = serializer.Arraylist<PinProgress>(pinProgress);
                recentlyAwardedPinIDs = serializer.Intvector(recentlyAwardedPinIDs);
                profileDisplayPinIDs = serializer.Intvector(profileDisplayPinIDs);
            }
            if (version >= (int)Revisions.PinFlags)
                pinsFlags = serializer.I8(pinsFlags);

            if (version >= (int)Revisions.WeekdaysPlayedPin)
                weekdaysPlayedBits = serializer.I8(weekdaysPlayedBits);

            if (revision.Has(Branch.Double11, (int)Revisions.D1MoePinProgress))
                moreOfEverythingPinProgress = serializer.I32(moreOfEverythingPinProgress);

            if (subVersion >= (int)Revisions.SlappedAsPin)
                slappedAsBits = serializer.I8(slappedAsBits);
            
            if (subVersion >= (int)Revisions.SimonSaysPin)
                simonSaysProgress = serializer.I32(simonSaysProgress);
        }

        public bool HasPin(int id) 
        {
            for (int i = 0; i < pinAwards.Count; i++) 
            {
                if (pinAwards[i].pinID == id) 
                {
                    return true;
                }
            }
            return false;
        }

        public int GetAllocatedSize() 
        {
            int size = BASE_ALLOCATION_SIZE;
            if (pinAwards != null) 
            {
                size += pinAwards.Count * PinAward.BASE_ALLOCATION_SIZE;
            }
            if (pinProgress != null) 
            {
                size += pinProgress.Count * PinProgress.BASE_ALLOCATION_SIZE;
            }
            if (recentlyAwardedPinIDs != null) 
            {
                size += recentlyAwardedPinIDs.Length * sizeof(int);
            }
            if (profileDisplayPinIDs != null) 
            {
                size += profileDisplayPinIDs.Length * sizeof(int);
            }
            return size;
        }
    }
}