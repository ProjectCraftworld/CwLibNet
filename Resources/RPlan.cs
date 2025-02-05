using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Resources.Structs.Inventory;
using CwLibNet.Resources.Structs.Things;
using CwLibNet.Resources.Structs.Things.Parts;
using CwLibNet.Types;

namespace CwLibNet.Resources
{
    public class RPlan : Resource
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public HashSet<ResourceDescriptor> dependencyCache = new HashSet();
        public bool isUsedForStreaming = false;
        public Revision revision = new Revision(Revision.LBP1_FINAL_REVISION, 0x4c44, 0x17);
        public byte[] thingData;
        public InventoryItemDetails inventoryData = new InventoryItemDetails();
        public byte compressionFlags = CompressionFlags.USE_ALL_COMPRESSION;
        public RPlan()
        {
            this.thingData = Array.Empty<byte>();
        }

        public RPlan(Revision revision, byte compressionFlags, Thing thing, PMetadata metadata)
        {
            this.revision = revision;
            this.compressionFlags = compressionFlags;
            this.SetThing(thing);
            this.inventoryData = new InventoryItemDetails(metadata);
            this.thingData = Array.Empty<byte>();
        }

        public RPlan(Revision revision, byte compressionFlags, Thing[] things, PMetadata metadata)
        {
            this.revision = revision;
            this.compressionFlags = compressionFlags;
            this.SetThings(things);
            this.inventoryData = new InventoryItemDetails(metadata);
            this.thingData = Array.Empty<byte>();
        }

        public RPlan(Revision revision, byte compressionFlags, Thing thing, InventoryItemDetails details)
        {
            this.revision = revision;
            this.compressionFlags = compressionFlags;
            this.SetThing(thing);
            this.inventoryData = details;
            this.thingData = Array.Empty<byte>();
        }

        public RPlan(Revision revision, byte compressionFlags, Thing[] things, InventoryItemDetails details)
        {
            this.revision = revision;
            this.compressionFlags = compressionFlags;
            this.SetThings(things);
            this.inventoryData = details;
            this.thingData = Array.Empty<byte>();
        }

        public override void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int head = revision.GetVersion();
            if (!serializer.IsWriting())
            {
                Collections.AddAll(dependencyCache, serializer.GetDependencies());
                serializer.ClearDependencies();
            }

            if (!serializer.IsWriting())
            {
                compressionFlags = serializer.GetCompressionFlags();
                this.revision = revision;
            }

            if (revision.GetSubVersion() >= Revisions.STREAMING_PLAN)
                isUsedForStreaming = serializer.Bool(isUsedForStreaming);
            if (serializer.IsWriting())
                serializer.I32(serializer.GetRevision().GetHead());
            else
                serializer.GetInput().I32();
            thingData = serializer.Bytearray(thingData);
            if (head >= Revisions.PLAN_DETAILS && !isUsedForStreaming)
            {
                inventoryData = serializer.Struct(inventoryData, typeof(InventoryItemDetails));
                if (revision.Has(Branch.LEERDAMMER, Revisions.LD_LAMS_KEYS) || head >= Revisions.LAMS_KEYS)
                {
                    inventoryData.location = serializer.U32(inventoryData.location);
                    inventoryData.category = serializer.U32(inventoryData.category);
                }
                else
                {
                    inventoryData.locationTag = serializer.Str(inventoryData.locationTag);
                    inventoryData.categoryTag = serializer.Str(inventoryData.categoryTag);
                    if (!serializer.IsWriting())
                    {
                        inventoryData.location = RTranslationTable.MakeLamsKeyID(inventoryData.locationTag);
                        inventoryData.category = RTranslationTable.MakeLamsKeyID(inventoryData.categoryTag);
                    }
                }
            }

            if (!serializer.IsWriting())
            {
                foreach (ResourceDescriptor descriptor in serializer.GetDependencies())
                    dependencyCache.Remove(descriptor);
                serializer.ClearDependencies();
            }
        }

        public override int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (this.thingData != null)
                size += this.thingData.length;
            if (this.inventoryData != null)
                size += this.inventoryData.GetAllocatedSize();
            return size;
        }

        public SerializationData Build()
        {
            return this.Build(this.revision, this.compressionFlags);
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            Serializer serializer = new Serializer(this.GetAllocatedSize() + 0x8000, revision, compressionFlags);
            serializer.Struct(this, typeof(RPlan));
            foreach (ResourceDescriptor descriptor in this.dependencyCache)
                serializer.AddDependency(descriptor);
            return new SerializationData(serializer.GetBuffer(), revision, compressionFlags, ResourceType.PLAN, SerializationType.BINARY, serializer.GetDependencies());
        }

        public virtual Thing[] GetThings()
        {
            Serializer serializer = new Serializer(this.thingData, this.revision, this.compressionFlags);
            Thing[] things = serializer.Array(null, typeof(Thing), true);
            foreach (Thing thing in things)
            {
                if (thing != null)
                    thing.Fixup(revision);
            }

            return things;
        }

        public void SetThings(Thing[] things)
        {
            Serializer serializer = new Serializer(0x800000, this.revision, this.compressionFlags);
            serializer.Array(things, typeof(Thing), true);
            this.thingData = serializer.GetBuffer();
            ResourceDescriptor[] dependencies = serializer.GetDependencies();
            this.dependencyCache.Clear();
            Collections.AddAll(this.dependencyCache, dependencies);
        }

        public void SetThing(Thing thing)
        {
            Serializer serializer = new Serializer(0x800000, this.revision, this.compressionFlags);
            serializer.Reference(thing, typeof(Thing));
            Thing[] things = serializer.GetThings();
            serializer = new Serializer(0x800000, this.revision, this.compressionFlags);
            serializer.Array(things, typeof(Thing), true);
            this.thingData = serializer.GetBuffer();
            ResourceDescriptor[] dependencies = serializer.GetDependencies();
            this.dependencyCache.Clear();
            Collections.AddAll(this.dependencyCache, dependencies);
        }
    }
}