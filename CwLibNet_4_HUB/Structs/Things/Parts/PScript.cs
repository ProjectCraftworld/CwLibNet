using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Things.Components.Script;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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