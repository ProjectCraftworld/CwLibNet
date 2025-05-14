using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

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

    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();
        var subVersion = serializer.GetRevision().GetSubVersion();

        Color = serializer.V4(Color);
        if (version >= 0x2fd)
            ColorOff = serializer.V4(ColorOff);
        Multiplier = serializer.F32(Multiplier);
        if (version >= 0x30a)
            MultiplierOff = serializer.F32(MultiplierOff);

        GlowRadius = serializer.F32(GlowRadius);
        FarDist = serializer.F32(FarDist);
        SourceSize = serializer.F32(SourceSize);
        falloffTexture = serializer.Resource(falloffTexture,
            ResourceType.Texture);

        LookAt = serializer.Thing(LookAt);
        Spotlight = serializer.Bool(Spotlight);

        if (version < 0x337)
        {
            EnableFogShadows = serializer.Bool(EnableFogShadows);
            EnableFog = serializer.Bool(EnableFog);
        }

        if (version >= 0x139)
            FogAmount = serializer.F32(FogAmount);

        if (version >= 0x13a)
        {
            if (version < 0x2c4)
                OnDest = serializer.F32(OnDest);
            OnSpeed = serializer.F32(OnSpeed);
            OffSpeed = serializer.F32(OffSpeed);
            FlickerProb = serializer.F32(FlickerProb);
            FlickerAmount = serializer.F32(FlickerAmount);
        }

        if (version >= 0x2c4)
            Behavior = serializer.I32(Behavior);

        if (subVersion >= 0x113)
            HighBeam = serializer.Bool(HighBeam);

        if (subVersion >= 0x146)
            Tracker = serializer.Bool(Tracker);

        if (subVersion >= 0x14a)
            TrackerType = serializer.I8(TrackerType);

        if (subVersion >= 0x151)
        {
            CausticStrength = serializer.F32(CausticStrength);
            CausticWidth = serializer.F32(CausticWidth);
        }

        if (subVersion < 0x16f) return;
        TrackingLimit = serializer.F32(TrackingLimit);
        TrackingAccel = serializer.F32(TrackingAccel);
        TrackingSpeed = serializer.F32(TrackingSpeed);
        MovementInput = serializer.S32(MovementInput);
        LightingInput = serializer.S32(MovementInput);

        BeamDir = serializer.V3(BeamDir);
        Azimuth = serializer.V3(Azimuth);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}