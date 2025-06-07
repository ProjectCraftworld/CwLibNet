using CwLibNet.Types.Data;

using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Resources 
{
    public class RBigProfile : Resource 
    {
        public const int BaseAllocationSize = 0x20;

        public List<InventoryItem>? Inventory = [];

        public List<Sha1> VitaCrossDependencyHashes = [];

        public List<DataLabel>? CreatorDataLabels = [];

        public StringLookupTable StringTable = new();
        public bool FromProductionBuild = true;

        public Dictionary<SlotID, Slot> MyMoonSlots = new();
        public List<Challenge>? NearMyChallengeDataLog = [];

        public List<Challenge>? NearMyChallengeDataOpen = [];

        public List<Treasure>? NearMyTreasureLog = [];
        public List<SlotID>? DownloadedSlots = [];
        public ResourceDescriptor? PlanetDecorations;


        public override void Serialize() 
        {
            Serializer.Serialize(ref Inventory);

            var revision = Serializer.GetRevision();
            var version = revision.GetVersion();

            if (version >= 0x3ea) 
            {
                if (Serializer.IsWriting())
                {
                    if (VitaCrossDependencyHashes != null) 
                    {
                        var stream = Serializer.GetOutput();
                        stream.I32(VitaCrossDependencyHashes.Count);
                        foreach (var hash in VitaCrossDependencyHashes) 
                        {
                            stream.Sha1(hash);
                        }
                    }
                    else Serializer.Serialize(ref 0);
                }
                else 
                {
                    var stream = Serializer.GetInput();
                    var size = stream.I32();
                    VitaCrossDependencyHashes = new List<Sha1>(size);
                    for (var i = 0; i < size; i++)
                    {
                        VitaCrossDependencyHashes.Add(stream.Sha1());
                    }
                }
            }

            if (version >= (int)Revisions.DATALABELS) 
            {
                Serializer.Serialize(ref CreatorDataLabels);
            }

            Serializer.Serialize(ref StringTable);

            if (version >= (int)Revisions.PRODUCTION_BUILD) 
            {
                Serializer.Serialize(ref FromProductionBuild);
            }

            if (Serializer.IsWriting())
            {
                var keys = MyMoonSlots.Keys.ToHashSet();
                Serializer.GetOutput().I32(keys.Count);
                foreach (var key in keys) 
                {
                    Serializer.Serialize(ref key);
                    Serializer.Serialize(ref MyMoonSlots[key]);
                }
            }
            else 
            {
                var count = Serializer.GetInput().I32();
                MyMoonSlots = new Dictionary<SlotID, Slot>(count);
                for (var i = 0; i < count; i++)
                {
                    MyMoonSlots.Add(
                        Serializer.Serialize(ref new SlotID()),
                        Serializer.Serialize(ref new Slot())
                    );
                }
            }

            if (revision.IsVita())
            {
                if (revision.Has(Branch.Double11, (int)Revisions.D_1DATALABELS))
                    Serializer.Serialize(ref CreatorDataLabels);

                if (revision.Has(Branch.Double11, (int)Revisions.D1_NEAR_CHALLENGES))
                {
                    Serializer.Serialize(ref NearMyChallengeDataLog);
                    Serializer.Serialize(ref NearMyChallengeDataOpen);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1_NEAR_TREASURES))
                {
                    Serializer.Serialize(ref NearMyTreasureLog);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D_1DOWNLOADED_SLOTS))
                {
                    Serializer.Serialize(ref DownloadedSlots);
                }
                if (revision.Has(Branch.Double11, (int)Revisions.D1_PLANET_DECORATIONS))
                {
                    Serializer.Serialize(ref PlanetDecorations, PlanetDecorations, ResourceType.Level, true);
                    if (PlanetDecorations != null)
                        Serializer.AddDependency(PlanetDecorations);
                }
            }
        }

        public SlotID GetNextSlotId() 
        {
            var next = new SlotID(SlotType.USER_CREATED_STORED_LOCAL, 0x0);
            while (true)
            {
                if (!MyMoonSlots.ContainsKey(next)) 
                {
                    return next;
                }

                next.SlotNumber++;
            }
        }

        public int GetNextUid()
        {
            var uid = (Inventory ?? throw new NullReferenceException()).Select(item => (int)(item.Uid & ~0x80000000)).Prepend(1).Max();
            return (int)((uid + 1) | 0x80000000);
        }

        public override int GetAllocatedSize() 
        {
            var size = BaseAllocationSize;
            size += StringTable.GetAllocatedSize();
            if (Inventory != null)
            {
                size += Inventory.Sum(item => item.GetAllocatedSize());
            }

            size += MyMoonSlots.Values.Sum(slot => slot.GetAllocatedSize() + SlotID.BaseAllocationSize);
            return size;
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            var serializer = new Serializer(GetAllocatedSize(), revision, compressionFlags);
            Serializer.Serialize(ref this);
            return new SerializationData(
                Serializer.GetOutput().GetBuffer(),
                revision,
                compressionFlags,
                ResourceType.BigProfile,
                SerializationType.BINARY,
                Serializer.GetDependencies()
            );
        }
    }
}