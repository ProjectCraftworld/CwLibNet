using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components.Popit;

namespace CwLibNet.Structs.Things.Components;

public class Poppet: ISerializable
{
    public const int BaseAllocationSize = 0x0;

    public bool IsInUse;
    public PoppetMode[]? ModeStack;

    public PoppetEditState Edit = new();

    public Thing? TweakObject;

    public Vector4? BackupCameraZoneTargetBox;

    public Vector3? BackupCameraZonePitchAngle;

    public float BackupCameraZoneZoomDistance;

    public float CameraZoneZoomSpeed;

    public PoppetTweakObjectPlacement TweakObjectPlacement = new();

    public Vector3? MarqueeSelectOrigin;

    public Thing[]? MarqueeSelectList;

    public RaycastResults Raycast = new();

    public int DangerMode;

    public Thing[]? FrozenList;

    public Thing[]? HiddenList;

    public PoppetMaterialOverride OverrideMaterial = new();

    public PoppetShapeOverride OverrideShape = new();

    public Thing[]? TweakObjects;
    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();

        IsInUse = serializer.Bool(IsInUse);
        ModeStack = serializer.Array(ModeStack);

        if (version < 0x2ed)
        {
            if (version < 0x232)
                serializer.V3(null);
            switch (version)
            {
                case < 0x135:
                // revision - 0x185 < 0x35
                // scriptobjectuid
                case >= 0x185 and < 0x1ba:
                    serializer.I32(0); // c32
                    break;
            }

            Edit = serializer.Struct(Edit);

            if (version < 0x18f)
                serializer.Intarray(null);
            if (version is >= 0x148 and < 0x185)
                serializer.Thing(null);

            if (version >= 0x147)
            {
                TweakObject = serializer.Thing(TweakObject);
                BackupCameraZoneTargetBox = serializer.V4(BackupCameraZoneTargetBox);
                BackupCameraZonePitchAngle =
                    serializer.V3(BackupCameraZonePitchAngle);
                BackupCameraZoneZoomDistance =
                    serializer.F32(BackupCameraZoneZoomDistance);
                CameraZoneZoomSpeed = serializer.F32(CameraZoneZoomSpeed);
                TweakObjectPlacement = serializer.Struct(TweakObjectPlacement);
                if (version < 0x211)
                    serializer.Bool(false);
            }

            switch (version)
            {
                case > 0x184 and < 0x1dd:
                    serializer.V3(null);
                    break;
                case > 0x1dc:
                {
                    if (version < 0x232) serializer.Bool(false);
                    MarqueeSelectOrigin = serializer.V3(MarqueeSelectOrigin);
                    MarqueeSelectList = serializer.Thingarray(MarqueeSelectList);
                    break;
                }
            }

            switch (version)
            {
                case >= 0x218:
                    OverrideMaterial = serializer.Struct(OverrideMaterial);
                    break;
                case >= 0x1b8 and < 0x1e2:
                    serializer.I32(0);
                    break;
            }

            switch (version)
            {
                case >= 0x1ba and < 0x1e2:
                    serializer.I32(0);
                    break;
                case >= 0x232:
                    Raycast = serializer.Struct(Raycast);
                    break;
            }

            if (version >= 0x232)
                DangerMode = serializer.I32(DangerMode);
            if (version >= 0x236)
            {
                serializer.I32(0);
                serializer.V3(null);
                if (version >= 0x23a)
                    serializer.V3(null);
            }

            return;
        }

        Raycast = serializer.Struct(Raycast);
        if (version > 0x2ec)
            FrozenList = serializer.Thingarray(FrozenList);
        if (version > 0x2f1)
            HiddenList = serializer.Thingarray(HiddenList);
        if (version >= 0x311)
        {
            OverrideMaterial = serializer.Struct(OverrideMaterial);
            OverrideShape = serializer.Struct(OverrideShape);
        }

        if (version >= 0x3a0)
            TweakObjects = serializer.Thingarray(TweakObjects);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}