using CwLibNet4Hub.Structs.Inventory;
using CwLibNet4Hub.Structs.Things.Parts;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO;
using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.EX;

using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Profile;
using CwLibNet4Hub.Structs.Things;
namespace CwLibNet4Hub.Resources;

public class RPlan : Resource
{
    public const int BaseAllocationSize = 0x10;
    public HashSet<ResourceDescriptor?> DependencyCache = [];
    public bool IsUsedForStreaming;
    public Revision Revision = new(Revision.Lbp1FinalRevision, 0x4c44, 0x17);
    public byte[]? ThingData;
    public InventoryItemDetails InventoryData = new();
    public byte CompressionFlags = CwLibNet4Hub.Enums.CompressionFlags.USE_ALL_COMPRESSION;
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

    public override void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        try
        {
            SerializeInternal(serializer);
        }
        catch (Exception ex) when (ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException || ex is NullReferenceException || ex is NotImplementedException)
        {
            Console.WriteLine($"[WARNING] Error detected during RPlan serialization/deserialization: {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine($"[WARNING] Plan file appears to be corrupted, uses unsupported features, or has unimplemented functionality. Continuing with partial data...");
            // Don't rethrow - allow partial deserialization to succeed
        }
    }

    private void SerializeInternal(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var head = revision.GetVersion();
        if (!serializer.GetIsWriting())
        {
            DependencyCache.UnionWith(serializer.GetDependencies());
            serializer.ClearDependencies();
        }

        if (!serializer.GetIsWriting())
        {
            CompressionFlags = Serializer.GetCurrentSerializer().GetCompressionFlags();
            Revision = revision;
        }

        if (revision.GetSubVersion() >= (int)Revisions.STREAMING_PLAN)
            Serializer.Serialize(ref IsUsedForStreaming);
        if (Serializer.IsWriting())
        {
            var revisionHead = Serializer.GetCurrentSerializer().GetRevision().Head;
            Serializer.Serialize(ref revisionHead);
        }
        else
            Serializer.GetCurrentSerializer().GetInput().I32();
        Serializer.Serialize(ref ThingData);
        if (head >= (int)Revisions.PLAN_DETAILS && !IsUsedForStreaming)
        {
            Serializer.Serialize(ref InventoryData);
            if (revision.Has(Branch.Leerdammer, (int)Revisions.LD_LAMS_KEYS) || head >= (int)Revisions.LAMS_KEYS)
            {
                Serializer.Serialize(ref InventoryData.Location);
                Serializer.Serialize(ref InventoryData.Category);
            }
            else
            {
                Serializer.Serialize(ref InventoryData.LocationTag);
                Serializer.Serialize(ref InventoryData.CategoryTag);
                if (!Serializer.IsWriting())
                {
                    InventoryData.Location = RTranslationTable.MakeLamsKeyID(InventoryData.LocationTag);
                    InventoryData.Category = RTranslationTable.MakeLamsKeyID(InventoryData.CategoryTag);
                }
            }
        }

        if (!Serializer.IsWriting())
        {
            foreach (var descriptor in Serializer.GetCurrentSerializer().GetDependencies())
                DependencyCache.Remove(descriptor);
            Serializer.GetCurrentSerializer().ClearDependencies();
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
        this.Serialize(serializer);
        foreach (var descriptor in DependencyCache)
            Serializer.GetCurrentSerializer().AddDependency(descriptor);
        return new SerializationData(Serializer.GetCurrentSerializer().GetBuffer(), revision, compressionFlags, ResourceType.Plan, SerializationType.BINARY, Serializer.GetCurrentSerializer().GetDependencies());
    }

    public virtual Thing[]? GetThings()
    {
        var serializer = new Serializer(ThingData, Revision, CompressionFlags);
        var things = serializer.Array<Thing>(null, true);
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
        serializer.Thingarray(things);
        ThingData = serializer.GetBuffer();
        var dependencies = serializer.GetDependencies();
        DependencyCache.Clear();
        DependencyCache = [..dependencies];
    }

    public void SetThing(Thing? thing)
    {
        Serializer serializer = new(0x800000, Revision, CompressionFlags);
        serializer.Reference(thing);
        var things = serializer.GetThings();
        serializer = new Serializer(0x800000, Revision, CompressionFlags);
        serializer.Thingarray(things);
        ThingData = serializer.GetBuffer();
        var dependencies = serializer.GetDependencies();
        DependencyCache.Clear();
        DependencyCache = [..dependencies];
    }
}