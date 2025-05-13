using System;
using System.Collections.Generic;

using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class PinAward : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int pinID, awardCount;

        public void Serialize(Serializer serializer) 
        {
            pinID = serializer.I32(pinID);
            awardCount = serializer.I32(awardCount);
        }

        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}