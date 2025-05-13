using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Mesh;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

public class PCostume: ISerializable
{
    public const int BaseAllocationSize = 0x80;

    public ResourceDescriptor? Mesh;
    public ResourceDescriptor? Material;

    
    public ResourceDescriptor? MaterialPlan;

    public List<int> MeshPartsHidden = [];
    public Primitive[]? Primitives;

    
    public byte CreatureFilter;

    public CostumePiece[]? CostumePieces;

    
    
    public CostumePiece[]? TemporaryCostumePiece;

    public PCostume()
    {
        Mesh = new ResourceDescriptor(1087, ResourceType.Mesh);
        CostumePieces = new CostumePiece[14];
        for (var i = 0; i < CostumePieces.Length; ++i)
            CostumePieces[i] = new CostumePiece();
        CostumePieces[(int)CostumePieceCategory.HEAD].Mesh =
            new ResourceDescriptor(9876, ResourceType.Mesh);
        CostumePieces[(int)CostumePieceCategory.TORSO].Mesh =
            new ResourceDescriptor(9877, ResourceType.Mesh);
        TemporaryCostumePiece = [new CostumePiece()];
    }

    
    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Mesh = serializer.Resource(Mesh, ResourceType.Mesh);
        Material = serializer.Resource(Material, ResourceType.GfxMaterial);

        if (version >= 0x19a)
            MaterialPlan = serializer.Resource(MaterialPlan, ResourceType.Plan,
                true);

        if (serializer.IsWriting())
        {
            var vec = MeshPartsHidden.ToArray();
            serializer.Intvector(vec);
        }
        else
        {
            var vec = serializer.Intvector(null);
            if (vec != null)
            {
                foreach (var v in vec)
                    MeshPartsHidden.Add(v);
            }
        }

        Primitives = serializer.Array(Primitives);

        if (subVersion >= 0xdb)
            CreatureFilter = serializer.I8(CreatureFilter);

        CostumePieces = serializer.Array(CostumePieces);

        if (version >= 0x2c5 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_TEMP_COSTUME))
            TemporaryCostumePiece = serializer.Array(TemporaryCostumePiece);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (CostumePieces != null) size += CostumePieces.Sum(piece => piece.GetAllocatedSize());
        if (Primitives != null)
            size += (Primitives.Length * Primitive.BaseAllocationSize);
        if (MeshPartsHidden != null) size += (MeshPartsHidden.Count * 4);
        return size;
    }


}