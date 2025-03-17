using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;

namespace CwLibNet.Structs.Things.Components.World;

public class GlobalSettings: ISerializable
{
    public const int BaseAllocationSize = 0x14;

    public float LightingFactor = 1.0f, ColorCorrectionFactor, FogFactor, FogTintFactor, DarknessFactor;
    
    public bool NonLinearFog; // Vita


    public void Serialize(Serializer serializer)
    {
        LightingFactor = serializer.F32(LightingFactor);
        ColorCorrectionFactor = serializer.F32(ColorCorrectionFactor);
        FogFactor = serializer.F32(FogFactor);
        FogTintFactor = serializer.F32(FogTintFactor);
        DarknessFactor = serializer.F32(DarknessFactor);
        if (serializer.GetRevision().Has(Branch.Double11, 0x78))
            NonLinearFog = serializer.Bool(NonLinearFog);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}