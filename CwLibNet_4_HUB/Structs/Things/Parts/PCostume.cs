using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Mesh;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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

    
    public void Serialize()
    {
        var revision = Serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Serializer.Serialize(ref Mesh, Mesh, ResourceType.Mesh);
        Serializer.Serialize(ref Material, Material, ResourceType.GfxMaterial);

        if (version >= 0x19a)
            MaterialPlan = Serializer.Serialize(ref MaterialPlan, ResourceType.Plan);

        if (Serializer.IsWriting())
        {
            var vec = MeshPartsHidden.ToArray();
            Serializer.Serialize(ref vec);
        }
        else
        {
            var Serializer.Serialize(ref null);
            if (vec != null)
            {
                foreach (var v in vec)
                    MeshPartsHidden.Add(v);
            }
        }

        Primitives = Serializer.Serialize(ref Primitives);

        if (subVersion >= 0xdb)
            Serializer.Serialize(ref CreatureFilter);

        CostumePieces = Serializer.Serialize(ref CostumePieces);

        if (version >= 0x2c5 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_TEMP_COSTUME))
            TemporaryCostumePiece = Serializer.Serialize(ref TemporaryCostumePiece);
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