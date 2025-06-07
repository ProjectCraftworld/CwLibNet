using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Resources;

public class RPalette : Resource
{
    public const int BaseAllocationSize = 0x20;

    public List<ResourceDescriptor?> PlanList = [];
    public int Location, Description;

    public ResourceDescriptor?[]? ConvertedPlans;

    public override void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
            
        if (Serializer.IsWriting())
        {
            var stream = Serializer.GetCurrentSerializer().GetOutput();
            stream.I32(PlanList.Count);
            foreach (var descriptor in PlanList)
            {
                var desc = descriptor;
                Serializer.Serialize(ref desc, descriptor!.GetResourceType());
            }
        }
        else
        {
            var count = Serializer.GetCurrentSerializer().GetInput().I32();        PlanList = new List<ResourceDescriptor?>(count);
        for (var i = 0; i < count; ++i)
        {
            ResourceDescriptor? temp = null;
            Serializer.Serialize(ref temp, ResourceType.Plan);
            PlanList.Add(temp);
        }
        }

        Serializer.Serialize(ref Location);
        Serializer.Serialize(ref Description);

        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() >= (int)Revisions.PALETTE_CONVERTED_PLANS)
        {
            if (Serializer.IsWriting())
            {
                var stream = Serializer.GetCurrentSerializer().GetOutput();
                stream.I32(ConvertedPlans?.Length ?? 0);
                foreach (var descriptor in ConvertedPlans)
                {
                    var desc = descriptor;
                    Serializer.Serialize(ref desc, descriptor!.GetResourceType());
                }
            }
            else 
            {
                var count = Serializer.GetCurrentSerializer().GetInput().I32();
                ConvertedPlans = new ResourceDescriptor[count];
                for (var i = 0; i < count; ++i)
                    Serializer.Serialize(ref ConvertedPlans[i], ResourceType.Plan);
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
        this.Serialize(serializer);
        return new SerializationData(
            Serializer.GetCurrentSerializer().GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Palette,
            SerializationType.BINARY,
            Serializer.GetCurrentSerializer().GetDependencies()
        );
    }
}