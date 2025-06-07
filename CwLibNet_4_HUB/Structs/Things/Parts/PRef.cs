using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PRef: ISerializable
{
    
    
    public GlobalThingDescriptor Thing;

    
    public ResourceDescriptor Plan;

    public int OldLifetime;

    
    public int OldAliveFrames;

    
    public bool ChildrenSelectable;

    
    public bool StripChildren;

    public PRef() { }

    public PRef(ResourceDescriptor descriptor)
    {
        Plan = descriptor;
    }

    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        if (version < 0x160)
            Serializer.Serialize(ref Thing);
        else Serializer.Serialize(ref Plan, Plan, ResourceType.Plan, true, false, false);

        Serializer.Serialize(ref OldLifetime);
        if (version >= 0x1c9)
            Serializer.Serialize(ref OldAliveFrames);
        if (version < 0x321)
            Serializer.Serialize(ref ChildrenSelectable);

        if (version < 0x19e)
            Serializer.Array<Thing>(null, true);

        if (version is >= 0x13d and < 0x321)
            Serializer.Serialize(ref StripChildren);

        if (version is > 0x171 and < 0x180) Serializer.Serialize(ref 0);
        if (version is <= 0x17f or >= 0x19e) return;
        Serializer.Serialize(ref 0);
        Serializer.Serialize(ref default(NetworkPlayerID));
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}