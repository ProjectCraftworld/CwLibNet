using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using CwLibNet.Types.Data;

namespace CwLibNet.Resources
{
    public class RSyncedProfile : Resource
    {
        public class CreatureAvatar : ISerializable
        {
            public const int BaseAllocationSize = 0x48;

            public int Creature { get; set; }
            public ResourceDescriptor? MainFormCostume { get; set; }
            public ResourceDescriptor? AltFormCostume { get; set; }

            public void Serialize(Serializer serializer)
            {
                Creature = serializer.AdventureCreatureReference(Creature);
                MainFormCostume = serializer.Resource(MainFormCostume, ResourceType.Plan, true);
                AltFormCostume = serializer.Resource(AltFormCostume, ResourceType.Plan, true);
            }

            public int GetAllocatedSize()
            {
                return BaseAllocationSize;
            }
        }

        public static readonly int BASE_ALLOCATION_SIZE = 0x20;

        public int TimePlayed { get; set; }
        public Thing? SackboyAvatarWorld { get; set; }
        public long UniqueNumber { get; set; }

        public int Primary { get; set; }
        public int Secondary { get; set; }
        public int Tertiary { get; set; }
        public int Emphasis { get; set; }

        public List<CreatureAvatar> CreatureAvatars { get; set; } = [];
        public NetworkPlayerID? PlayerID { get; set; }
        public int CreatureToSpawnAs { get; set; }
        public bool SpawnAsAlternateForm { get; set; }
        public int CreatureToPodAs { get; set; }
        public bool PodAsAlternateForm { get; set; }

        public override void Serialize(Serializer serializer)
        {
            var version = serializer.GetRevision().GetVersion();
            var subVersion = serializer.GetRevision().GetSubVersion();

            TimePlayed = serializer.I32(TimePlayed);
            SackboyAvatarWorld = serializer.Thing(SackboyAvatarWorld);

            if (subVersion > 0x17)
                CreatureAvatars = serializer.Arraylist<CreatureAvatar>(CreatureAvatars);

            if (version < 0x193)
                serializer.Array<Thing>(null, true);

            if (version < 0x13e)
                serializer.Resource(null, ResourceType.Texture);

            TimePlayed = serializer.I32(TimePlayed);
            UniqueNumber = serializer.U64(UniqueNumber);

            if (version > 0x163)
            {
                Primary = serializer.I32(Primary);
                Secondary = serializer.I32(Secondary);
                Tertiary = serializer.I32(Tertiary);
                if (version > 0x3a6)
                    Emphasis = serializer.I32(Emphasis);
            }

            if (version > 0x1a7)
                PlayerID = serializer.Struct(PlayerID);

            if (version is > 0x1c4 and < 0x213)
                serializer.I32(0);

            if (subVersion >= 0x9e)
                CreatureToSpawnAs = serializer.AdventureCreatureReference(CreatureToSpawnAs);
            if (subVersion >= 0x18a)
                SpawnAsAlternateForm = serializer.Bool(SpawnAsAlternateForm);
            if (subVersion < 0x18d) return;
            CreatureToPodAs = serializer.AdventureCreatureReference(CreatureToSpawnAs);
            PodAsAlternateForm = serializer.Bool(PodAsAlternateForm);
        }

        public override int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            // 16MB buffer for generation of levels, since the allocated size will get
            // stuck in a recursive loop until I fix it.
            var serializer = new Serializer(0x1000000, revision, compressionFlags);
            serializer.Struct(this);
            return new SerializationData(
                serializer.GetBuffer(),
                revision,
                compressionFlags,
                ResourceType.SyncedProfile,
                SerializationType.BINARY,
                serializer.GetDependencies()
            );
        }
    }
}
