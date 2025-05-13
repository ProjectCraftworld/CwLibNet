using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class PlayerMetrics : ISerializable
    {
        public const int BaseAllocationSize = 0x60;

        public int TotalTime, EditingTime, PlayingTime, IdlingTime;

        public int MultiplayerGamesCount;

        public Dictionary<int, int> LevelTimesMap;

        public int TotalLevelTime;

        public float PlayerLadderPoints;

        public int StoryLevelCompletionCount;

        public int CommunityLevelCompletionCount;
        public int LevelCompletionCount;
        public int LevelsTaggedCount;

        public int GamesWithRandomPlayersCount;

        public float PointsCollected;

        public long[]? Stats;

        public void Serialize(Serializer serializer)
        {
            var revision = serializer.GetRevision();
            var head = revision.GetVersion();

            if (head > 0x16f) 
            {
                TotalTime = serializer.I32(TotalTime);
                EditingTime = serializer.I32(EditingTime);
                PlayingTime = serializer.I32(PlayingTime);
                IdlingTime = serializer.I32(IdlingTime);
            }

            if (head > 0x183)
                MultiplayerGamesCount = serializer.I32(MultiplayerGamesCount);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_LEVEL_TIMES_MAP)) 
            {
                if (serializer.IsWriting())
                {
                    var stream = serializer.GetOutput();
                    var keys = new HashSet<int>(LevelTimesMap.Keys);
                    stream.I32(keys.Count);
                    foreach (var key in keys)
                    {
                        stream.I32(key);
                        stream.I32(LevelTimesMap[key]);
                    }
                }
                else 
                {
                    var stream = serializer.GetInput();
                    var count = stream.I32();
                    LevelTimesMap = new Dictionary<int, int>(count);
                    for (var i = 0; i < count; i++)
                    {
                        LevelTimesMap.Add(stream.I32(), stream.I32());
                    }
                }

                TotalLevelTime = serializer.I32(TotalLevelTime);
            }

            if (head > 0x1c9)
                PlayerLadderPoints = serializer.F32(PlayerLadderPoints);
            if (head > 0x1ab)
                StoryLevelCompletionCount = serializer.I32(StoryLevelCompletionCount);
            if (head > 0x1f7)
                CommunityLevelCompletionCount = serializer.I32(CommunityLevelCompletionCount);
            if (head > 0x1ab)
                LevelCompletionCount = serializer.I32(LevelCompletionCount);
            
            if (head > 0x1f7)
                LevelsTaggedCount = serializer.I32(LevelsTaggedCount);
            
            if (head > 0x1c9)
                GamesWithRandomPlayersCount = serializer.I32(GamesWithRandomPlayersCount);

            if (head is > 0x1de and < 0x2cb)
            {
                if (!revision.IsLeerdammer() || revision.Before(Branch.Leerdammer, (int)Revisions.LD_REMOVED_ENEMY_STAT))
                {
                    serializer.I32(0); // enemy kills
                }
            }

            if (head > 0x1de) 
                PointsCollected = serializer.F32(PointsCollected);
            if (head > 0x1f7)
                Stats = serializer.Longarray(Stats);
        }

        public int GetAllocatedSize() 
        {
            var size = BaseAllocationSize;
            if (Stats != null)
                size += Stats.Length * 0x8;
            if (LevelTimesMap != null)
                size += LevelTimesMap.Count * 0x8;
            return size;
        }
    }
}