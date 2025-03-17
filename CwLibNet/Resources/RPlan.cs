using System.Numerics;
using Cwlib.Enums;
using CwLibNet.Enums;
using CwLibNet.Extensions;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Inventory;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using CwLibNet.Types.Things;
using CwLibNet.Types.Things.Parts;

namespace CwLibNet.Resources
{
    public class RPlan : Resource
    {
        public const int BaseAllocationSize = 0x10;
        public HashSet<ResourceDescriptor?> DependencyCache = [];
        public bool IsUsedForStreaming = false;
        public Revision Revision = new Revision(Revision.Lbp1FinalRevision, 0x4c44, 0x17);
        public byte[] ThingData;
        public InventoryItemDetails InventoryData = new InventoryItemDetails();
        public byte CompressionFlags = Cwlib.Enums.CompressionFlags.USE_ALL_COMPRESSION;
        public RPlan()
        {
            this.ThingData = [];
        }

        public RPlan(Revision revision, byte compressionFlags, Thing thing, PMetadata metadata)
        {
            this.Revision = revision;
            this.CompressionFlags = compressionFlags;
            this.SetThing(thing);
            this.InventoryData = new InventoryItemDetails(metadata);
            this.ThingData = [];
        }

        public RPlan(Revision revision, byte compressionFlags, Thing[]? things, PMetadata metadata)
        {
            this.Revision = revision;
            this.CompressionFlags = compressionFlags;
            this.SetThings(things);
            this.InventoryData = new InventoryItemDetails(metadata);
            this.ThingData = [];
        }

        public RPlan(Revision revision, byte compressionFlags, Thing thing, InventoryItemDetails details)
        {
            this.Revision = revision;
            this.CompressionFlags = compressionFlags;
            this.SetThing(thing);
            this.InventoryData = details;
            this.ThingData = [];
        }

        public RPlan(Revision revision, byte compressionFlags, Thing[]? things, InventoryItemDetails details)
        {
            this.Revision = revision;
            this.CompressionFlags = compressionFlags;
            this.SetThings(things);
            this.InventoryData = details;
            this.ThingData = [];
        }

        public override void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int head = revision.GetVersion();
            if (!serializer.IsWriting())
            {
                DependencyCache.AddRange(serializer.GetDependencies());
                serializer.ClearDependencies();
            }

            if (!serializer.IsWriting())
            {
                CompressionFlags = serializer.GetCompressionFlags();
                this.Revision = revision;
            }

            if (revision.GetSubVersion() >= ((int)Revisions.StreamingPlan))
                IsUsedForStreaming = serializer.Bool(IsUsedForStreaming);
            if (serializer.IsWriting())
                serializer.I32(serializer.GetRevision().Head);
            else
                serializer.GetInput().I32();
            ThingData = serializer.Bytearray(ThingData);
            if (head >= ((int)Revisions.PlanDetails) && !IsUsedForStreaming)
            {
                InventoryData = serializer.Struct(InventoryData);
                if (revision.Has(Types.Branch.Leerdammer, ((int)Revisions.LdLamsKeys)) || head >= ((int)Revisions.LamsKeys))
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
                foreach (ResourceDescriptor? descriptor in serializer.GetDependencies())
                    DependencyCache.Remove(descriptor);
                serializer.ClearDependencies();
            }
        }

        public override int GetAllocatedSize()
        {
            int size = BaseAllocationSize;
            if (ThingData != null)
                size += ThingData.Length;
            if (InventoryData != null)
                size += InventoryData.GetAllocatedSize();
            return size;
        }

        public SerializationData Build()
        {
            return this.Build(this.Revision, this.CompressionFlags);
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            Serializer serializer = new Serializer(this.GetAllocatedSize() + 0x8000, revision, compressionFlags);
            serializer.Struct(this);
            foreach (ResourceDescriptor? descriptor in this.DependencyCache)
                serializer.AddDependency(descriptor);
            return new SerializationData(serializer.GetBuffer(), revision, compressionFlags, ResourceType.Plan, SerializationType.BINARY, serializer.GetDependencies());
        }

        public virtual Thing[]? GetThings()
        {
            Serializer serializer = new Serializer(this.ThingData, this.Revision, this.CompressionFlags);
            Thing[]? things = serializer.Array<Thing>(null, true);
            if (Revision.GetVersion() >= 0x341)
            {
                foreach (Thing thing in things!)
                {
                    if (thing?.Parent == null) continue;
                    PPos? pos = thing.GetPart<PPos>(Part.Parts["PPos"]);
                    if (pos == null) continue;
                    PPos? parent = thing.Parent.GetPart<PPos>(Part.Parts["PPos"]);
                    if (parent == null) continue;
                    Matrix4x4.Invert(parent.WorldPosition!.Value, out var inv);
                    pos.LocalPosition = Matrix4x4.Multiply(inv, pos.WorldPosition!.Value);
                }
            }

            return things;
        }

        public void SetThings(Thing[]? things)
        {
            Serializer serializer = new(0x800000, this.Revision, this.CompressionFlags);
            serializer.Array(things, true);
            this.ThingData = serializer.GetBuffer();
            ResourceDescriptor?[] dependencies = serializer.GetDependencies();
            this.DependencyCache.Clear();
            this.DependencyCache = [..dependencies];
        }

        public void SetThing(Thing thing)
        {
            Serializer serializer = new(0x800000, this.Revision, this.CompressionFlags);
            serializer.Reference(thing);
            Thing[]? things = serializer.GetThings();
            serializer = new Serializer(0x800000, this.Revision, this.CompressionFlags);
            serializer.Array(things, true);
            this.ThingData = serializer.GetBuffer();
            ResourceDescriptor?[] dependencies = serializer.GetDependencies();
            this.DependencyCache.Clear();
            this.DependencyCache = [..dependencies];
        }
    }
}