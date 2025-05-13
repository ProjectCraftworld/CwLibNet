using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs;
using CwLibNet.Structs.Profile;
using static CwLibNet.Structs.Profile.Challenge;
using static CwLibNet.Structs.Profile.DataLabel;
using static CwLibNet.Structs.Profile.InventoryItem;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;
using CwLibNet.Types;

namespace CwLibNet.Resources 
{
    public class RBigProfile : Resource 
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;

        public List<InventoryItem> inventory = new List<InventoryItem>();

        public List<SHA1> vitaCrossDependencyHashes = new List<SHA1>();

        public List<DataLabel> creatorDataLabels = new List<DataLabel>();

        public StringLookupTable stringTable = new StringLookupTable();
        public bool fromProductionBuild = true;

        public Dictionary<SlotID, Slot> myMoonSlots = new Dictionary<SlotID, Slot>();
        public List<Challenge> nearMyChallengeDataLog = new List<Challenge>(),
            nearMyChallengeDataOpen = new List<Challenge>();
        public List<Treasure> nearMyTreasureLog = new List<Treasure>();
        public List<SlotID> downloadedSlots = new List<SlotID>();
        public ResourceDescriptor planetDecorations;


        public override void Serialize(Serializer serializer) 
        {
            inventory = serializer.Arraylist<InventoryItem>(inventory, true);

            Revision revision = serializer.GetRevision();
            int version = revision.GetVersion();

            if (version >= 0x3ea) 
            {
                if (serializer.IsWriting())
                {
                    if (vitaCrossDependencyHashes != null) 
                    {
                        MemoryOutputStream stream = serializer.GetOutput();
                        stream.I32(vitaCrossDependencyHashes.Count);
                        foreach (SHA1 hash in vitaCrossDependencyHashes) 
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
                    vitaCrossDependencyHashes = new List<SHA1>(size);
                    for (int i = 0; i < size; i++)
                    {
                        vitaCrossDependencyHashes.Add(stream.Sha1());
                    }
                }
            }

            if (version >= (int)Revisions.Datalabels) 
            {
                creatorDataLabels = serializer.Arraylist<DataLabel>(creatorDataLabels, true);
            }

            stringTable = serializer.Struct<StringLookupTable>(stringTable);

            if (version >= (int)Revisions.ProductionBuild) 
            {
                fromProductionBuild = serializer.Bool(fromProductionBuild);
            }

            if (serializer.IsWriting())
            {
                HashSet<SlotID> keys = myMoonSlots.Keys.ToHashSet();
                serializer.GetOutput().I32(keys.Count);
                foreach (SlotID key in keys) 
                {
                    serializer.Struct<SlotID>(key);
                    serializer.Struct<Slot>(myMoonSlots[key]);
                }
            }
            else 
            {
                int count = serializer.GetInput().I32();
                myMoonSlots = new Dictionary<SlotID, Slot>(count);
                for (int i = 0; i < count; i++)
                {
                    myMoonSlots.Add(
                        serializer.Struct<SlotID>(new SlotID()),
                        serializer.Struct<Slot>(new Slot())
                    );
                }
            }

            if (revision.IsVita())
            {
                if (revision.Has(Branch.Double11, (int)Revisions.D1Datalabels))
                    creatorDataLabels = serializer.Arraylist<DataLabel>(creatorDataLabels, true);

                if (revision.Has(Branch.Double11, (int)Revisions.D1NearChallenges))
                {
                    nearMyChallengeDataLog = serializer.Arraylist<Challenge>(nearMyChallengeDataLog, true);
                    nearMyChallengeDataOpen = serializer.Arraylist<Challenge>(nearMyChallengeDataOpen, true);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1NearTreasures))
                {
                    nearMyTreasureLog = serializer.Arraylist<Treasure>(nearMyTreasureLog, true);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1DownloadedSlots))
                {
                    downloadedSlots = serializer.Arraylist<SlotID>(downloadedSlots, true);
                }
                if (revision.Has(Branch.Double11, (int)Revisions.D1PlanetDecorations))
                {
                    planetDecorations = serializer.Resource(planetDecorations, ResourceType.Level, true);
                    if (planetDecorations != null)
                        serializer.AddDependency(planetDecorations);
                }
            }
        }

        public SlotID GetNextSlotID() 
        {
            SlotID next = new SlotID(SlotType.USER_CREATED_STORED_LOCAL, 0x0);
            while (true)
            {
                if (!myMoonSlots.ContainsKey(next)) 
                {
                    return next;
                }
                else 
                {
                    next.SlotNumber++;
                }
            }
        }

        public int GetNextUID()
        {
            int UID = 1;
            foreach (InventoryItem item in inventory)
            {
                int fixedUID = (int)(item.UID & ~0x80000000);
                if (fixedUID > UID)
                    UID = fixedUID;
            }
            return (int)((UID + 1) | 0x80000000);
        }

        public override int GetAllocatedSize() 
        {
            int size = BASE_ALLOCATION_SIZE;
            size += stringTable.GetAllocatedSize();
            if (inventory != null)
            {
                foreach (InventoryItem item in inventory)
                {
                    size += item.GetAllocatedSize();
                }
            }
            foreach (Slot slot in myMoonSlots.Values)
            {
                size += slot.GetAllocatedSize() + SlotID.BASE_ALLOCATION_SIZE;
            }
            return size;
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            Serializer serializer = new Serializer(GetAllocatedSize(), revision, compressionFlags);
            serializer.Struct<RBigProfile>(this);
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