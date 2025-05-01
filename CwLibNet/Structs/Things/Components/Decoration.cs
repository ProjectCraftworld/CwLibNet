using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Components;

public class Decoration: ISerializable
{
    public const int BaseAllocationSize = 0x80;

    public PRenderMesh RenderMesh;
    public Matrix4x4? Offset = Matrix4x4.Identity;
    public int ParentBone = -1;
    public int ParentTriVert = -1;
    public float BaryU, BaryV;
    public float DecorationAngle;
    public int OnCostumePiece = -1;
    public int EarthDecoration = -1;
    public float DecorationScale = 1.0f;
    
    public short PlacedBy = -1;
    public bool Reversed;
    
    public bool HasShadow = true;
    
    public bool IsQuest;
    
    public int PlayModeFrame;
    
    public GUID? PlanGuid;
    
    public float ZBias; // Vita

    
    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        RenderMesh = serializer.Reference(RenderMesh);

        Offset = serializer.M44(Offset);
        ParentBone = serializer.I32(ParentBone);
        ParentTriVert = serializer.I32(ParentTriVert);

        BaryU = serializer.F32(BaryU);
        BaryV = serializer.F32(BaryV);

        DecorationAngle = serializer.F32(DecorationAngle);
        OnCostumePiece = serializer.S32(OnCostumePiece);
        EarthDecoration = serializer.S32(EarthDecoration);
        DecorationScale = serializer.F32(DecorationScale);

        if (version >= 0x214)
            PlacedBy = serializer.I16(PlacedBy);
        Reversed = serializer.Bool(Reversed);

        if (subVersion >= 0xc4)
            HasShadow = serializer.Bool(HasShadow);

        if (subVersion >= 0x16c)
            IsQuest = serializer.Bool(IsQuest);

        if (version >= 0x215)
            PlayModeFrame = serializer.I32(PlayModeFrame);

        if (version >= 0x25b)
            PlanGuid = serializer.Guid(PlanGuid);

        if (revision.Has(Branch.Double11, 0x7c))
            ZBias = serializer.F32(ZBias);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (RenderMesh != null)
            size += RenderMesh.GetAllocatedSize();
        return size;
    }


}