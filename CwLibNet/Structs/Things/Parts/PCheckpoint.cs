using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using CwLibNet.Types.Things;

namespace CwLibNet.Structs.Things.Parts;

public class PCheckpoint: ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x60;

    public byte ActiveFlags;
    public int ActivationFrame;

    
    public int SpawnsLeft;
    
    public int MaxSpawnsLeft;
    
    public bool InstanceInfiniteSpawns;

    
    public Thing[]? SpawningList;
    
    public int SpawningDelay;
    
    public int LifeMultiplier;

    
    public int TeamFilter;

    
    public int CreatureToSpawnAs;
    
    public bool IsStartPoint;
    
    public bool LinkVisibleOnPlanet;
    
    public string? Name;

    
    public int Unk1;
    
    public string? Unk2;

    
    public int CheckpointType, TeamFlags;

    
    
    public bool EnableAudio;

    
    public bool ContinueMusic;

    
    public int CreatureToChangeBackTo;
    
    public bool ChangeBackGate;

    
    public bool PersistLives;

    
    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        if (subVersion > 0x101) ActiveFlags = serializer.I8(ActiveFlags);
        else
        {
            if (serializer.IsWriting())
                serializer.GetOutput().Boole((ActiveFlags & 0xf) != 0);
            else
                ActiveFlags = (byte)(serializer.GetInput().Boole() ? (0xf) : ((byte) 0));
        }

        ActivationFrame = serializer.I32(ActivationFrame);

        if (version < 0x1f3)
        {
            serializer.Reference<Thing>(null);
            serializer.I32(0);
        }

        if (version >= 0x1a7)
            SpawnsLeft = serializer.S32(SpawnsLeft);
        if (version >= 0x1c6)
            MaxSpawnsLeft = serializer.S32(MaxSpawnsLeft);

        if (version >= 0x1eb)
            InstanceInfiniteSpawns = serializer.Bool(InstanceInfiniteSpawns);

        if (version >= 0x1f3)
        {
            SpawningList = serializer.Array(SpawningList, true);
            SpawningDelay = serializer.I32(SpawningDelay);
        }

        if (version >= 0x1fa)
            LifeMultiplier = serializer.I32(LifeMultiplier);

        if (version >= 0x2ae && subVersion < 0x100)
            TeamFilter = serializer.I32(TeamFilter);

        if (subVersion >= 0x1 && subVersion < 0x127)
            serializer.U8(0); // persistPoint

        if (subVersion >= 0x88)
        {

            if (subVersion <= 0x12a)
            {
                ResourceDescriptor descriptor = null;
                if (serializer.IsWriting())
                {
                    if (CreatureToSpawnAs != 0)
                        descriptor = new ResourceDescriptor((uint)CreatureToChangeBackTo,
                            ResourceType.Plan);
                }
                descriptor = serializer.Resource(descriptor, ResourceType.Plan);
                if (!serializer.IsWriting())
                {
                    if (descriptor != null && descriptor.IsGUID())
                        CreatureToChangeBackTo = (int) descriptor.GetGUID().Value.Value;
                }
            }

            if (subVersion >= 0xc5)
                CreatureToSpawnAs = serializer.S32(CreatureToSpawnAs);
        }
        if (subVersion >= 0xcb)
            IsStartPoint = serializer.Bool(IsStartPoint);
        if (subVersion >= 0xd5)
            LinkVisibleOnPlanet = serializer.Bool(LinkVisibleOnPlanet);

        if (subVersion >= 0x108 && subVersion < 0x129)
            serializer.U8(0);

        if (subVersion >= 0xcb)
            Name = serializer.Wstr(Name);
        if (subVersion >= 0xe4)
            Unk1 = serializer.I32(Unk1);
        if (subVersion >= 0xd1)
            Unk2 = serializer.Wstr(Unk2);

        if (subVersion >= 0x101)
        {
            CheckpointType = serializer.I32(CheckpointType);
            TeamFlags = serializer.I32(TeamFlags);
        }

        if (revision.Has(Branch.Double11, (int)Revisions.D1CheckpointPlayAudio) || subVersion >= 0x15a)
            EnableAudio = serializer.Bool(EnableAudio);

        if (subVersion >= 0x191)
            ContinueMusic = serializer.Bool(ContinueMusic);

        if (subVersion >= 0x199)
        {
            CreatureToChangeBackTo = serializer.I32(CreatureToChangeBackTo);
            ChangeBackGate = serializer.Bool(ChangeBackGate);
        }

        if (subVersion >= 0x19b)
            PersistLives = serializer.Bool(PersistLives);
    }

    
    public int GetAllocatedSize()
    {
        int size = BASE_ALLOCATION_SIZE;
        if (SpawningList != null) size += (SpawningList.Length * 4);
        if (Name != null) size += (Name.Length * 2);
        if (Unk2 != null) size += (Unk2.Length * 2);
        return size;
    }
}