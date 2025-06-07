using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (subVersion > 0x101) Serializer.Serialize(ref ActiveFlags);
        else
        {
            if (Serializer.IsWriting())
                Serializer.GetCurrentSerializer().GetOutput().Boole((ActiveFlags & 0xf) != 0);
            else
                ActiveFlags = (byte)(Serializer.GetCurrentSerializer().GetInput().Boole() ? 0xf : (byte) 0);
        }

        Serializer.Serialize(ref ActivationFrame);

        if (version < 0x1f3)
        {
            serializer.Reference<Thing>(null);
            Serializer.Serialize(ref temp_int);
        }

        if (version >= 0x1a7)
            Serializer.Serialize(ref SpawnsLeft);
        if (version >= 0x1c6)
            Serializer.Serialize(ref MaxSpawnsLeft);

        if (version >= 0x1eb)
            Serializer.Serialize(ref InstanceInfiniteSpawns);

        if (version >= 0x1f3)
        {
            SpawningList = serializer.Thingarray(SpawningList);
            Serializer.Serialize(ref SpawningDelay);
        }

        if (version >= 0x1fa)
            Serializer.Serialize(ref LifeMultiplier);

        if (version >= 0x2ae && subVersion < 0x100)
            Serializer.Serialize(ref TeamFilter);

        if (subVersion is >= 0x1 and < 0x127)
            Serializer.Serialize(ref temp_int); // persistPoint

        if (subVersion >= 0x88)
        {

            if (subVersion <= 0x12a)
            {
                ResourceDescriptor descriptor = null;
                if (Serializer.IsWriting())
                {
                    if (CreatureToSpawnAs != 0)
                        descriptor = new ResourceDescriptor((uint)CreatureToChangeBackTo,
                            ResourceType.Plan);
                }
                Serializer.Serialize(ref descriptor, ResourceType.Plan, false, true, false);
                if (!Serializer.IsWriting())
                {
                    if (descriptor != null && descriptor.IsGUID())
                        CreatureToChangeBackTo = (int) descriptor.GetGUID().Value.Value;
                }
            }

            if (subVersion >= 0xc5)
                Serializer.Serialize(ref CreatureToSpawnAs);
        }
        if (subVersion >= 0xcb)
            Serializer.Serialize(ref IsStartPoint);
        if (subVersion >= 0xd5)
            Serializer.Serialize(ref LinkVisibleOnPlanet);

        if (subVersion is >= 0x108 and < 0x129)
            Serializer.Serialize(ref temp_int);

        if (subVersion >= 0xcb)
            Serializer.Serialize(ref Name);
        if (subVersion >= 0xe4)
            Serializer.Serialize(ref Unk1);
        if (subVersion >= 0xd1)
            Serializer.Serialize(ref Unk2);

        if (subVersion >= 0x101)
        {
            Serializer.Serialize(ref CheckpointType);
            Serializer.Serialize(ref TeamFlags);
        }

        if (revision.Has(Branch.Double11, (int)Revisions.D1_CHECKPOINT_PLAY_AUDIO) || subVersion >= 0x15a)
            Serializer.Serialize(ref EnableAudio);

        if (subVersion >= 0x191)
            Serializer.Serialize(ref ContinueMusic);

        if (subVersion >= 0x199)
        {
            Serializer.Serialize(ref CreatureToChangeBackTo);
            Serializer.Serialize(ref ChangeBackGate);
        }

        if (subVersion >= 0x19b)
            Serializer.Serialize(ref PersistLives);
    }

    
    public int GetAllocatedSize()
    {
        var size = BASE_ALLOCATION_SIZE;
        if (SpawningList != null) size += SpawningList.Length * 4;
        if (Name != null) size += Name.Length * 2;
        if (Unk2 != null) size += Unk2.Length * 2;
        return size;
    }
}