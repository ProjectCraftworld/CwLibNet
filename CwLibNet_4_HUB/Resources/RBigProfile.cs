using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Profile;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

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


        public override void Serialize(Serializer serializer) 
        {
            Inventory = serializer.Arraylist(Inventory, true);

            var revision = serializer.GetRevision();
            var version = revision.GetVersion();

            if (version >= 0x3ea) 
            {
                if (serializer.IsWriting())
                {
                    if (VitaCrossDependencyHashes != null) 
                    {
                        var stream = serializer.GetOutput();
                        stream.I32(VitaCrossDependencyHashes.Count);
                        foreach (var hash in VitaCrossDependencyHashes) 
                        {
                            stream.Sha1(hash);
                        }
                    }
                    else serializer.I32(0);
                }
                else 
                {
                    var stream = serializer.GetInput();
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
                CreatorDataLabels = serializer.Arraylist(CreatorDataLabels, true);
            }

            StringTable = serializer.Struct(StringTable);

            if (version >= (int)Revisions.PRODUCTION_BUILD) 
            {
                FromProductionBuild = serializer.Bool(FromProductionBuild);
            }

            if (serializer.IsWriting())
            {
                var keys = MyMoonSlots.Keys.ToHashSet();
                serializer.GetOutput().I32(keys.Count);
                foreach (var key in keys) 
                {
                    serializer.Struct(key);
                    serializer.Struct(MyMoonSlots[key]);
                }
            }
            else 
            {
                var count = serializer.GetInput().I32();
                MyMoonSlots = new Dictionary<SlotID, Slot>(count);
                for (var i = 0; i < count; i++)
                {
                    MyMoonSlots.Add(
                        serializer.Struct(new SlotID()),
                        serializer.Struct(new Slot())
                    );
                }
            }

            if (revision.IsVita())
            {
                if (revision.Has(Branch.Double11, (int)Revisions.D_1DATALABELS))
                    CreatorDataLabels = serializer.Arraylist(CreatorDataLabels, true);

                if (revision.Has(Branch.Double11, (int)Revisions.D1_NEAR_CHALLENGES))
                {
                    NearMyChallengeDataLog = serializer.Arraylist(NearMyChallengeDataLog, true);
                    NearMyChallengeDataOpen = serializer.Arraylist(NearMyChallengeDataOpen, true);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1_NEAR_TREASURES))
                {
                    NearMyTreasureLog = serializer.Arraylist(NearMyTreasureLog, true);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D_1DOWNLOADED_SLOTS))
                {
                    DownloadedSlots = serializer.Arraylist(DownloadedSlots, true);
                }
                if (revision.Has(Branch.Double11, (int)Revisions.D1_PLANET_DECORATIONS))
                {
                    PlanetDecorations = serializer.Resource(PlanetDecorations, ResourceType.Level, true);
                    if (PlanetDecorations != null)
                        serializer.AddDependency(PlanetDecorations);
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
            serializer.Struct(this);
            return new SerializationData(
                serializer.GetOutput().GetBuffer(),
                revision,
                compressionFlags,
                ResourceType.BigProfile,
                SerializationType.BINARY,
                serializer.GetDependencies()
            );
        }
    }
}