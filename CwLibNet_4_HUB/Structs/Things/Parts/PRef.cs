using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types.Data;

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

    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();

        if (version < 0x160)
            Thing = serializer.Struct(Thing);
        else Plan = serializer.Resource(Plan, ResourceType.Plan, true, false, false);

        OldLifetime = serializer.I32(OldLifetime);
        if (version >= 0x1c9)
            OldAliveFrames = serializer.I32(OldAliveFrames);
        if (version < 0x321)
            ChildrenSelectable = serializer.Bool(ChildrenSelectable);

        if (version < 0x19e)
            serializer.Array<Thing>(null, true);

        if (version is >= 0x13d and < 0x321)
            StripChildren = serializer.Bool(StripChildren);

        if (version is > 0x171 and < 0x180) serializer.U8(0);
        if (version is <= 0x17f or >= 0x19e) return;
        serializer.U8(0);
        serializer.Struct<NetworkPlayerID>(null);
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}