using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components;

public class EmittedObjectSource: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public Thing[] Things;
    public ResourceDescriptor Plan;

    // I'll check what these are at some point
    
    public float F0, F1, F2, F3, F4, F5;
    
    public byte B0;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Things);
        Serializer.Serialize(ref Plan, ResourceType.Plan, false, true, false);

        if (Serializer.GetCurrentSerializer().GetRevision().GetSubVersion() <= 0xcc) return;
        Serializer.Serialize(ref F0);
        Serializer.Serialize(ref F1);
        Serializer.Serialize(ref F2);
        Serializer.Serialize(ref F3);
        Serializer.Serialize(ref F4);
        Serializer.Serialize(ref F5);
        Serializer.Serialize(ref B0);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Things != null) size += Things.Length * 0x4;
        return size;
    }


}