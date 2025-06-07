using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
        var isWriting = Serializer.IsWriting();

        if (version is >= 0x18e and <= 0x1b0)
            Serializer.Serialize(ref Things, true);

        if (version is >= 0x18e and < 0x341)
        {
            if (isWriting) Serializer.GetCurrentSerializer().GetOutput().Boole((Flags & GroupFlags.COPYRIGHT) != 0);
            else if (Serializer.GetCurrentSerializer().GetInput().Boole())
                Flags |= GroupFlags.COPYRIGHT;
        }

        if (version >= 0x18e)
            Serializer.Serialize(ref Creator);

        if (version is >= 0x25e or 0x252)
            Serializer.Serialize(ref PlanDescriptor, ResourceType.Plan);

        if (version is >= 0x25e and < 0x341)
        {
            if (isWriting) Serializer.GetCurrentSerializer().GetOutput().Boole((Flags & GroupFlags.EDITABLE) != 0);
            else if (Serializer.GetCurrentSerializer().GetInput().Boole())
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
                Serializer.GetCurrentSerializer().GetOutput().Boole((Flags & GroupFlags.PICKUP_ALL_MEMBERS) != 0);
            else if (Serializer.GetCurrentSerializer().GetInput().Boole())
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