using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
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
    
    public void Serialize()
    {
        Serializer.Serialize(ref Hitpoint);
        Serializer.Serialize(ref Normal);

        Serializer.Serialize(ref BaryU);
        Serializer.Serialize(ref BaryV);
        Serializer.Serialize(ref TriIndex);

        HitThing = Serializer.Reference(HitThing);
        RefThing = Serializer.Reference(RefThing);

        Serializer.Serialize(ref OnCostumePiece);
        Serializer.Serialize(ref DecorationIdx);
        Serializer.Serialize(ref SwitchConnector);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}