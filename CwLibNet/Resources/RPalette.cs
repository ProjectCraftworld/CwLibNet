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
        public const int BaseAllocationSize = 0x20;

        public List<ResourceDescriptor?> PlanList = [];
        public int Location, Description;

        public ResourceDescriptor?[]? ConvertedPlans;

        public override void Serialize(Serializer serializer)
        {
            
            if (serializer.IsWriting())
            {
                MemoryOutputStream stream = serializer.GetOutput();
                stream.I32(PlanList.Count);
                foreach (ResourceDescriptor? descriptor in PlanList)
                    serializer.Resource(descriptor, descriptor!.GetResourceType());
            }
            else
            {
                int count = serializer.GetInput().I32();
                PlanList = new List<ResourceDescriptor?>(count);
                for (int i = 0; i < count; ++i)
                    PlanList.Add(serializer.Resource(null, ResourceType.Plan));
            }

            Location = serializer.I32(Location);
            Description = serializer.I32(Description);

            if (serializer.GetRevision().GetVersion() >= (int)Revisions.PaletteConvertedPlans)
            {
                if (serializer.IsWriting())
                {
                    MemoryOutputStream stream = serializer.GetOutput();
                    stream.I32(ConvertedPlans?.Length ?? 0);
                    foreach (ResourceDescriptor? descriptor in ConvertedPlans)
                        serializer.Resource(descriptor, descriptor!.GetResourceType());
                }
                else 
                {
                    int count = serializer.GetInput().I32();
                    ConvertedPlans = new ResourceDescriptor[count];
                    for (int i = 0; i < count; ++i)
                        ConvertedPlans[i] = serializer.Resource(null, ResourceType.Plan);
                }
            }
        }

        public override int GetAllocatedSize()
        {
            return BaseAllocationSize + (PlanList.Count * 0x24);
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            Serializer serializer = new Serializer(GetAllocatedSize(), revision, compressionFlags);
            serializer.Struct(this);
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