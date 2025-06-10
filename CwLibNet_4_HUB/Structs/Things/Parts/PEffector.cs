using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Parts;

public class PEffector: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public Vector3? PosVel;
    public float AngVel, Viscosity, Density;
    public Vector3? Gravity = new Vector3(0.0f, -2.7f, 0.0f);
    public bool PushBack, Swimmable;
    public float ViscosityCheap = 0.05f;
    public float ModScale = 1.0f;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref PosVel);
        Serializer.Serialize(ref AngVel);
        Serializer.Serialize(ref Viscosity);
        Serializer.Serialize(ref Density);
        Serializer.Serialize(ref Gravity);
        Serializer.Serialize(ref PushBack);
        Serializer.Serialize(ref Swimmable);
        Serializer.Serialize(ref ViscosityCheap);
        Serializer.Serialize(ref ModScale);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}