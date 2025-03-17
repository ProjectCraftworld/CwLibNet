using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components.Popit;
using CwLibNet.Types.Things;

namespace CwLibNet.Structs.Things.Components;

public class Poppet: ISerializable
{
    public const int BaseAllocationSize = 0x0;

    public bool IsInUse;
    public PoppetMode[]? ModeStack;

    public PoppetEditState Edit = new PoppetEditState();

    public Thing? TweakObject;

    public Vector4? BackupCameraZoneTargetBox;

    public Vector3? BackupCameraZonePitchAngle;

    public float BackupCameraZoneZoomDistance;

    public float CameraZoneZoomSpeed;

    public PoppetTweakObjectPlacement TweakObjectPlacement = new PoppetTweakObjectPlacement();

    public Vector3? MarqueeSelectOrigin;

    public Thing[]? MarqueeSelectList;

    public RaycastResults Raycast = new RaycastResults();

    public int DangerMode;

    public Thing[]? FrozenList;

    public Thing[]? HiddenList;

    public PoppetMaterialOverride OverrideMaterial = new PoppetMaterialOverride();

    public PoppetShapeOverride OverrideShape = new PoppetShapeOverride();

    public Thing[]? TweakObjects;
    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        IsInUse = serializer.Bool(IsInUse);
        ModeStack = serializer.Array(ModeStack);

        if (version < 0x2ed)
        {
            if (version < 0x232)
                serializer.V3(null);
            if (version < 0x135)
                serializer.I32(0); // c32

            // revision - 0x185 < 0x35
            if (version >= 0x185 && version < 0x1ba)
                serializer.I32(0); // scriptobjectuid

            Edit = serializer.Struct(Edit);

            if (version < 0x18f)
                serializer.Intarray(null);
            if (version >= 0x148 && version < 0x185)
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

            if (version > 0x184 && version < 0x1dd)
                serializer.V3(null);

            if (version > 0x1dc)
            {
                if (version < 0x232) serializer.Bool(false);
                MarqueeSelectOrigin = serializer.V3(MarqueeSelectOrigin);
                MarqueeSelectList = serializer.Thingarray(MarqueeSelectList);
            }

            if (version >= 0x218)
                OverrideMaterial = serializer.Struct(OverrideMaterial);

            if (version >= 0x1b8 && version < 0x1e2)
                serializer.I32(0);
            if (version >= 0x1ba && version < 0x1e2)
                serializer.I32(0);

            if (version >= 0x232)
                Raycast = serializer.Struct(Raycast);
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