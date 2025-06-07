using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Resources;

public class RPalette : Resource
{
    public const int BaseAllocationSize = 0x20;

    public List<ResourceDescriptor?> PlanList = [];
    public int Location, Description;

    public ResourceDescriptor?[]? ConvertedPlans;

    public override void Serialize()
    {
            
        if (Serializer.IsWriting())
        {
            var stream = Serializer.GetOutput();
            stream.I32(PlanList.Count);
            foreach (var descriptor in PlanList)
                Serializer.Serialize(ref descriptor, descriptor!.GetResourceType());
        }
        else
        {
            var count = Serializer.GetInput().I32();
            PlanList = new List<ResourceDescriptor?>(count);
            for (var i = 0; i < count; ++i)
                PlanList.Add(Serializer.Serialize(ref null, ResourceType.Plan));
        }

        Serializer.Serialize(ref Location);
        Serializer.Serialize(ref Description);

        if (Serializer.GetRevision().GetVersion() >= (int)Revisions.PALETTE_CONVERTED_PLANS)
        {
            if (Serializer.IsWriting())
            {
                var stream = Serializer.GetOutput();
                stream.I32(ConvertedPlans?.Length ?? 0);
                foreach (var descriptor in ConvertedPlans)
                    Serializer.Serialize(ref descriptor, descriptor!.GetResourceType());
            }
            else 
            {
                var count = Serializer.GetInput().I32();
                ConvertedPlans = new ResourceDescriptor[count];
                for (var i = 0; i < count; ++i)
                    Serializer.Serialize(ref ConvertedPlans[i]);
            }
        }
    }

    public override int GetAllocatedSize()
    {
        return BaseAllocationSize + PlanList.Count * 0x24;
    }

    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        var serializer = new Serializer(GetAllocatedSize(), revision, compressionFlags);
        Serializer.Serialize(ref this);
        return new SerializationData(
            Serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Palette,
            SerializationType.BINARY,
            Serializer.GetDependencies()
        );
    }
}