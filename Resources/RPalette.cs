using System.Runtime.InteropServices;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Resources
{

    public class RPalette : Resource
    {
        public readonly int BASE_ALLOCATION_SIZE = 0x20;

        public List<ResourceDescriptor?> planList = new List<ResourceDescriptor?>();
        public int location, description;

        public ResourceDescriptor?[] convertedPlans;

        public override void Serialize(Serializer serializer)
        {
            
            if (serializer.IsWriting())
            {
                MemoryOutputStream stream = serializer.GetOutput();
                stream.I32(planList.Count);
                foreach (ResourceDescriptor? descriptor in planList)
                    serializer.Resource(descriptor, descriptor!.GetResourceType());
            }
            else
            {
                int count = serializer.GetInput().I32();
                planList = new List<ResourceDescriptor?>(count);
                for (int i = 0; i < count; ++i)
                    planList.Add(serializer.Resource(null, ResourceType.Plan));
            }

            location = serializer.I32(location);
            description = serializer.I32(description);

            if (serializer.GetRevision().GetVersion() >= (int)Revisions.PaletteConvertedPlans)
            {
                if (serializer.IsWriting())
                {
                    MemoryOutputStream stream = serializer.GetOutput();
                    stream.I32(convertedPlans.Length);
                    foreach (ResourceDescriptor? descriptor in convertedPlans)
                        serializer.Resource(descriptor, descriptor!.GetResourceType());
                }
                else 
                {
                    int count = serializer.GetInput().I32();
                    convertedPlans = new ResourceDescriptor[count];
                    for (int i = 0; i < count; ++i)
                        convertedPlans[i] = serializer.Resource(null, ResourceType.Plan);
                }
            }
        }

        public override int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE + (this.planList.Count * 0x24);
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            Serializer serializer = new Serializer(this.GetAllocatedSize(), revision, compressionFlags);
            serializer.Struct<RPalette>(this);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Palette,
            SerializationType.BINARY,
            serializer.GetDependencies()
        );
        }
    }
}