using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components.Script;
using CwLibNet.Types.Data;

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

    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();
        if (version is > 0x179 and < 0x1a1)
            serializer.Bool(false); // unknown
        Instance = serializer.Struct(Instance);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}