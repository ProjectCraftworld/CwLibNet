using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PEffector: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public Vector3? PosVel;
    public float AngVel, Viscosity, Density;
    public Vector3? Gravity = new Vector3(0.0f, -2.7f, 0.0f);
    public bool PushBack, Swimmable;
    public float ViscosityCheap = 0.05f;
    public float ModScale = 1.0f;

    
    public void Serialize(Serializer serializer)
    {
        PosVel = serializer.V3(PosVel);
        AngVel = serializer.F32(AngVel);
        Viscosity = serializer.F32(Viscosity);
        Density = serializer.F32(Density);
        Gravity = serializer.V3(Gravity);
        PushBack = serializer.Bool(PushBack);
        Swimmable = serializer.Bool(Swimmable);
        ViscosityCheap = serializer.F32(ViscosityCheap);
        ModScale = serializer.F32(ModScale);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}