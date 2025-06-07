using CwLibNet.IO;
using CwLibNet.Structs.Things.Components.Script;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
        if (version is > 0x179 and < 0x1a1)
            Serializer.Serialize(ref temp_bool_false); // unknown
        Serializer.Serialize(ref Instance);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}