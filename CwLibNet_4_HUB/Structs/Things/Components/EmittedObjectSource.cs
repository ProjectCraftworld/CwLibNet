using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Components;

public class EmittedObjectSource: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public Thing[] Things;
    public ResourceDescriptor Plan;

    // I'll check what these are at some point
    
    public float F0, F1, F2, F3, F4, F5;
    
    public byte B0;

    
    public void Serialize(Serializer serializer)
    {
        Things = serializer.Thingarray(Things);
        Plan = serializer.Resource(Plan, ResourceType.Plan);

        if (serializer.GetRevision().GetSubVersion() <= 0xcc) return;
        F0 = serializer.F32(F0);
        F1 = serializer.F32(F1);
        F2 = serializer.F32(F2);
        F3 = serializer.F32(F3);
        F4 = serializer.F32(F4);
        F5 = serializer.F32(F5);
        B0 = serializer.I8(B0);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Things != null) size += Things.Length * 0x4;
        return size;
    }


}