using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Resources;
using CwLibNet.Structs.Mesh;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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

    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();
        var subVersion = Serializer.GetRevision().GetSubVersion();

        Serializer.Serialize(ref Mesh, Mesh, ResourceType.Mesh);
        Serializer.Serialize(ref CategoriesUsed);

        if (subVersion < 0x105)
        {
            var size = Serializer.Serialize(ref MorphParamRemap != null ?
                MorphParamRemap.Length : 0);
            if (Serializer.IsWriting() && size != 0)
            {
                var stream = Serializer.GetOutput();
                foreach (var param in MorphParamRemap)
                    stream.I32(param);
            }
            else if (!Serializer.IsWriting())
            {
                MorphParamRemap = new byte[size];
                var stream = Serializer.GetInput();
                for (var i = 0; i < size; ++i)
                    MorphParamRemap[i] = (byte) (stream.I32() & 0xFF);
            }
        }
        else Serializer.Serialize(ref MorphParamRemap);

        Primitives = Serializer.Serialize(ref Primitives);

        if (version >= 0x19a)
            Serializer.Serialize(ref Plan, Plan, ResourceType.Plan, true);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}