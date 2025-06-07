using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        Serializer.Serialize(ref RenderMesh);

        Serializer.Serialize(ref Offset);
        Serializer.Serialize(ref ParentBone);
        Serializer.Serialize(ref ParentTriVert);

        Serializer.Serialize(ref BaryU);
        Serializer.Serialize(ref BaryV);

        Serializer.Serialize(ref DecorationAngle);
        Serializer.Serialize(ref OnCostumePiece);
        Serializer.Serialize(ref EarthDecoration);
        Serializer.Serialize(ref DecorationScale);

        if (version >= 0x214)
            Serializer.Serialize(ref PlacedBy);
        Serializer.Serialize(ref Reversed);

        if (subVersion >= 0xc4)
            Serializer.Serialize(ref HasShadow);

        if (subVersion >= 0x16c)
            Serializer.Serialize(ref IsQuest);

        if (version >= 0x215)
            Serializer.Serialize(ref PlayModeFrame);

        if (version >= 0x25b)
            Serializer.Serialize(ref PlanGuid);

        if (revision.Has(Branch.Double11, 0x7c))
            Serializer.Serialize(ref ZBias);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (RenderMesh != null)
            size += RenderMesh.GetAllocatedSize();
        return size;
    }


}