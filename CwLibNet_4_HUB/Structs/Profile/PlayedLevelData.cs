using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Slot;

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

        public void Serialize(Serializer serializer)
        {
            var revision = serializer.GetRevision();
            var version = revision.GetVersion();
            var subVersion = revision.GetSubVersion();

            SlotId = serializer.Struct<SlotID>(SlotId);
            if (version > 0x1fd)
                LastPlayedTimestamp = serializer.S64(LastPlayedTimestamp);
            if (version > 0x200)
                LocalHighScore = serializer.Intarray(LocalHighScore);

            if (version is > 0x268 and < 0x399) 
            {
                serializer.Bool(false); // Discovered
                serializer.Bool(false); // Unlocked
            }

            if (version < 0x269) 
            {
                PlayCount = (short)serializer.I32(PlayCount);
                CompletionCount = (short)serializer.I32(CompletionCount);
                AcedCount = (short)serializer.I32(AcedCount);
            }
            else 
            {
                PlayCount = serializer.I16(PlayCount);
                CompletionCount = serializer.I16(CompletionCount);
                AcedCount = serializer.I16(AcedCount);
            }

            if (version > 0x1c1) 
                Collectables = serializer.Array<CollectableData>(Collectables);
            if (version > 0x1c3 && subVersion < 0x106)
                Videos = serializer.Intarray(Videos);
            
            if (version > 0x363) 
            {
                LinkedLevels = serializer.Array<SlotID>(LinkedLevels);
                SubLevels = serializer.Array<SlotID>(SubLevels);
            }

            if (revision.Has(Branch.Double11, (int)Revisions.D_1DEFERRED_PLAYS))
            {
                DeferredPlayCount = serializer.I16(DeferredPlayCount);
                DeferredPlayCountUploaded = serializer.I16(DeferredPlayCountUploaded);
                if (revision.Has(Branch.Double11, (int)Revisions.D1_UPLOADED_HIGH_SCORE))
                    UploadedLocalHighScore = serializer.Intarray(UploadedLocalHighScore);
                if (revision.Has(Branch.Double11, (int)Revisions.D1_TROPHIES))
                {
                    GoldTrophyCount = serializer.I16(GoldTrophyCount);
                    SilverTrophyCount = serializer.I16(SilverTrophyCount);
                    BronzeTrophyCount = serializer.I16(BronzeTrophyCount);
                }
            }

            if (subVersion >= 0x1ad)
            {
                BestTime = serializer.I32(BestTime);
                MultiplayerCompletionCount = serializer.I16(MultiplayerCompletionCount);
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