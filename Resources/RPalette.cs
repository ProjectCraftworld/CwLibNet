using System.Runtime.InteropServices;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types;
using Serializer = CwLibNet.IO.Serialization;

namespace CwLibNet.Resources
{

    public class RPalette : Resource
    {
        public readonly int BASE_ALLOCATION_SIZE = 0x20;

        public List<ResourceDescriptor> planList = new List<ResourceDescriptor>();
        public int location, description;

        public ResourceDescriptor[] convertedPlans;

        public override void Serialize(Serializer serializer)
        {
            
            if (serializer.isWriting())
            {
                MemoryOutputStream stream = serializer.getOutput();
                serializer.Serialize(planList.size());
                foreach (ResourceDescriptor descriptor in planList)
                    serializer.resource(descriptor, descriptor.getType());
            }
            else
            {
                int count = serializer.getInput().i32();
                planList = new List<ResourceDescriptor>(count);
                for (int i = 0; i < count; ++i)
                    planList.Add(serializer.resource(null, ResourceType.PLAN));
            }

            location = serializer.Serialize(location);
            description = serializer.Serialize(description);

            if (serializer.getRevision().getVersion() >= Revisions.PALETTE_CONVERTED_PLANS)
            {
                if (serializer.isWriting())
                {
                    MemoryOutputStream stream = serializer.getOutput();
                    serializer.Serialize(convertedPlans.Length);
                    foreach (ResourceDescriptor descriptor in convertedPlans)
                        serializer.resource(descriptor, descriptor.getType());
                }
                else 
                {
                    int count = serializer.getInput().i32();
                    convertedPlans = new ResourceDescriptor[count];
                    for (int i = 0; i < count; ++i)
                        convertedPlans[i] = serializer.resource(null, ResourceType.PLAN);
                }
            }
        }

        public override int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE + (this.planList.size() * 0x24);
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            Serializer serializer = new Serializer(this.GetAllocatedSize(), revision, compressionFlags);
            serializer.Struct<RPalette>(this);
        return new SerializationData(
            serializer.getBuffer(),
            revision,
            compressionFlags,
            ResourceType.PALETTE,
            SerializationType.BINARY,
            serializer.getDependencies()
        );
        }
    }
}