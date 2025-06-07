using CwLibNet.Types.Data;

using static net.torutheredfox.craftworld.serialization.Serializer;
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

            public void Serialize()
            {
                Creature = Serializer.Reference(Creature);
                Serializer.Serialize(ref MainFormCostume, MainFormCostume, ResourceType.Plan, true);
                Serializer.Serialize(ref AltFormCostume, AltFormCostume, ResourceType.Plan, true);
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

        public override void Serialize()
        {
            var version = Serializer.GetRevision().GetVersion();
            var subVersion = Serializer.GetRevision().GetSubVersion();

            Serializer.Serialize(ref TimePlayed);
            SackboyAvatarWorld = Serializer.Reference(SackboyAvatarWorld);

            if (subVersion > 0x17)
                CreatureAvatars = Serializer.Serialize(ref CreatureAvatars);

            if (version < 0x193)
                Serializer.Array<Thing>(null, true);

            if (version < 0x13e)
                Serializer.Serialize(ref null, ResourceType.Texture);

            Serializer.Serialize(ref TimePlayed);
            Serializer.Serialize(ref UniqueNumber);

            if (version > 0x163)
            {
                Serializer.Serialize(ref Primary);
                Serializer.Serialize(ref Secondary);
                Serializer.Serialize(ref Tertiary);
                if (version > 0x3a6)
                    Serializer.Serialize(ref Emphasis);
            }

            if (version > 0x1a7)
                Serializer.Serialize(ref PlayerID);

            if (version is > 0x1c4 and < 0x213)
                Serializer.Serialize(ref 0);

            if (subVersion >= 0x9e)
                CreatureToSpawnAs = Serializer.Reference(CreatureToSpawnAs);
            if (subVersion >= 0x18a)
                Serializer.Serialize(ref SpawnAsAlternateForm);
            if (subVersion < 0x18d) return;
            CreatureToPodAs = Serializer.Reference(CreatureToSpawnAs);
            Serializer.Serialize(ref PodAsAlternateForm);
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
            Serializer.Serialize(ref this);
            return new SerializationData(
                Serializer.GetBuffer(),
                revision,
                compressionFlags,
                ResourceType.SyncedProfile,
                SerializationType.BINARY,
                Serializer.GetDependencies()
            );
        }
    }
}
