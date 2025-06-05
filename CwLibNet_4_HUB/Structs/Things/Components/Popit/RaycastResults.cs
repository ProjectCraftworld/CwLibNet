using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.Popit;

public class RaycastResults: ISerializable
{
    public const int BaseAllocationSize = 0x50;

    public Vector4? Hitpoint;
    public Vector4? Normal;
    public float BaryU, BaryV;
    public int TriIndex;
    public Thing? HitThing;
    public Thing? RefThing;
    public int OnCostumePiece, DecorationIdx;
    public bool SwitchConnector;
    
    public void Serialize(Serializer serializer)
    {
        Hitpoint = serializer.V4(Hitpoint);
        Normal = serializer.V4(Normal);

        BaryU = serializer.F32(BaryU);
        BaryV = serializer.F32(BaryV);
        TriIndex = serializer.I32(TriIndex);

        HitThing = serializer.Thing(HitThing);
        RefThing = serializer.Thing(RefThing);

        OnCostumePiece = serializer.S32(OnCostumePiece);
        DecorationIdx = serializer.I32(DecorationIdx);
        SwitchConnector = serializer.Bool(SwitchConnector);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}