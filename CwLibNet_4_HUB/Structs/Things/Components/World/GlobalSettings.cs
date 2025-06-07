using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Things.Components.World;

public class GlobalSettings: ISerializable
{
    public const int BaseAllocationSize = 0x14;

    public float LightingFactor = 1.0f, ColorCorrectionFactor, FogFactor, FogTintFactor, DarknessFactor;
    
    public bool NonLinearFog; // Vita


    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref LightingFactor);
        Serializer.Serialize(ref ColorCorrectionFactor);
        Serializer.Serialize(ref FogFactor);
        Serializer.Serialize(ref FogTintFactor);
        Serializer.Serialize(ref DarknessFactor);
        if (Serializer.GetCurrentSerializer().GetRevision().Has(Branch.Double11, 0x78))
            Serializer.Serialize(ref NonLinearFog);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}