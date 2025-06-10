using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Profile 
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

        public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
        {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

            var revision = Serializer.GetCurrentSerializer().GetRevision();
            var head = revision.GetVersion();

            if (head > 0x16f) 
            {
                Serializer.Serialize(ref TotalTime);
                Serializer.Serialize(ref EditingTime);
                Serializer.Serialize(ref PlayingTime);
                Serializer.Serialize(ref IdlingTime);
            }

            if (head > 0x183)
                Serializer.Serialize(ref MultiplayerGamesCount);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_LEVEL_TIMES_MAP)) 
            {
                if (Serializer.IsWriting())
                {
                    var stream = Serializer.GetCurrentSerializer().GetOutput();
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
                    var stream = Serializer.GetCurrentSerializer().GetInput();
                    var count = stream.I32();
                    LevelTimesMap = new Dictionary<int, int>(count);
                    for (var i = 0; i < count; i++)
                    {
                        LevelTimesMap.Add(stream.I32(), stream.I32());
                    }
                }

                Serializer.Serialize(ref TotalLevelTime);
            }

            if (head > 0x1c9)
                Serializer.Serialize(ref PlayerLadderPoints);
            if (head > 0x1ab)
                Serializer.Serialize(ref StoryLevelCompletionCount);
            if (head > 0x1f7)
                Serializer.Serialize(ref CommunityLevelCompletionCount);
            if (head > 0x1ab)
                Serializer.Serialize(ref LevelCompletionCount);
            
            if (head > 0x1f7)
                Serializer.Serialize(ref LevelsTaggedCount);
            
            if (head > 0x1c9)
                Serializer.Serialize(ref GamesWithRandomPlayersCount);

            if (head is > 0x1de and < 0x2cb)
            {
                if (!revision.IsLeerdammer() || revision.Before(Branch.Leerdammer, (int)Revisions.LD_REMOVED_ENEMY_STAT))
                {
                    Serializer.Serialize(ref temp_int); // enemy kills
                }
            }

            if (head > 0x1de) 
                Serializer.Serialize(ref PointsCollected);
            if (head > 0x1f7)
                Stats = Serializer.GetCurrentSerializer().Longarray(Stats);
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