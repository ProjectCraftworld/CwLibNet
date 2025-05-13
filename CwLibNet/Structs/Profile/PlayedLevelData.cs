using System;
using System.Collections.Generic;

using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Slot;
using static CwLibNet.Structs.Slot.SlotID;
using CwLibNet.Types;

namespace CwLibNet.Structs.Profile 
{
    public class PlayedLevelData : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x80;

        public SlotID slotID;
        public long lastPlayedTimestamp;
        public int[] localHighScore;
        public short playCount, completionCount, acedCount;
        public CollectableData[] collectables;

        public int[] videos;
        public SlotID[] linkedLevels;
        public SlotID[] subLevels;
        public SHA1 levelHash;
        public short flags;

        // LBP3

        public int bestTime;
        public short multiplayerCompletionCount;

        // Vita

        public short deferredPlayCount, deferredPlayCountUploaded;

        public int[] uploadedLocalHighScore;

        public short goldTrophyCount, silverTrophyCount, bronzeTrophyCount;

        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int version = revision.GetVersion();
            int subVersion = revision.GetSubVersion();

            slotID = serializer.Struct<SlotID>(slotID);
            if (version > 0x1fd)
                lastPlayedTimestamp = serializer.S64(lastPlayedTimestamp);
            if (version > 0x200)
                localHighScore = serializer.Intarray(localHighScore);

            if (version > 0x268 && version < 0x399) 
            {
                serializer.Bool(false); // Discovered
                serializer.Bool(false); // Unlocked
            }

            if (version < 0x269) 
            {
                playCount = (short)serializer.I32(playCount);
                completionCount = (short)serializer.I32(completionCount);
                acedCount = (short)serializer.I32(acedCount);
            }
            else 
            {
                playCount = serializer.I16(playCount);
                completionCount = serializer.I16(completionCount);
                acedCount = serializer.I16(acedCount);
            }

            if (version > 0x1c1) 
                collectables = serializer.Array<CollectableData>(collectables);
            if (version > 0x1c3 && subVersion < 0x106)
                videos = serializer.Intarray(videos);
            
            if (version > 0x363) 
            {
                linkedLevels = serializer.Array<SlotID>(linkedLevels);
                subLevels = serializer.Array<SlotID>(subLevels);
            }

            if (revision.Has(Branch.Double11, (int)Revisions.D1DeferredPlays))
            {
                deferredPlayCount = serializer.I16(deferredPlayCount);
                deferredPlayCountUploaded = serializer.I16(deferredPlayCountUploaded);
                if (revision.Has(Branch.Double11, (int)Revisions.D1UploadedHighScore))
                    uploadedLocalHighScore = serializer.Intarray(uploadedLocalHighScore);
                if (revision.Has(Branch.Double11, (int)Revisions.D1Trophies))
                {
                    goldTrophyCount = serializer.I16(goldTrophyCount);
                    silverTrophyCount = serializer.I16(silverTrophyCount);
                    bronzeTrophyCount = serializer.I16(bronzeTrophyCount);
                }
            }

            if (subVersion >= 0x1ad)
            {
                bestTime = serializer.I32(bestTime);
                multiplayerCompletionCount = serializer.I16(multiplayerCompletionCount);
            }
        }

        public int GetAllocatedSize() 
        {
            int size = BASE_ALLOCATION_SIZE;
            if (localHighScore != null)
                size += localHighScore.Length * 4;
            if (collectables != null)
                size += collectables.Length * CollectableData.BASE_ALLOCATION_SIZE;
            if (linkedLevels != null)
                size += linkedLevels.Length * SlotID.BASE_ALLOCATION_SIZE;
            if (subLevels != null)
                size += subLevels.Length * SlotID.BASE_ALLOCATION_SIZE;
            if (uploadedLocalHighScore != null)
                size += uploadedLocalHighScore.Length * 4;
            return size;
        }
    }
}