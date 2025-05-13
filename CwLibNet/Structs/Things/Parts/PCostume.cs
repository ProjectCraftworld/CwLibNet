using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Mesh;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

public class PCostume: ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x80;

    public ResourceDescriptor mesh;
    public ResourceDescriptor material;

    
    public ResourceDescriptor materialPlan;

    public List<int> meshPartsHidden = [];
    public Primitive[] primitives;

    
    public byte creatureFilter;

    public CostumePiece[] costumePieces;

    
    
    public CostumePiece[] temporaryCostumePiece;

    public PCostume()
    {
        mesh = new ResourceDescriptor(1087, ResourceType.Mesh);
        costumePieces = new CostumePiece[14];
        for (int i = 0; i < costumePieces.Length; ++i)
            costumePieces[i] = new CostumePiece();
        costumePieces[(int)CostumePieceCategory.HEAD].Mesh =
            new ResourceDescriptor(9876, ResourceType.Mesh);
        costumePieces[(int)CostumePieceCategory.TORSO].Mesh =
            new ResourceDescriptor(9877, ResourceType.Mesh);
        temporaryCostumePiece = new CostumePiece[] { new CostumePiece() };
    }

    
    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        mesh = serializer.Resource(mesh, ResourceType.Mesh);
        material = serializer.Resource(material, ResourceType.GfxMaterial);

        if (version >= 0x19a)
            materialPlan = serializer.Resource(materialPlan, ResourceType.Plan,
                true);

        if (serializer.IsWriting())
        {
            int[] vec = meshPartsHidden.ToArray();
            serializer.Intvector(vec);
        }
        else
        {
            int[] vec = serializer.Intvector(null);
            if (vec != null)
            {
                foreach (var v in vec)
                    meshPartsHidden.Add(v);
            }
        }

        primitives = serializer.Array(primitives);

        if (subVersion >= 0xdb)
            creatureFilter = serializer.I8(creatureFilter);

        costumePieces = serializer.Array(costumePieces);

        if (version >= 0x2c5 || revision.Has(Branch.Leerdammer, (int)Revisions.LD_TEMP_COSTUME))
            temporaryCostumePiece = serializer.Array(temporaryCostumePiece);
    }

    
    public int GetAllocatedSize()
    {
        int size = BASE_ALLOCATION_SIZE;
        if (costumePieces != null)
            foreach (CostumePiece piece in costumePieces)
                size += piece.GetAllocatedSize();
        if (primitives != null)
            size += (primitives.Length * Primitive.BaseAllocationSize);
        if (meshPartsHidden != null) size += (meshPartsHidden.Count() * 4);
        return size;
    }


}