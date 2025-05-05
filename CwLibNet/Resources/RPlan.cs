using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.Extensions;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Inventory;
using CwLibNet.Structs.Things;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.Types;
using CwLibNet.Types.Data;

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

    public override void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var head = revision.GetVersion();
        if (!serializer.IsWriting())
        {
            DependencyCache.AddRange(serializer.GetDependencies());
            serializer.ClearDependencies();
        }

        if (!serializer.IsWriting())
        {
            CompressionFlags = serializer.GetCompressionFlags();
            Revision = revision;
        }

        if (revision.GetSubVersion() >= (int)Revisions.STREAMING_PLAN)
            IsUsedForStreaming = serializer.Bool(IsUsedForStreaming);
        if (serializer.IsWriting())
            serializer.I32(serializer.GetRevision().Head);
        else
            serializer.GetInput().I32();
        ThingData = serializer.Bytearray(ThingData);
        if (head >= (int)Revisions.PLAN_DETAILS && !IsUsedForStreaming)
        {
            InventoryData = serializer.Struct(InventoryData);
            if (revision.Has(Branch.Leerdammer, (int)Revisions.LD_LAMS_KEYS) || head >= (int)Revisions.LAMS_KEYS)
            {
                InventoryData.Location = serializer.U32(InventoryData.Location);
                InventoryData.Category = serializer.U32(InventoryData.Category);
            }
            else
            {
                InventoryData.LocationTag = serializer.Str(InventoryData.LocationTag);
                InventoryData.CategoryTag = serializer.Str(InventoryData.CategoryTag);
                if (!serializer.IsWriting())
                {
                    InventoryData.Location = RTranslationTable.MakeLamsKeyID(InventoryData.LocationTag);
                    InventoryData.Category = RTranslationTable.MakeLamsKeyID(InventoryData.CategoryTag);
                }
            }
        }

        if (!serializer.IsWriting())
        {
            foreach (var descriptor in serializer.GetDependencies())
                DependencyCache.Remove(descriptor);
            serializer.ClearDependencies();
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
        serializer.Struct(this);
        foreach (var descriptor in DependencyCache)
            serializer.AddDependency(descriptor);
        return new SerializationData(serializer.GetBuffer(), revision, compressionFlags, ResourceType.Plan, SerializationType.BINARY, serializer.GetDependencies());
    }

    public virtual Thing[]? GetThings()
    {
        var serializer = new Serializer(ThingData, Revision, CompressionFlags);
        Thing[]? things = serializer.Array<Thing>(null, true);
        if (Revision.GetVersion() >= 0x341)
        {
            foreach (var thing in things!)
            {
                if (thing?.Parent == null) continue;
                var pos = thing.GetPart<PPos>(Part.Parts["PPos"]);
                if (pos == null) continue;
                var parent = thing.Parent.GetPart<PPos>(Part.Parts["PPos"]);
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
        serializer.Array(things, true);
        ThingData = serializer.GetBuffer();
        ResourceDescriptor?[] dependencies = serializer.GetDependencies();
        DependencyCache.Clear();
        DependencyCache = [..dependencies];
    }

    public void SetThing(Thing? thing)
    {
        Serializer serializer = new(0x800000, Revision, CompressionFlags);
        serializer.Reference(thing);
        Thing[]? things = serializer.GetThings();
        serializer = new Serializer(0x800000, Revision, CompressionFlags);
        serializer.Array(things, true);
        ThingData = serializer.GetBuffer();
        ResourceDescriptor?[] dependencies = serializer.GetDependencies();
        DependencyCache.Clear();
        DependencyCache = [..dependencies];
    }
}