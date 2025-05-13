using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CwLibNet.Enums;

using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types;
using CwLibNet.Enums;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class CollectedBubble : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x50;

        public ResourceDescriptor level;
        public int thingUID;
        public ResourceDescriptor plan;

        public void Serialize(Serializer serializer)
        {
            level = serializer.Resource(level, ResourceType.Level, true);
            thingUID = serializer.I32(thingUID);
            plan = serializer.Resource(plan, ResourceType.Plan, true);
        }

        public int GetAllocatedSize() 
        {
            return CollectedBubble.BASE_ALLOCATION_SIZE;
        }
    }
}
