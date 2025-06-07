using CwLibNet.IO;
using CwLibNet.Structs.Things.Components.Script;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PScript: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ScriptInstance Instance = new();

    public PScript() { }

    public PScript(ResourceDescriptor script)
    {
        Instance.Script = script;
    }

    public bool Is(GUID guid)
    {
        if (Instance?.Script == null) return false;
        return Instance.Script.IsGUID() && guid.Equals(Instance.Script.GetGUID());
    }

    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();
        if (version is > 0x179 and < 0x1a1)
            Serializer.Serialize(ref false); // unknown
        Serializer.Serialize(ref Instance);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}