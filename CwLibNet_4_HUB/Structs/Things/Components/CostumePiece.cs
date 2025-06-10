using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Resources;
using CwLibNet4Hub.Structs.Mesh;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components;

public class CostumePiece: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public ResourceDescriptor? Mesh;
    public int CategoriesUsed;
    public byte[] MorphParamRemap;
    public Primitive[]? Primitives;
    
    public ResourceDescriptor? Plan;

    public CostumePiece()
    {
        MorphParamRemap = new byte[RMesh.MaxMorphs];
        for (var i = 0; i < MorphParamRemap.Length; ++i)
            MorphParamRemap[i] = 0xFF; // 255 in decimal, equivalent to -1 in byte
    }

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        Serializer.Serialize(ref Mesh, ResourceType.Mesh, true, true, false);
        Serializer.Serialize(ref CategoriesUsed);

        if (subVersion < 0x105)
        {
            int size = MorphParamRemap != null ? MorphParamRemap.Length : 0;
            Serializer.Serialize(ref size);
            if (Serializer.IsWriting() && size != 0)
            {
                var stream = Serializer.GetCurrentSerializer().GetOutput();
                foreach (var param in MorphParamRemap)
                    stream.I32(param);
            }
            else if (!Serializer.IsWriting())
            {
                MorphParamRemap = new byte[size];
                var stream = Serializer.GetCurrentSerializer().GetInput();
                for (var i = 0; i < size; ++i)
                    MorphParamRemap[i] = (byte) (stream.I32() & 0xFF);
            }
        }
        else Serializer.Serialize(ref MorphParamRemap);

        Serializer.Serialize(ref Primitives);

        if (version >= 0x19a)
            Serializer.Serialize(ref Plan, ResourceType.Plan, true, true, true);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}