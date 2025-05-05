using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

public class PGroup: ISerializable
{
    public const int BaseAllocationSize = 0x60;


    public Thing[] Things;

    
    public ResourceDescriptor PlanDescriptor;

    
    public NetworkPlayerID Creator = new NetworkPlayerID();

    
    public Thing Emitter;

    
    public int Lifetime, AliveFrames;

    public int Flags;

    
    
    public bool MainSelectableObject;

    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();
        var isWriting = serializer.IsWriting();

        if (version is >= 0x18e and <= 0x1b0)
            Things = serializer.Array(Things, true);

        if (version is >= 0x18e and < 0x341)
        {
            if (isWriting) serializer.GetOutput().Boole((Flags & GroupFlags.COPYRIGHT) != 0);
            else if (serializer.GetInput().Boole())
                Flags |= GroupFlags.COPYRIGHT;
        }

        if (version >= 0x18e)
            Creator = serializer.Struct(Creator);

        if (version is >= 0x25e or 0x252)
            PlanDescriptor = serializer.Resource(PlanDescriptor, ResourceType.Plan,
                true);

        if (version is >= 0x25e and < 0x341)
        {
            if (isWriting) serializer.GetOutput().Boole((Flags & GroupFlags.EDITABLE) != 0);
            else if (serializer.GetInput().Boole())
                Flags |= GroupFlags.EDITABLE;
        }

        if (version >= 0x267)
        {
            Emitter = serializer.Reference(Emitter);
            Lifetime = serializer.I32(Lifetime);
            AliveFrames = serializer.I32(AliveFrames);
        }

        if (version is >= 0x26e and < 0x341)
        {
            if (isWriting)
                serializer.GetOutput().Boole((Flags & GroupFlags.PICKUP_ALL_MEMBERS) != 0);
            else if (serializer.GetInput().Boole())
                Flags |= GroupFlags.PICKUP_ALL_MEMBERS;
        }

        switch (version)
        {
            case >= 0x30f and < 0x341:
                MainSelectableObject = serializer.Bool(MainSelectableObject);
                break;
            case >= 0x341:
                Flags = serializer.U8(Flags);
                break;
        }
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}