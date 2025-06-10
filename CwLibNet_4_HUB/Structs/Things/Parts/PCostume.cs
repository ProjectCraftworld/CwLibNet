using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Mesh;
using CwLibNet4Hub.Structs.Things.Components;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        int[]? vec = null;
        
        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Serializer.Serialize(ref Mesh, ResourceType.Mesh, false, true, false);
        Serializer.Serialize(ref Material, ResourceType.GfxMaterial, false, true, false);

        if (version >= 0x19a)
            Serializer.Serialize(ref MaterialPlan, ResourceType.Plan, false, true, false);

        if (Serializer.IsWriting())
        {
            vec = MeshPartsHidden.ToArray();
            Serializer.Serialize(ref vec);
        }
        else
        {
            Serializer.Serialize(ref vec);
            if (vec != null)
            {
                foreach (var v in vec)
                    MeshPartsHidden.Add(v);
            }
        }

        Serializer.Serialize(ref Primitives);

        if (subVersion >= 0xdb)
            Serializer.Serialize(ref CreatureFilter);

        Serializer.Serialize(ref CostumePieces);

        if (version >= 0x2c5 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_TEMP_COSTUME))
            Serializer.Serialize(ref TemporaryCostumePiece);
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