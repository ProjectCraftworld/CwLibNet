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
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile 
{
    public class CollectableData : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x30;

        public ResourceDescriptor plan;
        public int source;

        // -1 = invalid
        // 0 = egg (prize bubble)
        // 1 = award_complete
        // 2 = award_collect
        // 3 = award_ace

        public void Serialize(Serializer serializer)
        {
            if (serializer.GetRevision().GetVersion() >= 0x1c2) 
            {
                plan = serializer.Resource(plan, ResourceType.Plan, true);
                source = serializer.S32(source);
            }
        }

        public int GetAllocatedSize() 
        {
            return CollectableData.BASE_ALLOCATION_SIZE;
        }
    }
}