using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Resources;
using CwLibNet.Structs.Mesh;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Components;

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

    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();
        var subVersion = serializer.GetRevision().GetSubVersion();

        Mesh = serializer.Resource(Mesh, ResourceType.Mesh);
        CategoriesUsed = serializer.I32(CategoriesUsed);

        if (subVersion < 0x105)
        {
            var size = serializer.I32(MorphParamRemap != null ?
                MorphParamRemap.Length : 0);
            if (serializer.IsWriting() && size != 0)
            {
                var stream = serializer.GetOutput();
                foreach (var param in MorphParamRemap)
                    stream.I32(param);
            }
            else if (!serializer.IsWriting())
            {
                MorphParamRemap = new byte[size];
                var stream = serializer.GetInput();
                for (var i = 0; i < size; ++i)
                    MorphParamRemap[i] = (byte) (stream.I32() & 0xFF);
            }
        }
        else MorphParamRemap = serializer.Bytearray(MorphParamRemap);

        Primitives = serializer.Array(Primitives);

        if (version >= 0x19a)
            Plan = serializer.Resource(Plan, ResourceType.Plan, true);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}