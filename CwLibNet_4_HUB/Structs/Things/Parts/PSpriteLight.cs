using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

public class PSpriteLight: ISerializable
{
    public const int BaseAllocationSize = 0xD0;

    public Vector4? Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

    
    public Vector4? ColorOff;

    public float Multiplier = 1.8f;

    
    public float MultiplierOff;

    public float GlowRadius = 20.0f;
    public float FarDist = 2.3f;
    public float SourceSize = 0.28f;
    private ResourceDescriptor? falloffTexture;
    public Thing? LookAt;
    public bool Spotlight;

    public bool EnableFogShadows = true, EnableFog = true;

    
    public float FogAmount = 0.6f;

    public float OnDest = 1.0f;

    
    public float OnSpeed = 1.0f, OffSpeed = 1.0f, FlickerProb, FlickerAmount;

    
    public int Behavior;

    
    public bool HighBeam;

    
    public bool Tracker;

    
    public byte TrackerType;

    
    public float CausticStrength, CausticWidth;

    
    public float TrackingLimit, TrackingAccel, TrackingSpeed;

    
    public int MovementInput, LightingInput;


    public Vector3? BeamDir;
    public Vector3? Azimuth;

    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        Serializer.Serialize(ref Color);
        if (version >= 0x2fd)
            Serializer.Serialize(ref ColorOff);
        Serializer.Serialize(ref Multiplier);
        if (version >= 0x30a)
            Serializer.Serialize(ref MultiplierOff);

        Serializer.Serialize(ref GlowRadius);
        Serializer.Serialize(ref FarDist);
        Serializer.Serialize(ref SourceSize);
        Serializer.Serialize(ref falloffTexture, ResourceType.Texture);

        LookAt = Serializer.SerializeReference(LookAt);
        Serializer.Serialize(ref Spotlight);

        if (version < 0x337)
        {
            Serializer.Serialize(ref EnableFogShadows);
            Serializer.Serialize(ref EnableFog);
        }

        if (version >= 0x139)
            Serializer.Serialize(ref FogAmount);

        if (version >= 0x13a)
        {
            if (version < 0x2c4)
                Serializer.Serialize(ref OnDest);
            Serializer.Serialize(ref OnSpeed);
            Serializer.Serialize(ref OffSpeed);
            Serializer.Serialize(ref FlickerProb);
            Serializer.Serialize(ref FlickerAmount);
        }

        if (version >= 0x2c4)
            Serializer.Serialize(ref Behavior);

        if (subVersion >= 0x113)
            Serializer.Serialize(ref HighBeam);

        if (subVersion >= 0x146)
            Serializer.Serialize(ref Tracker);

        if (subVersion >= 0x14a)
            Serializer.Serialize(ref TrackerType);

        if (subVersion >= 0x151)
        {
            Serializer.Serialize(ref CausticStrength);
            Serializer.Serialize(ref CausticWidth);
        }

        if (subVersion < 0x16f) return;
        Serializer.Serialize(ref TrackingLimit);
        Serializer.Serialize(ref TrackingAccel);
        Serializer.Serialize(ref TrackingSpeed);
        Serializer.Serialize(ref MovementInput);
        Serializer.Serialize(ref MovementInput);

        Serializer.Serialize(ref BeamDir);
        Serializer.Serialize(ref Azimuth);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}