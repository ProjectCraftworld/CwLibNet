using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
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

    
    public float ZEffectAmount, ZEffectBright, ZEffectContrast = 0.333f;

    
    public float DofNear2 = -7500f, DofFar2 = -3500f;
    
    public float DofFar3 = -100000f;
    
    public virtual void Serialize(Serializer serializer)
    {
        var revision = Serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        SunPosition = Serializer.Serialize(ref SunPosition);
        Serializer.Serialize(ref SunPositionScale);
        Serializer.Serialize(ref SunColor);
        Serializer.Serialize(ref AmbientColor);
        Serializer.Serialize(ref SunMultiplier);
        Serializer.Serialize(ref Exposure);
        Serializer.Serialize(ref FogColor);
        Serializer.Serialize(ref FogNear);
        Serializer.Serialize(ref FogFar);
        Serializer.Serialize(ref RimColor);

        if (version >= 0x138)
            Serializer.Serialize(ref RimColor2);

        if (version >= 0x325)
        {
            Serializer.Serialize(ref BakedShadowAmount); // 0x1f
            Serializer.Serialize(ref BakedShadowBlur); // 0x20
            Serializer.Serialize(ref BakedAoBias); // 0x21
            Serializer.Serialize(ref BakedAoScale); // 0x22
            Serializer.Serialize(ref DynamicAoAmount); // 0x23
            Serializer.Serialize(ref DofNear); // 0x24
        }

        if (version is > 0x324 and < 0x331)
            Serializer.Serialize(ref 0);

        if (version >= 0x326)
            Serializer.Serialize(ref DofFar); // 0x25

        switch (version)
        {
            case > 0x324 and < 0x331:
                Serializer.Serialize(ref 0);
                break;
            case >= 0x331:
                Serializer.Serialize(ref ZEffectAmount);
                Serializer.Serialize(ref ZEffectBright);
                Serializer.Serialize(ref ZEffectContrast);
                break;
        }

        if (subVersion >= 0x7a)
        {
            Serializer.Serialize(ref DofNear2);
            Serializer.Serialize(ref DofFar2);
            if (subVersion >= 0xce)
                Serializer.Serialize(ref DofFar3);
        }
    }

    public virtual int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}