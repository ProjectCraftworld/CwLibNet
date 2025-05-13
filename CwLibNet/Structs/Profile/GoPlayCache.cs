using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Slot;

namespace CwLibNet.Structs.Profile 
{
    public class GoPlayCache : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x20;

        public SlotID levelID;
        public SlotID planetID;
        public byte numBubbleCollected;
        public byte awardLevelComplete;
        public byte awardCollectAll;
        public byte awardAcedLevel;

        public void Serialize(Serializer serializer)
        {
            levelID = serializer.Struct<SlotID>(levelID);
            planetID = serializer.Struct<SlotID>(planetID);
            numBubbleCollected = serializer.I8(numBubbleCollected);
            awardLevelComplete = serializer.I8(awardLevelComplete);
            awardCollectAll = serializer.I8(awardCollectAll);
            awardAcedLevel = serializer.I8(awardAcedLevel);
        }

        public int GetAllocatedSize() 
        {
            return GoPlayCache.BASE_ALLOCATION_SIZE;
        }
    }
}