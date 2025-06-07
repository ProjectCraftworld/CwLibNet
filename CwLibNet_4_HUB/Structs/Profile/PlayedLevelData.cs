using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Slot;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class PlayedLevelData : ISerializable 
    {
        public const int BaseAllocationSize = 0x80;

        public SlotID SlotId;
        public long LastPlayedTimestamp;
        public int[]? LocalHighScore;
        public short PlayCount, CompletionCount, AcedCount;
        public CollectableData[]? Collectables;

        public int[] Videos;
        public SlotID[]? LinkedLevels;
        public SlotID[]? SubLevels;
        public Sha1 LevelHash;
        public short Flags;

        // LBP3

        public int BestTime;
        public short MultiplayerCompletionCount;

        // Vita

        public short DeferredPlayCount, DeferredPlayCountUploaded;

        public int[]? UploadedLocalHighScore;

        public short GoldTrophyCount, SilverTrophyCount, BronzeTrophyCount;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

            var revision = Serializer.GetCurrentSerializer().GetRevision();
            var version = revision.GetVersion();
            var subVersion = revision.GetSubVersion();

            Serializer.Serialize(ref SlotId);
            if (version > 0x1fd)
                Serializer.Serialize(ref LastPlayedTimestamp);
            if (version > 0x200)
                Serializer.Serialize(ref LocalHighScore);

            if (version is > 0x268 and < 0x399) 
            {
                Serializer.Serialize(ref temp_bool_false); // Discovered
                Serializer.Serialize(ref temp_bool_false); // Unlocked
            }

            if (version < 0x269) 
            {
                Serializer.Serialize(ref PlayCount);
                Serializer.Serialize(ref CompletionCount);
                Serializer.Serialize(ref AcedCount);
            }
            else 
            {
                Serializer.Serialize(ref PlayCount);
                Serializer.Serialize(ref CompletionCount);
                Serializer.Serialize(ref AcedCount);
            }

            if (version > 0x1c1) 
                Serializer.Serialize(ref Collectables);
            if (version > 0x1c3 && subVersion < 0x106)
                Serializer.Serialize(ref Videos);
            
            if (version > 0x363) 
            {
                Serializer.Serialize(ref LinkedLevels);
                Serializer.Serialize(ref SubLevels);
            }

            if (revision.Has(Branch.Double11, (int)Revisions.D_1DEFERRED_PLAYS))
            {
                Serializer.Serialize(ref DeferredPlayCount);
                Serializer.Serialize(ref DeferredPlayCountUploaded);
                if (revision.Has(Branch.Double11, (int)Revisions.D1_UPLOADED_HIGH_SCORE))
                    Serializer.Serialize(ref UploadedLocalHighScore);
                if (revision.Has(Branch.Double11, (int)Revisions.D1_TROPHIES))
                {
                    Serializer.Serialize(ref GoldTrophyCount);
                    Serializer.Serialize(ref SilverTrophyCount);
                    Serializer.Serialize(ref BronzeTrophyCount);
                }
            }

            if (subVersion >= 0x1ad)
            {
                Serializer.Serialize(ref BestTime);
                Serializer.Serialize(ref MultiplayerCompletionCount);
            }
        }

        public int GetAllocatedSize() 
        {
            var size = BaseAllocationSize;
            if (LocalHighScore != null)
                size += LocalHighScore.Length * 4;
            if (Collectables != null)
                size += Collectables.Length * CollectableData.BaseAllocationSize;
            if (LinkedLevels != null)
                size += LinkedLevels.Length * SlotID.BaseAllocationSize;
            if (SubLevels != null)
                size += SubLevels.Length * SlotID.BaseAllocationSize;
            if (UploadedLocalHighScore != null)
                size += UploadedLocalHighScore.Length * 4;
            return size;
        }
    }
}