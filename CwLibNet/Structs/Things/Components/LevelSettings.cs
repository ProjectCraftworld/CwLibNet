using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;

namespace CwLibNet.Structs.Things.Components;

public class LevelSettings: ISerializable
{
    public const int BaseAllocationSize = 0xb0;

    public Vector3? SunPosition = new Vector3(0.86f, 0.987f, 0.568f);
    public float SunPositionScale = 300000f;
    public Vector4? SunColor = new Vector4(1.0f, 0.9f, 0.8f, 1.0f);
    public Vector4? AmbientColor = new Vector4(0.2f, 0.3f, 0.4f, 1.0f);
    public float SunMultiplier = 1.0f, Exposure = 1.0f;
    public Vector4? FogColor = new Vector4(0.3f, 0.5f, 0.7f, 1.0f);
    public float FogNear = 1000f, FogFar = 4000f;
    public Vector4? RimColor = new Vector4(0.3f, 0.2f, 0.1f, 1.0f);
    
    public Vector4? RimColor2 = new Vector4(0.3f, 0.4f, 0.5f, 1.0f);

    
    public float BakedShadowAmount, BakedShadowBlur = 0.1f;
    
    public float BakedAoBias, BakedAoScale = 1.0f;
    
    public float DynamicAoAmount = 0.4f;
    
    public float DofNear = 4000f;
    
    public float DofFar = 50000f;

    
    public float ZEffectAmount = 0f, ZEffectBright = 0f, ZEffectContrast = 0.333f;

    
    public float DofNear2 = -7500f, DofFar2 = -3500f;
    
    public float DofFar3 = -100000f;
    
    public virtual void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();

        SunPosition = serializer.V3(SunPosition);
        SunPositionScale = serializer.F32(SunPositionScale);
        SunColor = serializer.V4(SunColor);
        AmbientColor = serializer.V4(AmbientColor);
        SunMultiplier = serializer.F32(SunMultiplier);
        Exposure = serializer.F32(Exposure);
        FogColor = serializer.V4(FogColor);
        FogNear = serializer.F32(FogNear);
        FogFar = serializer.F32(FogFar);
        RimColor = serializer.V4(RimColor);

        if (version >= 0x138)
            RimColor2 = serializer.V4(RimColor2);

        if (version >= 0x325)
        {
            BakedShadowAmount = serializer.F32(BakedShadowAmount); // 0x1f
            BakedShadowBlur = serializer.F32(BakedShadowBlur); // 0x20
            BakedAoBias = serializer.F32(BakedAoBias); // 0x21
            BakedAoScale = serializer.F32(BakedAoScale); // 0x22
            DynamicAoAmount = serializer.F32(DynamicAoAmount); // 0x23
            DofNear = serializer.F32(DofNear); // 0x24
        }

        if (version > 0x324 && version < 0x331)
            serializer.F32(0);

        if (version >= 0x326)
            DofFar = serializer.F32(DofFar); // 0x25

        if (version > 0x324 && version < 0x331)
            serializer.F32(0);

        if (version >= 0x331)
        {
            ZEffectAmount = serializer.F32(ZEffectAmount);
            ZEffectBright = serializer.F32(ZEffectBright);
            ZEffectContrast = serializer.F32(ZEffectContrast);
        }

        if (subVersion >= 0x7a)
        {
            DofNear2 = serializer.F32(DofNear2);
            DofFar2 = serializer.F32(DofFar2);
            if (subVersion >= 0xce)
                DofFar3 = serializer.F32(DofFar3);
        }
    }

    public virtual int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}