using CwLibNet.Types.Data;

using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Resources;

public class RPlan : Resource
{
    public const int BaseAllocationSize = 0x10;
    public HashSet<ResourceDescriptor?> DependencyCache = [];
    public bool IsUsedForStreaming;
    public Revision Revision = new(Revision.Lbp1FinalRevision, 0x4c44, 0x17);
    public byte[]? ThingData;
    public InventoryItemDetails InventoryData = new();
    public byte CompressionFlags = CwLibNet.Enums.CompressionFlags.USE_ALL_COMPRESSION;
    public RPlan()
    {
        ThingData = [];
    }

    public RPlan(Revision revision, byte compressionFlags, Thing thing, PMetadata? metadata)
    {
        Revision = revision;
        CompressionFlags = compressionFlags;
        SetThing(thing);
        InventoryData = new InventoryItemDetails(metadata);
        ThingData = [];
    }

    public RPlan(Revision revision, byte compressionFlags, Thing[]? things, PMetadata? metadata)
    {
        Revision = revision;
        CompressionFlags = compressionFlags;
        SetThings(things);
        InventoryData = new InventoryItemDetails(metadata);
        ThingData = [];
    }

    public RPlan(Revision revision, byte compressionFlags, Thing thing, InventoryItemDetails details)
    {
        Revision = revision;
        CompressionFlags = compressionFlags;
        SetThing(thing);
        InventoryData = details;
        ThingData = [];
    }

    public RPlan(Revision revision, byte compressionFlags, Thing[]? things, InventoryItemDetails details)
    {
        Revision = revision;
        CompressionFlags = compressionFlags;
        SetThings(things);
        InventoryData = details;
        ThingData = [];
    }

    public override void Serialize()
    {
        var revision = Serializer.GetRevision();
        var head = revision.GetVersion();
        if (!Serializer.IsWriting())
        {
            DependencyCache.AddRange(Serializer.GetDependencies());
            Serializer.ClearDependencies();
        }

        if (!Serializer.IsWriting())
        {
            CompressionFlags = Serializer.GetCompressionFlags();
            Revision = revision;
        }

        if (revision.GetSubVersion() >= (int)Revisions.STREAMING_PLAN)
            Serializer.Serialize(ref IsUsedForStreaming);
        if (Serializer.IsWriting())
            Serializer.Serialize(ref Serializer.GetRevision().Head);
        else
            Serializer.GetInput().I32();
        Serializer.Serialize(ref ThingData);
        if (head >= (int)Revisions.PLAN_DETAILS && !IsUsedForStreaming)
        {
            Serializer.Serialize(ref InventoryData);
            if (revision.Has(Branch.Leerdammer, (int)Revisions.LD_LAMS_KEYS) || head >= (int)Revisions.LAMS_KEYS)
            {
                InventoryData.Location = Serializer.Serialize(ref InventoryData.Location);
                InventoryData.Category = Serializer.Serialize(ref InventoryData.Category);
            }
            else
            {
                InventoryData.LocationTag = Serializer.Serialize(ref InventoryData.LocationTag);
                InventoryData.CategoryTag = Serializer.Serialize(ref InventoryData.CategoryTag);
                if (!Serializer.IsWriting())
                {
                    InventoryData.Location = RTranslationTable.MakeLamsKeyID(InventoryData.LocationTag);
                    InventoryData.Category = RTranslationTable.MakeLamsKeyID(InventoryData.CategoryTag);
                }
            }
        }

        if (!Serializer.IsWriting())
        {
            foreach (var descriptor in Serializer.GetDependencies())
                DependencyCache.Remove(descriptor);
            Serializer.ClearDependencies();
        }
    }

    public override int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (ThingData != null)
            size += ThingData.Length;
        if (InventoryData != null)
            size += InventoryData.GetAllocatedSize();
        return size;
    }

    public SerializationData Build()
    {
        return Build(Revision, CompressionFlags);
    }

    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        var serializer = new Serializer(GetAllocatedSize() + 0x8000, revision, compressionFlags);
        Serializer.Serialize(ref this);
        foreach (var descriptor in DependencyCache)
            Serializer.AddDependency(descriptor);
        return new SerializationData(Serializer.GetBuffer(), revision, compressionFlags, ResourceType.Plan, SerializationType.BINARY, Serializer.GetDependencies());
    }

    public virtual Thing[]? GetThings()
    {
        var serializer = new Serializer(ThingData, Revision, CompressionFlags);
        var things = Serializer.Array<Thing>(null, true);
        if (Revision.GetVersion() >= 0x341)
        {
            foreach (var thing in things!)
            {
                if (thing?.Parent == null) continue;
                var pos = thing.GetPart<PPos>(Part.Parts["POS"]);
                if (pos == null) continue;
                var parent = thing.Parent.GetPart<PPos>(Part.Parts["POS"]);
                if (parent == null) continue;
                Matrix4x4.Invert(parent.WorldPosition!.Value, out var inv);
                pos.LocalPosition = Matrix4x4.Multiply(inv, pos.WorldPosition!.Value);
            }
        }

        return things;
    }

    public void SetThings(Thing[]? things)
    {
        Serializer serializer = new(0x800000, Revision, CompressionFlags);
        Serializer.Serialize(ref things, true);
        ThingData = Serializer.GetBuffer();
        var dependencies = Serializer.GetDependencies();
        DependencyCache.Clear();
        DependencyCache = [..dependencies];
    }

    public void SetThing(Thing? thing)
    {
        Serializer serializer = new(0x800000, Revision, CompressionFlags);
        Serializer.Reference(thing);
        var things = Serializer.GetThings();
        serializer = new Serializer(0x800000, Revision, CompressionFlags);
        Serializer.Serialize(ref things, true);
        ThingData = Serializer.GetBuffer();
        var dependencies = Serializer.GetDependencies();
        DependencyCache.Clear();
        DependencyCache = [..dependencies];
    }
}