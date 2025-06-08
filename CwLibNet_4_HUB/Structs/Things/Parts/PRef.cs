using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        if (version < 0x160)
            Serializer.Serialize(ref Thing);
        else Serializer.Serialize(ref Plan, ResourceType.Plan, true, false, false);

        Serializer.Serialize(ref OldLifetime);
        if (version >= 0x1c9)
            Serializer.Serialize(ref OldAliveFrames);
        if (version < 0x321)
            Serializer.Serialize(ref ChildrenSelectable);

        if (version < 0x19e)
            serializer.Array<Thing>(null, true);

        if (version is >= 0x13d and < 0x321)
            Serializer.Serialize(ref StripChildren);

        if (version is > 0x171 and < 0x180) 
        {
            var tempInt = 0;
            Serializer.Serialize(ref tempInt);
        }
        if (version is <= 0x17f or >= 0x19e) return;
        {
            var tempInt = 0;
            Serializer.Serialize(ref tempInt);
        }
        {
            var tempNetworkId = default(NetworkPlayerID);
            Serializer.Serialize(ref tempNetworkId);
        }
    }

    // TODO: Actually implement
    
    public int GetAllocatedSize()
    {
        return 0;
    }


}