using CwLibNet.Types.Data;
using CwLibNet.IO;
using CwLibNet.Enums;
using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
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

            public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
            {
                int tempCreature = Creature;
                Serializer.Serialize(ref tempCreature);
                Creature = tempCreature;
                var tempMainForm = MainFormCostume;
                Serializer.Serialize(ref tempMainForm, ResourceType.Plan, true, false, false);
                MainFormCostume = tempMainForm;
                var tempAltForm = AltFormCostume;
                Serializer.Serialize(ref tempAltForm, ResourceType.Plan, true, false, false);
                AltFormCostume = tempAltForm;
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

        public override void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
            var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
            var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

            var tempTimePlayed1 = TimePlayed;
            Serializer.Serialize(ref tempTimePlayed1);
            TimePlayed = tempTimePlayed1;
            var sackboyTemp = SackboyAvatarWorld;
            sackboyTemp = Serializer.SerializeReference(sackboyTemp);
            SackboyAvatarWorld = sackboyTemp;

            if (subVersion > 0x17)
            {
                var creaturesTemp = CreatureAvatars;
                Serializer.Serialize(ref creaturesTemp);
                CreatureAvatars = creaturesTemp;
            }

            if (version < 0x193)
                Serializer.GetCurrentSerializer().Array<Thing>(null, true);

            if (version < 0x13e)
            {
                ResourceDescriptor? tempTexture = null;
                Serializer.Serialize(ref tempTexture, ResourceType.Texture, true, false, false);
            }

            var tempTimePlayed2 = TimePlayed;
            Serializer.Serialize(ref tempTimePlayed2);
            TimePlayed = tempTimePlayed2;
            var tempUniqueNumber = UniqueNumber;
            Serializer.Serialize(ref tempUniqueNumber);
            UniqueNumber = tempUniqueNumber;

            if (version > 0x163)
            {
                int tempPrimary = Primary;
                int tempSecondary = Secondary;
                int tempTertiary = Tertiary;
                Serializer.Serialize(ref tempPrimary);
                Serializer.Serialize(ref tempSecondary);
                Serializer.Serialize(ref tempTertiary);
                Primary = tempPrimary;
                Secondary = tempSecondary;
                Tertiary = tempTertiary;
                if (version > 0x3a6)
                {
                    int tempEmphasis = Emphasis;
                    Serializer.Serialize(ref tempEmphasis);
                    Emphasis = tempEmphasis;
                }
            }

            if (version > 0x1a7)
            {
                var tempPlayerID = PlayerID;
                Serializer.Serialize(ref tempPlayerID);
                PlayerID = tempPlayerID;
            }

            if (version is > 0x1c4 and < 0x213)
            {
                int tempValue = 0;
                Serializer.Serialize(ref tempValue);
            }

            if (subVersion >= 0x9e)
            {
                int tempCreatureToSpawn = CreatureToSpawnAs;
                Serializer.Serialize(ref tempCreatureToSpawn);
                CreatureToSpawnAs = tempCreatureToSpawn;
            }
            if (subVersion >= 0x18a)
            {
                bool tempSpawnAsAlternate = SpawnAsAlternateForm;
                Serializer.Serialize(ref tempSpawnAsAlternate);
                SpawnAsAlternateForm = tempSpawnAsAlternate;
            }
            if (subVersion < 0x18d) return;
            int tempCreatureToPod = CreatureToPodAs;
            Serializer.Serialize(ref tempCreatureToPod);
            CreatureToPodAs = tempCreatureToPod;
            bool tempPodAsAlternate = PodAsAlternateForm;
            Serializer.Serialize(ref tempPodAsAlternate);
            PodAsAlternateForm = tempPodAsAlternate;
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
            this.Serialize(serializer);
            return new SerializationData(
                Serializer.GetCurrentSerializer().GetBuffer(),
                revision,
                compressionFlags,
                ResourceType.SyncedProfile,
                SerializationType.BINARY,
                Serializer.GetCurrentSerializer().GetDependencies()
            );
        }
    }
}
