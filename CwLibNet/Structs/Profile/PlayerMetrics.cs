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
    public class PlayerMetrics : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x60;

        private int totalTime, editingTime, playingTime, idlingTime;

        private int multiplayerGamesCount;

        private Dictionary<int, int> levelTimesMap;

        private int totalLevelTime;

        private float playerLadderPoints;

        private int storyLevelCompletionCount;

        private int communityLevelCompletionCount;
        private int levelCompletionCount;
        private int levelsTaggedCount;

        private int gamesWithRandomPlayersCount;

        private float pointsCollected;

        private long[] stats;

        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int head = revision.GetVersion();

            if (head > 0x16f) 
            {
                totalTime = serializer.I32(totalTime);
                editingTime = serializer.I32(editingTime);
                playingTime = serializer.I32(playingTime);
                idlingTime = serializer.I32(idlingTime);
            }

            if (head > 0x183)
                multiplayerGamesCount = serializer.I32(multiplayerGamesCount);

            if (revision.Has(Branch.Double11, (int)Revisions.D1LevelTimesMap)) 
            {
                if (serializer.IsWriting())
                {
                    MemoryOutputStream stream = serializer.GetOutput();
                    HashSet<int> keys = new HashSet<int>(levelTimesMap.Keys);
                    stream.I32(keys.Count);
                    foreach (int key in keys)
                    {
                        stream.I32(key);
                        stream.I32(levelTimesMap[key]);
                    }
                }
                else 
                {
                    MemoryInputStream stream = serializer.GetInput();
                    int count = stream.I32();
                    levelTimesMap = new Dictionary<int, int>(count);
                    for (int i = 0; i < count; i++)
                    {
                        levelTimesMap.Add(stream.I32(), stream.I32());
                    }
                }

                totalLevelTime = serializer.I32(totalLevelTime);
            }

            if (head > 0x1c9)
                playerLadderPoints = serializer.F32(playerLadderPoints);
            if (head > 0x1ab)
                storyLevelCompletionCount = serializer.I32(storyLevelCompletionCount);
            if (head > 0x1f7)
                communityLevelCompletionCount = serializer.I32(communityLevelCompletionCount);
            if (head > 0x1ab)
                levelCompletionCount = serializer.I32(levelCompletionCount);
            
            if (head > 0x1f7)
                levelsTaggedCount = serializer.I32(levelsTaggedCount);
            
            if (head > 0x1c9)
                gamesWithRandomPlayersCount = serializer.I32(gamesWithRandomPlayersCount);

            if (head > 0x1de && head < 0x2cb)
            {
                if (!revision.IsLeerdammer() || revision.Before(Branch.Leerdammer, (int)Revisions.LdRemovedEnemyStat))
                {
                    serializer.I32(0); // enemy kills
                }
            }

            if (head > 0x1de) 
                pointsCollected = serializer.F32(pointsCollected);
            if (head > 0x1f7)
                stats = serializer.Longarray(stats);
        }

        public int GetAllocatedSize() 
        {
            int size = BASE_ALLOCATION_SIZE;
            if (stats != null)
                size += stats.Length * 0x8;
            if (levelTimesMap != null)
                size += levelTimesMap.Count * 0x8;
            return size;
        }
    }
}