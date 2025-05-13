using System;
using System.Collections.Generic;
 
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile
{
    public class PinProgress : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public int progressType, progressCount;

        public void Serialize(Serializer serializer) 
        {
            progressType = serializer.I32(progressType);
            progressCount = serializer.I32(progressCount);
        }
        public int GetAllocatedSize() 
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}