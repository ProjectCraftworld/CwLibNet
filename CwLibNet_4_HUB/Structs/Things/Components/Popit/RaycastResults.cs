using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components.Popit;

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
    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Hitpoint);
        Serializer.Serialize(ref Normal);

        Serializer.Serialize(ref BaryU);
        Serializer.Serialize(ref BaryV);
        Serializer.Serialize(ref TriIndex);

        HitThing = Serializer.SerializeReference(HitThing);
        RefThing = Serializer.SerializeReference(RefThing);

        Serializer.Serialize(ref OnCostumePiece);
        Serializer.Serialize(ref DecorationIdx);
        Serializer.Serialize(ref SwitchConnector);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}