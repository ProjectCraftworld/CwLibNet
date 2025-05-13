using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Profile;
using CwLibNet.Structs.Slot;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Resources 
{
    public class RBigProfile : Resource 
    {
        public const int BaseAllocationSize = 0x20;

        public List<InventoryItem>? Inventory = [];

        public List<SHA1> VitaCrossDependencyHashes = [];

        public List<DataLabel>? CreatorDataLabels = [];

        public StringLookupTable StringTable = new StringLookupTable();
        public bool FromProductionBuild = true;

        public Dictionary<SlotID, Slot> MyMoonSlots = new Dictionary<SlotID, Slot>();
        public List<Challenge>? NearMyChallengeDataLog = [];

        public List<Challenge>? NearMyChallengeDataOpen = [];

        public List<Treasure>? NearMyTreasureLog = [];
        public List<SlotID>? DownloadedSlots = [];
        public ResourceDescriptor PlanetDecorations;


        public override void Serialize(Serializer serializer) 
        {
            Inventory = serializer.Arraylist(Inventory, true);

            Revision revision = serializer.GetRevision();
            int version = revision.GetVersion();

            if (version >= 0x3ea) 
            {
                if (serializer.IsWriting())
                {
                    if (VitaCrossDependencyHashes != null) 
                    {
                        MemoryOutputStream stream = serializer.GetOutput();
                        stream.I32(VitaCrossDependencyHashes.Count);
                        foreach (SHA1 hash in VitaCrossDependencyHashes) 
                        {
                            stream.Sha1(hash);
                        }
                    }
                    else serializer.I32(0);
                }
                else 
                {
                    MemoryInputStream stream = serializer.GetInput();
                    int size = stream.I32();
                    VitaCrossDependencyHashes = new List<SHA1>(size);
                    for (int i = 0; i < size; i++)
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
                HashSet<SlotID> keys = MyMoonSlots.Keys.ToHashSet();
                serializer.GetOutput().I32(keys.Count);
                foreach (SlotID key in keys) 
                {
                    serializer.Struct(key);
                    serializer.Struct(MyMoonSlots[key]);
                }
            }
            else 
            {
                int count = serializer.GetInput().I32();
                MyMoonSlots = new Dictionary<SlotID, Slot>(count);
                for (int i = 0; i < count; i++)
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
            SlotID next = new SlotID(SlotType.USER_CREATED_STORED_LOCAL, 0x0);
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
            int uid = Inventory.Select(item => (int)(item.Uid & ~0x80000000)).Prepend(1).Max();
            return (int)((uid + 1) | 0x80000000);
        }

        public override int GetAllocatedSize() 
        {
            int size = BaseAllocationSize;
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
            Serializer serializer = new Serializer(GetAllocatedSize(), revision, compressionFlags);
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