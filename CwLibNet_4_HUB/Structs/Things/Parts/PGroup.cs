using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PGroup: ISerializable
{
    public const int BaseAllocationSize = 0x60;


    public Thing[]? Things;

    
    public ResourceDescriptor? PlanDescriptor;

    
    public NetworkPlayerID Creator = new();

    
    public Thing? Emitter;

    
    public int Lifetime, AliveFrames;

    public int Flags;

    
    
    public bool MainSelectableObject;

    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();
        var isWriting = Serializer.IsWriting();

        if (version is >= 0x18e and <= 0x1b0)
            Things = Serializer.Serialize(ref Things, true);

        if (version is >= 0x18e and < 0x341)
        {
            if (isWriting) Serializer.GetOutput().Boole((Flags & GroupFlags.COPYRIGHT) != 0);
            else if (Serializer.GetInput().Boole())
                Flags |= GroupFlags.COPYRIGHT;
        }

        if (version >= 0x18e)
            Serializer.Serialize(ref Creator);

        if (version is >= 0x25e or 0x252)
            PlanDescriptor = Serializer.Serialize(ref PlanDescriptor, ResourceType.Plan);

        if (version is >= 0x25e and < 0x341)
        {
            if (isWriting) Serializer.GetOutput().Boole((Flags & GroupFlags.EDITABLE) != 0);
            else if (Serializer.GetInput().Boole())
                Flags |= GroupFlags.EDITABLE;
        }

        if (version >= 0x267)
        {
            Serializer.Serialize(ref Emitter);
            Serializer.Serialize(ref Lifetime);
            Serializer.Serialize(ref AliveFrames);
        }

        if (version is >= 0x26e and < 0x341)
        {
            if (isWriting)
                Serializer.GetOutput().Boole((Flags & GroupFlags.PICKUP_ALL_MEMBERS) != 0);
            else if (Serializer.GetInput().Boole())
                Flags |= GroupFlags.PICKUP_ALL_MEMBERS;
        }

        switch (version)
        {
            case >= 0x30f and < 0x341:
                Serializer.Serialize(ref MainSelectableObject);
                break;
            case >= 0x341:
                Serializer.Serialize(ref Flags);
                break;
        }
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}